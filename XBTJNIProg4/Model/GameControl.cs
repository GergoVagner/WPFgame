using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using XBTJNIProg4.Model;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Media;

namespace XBTJNIProg4.Model
{
    class GameControl : FrameworkElement
    {
        GameRenderer renderer;
        GameLogic logic;
        GameModel model;
        Stopwatch stopwatch;
        string[] maps = new string[3];
        int counter;
        public GameControl()
        {
            Loaded += GameControl_Loaded;
            maps[0] = "XBTJNIProg4.Levels.Lvl2.lvl";
            maps[1] = "XBTJNIProg4.Levels.Lvl3.lvl";
            maps[2] = "XBTJNIProg4.Levels.lvl4.lvl";
        }
        /// <summary>
        /// Loads the map from the given path
        /// actualwidth and actual height comes from FrameworkElement
        /// renderer loads the models
        /// </summary>
        private void GameControl_Loaded(object sender, RoutedEventArgs e)
        {
            stopwatch = new Stopwatch(); 
            model = new GameModel(ActualWidth, ActualHeight);
            logic = new GameLogic(model, "XBTJNIProg4.Levels.Ltest.lvl"); 
            renderer = new GameRenderer(model);

            Window win = Window.GetWindow(this);
            if (win != null)
            {
                win.KeyDown += Win_KeyDown;
                MouseDown += GameControl_MouseDown;
            }
            InvalidateVisual();
            stopwatch.Start();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (renderer != null)
            {
                drawingContext.DrawDrawing(renderer.BuildDrawing());
            }
        }
        private void GameControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
        //    Point mousePos = e.GetPosition(this);
           // Point tilePos = logic.GetTilePos(mousePos);
         //   MessageBox.Show(mousePos + "\n" + tilePos);
        }
        /// <summary>
        /// Character navigation and next map 
        /// </summary>
        private void Win_KeyDown(object sender,KeyEventArgs e)
        {
            bool finished = false;
            switch (e.Key)
            {
                case Key.Up: finished = logic.Move(0,-2); break;
                case Key.Right: finished = logic.Move(1,0); break;
                case Key.Left: finished = logic.Move(-1,0); break;
                case Key.Down: finished = logic.Move(0,1); break;
            }
            InvalidateVisual();
            if (finished)
            {
                stopwatch.Stop();
                MessageBox.Show("Time" + stopwatch.Elapsed.ToString(@"hh\:mm\:ss\.fff"));
                if (counter < maps.Length)
                {
                    stopwatch.Restart();
                    model = new GameModel(ActualWidth, ActualHeight);
                    logic = new GameLogic(model, maps[counter++]);
                    renderer = new GameRenderer(model);
                }
                else
                    {
                    Window.GetWindow(this).Close();
                    var thankyou = new ThankYouWindow();
                    thankyou.ShowDialog();
                    }
           
            }
        }
    }
}
