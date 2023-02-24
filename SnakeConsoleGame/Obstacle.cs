using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace SnakeConsoleGame
{
    class Obstacle
    {
        public string ObstacleShape;
        public ConsoleColor ObstacleColor;
        public int xCoord;
        public int yCoord;
        public static Random RandomX = new Random();
        public static Random RandomY = new Random();
        public bool IsPrinted { set; get; }
        /// <summary>
        /// Constructor to create a new obstacle to be placed in the obstacle queue.
        /// </summary>
        /// <param name="obstacleShape">The string that will be the shape of the obstacle</param>
        /// <param name="obstacleColor">The foreground color the obstacleShape will be printed in</param>
        /// <param name="x">The unique x coordinate position to be used for SetCursorPosition(x,y)</param>
        /// <param name="y">The unique y coordinate position to be used for SetCursorPosition(x,y)</param>
        public Obstacle(string obstacleShape, ConsoleColor obstacleColor, int x, int y)
        {
            ObstacleColor = obstacleColor;
            ObstacleShape = obstacleShape;
            xCoord = x;
            yCoord = y;
            IsPrinted = false;
        }

        /// <summary>
        /// Creates a new queue of type Obstacle and fills the queue with Obstacle objects with random
        /// x and y values that fall within the game boundaries and are also not located directly on top
        /// of the snakes current position.
        /// </summary>
        /// <param name="MinX">The minimum int x value that falls inside the game boundaries</param>
        /// <param name="MaxX">The maximum int x value that falls inside the game boundaries</param>
        /// <param name="MinY">The minimum int y value that falls inside the game boundaries</param>
        /// <param name="MaxY">The maximum int y value that falls inside the game boundaries</param>
        /// <param name="snakePosition">The queue that holds the snake body objects x and y coordinates</param>
        /// <returns>The Obstacles queue that will be used to print the newly generated obstacles on the game board</returns>
        public static Queue<Obstacle> GenerateObstacles(int MinX, int MaxX, int MinY, int MaxY,Queue<SnakeBodyCoordinates> snakePosition)
        {
            int ObstacleX;
            int ObstacleY;
            Queue<Obstacle> Obstacles = new Queue<Obstacle>(5);
            while (!Obstacles.IsFull())
            {
                ObstacleX = RandomX.Next(MinX,MaxX);
                while (ObstacleX % 2 != snakePosition.Items.Last().BodyX % 2)
                {
                    ObstacleX = RandomX.Next(MinX, MaxX);
                }
                ObstacleY = RandomY.Next(MinY,MaxY);
                Obstacle NewObstacle = new Obstacle("¤", ConsoleColor.Red,ObstacleX,ObstacleY);
                Obstacles.Enqueue(NewObstacle);
            }
            // double checking that the obstacles are not printed right on top of the snake
            foreach (SnakeBodyCoordinates checkBodyCoord in snakePosition.Items)
            {
                foreach (Obstacle obstacle in Obstacles.Items)
                {
                    while (obstacle.xCoord == checkBodyCoord.BodyX && obstacle.yCoord == checkBodyCoord.BodyY)
                    {
                        obstacle.xCoord = RandomX.Next(MinX, MaxX);
                        while (obstacle.xCoord % 2 != checkBodyCoord.BodyX % 2)
                        {
                            obstacle.xCoord = RandomX.Next(MinX, MaxX);
                        }
                        obstacle.yCoord = RandomY.Next(MinY, MaxY);
                    }
                }
            }
            return Obstacles;
        }
        /// <summary>
        /// Method used to delete the obstacle that is passed in at the obstacle.xCoord, obstacle.yCoord.
        /// </summary>
        /// <param name="obstacle">The Obstacle object that is to be deleted from the game board</param>
        public void DeleteObstacle(Obstacle obstacle)
        {
            Console.SetCursorPosition(obstacle.xCoord,obstacle.yCoord);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(" ");
        }
        /// <summary>
        /// Prints each obstacle in the obstacleList that is passed in on the game board.
        /// </summary>
        /// <param name="obstacleList">The Queue of type Obstacle containing the list of obstacles to be printed</param>
        public static void PrintObstacles(Queue<Obstacle> obstacleList)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            foreach (Obstacle obstacle in obstacleList.Items)
            {
                Console.SetCursorPosition(obstacle.xCoord, obstacle.yCoord);
                Console.ForegroundColor = obstacle.ObstacleColor;
                Console.Write(obstacle.ObstacleShape);
                obstacle.IsPrinted = true;
            }
        }
    }
}
