using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeConsoleGame
{
    public class Snake
    {
        public ConsoleKey Direction { get; set; }
        public ConsoleColor HeadColor { get; set; }
        public ConsoleColor BodyColor { get; set; }
        public int SnakeLength { get; set; }
        public int SnakeSpeed { get; set; }
        public int SnakeX { get; set; }
        public int SnakeY { get; set; }

        public Snake(ConsoleKey direction, ConsoleColor headColor, ConsoleColor bodyColor, int snakeLength, int snakeSpeed, int x, int y) 
        {
            Direction = direction;
            HeadColor = headColor;
            BodyColor = bodyColor;
            SnakeLength = snakeLength;
            SnakeSpeed = snakeSpeed;
            SnakeX = x;
            SnakeY = y;
        }

        
    }
}
