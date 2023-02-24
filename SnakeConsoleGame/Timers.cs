using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace SnakeConsoleGame
{
    
    public class GameTimers
    {
        public static Timer FlashingColorTimer = new Timer(200);
        public static int FlashCount;

        /// <summary>
        /// This method is called when new obstacles are about to be placed to
        /// flash white, gray, and then black to indicate the new locations
        /// where obstacles will be placed.
        /// </summary>
        public static void FlashingColorTimer_Elapsed()
        {
            if (FlashCount % 2 == 0) 
            { 
                Program.ObstacleWarning(ConsoleColor.White);
            }
            else if (FlashCount < 5)
            {
                Program.ObstacleWarning(ConsoleColor.Gray);
            }
            
            if (FlashCount >= 4 && FlashCount <= 5)
            {
                Program.ObstacleWarning(ConsoleColor.Black);
                
            }
            if (FlashCount == 5)
            {
                FlashCount = 0;
                Program.ObstacleWarning(ConsoleColor.Black);
            }
            FlashCount++;
        }

        

    }
}
