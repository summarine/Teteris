using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.IO;

namespace Tetris
{
    public class BoxFactory
    {

        public BoxFactory()
        {
            seed = (new Random()).Next();
        }

        public void SetSeed(int s)
        {
            seed = s;
        }

        public Box GetNewBasicBox(GameFrame gf = null)
        {
            Random rand = new Random(seed);
            seed = rand.Next();
            int id = rand.Next(7)+1;
            return GetBoxFromId(id, gf);
        }

        static public Box GetBoxFromId(int id,GameFrame gf = null)
        {
            Box box=null;
            switch (id)
            {
                case 1:
                    box = new Z_Box(gf);
                    break;
                case 2:
                    box = new S_Box(gf);
                    break;
                case 3:
                    box = new T_Box(gf);
                    break;
                case 4:
                    box = new I_Box(gf);
                    break;
                case 5:
                    box = new O_Box(gf);
                    break;
                case 6:
                    box = new L_Box(gf);
                    break;
                case 7:
                    box = new J_Box(gf);
                    break;
            }
            if (box == null)
                box = new I_Box(gf);
            return box;
        }

        static private ImageBrush[] brush = new ImageBrush[20];

        static public ImageBrush GetBoxImageBrush(BoxShape bs)
        {
            if (bs == BoxShape.BLANK || bs == null) return null;
            if ((int)bs < 20)
            {
                if (brush[(int)bs] == null)
                {
                    string path = System.Environment.CurrentDirectory + "/images/BoxUnit_";
                    path += Resources.GetStringfromBoxShape(bs);
                    if (File.Exists(path))
                        brush[(int)bs] = new ImageBrush(new BitmapImage(new Uri(path, UriKind.Absolute)));
                }
                return brush[(int)bs];
            }
            else
                return null;
        }

        private int seed;
    }
}
