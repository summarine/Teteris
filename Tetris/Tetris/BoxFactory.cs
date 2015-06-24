using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class BoxFactory
    {
        static public Box GetNewBasicBox(GameFrame gf)
        {
            Box box = null;
            Random rand = new Random();
            int id = rand.Next(7);
            switch (id)
            {
                case 0: box = new L_Box(gf);
                    break;
                case 1: box = new J_Box(gf);
                    break;
                case 2: box = new I_Box(gf);
                    break;
                case 3: box = new Z_Box(gf);
                    break;
                case 4: box = new S_Box(gf);
                    break;
                case 5: box = new O_Box(gf);
                    break;
                case 6: box = new T_Box(gf);
                    break;
            }
            if (box == null)
                box = new I_Box(gf);
            return box;
        }
    }
}
