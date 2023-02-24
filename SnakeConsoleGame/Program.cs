using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Media;

namespace SnakeConsoleGame
{
    class Program
    {
        public static Snake newSnake = new Snake(ConsoleKey.Enter, ConsoleColor.Yellow, ConsoleColor.Cyan, 10, 100, 10, 10);
        public static int MaxX = 60;
        public static int MaxY = 20;
        public static int X;
        public static int Y;

        public static int Tick = 0;
        public static bool PrintObstacles;
        public static bool PrintObstaclesDone = false;
        public static int Score = 0;

        public static int WindowWidth;
        public static int WindowHeight;

        static ConsoleKey KeyPress = ConsoleKey.Spacebar;
        public static string GameOverText = "Game Over!";
        public static Queue<SnakeBodyCoordinates> SnakeBody = new Queue<SnakeBodyCoordinates>(newSnake.SnakeLength);
        public static Queue<SnakeBodyCoordinates> BodyCoordinates = new Queue<SnakeBodyCoordinates>(newSnake.SnakeLength-1);
        public static Queue<Obstacle> ObstacleList;
        public static Queue<Obstacle> PreviousObstacleList;
        public static Thread thr1;
        public static Thread thr2;
        public static Thread thr3;
        public static SoundPlayer bgMusic;
        public static SoundPlayer sound;

