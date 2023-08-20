using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace XBTJNIProg4.Model
{
    class GameRenderer
    {
        GameModel model;

        Drawing oldBackground;
        Drawing oldWalls;
        Drawing oldCoin;
        Drawing oldExit;
        Drawing oldPlayer;
        System.Windows.Point oldCoinPos;
        System.Windows.Point oldPlayerPos;

        /// <summary>
        /// Dictioray that stores the brushes for different elements of the game
        /// </summary>

        Dictionary<string, Brush> brushes = new Dictionary<string, Brush>();
        public GameRenderer(GameModel model)
        {
            this.model = model;
        }
        /// <summary>
        /// Used to change map at the end
        /// </summary>
        public void Reset()
        {
            oldBackground = null;
            oldWalls = null;
            oldCoin = null;
            oldExit = null;
            oldPlayer = null;
            oldCoinPos = new System.Windows.Point(-1, -1);
            oldPlayerPos = new System.Windows.Point(-1, -1);
            brushes.Clear();
        }

        /// <summary>
        /// Gets the path to the images with reflection
        /// </summary>
        /// <param name="fname"></param>
        /// <param name="isTiled"></param>
        /// <returns></returns>
        Brush GetBrush(string fname, bool isTiled)
        {
            if (!brushes.ContainsKey(fname))
            {
                BitmapImage bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream(fname);
                bmp.EndInit();
                ImageBrush imageBrush = new ImageBrush(bmp);

                if (isTiled)
                {
                    imageBrush.TileMode = TileMode.Tile;
                    imageBrush.Viewport = new Rect(0, 0, model.TileSize, model.TileSize);
                    imageBrush.ViewportUnits = BrushMappingMode.Absolute;
                }
                brushes[fname] = imageBrush;
            }
            return brushes[fname];
        }
        Brush BackgroundBrush { get { return GetBrush("XBTJNIProg4.Images.mariobg.jpg", false); } }
        Brush PlayerBrush { get { return GetBrush("XBTJNIProg4.Images.mariocharacter.jpg", false); } }
        Brush ExitBrush { get { return GetBrush("XBTJNIProg4.Images.mariofinishflag.jpg", false); } }
        Brush CoinBrush { get { return GetBrush("XBTJNIProg4.Images.transparentmariocoin.png", false); } }
        Brush WallBrush { get { return GetBrush("XBTJNIProg4.Images.marioblock.jpg", true); } }

        public Drawing BuildDrawing()
        {
            DrawingGroup dg = new DrawingGroup();
            dg.Children.Add(GetBackground());
            dg.Children.Add(GetWalls());
            dg.Children.Add(GetGetExit());
            dg.Children.Add(GetPlayer());
            dg.Children.Add(GetCoin());
            return dg;
        }
        private Drawing GetBackground()
        {
            if (oldBackground == null)
            {
                Geometry geo = new RectangleGeometry(new Rect(0, 0, model.GameWidth, model.GameHeight));
                oldBackground = new GeometryDrawing(BackgroundBrush, null, geo);
            }
            return oldBackground;
        }
        private Drawing GetGetExit()
        {
            if (oldExit == null)
            {
                Geometry geo = new RectangleGeometry(new Rect(model.Exit.X * model.TileSize,
                model.Exit.Y * model.TileSize, model.TileSize, model.TileSize));
                oldExit = new GeometryDrawing(ExitBrush, null, geo);
            }
            return oldExit;
        }
        private Drawing GetPlayer()
        {
            if (oldPlayer == null || oldPlayerPos != model.Player)
            {
                Geometry geo = new RectangleGeometry(new Rect(model.Player.X * model.TileSize,
                model.Player.Y * model.TileSize, model.TileSize, model.TileSize));
                oldPlayer = new GeometryDrawing(PlayerBrush, null, geo);
                oldPlayerPos = model.Player;
            }
            return oldPlayer;
        }
        private Drawing GetCoin()
        {
            if (oldCoinPos == model.Player || oldPlayerPos == model.Coin)
            {
                model.Coin = new System.Windows.Point(-1, -1);
                Geometry geo = new RectangleGeometry(new Rect(model.Coin.X * model.TileSize,
                model.Coin.Y * model.TileSize, model.TileSize, model.TileSize));
                oldCoin = new GeometryDrawing(CoinBrush, null, geo);
            }
            if (oldCoin == null)
            {
                Geometry geo = new RectangleGeometry(new Rect(model.Coin.X * model.TileSize,
                model.Coin.Y * model.TileSize, model.TileSize, model.TileSize));
                oldCoin = new GeometryDrawing(CoinBrush, null, geo);
            }
            return oldCoin;
        }
        private Drawing GetWalls()
        {
            if (oldWalls == null)
            {
                GeometryGroup geo = new GeometryGroup();
                for (int x = 0; x<model.Walls.GetLength(0); x++)
                {
                    for (int y = 0; y < model.Walls.GetLength(1); y++)
                    {
                        if (model.Walls[x, y])
                        {
                            Geometry box = new RectangleGeometry(new Rect(x*model.TileSize, y*model.TileSize,
                            model.TileSize, model.TileSize));
                            geo.Children.Add(box);
                        }
                    }
                }
                oldWalls = new GeometryDrawing(WallBrush, null, geo);
            }
            return oldWalls;
        }
    }
}
