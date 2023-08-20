using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XBTJNIProg4.Model
{
    class GameModel
    {
        public bool[,] Walls { get; set; }
        public System.Windows.Point Player { get; set; }
        public System.Windows.Point Coin { get; set; }
        public System.Windows.Point Exit { get; set; }

        public double GameWidth { get; private set; }
        public double GameHeight { get; private set; }
        public double TileSize { get; set; }

        /// <summary>
        /// Contructor that takes 2 double parameters
        /// width and height of the window
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public GameModel(double width, double height)
        {
            GameWidth = width;
            GameHeight = height;
        }
    }
}
