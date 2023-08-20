using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xaml;
using XBTJNIProg4.Model;

namespace XBTJNIProg4.Model
{
    class GameLogic
    {
        private bool isJumping = false;
        private int jumpHeight = 3; // Adjust this value for jump intensity, not used right now
        private int currentJump = 0;
        GameModel model;
        /// <summary>
        /// Constructor that takes a model and a string parameter
        /// </summary>
        /// <param name="model"></param>
        /// <param name="fname"></param>
        public GameLogic(GameModel model, string fname)
        {
            this.model = model;
            InitModel(fname);
        }
        /// <summary>
        /// initializing the map, reads the width and height from the first 2 lines
        /// and loads the map according to the different letters in the lvl file
        /// </summary>
        /// <param name="fname"></param>
        private void InitModel(string fname)
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(fname);
            StreamReader sr = new StreamReader(stream);
            string[] lines = sr.ReadToEnd().Replace("\r", "").Split('\n');

            int width = int.Parse(lines[0]);
            int height = int.Parse(lines[1]);
            model.Walls = new bool[width, height];
            model.TileSize = Math.Min(model.GameWidth / width, model.GameHeight / height);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    char current = lines[y + 2][x];
                    model.Walls[x, y] = (current == 'e');
                    if (current == 'S') model.Player = new System.Windows.Point(x, y);
                    if (current == 'F') model.Exit = new System.Windows.Point(x, y);
                    if (current == 'C') model.Coin = new System.Windows.Point(x, y);
                }
            }
        }

        // The movement is very basic
        public bool Move(int dx, int dy)
        {
            int newX = (int)(model.Player.X + dx);
            int newY = (int)(model.Player.Y + dy);

            if (isJumping)
            {
                newY -= currentJump;
                currentJump--;

                if (currentJump <= 0)
                {
                    currentJump = 0;
                    isJumping = false;
                }
            }
            else if (newY < model.Walls.GetLength(1) &&
                     (newY == model.Player.Y || model.Walls[newX, newY]))
            {
                // Falling logic
                while (newY < model.Walls.GetLength(1) && !model.Walls[newX, newY])
                {
                    newY++;
                }
                newY--; // Move back to the last valid position

                if (newY > model.Player.Y)
                {
                    // Player is falling
                    // Could add additional logic here like collision with ground
                }
            }

            if (newX >= 0 && newY >= 0 &&
                newX < model.Walls.GetLength(0) &&
                newY < model.Walls.GetLength(1) &&
                !model.Walls[newX, newY])
            {
                model.Player = new System.Windows.Point(newX, newY);
            }

            return model.Player.Equals(model.Exit);
        }


        /// <summary>
        /// Pixel pos to tile pos
        /// </summary>
        /// <param name="mousePos"></param>
        /// <returns></returns>
        //public System.Windows.Point GetTilePos(System.Windows.Point mousePos)
        //   {
        //      return new System.Windows.Point((int)(mousePos.X / model.TileSize), 
        //                       (int)(mousePos.Y / model.TileSize));
        //  }

    }
}