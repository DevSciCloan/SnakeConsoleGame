using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeConsoleGame
{
    public struct SnakeBodyCoordinates
    {
        public int BodyX {get; private set;}
        public int BodyY { get; private set; }

        public SnakeBodyCoordinates(int xCoord, int yCoord)
        {
            BodyX = xCoord;
            BodyY = yCoord;
        }
    }
}
