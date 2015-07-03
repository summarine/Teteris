using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Tetris
{
    class MyGameFrame : GameFrame
    {

        public MyGameFrame(Grid gameGrid, int p1, int p2) : base(gameGrid,p1,p2)
        {
            shadow = new BoxShadow(this);
            ActiveBoxMoved += shadow.Reflect;
        }

        public override void add_boxNum(int v = 1)
        {
            base.add_boxNum(v);
            Random r = new Random();
            int c = r.Next(10);

        }

        private BoxShadow shadow;
    }
}
