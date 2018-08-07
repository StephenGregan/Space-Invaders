using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    class Alien
    {
        private Position _pos;
        public Position Position
        {
            get
            {
                return _pos;
            }
        }

        public Alien(int startX)
        {
            _pos = new Position(startX, 10);
        }

        public void Update(SpaceInvaders game)
        {
            Position.Y += 1;
        }

        public void Draw(SpaceInvaders game)
        {
            game.Window.DrawText(Position, ConsoleColor.Black, ConsoleColor.Red, "███");
        }
    }
}
