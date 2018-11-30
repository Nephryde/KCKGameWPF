using System;
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
        private Point firstPlayerPosition = new Point();
        private readonly Brush snake1Color = Brushes.Green;

        static int secondPlayerScore = 0;
        static int secondPlayerDirection = left;
        private Point secondPlayerPosition = new Point();
        private readonly Brush snake2Color = Brushes.Red;

        private Point startingPoint = new Point(5, 225);
        private Point startingPoint2 = new Point(775, 225);
        

        static bool[,] isUsed;


        private enum SnakeSize
        {
            Thin = 4,
            Normal = 6,
            Thick = 8
        };

        private enum GameSpeed
        {
            Fast = 1,
            Moderate = 10000,
            Slow = 50000,
            DamnSlow = 500000
        };

        private readonly int headSize = (int)SnakeSize.Normal;

        public MainWindow()
        {
            InitializeComponent();

            SetGameField();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(TimerTick);

            timer.Interval = new TimeSpan((int)GameSpeed.Moderate);
            timer.Start();

            this.KeyDown += new KeyEventHandler(ChangePlayerDirection);

            WriteOnPosition(startingPoint, snake1Color);
            firstPlayerPosition = startingPoint;

            WriteOnPosition(startingPoint2, snake2Color);
            secondPlayerPosition = startingPoint2;

            isUsed[(int)firstPlayerPosition.X, (int)firstPlayerPosition.Y] = true;
            isUsed[(int)secondPlayerPosition.X, (int)secondPlayerPosition.Y] = true;
        }

        private void TimerTick(object sender, EventArgs e)
        {
            MovePlayers();

            bool firstPlayerLoses = DoesPlayerLose(firstPlayerPosition);
            bool secondPlayerLoses = DoesPlayerLose(secondPlayerPosition);

            if (firstPlayerLoses || secondPlayerLoses)
            {
                GameOver();
            }

            isUsed[(int)firstPlayerPosition.X, (int)firstPlayerPosition.Y] = true;
            isUsed[(int)secondPlayerPosition.X, (int)secondPlayerPosition.Y] = true;

            WriteOnPosition(firstPlayerPosition, snake1Color);
            WriteOnPosition(secondPlayerPosition, snake2Color);
        }

        private void GameOver()
        {
            MessageBox.Show($@"You Lose!", "Game Over", MessageBoxButton.OK, MessageBoxImage.Hand);
            this.Close();
        }

        ////
        // Nowe
        ////

        private void SetGameField()
        {
            isUsed = new bool[800, 500];

            startingPoint = new Point(5, 225);
            startingPoint2 = new Point(775, 225);
        }

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
            }
            if (firstPlayerDirection == left)
            {
                firstPlayerPosition.X -= 1;
            }
            if (firstPlayerDirection == up)
            {
                firstPlayerPosition.Y -= 1;
            }
            if (firstPlayerDirection == down)
            {
                firstPlayerPosition.Y += 1;
            }

            if (secondPlayerDirection == right)
            {
                secondPlayerPosition.X += 1;
            }
            if (secondPlayerDirection == left)
            {
                secondPlayerPosition.X -= 1;
            }
            if (secondPlayerDirection == up)
            {
                secondPlayerPosition.Y -= 1;
            }
            if (secondPlayerDirection == down)
            {
                secondPlayerPosition.Y += 1;
            }
        }

        private bool DoesPlayerLose(Point playerPosition)
        {
            if ((playerPosition.X < 5) || (playerPosition.X > 780) ||
                (playerPosition.Y < 5) || (playerPosition.Y > 480))
                return true;

            if(isUsed[(int)playerPosition.X, (int)playerPosition.Y])
                return true;

            return false;
        }

        private void WriteOnPosition(Point currentposition, Brush color)
        {
            Ellipse newEllipse = new Ellipse
            {
                Fill = color,
                Width = headSize,
                Height = headSize
            };

            Canvas.SetTop(newEllipse, currentposition.Y);
            Canvas.SetLeft(newEllipse, currentposition.X);

            PaintCanvas.Children.Add(newEllipse);
        }
    }
}
