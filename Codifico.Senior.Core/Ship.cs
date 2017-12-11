using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codifico.Senior.Core
{
    public enum Ship
    {
        SubMarino = 1,
        Desctructor = 2,
        Crucero = 3,
        Acorsado = 4
    }
    public class CreateShip
    {
        private Ship _ship;
        private Tuple<int, int> initialPosicion;
        private Tuple<int, int> finalPosicion;

        public CreateShip(Ship navy, int MaxPosition)
        {
            this._ship = navy;

            Random rnd = new Random();
            bool vertical = false;
            if (rnd.Next(0, MaxPosition) % 2 == 0)
                vertical = true;

            NewRnd:
            int pox = rnd.Next(0, MaxPosition);
            int poy = rnd.Next(0, MaxPosition);

            initialPosicion = new Tuple<int, int>(pox, poy);


            if (vertical)
            {
                if ((initialPosicion.Item2 + getLengthShip()) < MaxPosition)
                    finalPosicion = new Tuple<int, int>(pox, poy + getLengthShip());
                else
                    goto NewRnd;
            }
            else
            {
                if ((initialPosicion.Item1 + getLengthShip()) < MaxPosition)
                    finalPosicion = new Tuple<int, int>(pox + getLengthShip(), poy);
                else
                    goto NewRnd;
            }
        }
        public int getLengthShip()
        {
            int length = 0;
            switch (this._ship)
            {
                case Ship.SubMarino:
                    length = 1;
                    break;
                case Ship.Desctructor:
                    length = 2;
                    break;
                case Ship.Crucero:
                    length = 3;
                    break;
                case Ship.Acorsado:
                    length = 4;
                    break;
                default:
                    break;
            }
            return length;
        }
        public Tuple<int, int> getInitialPosition()
        {
            return initialPosicion;
        }
        public Tuple<int, int> getFinalPosition()
        {
            return finalPosicion;
        }
    }
}
