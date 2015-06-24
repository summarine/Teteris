using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class BoxFactory
    {
        public Box GetNewBasicBox()
        {
            Box box = null;
            Random rand = new Random();
            int id = rand.Next(7);
            switch (id)
            {
                case 0: box = new L_Box();
                    break;
                case 1: box = new J_Box();
                    break;
                case 2: box = new I_Box();
                    break;
                case 3: box = new Z_Box();
                    break;
                case 4: box = new S_Box();
                    break;
                case 5: box = new O_Box();
                    break;
                case 6: box = new T_Box();
                    break;
            }
            if (box != null)
                return box;
            else
                return new I_Box();
        }
    }
}