        static void Main(string[] args)
        {
            Console.Title = "Snake";
            Console.CursorVisible = false;
            sound = new SoundPlayer($"{Environment.CurrentDirectory}\\gameoversound.wav");
            bgMusic = new SoundPlayer($"{Environment.CurrentDirectory}\\bgmusic.wav");
            //thr2 = new Thread(PrintTitleMenu);
            thr3 = new Thread(PlayBGMusic);
            //thr2.Start();
            PrintTitleMenu();

        }
        /// <summary>
        /// Prints the 3 menu options to the console display and waits for the user to pick one of the 3 options
        /// If the user presses any keys other than the 3 options provided, the method is called again.
        /// </summary>
        private static void PrintTitleMenu()
        {
            string PressEnter = "Press Enter to start.";
            string PrintControl = "Press 1 to print controls.";
            string PressEscape = "Press Escape to Exit.";

            WindowWidth = (Console.WindowWidth / 2);
            WindowHeight = (Console.WindowHeight / 2);
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            for (int i = -2; i < 8; i++)
            {
                Console.SetCursorPosition(WindowWidth / 2, i+WindowHeight/2);
                for (int j = 0; j < WindowWidth; j++)
                {
                    Console.Write(" ");
                }
            }

            Console.SetCursorPosition(WindowWidth-(PressEnter.Length/2), WindowHeight / 2 );
            Console.Write(PressEnter);
            Console.SetCursorPosition(WindowWidth-(PrintControl.Length/2), WindowHeight/2+2);
            Console.Write(PrintControl);
            Console.SetCursorPosition(WindowWidth-(PressEscape.Length/2), WindowHeight/2+4);
            Console.Write(PressEscape);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.CursorVisible = false;
            int textLength = Console.WindowWidth/2 - "-------------------------------".Length;
            Console.SetCursorPosition(textLength, WindowHeight / 2 + 10);
            Console.Write("   ▄████████ ███▄▄▄▄      ▄████████    ▄█   ▄█▄    ▄████████ \n");
            Console.CursorLeft = textLength;
            Console.Write("  ███    ███ ███▀▀▀██▄   ███    ███   ███ ▄███▀   ███    ███ \n");
            Console.CursorLeft = textLength;
            Console.Write("  ███    █▀  ███   ███   ███    ███   ███▐██▀     ███    █▀  \n");
            Console.CursorLeft = textLength;
            Console.Write("  ███        ███   ███   ███    ███  ▄█████▀     ▄███▄▄▄     \n");
            Console.CursorLeft = textLength;
            Console.Write("▀███████████ ███   ███ ▀███████████ ▀▀█████▄    ▀▀███▀▀▀     \n");
            Console.CursorLeft = textLength;
            Console.Write("         ███ ███   ███   ███    ███   ███▐██▄     ███    █▄  \n");
            Console.CursorLeft = textLength;
            Console.Write("   ▄█    ███ ███   ███   ███    ███   ███ ▀███▄   ███    ███ \n");
            Console.CursorLeft = textLength;
            Console.Write(" ▄████████▀   ▀█   █▀    ███    █▀    ███   ▀█▀   ██████████ \n");
            Console.CursorLeft = textLength;
            Console.Write("                                      ▀                      \n");
            ConsoleKey MenuSelection = Console.ReadKey().Key;
            if (MenuSelection == ConsoleKey.Enter)
            {
                PlayBGMusic();
                PlayGame();
            }
            else if (MenuSelection == ConsoleKey.D1)
            {
                Console.Clear();
                Console.OutputEncoding = System.Text.Encoding.Unicode;
                Console.SetCursorPosition(WindowWidth/2, WindowHeight/2);
                Console.Write("˂ Change snake direction using W,A,S,D keys >");
                Console.SetCursorPosition(WindowWidth/2, WindowHeight/2+2);
                Console.Write("► Press Escape to shutdown the game during gameplay ◄");
                Console.SetCursorPosition(WindowWidth/2, (WindowHeight/2)+4);
                Console.Write("ǁ˃ Press any other key during game play to pause the game");
                Console.SetCursorPosition(WindowWidth, WindowHeight);
                Console.Write("Press any key to return to title menu... ⌂");
                Console.ReadKey();
                Console.Clear();
                PrintTitleMenu();
            }
            else if (MenuSelection == ConsoleKey.Escape) 
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.SetCursorPosition((Console.WindowWidth / 2)-4, (Console.WindowHeight / 2));
                Console.Write("Goodbye!");
                Console.SetCursorPosition(0,0);
                KeyPress = ConsoleKey.Escape;
                System.Threading.Thread.Sleep(3000);
                // TODO maybe a shut down animation or something.
            }
            else
            {
                Console.Clear();
                PrintTitleMenu();
            }
            //thr2.Abort();
        }

        
        /// <summary>
        /// This method clears the console and creates a new game board based on the console windows width and height.
        /// This method also creates a new snake that the user will be controlling in the do-while switch inside this method.
        /// The do-while switch is used to control the direction the snake is moving.
        /// </summary>
        private static void PlayGame()
        {
            Console.Clear();
            CreateBorder();
            CreateNewSnake();
            ObstacleList = Obstacle.GenerateObstacles(WindowWidth+1, MaxX + WindowWidth - 1, WindowHeight+1, MaxY + WindowHeight-1, SnakeBody);
            PreviousObstacleList = ObstacleList;

            do
            {
                Thread.Sleep(newSnake.SnakeSpeed);
                if (KeyPress == ConsoleKey.W || KeyPress == ConsoleKey.A || KeyPress == ConsoleKey.S || KeyPress == ConsoleKey.D)
                {
                    Console.CursorVisible = false;
                    TimeTickEvents(Tick);
                    Tick++;
                }

                if (Console.KeyAvailable)
                {
                    KeyPress = Console.ReadKey(true).Key;
                }
                
                switch (KeyPress)
                {

                    case ConsoleKey.A:
                        X -= 2;
                        Move(X,Y);
                        ChangeColor(X + 2, Y);
                        KeyPress = ConsoleKey.A;
                        break;
                    case ConsoleKey.W:
                        Y -= 1;
                        Move(X, Y);
                        ChangeColor(X, Y + 1);
                        KeyPress = ConsoleKey.W;
                        break;
                    case ConsoleKey.S:
                        Y += 1;
                        Move(X, Y);
                        ChangeColor(X, Y - 1);
                        KeyPress = ConsoleKey.S;
                        break;
                    case ConsoleKey.D:
                        X += 2;
                        Move(X, Y);
                        ChangeColor(X - 2, Y);
                        KeyPress = ConsoleKey.D;
                        break;
                    case ConsoleKey.Spacebar:
                        KeyPress = ConsoleKey.Spacebar;
                        break;
                    case ConsoleKey.Escape:
                        KeyPress = ConsoleKey.Escape;
                        break;
                }

                
                DetectCollision();

            } while (KeyPress != ConsoleKey.Escape);
            
        }
        /// <summary>
        /// This method counts by Tick++ every time the snake moves 1 spot.
        /// Every time tick == 30 the snake movement speed is increased (sleep interval is decreased).
        /// The score is calculated based on how fast the snake is moving after every time tick == 30.
        /// The locations of the obstacles that will be placed are flashed as a warning to the player before
        /// placing the new obstacles to avoid an impossible game.
        /// </summary>
        /// <param name="tick">The number of times the snake has moved one spot in any direction</param>
        private static void TimeTickEvents(int tick)
        {
            if (tick == 30)
            {
                if (newSnake.SnakeSpeed > 90)
                {
                    newSnake.SnakeSpeed -= 8;
                }
                else if (newSnake.SnakeSpeed > 60)
                {
                    newSnake.SnakeSpeed -= 4;
                }
                else if (newSnake.SnakeSpeed > 50)
                {
                    newSnake.SnakeSpeed -= 2;
                }
                
                Score += (newSnake.SnakeSpeed - (newSnake.SnakeSpeed / 4));
                PreviousObstacleList = ObstacleList;
                ObstacleList = Obstacle.GenerateObstacles(WindowWidth + 1, MaxX + WindowWidth - 1, WindowHeight + 1, MaxY + WindowHeight - 1, SnakeBody);
                PrintObstaclesDone = false;
                
            }
            if (tick > 30 && tick <= 45)
            {
                GameTimers.FlashingColorTimer_Elapsed();
                if (PrintObstacles == false)
                {
                    PrintObstacles = true;
                }
            }
            else if (PrintObstacles == true)
            {
                Obstacle.PrintObstacles(ObstacleList);
                ErasePreviousObstacles();
                PreviousObstacleList = ObstacleList;
                PrintObstaclesDone = true;
                PrintObstacles = false;
                Tick = 0;
            }
        }
        /// <summary>
        /// This method switches the background color that is being flashed as a warning before placing the new obstacles.
        /// </summary>
        /// <param name="color">The color that will be used to indicate where a new obstacle is being placed</param>
        public static void ObstacleWarning(ConsoleColor color) // TODO
        {
            foreach (Obstacle obstacle in ObstacleList.Items)
            {
                Console.SetCursorPosition(obstacle.xCoord, obstacle.yCoord);
                Console.BackgroundColor = color;
                Console.Write(" ");
                Console.BackgroundColor = ConsoleColor.Black;
            }
            
        }
        /// <summary>
        /// This method will change the color of the previous head of the snake
        /// using the x and y parameters as coordinates for SetCursorPosition(x,y)
        /// </summary>
        /// <param name="x">the x coordinate of the snake body part</param>
        /// <param name="y">the y coordinate of the snake body part</param>
        private static void ChangeColor(int x, int y)
        {
            if (SnakeBody.QueueSize() >= 2)
            {
                Console.SetCursorPosition(x, y);
                SnakeBodyCoordinates BodyPart = new SnakeBodyCoordinates(x, y);
                BodyCoordinates.Enqueue(BodyPart);
                Console.ForegroundColor = newSnake.BodyColor;
                Console.Write("■");
            }
        }
        /// <summary>
        /// This method will dequeue the tail of the snake and enqueue a new head after printing it one place
        /// forward in the direction the snake is moving. The location of the new head is specified using 
        /// the x and y coordinates to position the console cursor.
        /// </summary>
        /// <param name="x">The x coordinate for the new head position</param>
        /// <param name="y">The x coordinate for the new head position</param>
        private static void Move(int x, int y)
        {
            if (SnakeBody.QueueSize() >= newSnake.SnakeLength)
            {
                SnakeBodyCoordinates DequeueTail = BodyCoordinates.Dequeue();
                SnakeBody.Dequeue();
                Console.SetCursorPosition(DequeueTail.BodyX, DequeueTail.BodyY);
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write(" ");
            }
            Console.SetCursorPosition(x, y);
            newSnake.SnakeX = x;
            newSnake.SnakeY = y;
            Console.ForegroundColor = newSnake.HeadColor;
            Console.Write("■");
            SnakeBodyCoordinates NewHead = new SnakeBodyCoordinates(x,y);
            SnakeBody.Enqueue(NewHead);
            if (ObstacleList.Items[0].IsPrinted)
                Obstacle.PrintObstacles(ObstacleList);
        }
        /// <summary>
        /// This method places the snake on the board based on the dimensions of the board in 
        /// correlation to the console window dimensions.
        /// The cursor is set to visible so the player can see where the snake will start from.
        /// </summary>
        private static void CreateNewSnake()
        {
            Console.ForegroundColor = newSnake.HeadColor;
            newSnake.SnakeX += WindowWidth;
            newSnake.SnakeY += WindowHeight;
            X = newSnake.SnakeX;
            Y = newSnake.SnakeY;
            Console.SetCursorPosition(newSnake.SnakeX, newSnake.SnakeY);
            Console.CursorVisible = true;
        }
        /// <summary>
        /// This method will draw the game border in the center of the console window.
        /// </summary>
        private static void CreateBorder()
        {
            WindowWidth = (Console.WindowWidth / 2) - 30;
            WindowHeight = (Console.WindowHeight / 2) - 10;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            for (int i = WindowHeight; i < MaxY + WindowHeight; i++)
            {
                Console.SetCursorPosition(WindowWidth, i);
                Console.Write("◊");
                Console.SetCursorPosition(MaxX + WindowWidth, i);
                Console.Write("◊");
            }

            for (int i = WindowWidth; i < MaxX + WindowWidth + 1; i++)
            {
                
                Console.SetCursorPosition(i, WindowHeight);
                Console.Write("▲");
                Console.SetCursorPosition(i, MaxY + WindowHeight);
                Console.Write("▼");
            }
        }
        /// <summary>
        /// This method will compare the position of the head of the snake with the body of the snake,
        /// the boundaries of the game board, and the obstacles that have been placed on the board.
        /// </summary>
        public static void DetectCollision()
        {
            // Boundary collision
            if (newSnake.SnakeX == WindowWidth || newSnake.SnakeX == WindowWidth + 60 || newSnake.SnakeY == WindowHeight || newSnake.SnakeY == WindowHeight + 20)
            {
                GameOver();
            }
            // self collision
            foreach (SnakeBodyCoordinates BodyPosition in BodyCoordinates.Items)
            {
                if (BodyPosition.BodyX == X && BodyPosition.BodyY == Y)
                {
                    GameOver();
                }
                
            }
            // TODO Obstacle Collision
            if ((!PreviousObstacleList.IsEmpty())) 
            { 
                foreach (Obstacle anObstacle in PreviousObstacleList.Items)
                {
                    if (anObstacle.xCoord == X && anObstacle.yCoord == Y && anObstacle.IsPrinted)
                    {
                        GameOver();
                    }
                }
            }
        }
        /// <summary>
        /// This method will remove the obstacles that are no longer able to be detected as collisions.
        /// </summary>
        public static void ErasePreviousObstacles()
        {
            foreach (Obstacle obstacle in PreviousObstacleList.Items) 
            {
                Console.SetCursorPosition(obstacle.xCoord, obstacle.yCoord);
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write(" ");
            }

        }
        /// <summary>
        /// This method is executed whenever a game over condition is met while playing the game.
        /// This method will play the game over sound and display the players current score before 
        /// resetting all of the variables back to default.
        /// The only way to continue another game or return to the title menu is by pressing the enter key.
        /// </summary>
        private static void GameOver()
        {
            thr1 = new Thread(PlayGameOverSound);
            thr1.Start();
            thr3.Abort();
            KeyPress = ConsoleKey.Spacebar;
            Console.SetCursorPosition((WindowWidth + 30) - (GameOverText.Length / 2), WindowHeight + 10);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write(GameOverText);
            Console.SetCursorPosition((WindowWidth + 30) - ((Score.ToString().Length + 13) / 2), WindowHeight + 11);
            Console.Write($"Total Score: {Score}");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.SetCursorPosition(Console.WindowWidth / 2, WindowHeight + MaxY + 2);
            Console.Write("Press Enter to return to title menu...");
            Score = 0;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(0, 0);
            while (Console.ReadKey().Key != ConsoleKey.Enter)
            {

            }
            Console.ForegroundColor = ConsoleColor.DarkGray;

            Console.Clear();
            newSnake = new Snake(ConsoleKey.Enter, ConsoleColor.Yellow, ConsoleColor.Cyan, 10, 100, 10, 10);
            X = newSnake.SnakeX;
            Y = newSnake.SnakeY;
            SnakeBody = new Queue<SnakeBodyCoordinates>(newSnake.SnakeLength);
            BodyCoordinates = new Queue<SnakeBodyCoordinates>(newSnake.SnakeLength - 1);
            PreviousObstacleList = Obstacle.GenerateObstacles(WindowWidth + 1, MaxX + WindowWidth - 1, WindowHeight + 1, MaxY + WindowHeight - 1, SnakeBody);
            PrintObstaclesDone = false;
            PrintObstacles = false;
            Tick = 0;
            PrintTitleMenu();
        }
        /// <summary>
        /// This method will play the wav file that is stored in the variable sound.
        /// </summary>
        private static void PlayGameOverSound()
        {
            sound.Play();
        }
        /// <summary>
        /// This method will play the wav file that is stored in the variable bgMusic.
        /// </summary>
        private static void PlayBGMusic()
        {
            bgMusic.Play();
        }
    }
}
