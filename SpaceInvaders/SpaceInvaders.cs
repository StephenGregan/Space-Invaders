using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    class GameWindow
    {
        public GameWindow()
        {

        }

        public void Clear()
        {
            Console.SetWindowSize(145, 40);
            Console.Clear();
        }

        public void ClearGame()
        {
            Console.SetWindowSize(145, 40);
            DrawBox(new Position(6, 4), new Position(98, 32), ConsoleColor.Black, ConsoleColor.Black);
        }

        public void DrawText(Position pos, ConsoleColor backColor, ConsoleColor frontColor, string text)
        {
            Console.BackgroundColor = backColor;
            Console.ForegroundColor = frontColor;

            Console.SetCursorPosition(pos.X, pos.Y);
            Console.Write(text);

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void DrawBox(Position pos, Position size, ConsoleColor innerColor, ConsoleColor outerColor)
        {
            Console.BackgroundColor = innerColor;
            Console.ForegroundColor = outerColor;

            for (int y = pos.Y; y < pos.Y; y++)
            {
                Console.SetCursorPosition(pos.X, y);

                if (y == pos.Y || y == (pos.Y + size.Y) - 1)
                {
                    for (int x = pos.X; x < pos.X; x++)
                    {
                        Console.Write("█");
                    }
                }
                else
                {
                    Console.Write("█");
                    Console.SetCursorPosition((pos.X + size.X) -1, y);
                    Console.Write("█");
                }
            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }
    }

    class SpaceInvaders
    {
        public enum GameState
        {
            Menu = 0,
            Game = 1,
            GameOver = 2,
        }

        private List<Alien> _aliens;
        private GameState _state;
        private GameWindow _gameWindow;
        private Player _player;
        private int _maxAliens;
        private Mutex _mutex;

        public Mutex Mutex
        {
            get
            {
                return _mutex;

            
            }
        }

        public GameState State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
            }
        }

        public GameWindow Window
        {
            get
            {
                return _gameWindow;
            }
        }

        public Player Player
        {
            get
            {
                return _player;
            }
        }

        public List<Alien> Aliens
        {
            get
            {
                return _aliens;
            }
        }

        public SpaceInvaders()
        {
            _mutex = new Mutex();
            _aliens = new List<Alien>();
            _gameWindow = new GameWindow();
            _player = new Player();
            _state = 0;
            _maxAliens = 5;

            Window.Clear();
        }

        public void DrawText(Position pos, ConsoleColor backColor, ConsoleColor frontColor, string text)
        {
            Console.BackgroundColor = backColor;
            Console.ForegroundColor = frontColor;

            Console.SetCursorPosition(pos.X, pos.Y);
            Console.Write(text);

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void Update()
        {
            if (State == GameState.Game)
            {
                Player.Update(this);
            }

        }

        public void AlienThread()
        {
            while (true)
            {
                Thread.Sleep(1000);

                Mutex.WaitOne();
                foreach (Alien alien in Aliens)
                {
                    alien.Update(this);
                }
                Mutex.ReleaseMutex();
            }
        }

        public void Intialize()
        {
            AddAliens();
        }

        public void OnConsoleInput(ConsoleKey key)
        {
            if (State == GameState.Menu)
            {
                if (key == ConsoleKey.S)
                {
                    Window.Clear();
                    State = GameState.Game;
                    Intialize();

                    Window.DrawBox(new Position(5, 3), new Position(100, 34), ConsoleColor.Black, ConsoleColor.Green);
                    Window.DrawBox(new Position(106, 3), new Position(30, 6), ConsoleColor.Black, ConsoleColor.Green);

                    Window.DrawText(new Position(107, 4), ConsoleColor.Black, ConsoleColor.Green, "keys:");
                    Window.DrawText(new Position(107, 5), ConsoleColor.Black, ConsoleColor.Green, "[A] - left");
                    Window.DrawText(new Position(107, 6), ConsoleColor.Black, ConsoleColor.Green, "[D] - Right");
                    Window.DrawText(new Position(107, 7), ConsoleColor.Black, ConsoleColor.Green, "[Space] - Shoot");
                }
                else if (key == ConsoleKey.Q)
                {
                    Environment.Exit(0);
                }
            }
            else if (State == GameState.Game)
            {
                if (key == ConsoleKey.Spacebar)
                {
                    Player.Shoot();
                }
                else if (key == ConsoleKey.A)
                {
                    Player.GoLeft();
                }
                else if (key == ConsoleKey.D)
                {
                    Player.GoRight();
                }
            }
        }

        public void AddAliens()
        {
            Mutex.WaitOne();
            Aliens.Add(new Alien(new Random().Next(10, 90)));
            Aliens.Add(new Alien(new Random().Next(10, 90)));
            Aliens.Add(new Alien(new Random().Next(10, 90)));
            Aliens.Add(new Alien(new Random().Next(10, 90)));
            Aliens.Add(new Alien(new Random().Next(10, 90)));
            Mutex.ReleaseMutex();
        }

        public void CheckAliens()
        {
            Mutex.WaitOne();
            int alienCount = Aliens.Count;
            for (int i = alienCount; i < _maxAliens; i++)
            {
                int alien_X = new Random().Next(10, 90);
                Aliens.Add(new Alien(alien_X));
            }
            Mutex.ReleaseMutex();
        }

        public void Draw()
        {
            if (State == GameState.Game)
            {
                Window.ClearGame();
                Player.Draw(this);
                Mutex.WaitOne();

                foreach (Alien alien in _aliens)
                {
                    alien.Draw(this);
                }
                Mutex.ReleaseMutex();
            }
            else if (State == GameState.Menu)
            {
                Window.DrawText(new Position(1, 1), ConsoleColor.Black, ConsoleColor.Green, " ________  ________  ________  ________  _______           ___  ________   ___      ___ ________  ________  _______   ________  ________      ");
                Window.DrawText(new Position(1, 2), ConsoleColor.Black, ConsoleColor.Green, "|\\   ____\\|\\   __  \\|\\   __  \\|\\   ____\\|\\  ___ \\         |\\  \\|\\   ___  \\|\\  \\    /  /|\\   __  \\|\\   ___ \\|\\  ___ \\ |\\   __  \\|\\   ____\\     ");
                Window.DrawText(new Position(1, 3), ConsoleColor.Black, ConsoleColor.Green, "\\ \\  \\___|\\ \\  \\|\\  \\ \\  \\|\\  \\ \\  \\___|\\ \\   __/|        \\ \\  \\ \\  \\\\ \\  \\ \\  \\  /  / | \\  \\|\\  \\ \\  \\_|\\ \\ \\   __/|\\ \\  \\|\\  \\ \\  \\___|_    ");
                Window.DrawText(new Position(1, 4), ConsoleColor.Black, ConsoleColor.Green, " \\ \\_____  \\ \\   ____\\ \\   __  \\ \\  \\    \\ \\  \\_|/__       \\ \\  \\ \\  \\\\ \\  \\ \\  \\/  / / \\ \\   __  \\ \\  \\ \\\\ \\ \\  \\_|/_\\ \\   _  _\\ \\_____  \\   ");
                Window.DrawText(new Position(1, 5), ConsoleColor.Black, ConsoleColor.Green, "  \\|____|\\  \\ \\  \\___|\\ \\  \\ \\  \\ \\  \\____\\ \\  \\_|\\ \\       \\ \\  \\ \\  \\\\ \\  \\ \\    / /   \\ \\  \\ \\  \\ \\  \\_\\\\ \\ \\  \\_|\\ \\ \\  \\\\  \\\\|____|\\  \\  ");
                Window.DrawText(new Position(1, 6), ConsoleColor.Black, ConsoleColor.Green, "    ____\\_\\  \\ \\__\\    \\ \\__\\ \\__\\ \\_______\\ \\_______\\       \\ \\__\\ \\__\\\\ \\__\\ \\__/ /     \\ \\__\\ \\__\\ \\_______\\ \\_______\\ \\__\\\\ _\\ ____\\_\\  \\ ");
                Window.DrawText(new Position(1, 7), ConsoleColor.Black, ConsoleColor.Green, "   |\\_________\\|__|     \\|__|\\|__|\\|_______|\\|_______|        \\|__|\\|__| \\|__|\\|__|/       \\|__|\\|__|\\|_______|\\|_______|\\|__|\\|__|\\_________\\");
                Window.DrawText(new Position(1, 8), ConsoleColor.Black, ConsoleColor.Green, "   \\|_________|                                                                                                                   \\|_________|");

                // Credits
                Window.DrawText(new Position(5, 11), ConsoleColor.Black, ConsoleColor.Green, "Credits: Micky Langeveld & Robin de Bruin");

                // Actions
                Window.DrawText(new Position(5, 13), ConsoleColor.Black, ConsoleColor.Green, "- [S]tart");
                Window.DrawText(new Position(5, 14), ConsoleColor.Black, ConsoleColor.Green, "- [Q]uit");

                // Input line
                Window.DrawText(new Position(0, Console.WindowHeight - 1), ConsoleColor.Black, ConsoleColor.Green, "Please select an option: ");
            }
            // game over state
            else if (State == GameState.GameOver)
            {
                // Game Over
                Window.DrawText(new Position(1, 1), ConsoleColor.Black, ConsoleColor.Red, " ________  ________  _____ ______   _______           ________  ___      ___ _______   ________     ");
                Window.DrawText(new Position(1, 2), ConsoleColor.Black, ConsoleColor.Red, "|\\   ____\\|\\   __  \\|\\   _ \\  _   \\|\\  ___ \\         |\\   __  \\|\\  \\    /  /|\\  ___ \\ |\\   __  \\    ");
                Window.DrawText(new Position(1, 3), ConsoleColor.Black, ConsoleColor.Red, "\\ \\  \\___|\\ \\  \\|\\  \\ \\  \\\\\\__\\ \\  \\ \\   __/|        \\ \\  \\|\\  \\ \\  \\  /  / | \\   __/|\\ \\  \\|\\  \\   ");
                Window.DrawText(new Position(1, 4), ConsoleColor.Black, ConsoleColor.Red, " \\ \\  \\  __\\ \\   __  \\ \\  \\\\|__| \\  \\ \\  \\_|/__       \\ \\  \\\\\\  \\ \\  \\/  / / \\ \\  \\_|/_\\ \\   _  _\\  ");
                Window.DrawText(new Position(1, 5), ConsoleColor.Black, ConsoleColor.Red, "  \\ \\  \\|\\  \\ \\  \\ \\  \\ \\  \\    \\ \\  \\ \\  \\_|\\ \\       \\ \\  \\\\\\  \\ \\    / /   \\ \\  \\_|\\ \\ \\  \\\\  \\| ");
                Window.DrawText(new Position(1, 6), ConsoleColor.Black, ConsoleColor.Red, "   \\ \\_______\\ \\__\\ \\__\\ \\__\\    \\ \\__\\ \\_______\\       \\ \\_______\\ \\__/ /     \\ \\_______\\ \\__\\\\ _\\ ");
                Window.DrawText(new Position(1, 7), ConsoleColor.Black, ConsoleColor.Red, "    \\|_______|\\|__|\\|__|\\|__|     \\|__|\\|_______|        \\|_______|\\|__|/       \\|_______|\\|__|\\|__|");
            }
        }
    }
}
