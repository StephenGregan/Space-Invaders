using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    class Program
    {
        private static SpaceInvaders game;

        static void InputThread()
        {
            while (true)
            {
                game.OnConsoleInput(Console.ReadKey().Key);
            }
        }
        static void Main(string[] args)
        {
            game = new SpaceInvaders();
            Thread inputThread = new Thread(InputThread);
            inputThread.Start();

            while (true)
            {
                Thread.Sleep(200);
                game.Update();
                game.Draw();
            }
        }
    }
}
