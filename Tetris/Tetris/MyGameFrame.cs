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

        private BoxShadow shadow;
    }
}
