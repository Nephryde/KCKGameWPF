using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace KCKGameWPF
{
    public partial class MainWindow : Window
    {
        static readonly int left = 4;
        static readonly int right = 6;
        static readonly int up = 8;
        static readonly int down = 2;


        static int firstPlayerScore = 0;
        static int firstPlayerDirection = right;
        private List<Point> snake1 = new List<Point>();
        private readonly Brush snake1Color = Brushes.Green;

        private readonly Point startingPoint = new Point(5, 225);
        private Point firstPlayerPosition = new Point();


        static int secondPlayerScore = 0;
        static int secondPlayerDirection = left;
        private List<Point> snake2 = new List<Point>();
        private readonly Brush snake2Color = Brushes.Red;

        private readonly Point startingPoint2 = new Point(775, 225);
        private Point secondPlayerPosition = new Point();


        private enum SnakeSize
        {
            Thin = 4,
            Normal = 6,
            Thick = 8
        };

        //TimeSpan values
        private enum GameSpeed
        {
            Fast = 1,
            Moderate = 10000,
            Slow = 50000,
            DamnSlow = 500000
        };

        /* Here user can change the size of the snake. 
         * Possible sizes are THIN, NORMAL and THICK */
        private readonly int headSize = (int)SnakeSize.Thick;

        public MainWindow()
        {
            InitializeComponent();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(TimerTick);

            /* Here user can change the speed of the snake. 
             * Possible speeds are FAST, MODERATE, SLOW and DAMNSLOW */
            timer.Interval = new TimeSpan((int)GameSpeed.Moderate);
            timer.Start();

            this.KeyDown += new KeyEventHandler(ChangePlayerDirection);

            WriteOnPosition(ref snake1, startingPoint, snake1Color);
            firstPlayerPosition = startingPoint;

            WriteOnPosition(ref snake2, startingPoint2, snake2Color);
            secondPlayerPosition = startingPoint2;
        }

        private void TimerTick(object sender, EventArgs e)
        {
            MovePlayers();

            if(DoesPlayerLose())
            {
                GameOver();
            }

        }

        private void GameOver()
        {
            MessageBox.Show($@"You Lose!", "Game Over", MessageBoxButton.OK, MessageBoxImage.Hand);
            this.Close();
        }

        ////
        // Nowe
        ////

        private void ChangePlayerDirection(object sender, KeyEventArgs key)
        {
            if (key.Key == Key.W && firstPlayerDirection != down)
            {
                firstPlayerDirection = up;
            }
            if (key.Key == Key.A && firstPlayerDirection != right)
            {
                firstPlayerDirection = left;
            }
            if (key.Key == Key.D && firstPlayerDirection != left)
            {
                firstPlayerDirection = right;
            }
            if (key.Key == Key.S && firstPlayerDirection != up)
            {
                firstPlayerDirection = down;
            }

            if (key.Key == Key.Up && secondPlayerDirection != down)
            {
                secondPlayerDirection = up;
            }
            if (key.Key == Key.Left && secondPlayerDirection != right)
            {
                secondPlayerDirection = left;
            }
            if (key.Key == Key.Right && secondPlayerDirection != left)
            {
                secondPlayerDirection = right;
            }
            if (key.Key == Key.Down && secondPlayerDirection != up)
            {
                secondPlayerDirection = down;
            }
        }

        private void MovePlayers()
        {
            if (firstPlayerDirection == right)
            {
                firstPlayerPosition.X += 1;
                WriteOnPosition(ref snake1, firstPlayerPosition, snake1Color);
            }
            if (firstPlayerDirection == left)
            {
                firstPlayerPosition.X -= 1;
                WriteOnPosition(ref snake1, firstPlayerPosition, snake1Color);
            }
            if (firstPlayerDirection == up)
            {
                firstPlayerPosition.Y -= 1;
                WriteOnPosition(ref snake1, firstPlayerPosition, snake1Color);
            }
            if (firstPlayerDirection == down)
            {
                firstPlayerPosition.Y += 1;
                WriteOnPosition(ref snake1, firstPlayerPosition, snake1Color);
            }

            if (secondPlayerDirection == right)
            {
                secondPlayerPosition.X += 1;
                WriteOnPosition(ref snake2, secondPlayerPosition, snake2Color);
            }
            if (secondPlayerDirection == left)
            {
                secondPlayerPosition.X -= 1;
                WriteOnPosition(ref snake2, secondPlayerPosition, snake2Color);
            }
            if (secondPlayerDirection == up)
            {
                secondPlayerPosition.Y -= 1;
                WriteOnPosition(ref snake2, secondPlayerPosition, snake2Color);
            }
            if (secondPlayerDirection == down)
            {
                secondPlayerPosition.Y += 1;
                WriteOnPosition(ref snake2, secondPlayerPosition, snake2Color);
            }
        }

        private bool DoesPlayerLose()
        {
            // Restrict to boundaries of the Canvas
            if ((firstPlayerPosition.X < 5) || (firstPlayerPosition.X > 780) ||
                (firstPlayerPosition.Y < 5) || (firstPlayerPosition.Y > 480))
                return true;

            // Restrict to boundaries of the Canvas
            if ((secondPlayerPosition.X < 5) || (secondPlayerPosition.X > 780) ||
                (secondPlayerPosition.Y < 5) || (secondPlayerPosition.Y > 480))
                return true;

            // Restrict hits to body of Snake
            for (int q = 0; q < (snake1.Count - headSize * 2); q++)
            {
                Point point = new Point(snake1[q].X, snake1[q].Y);
                if ((Math.Abs(point.X - firstPlayerPosition.X) < (headSize)) &&
                     (Math.Abs(point.Y - firstPlayerPosition.Y) < (headSize)))
                {
                    return true;
                }
            }

            for (int y = 0; y < (snake2.Count - headSize * 2); y++)
            {
                Point point = new Point(snake2[y].X, snake2[y].Y);
                if ((Math.Abs(point.X - secondPlayerPosition.X) < (headSize)) &&
                     (Math.Abs(point.Y - secondPlayerPosition.Y) < (headSize)))
                {
                    return true;
                }
            }

            return false;
        }

        private void WriteOnPosition(ref List<Point> snake, Point currentposition, Brush color)
        {
            Ellipse newEllipse = new Ellipse
            {
                Fill = color,
                Width = headSize,
                Height = headSize
            };

            Canvas.SetTop(newEllipse, currentposition.Y);
            Canvas.SetLeft(newEllipse, currentposition.X);

            int count = PaintCanvas.Children.Count;
            PaintCanvas.Children.Add(newEllipse);
            snake.Add(currentposition);
        }

    }
}
