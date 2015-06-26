using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class BoxEventArgs : EventArgs
    {
        public BoxShape box;

        public BoxEventArgs(BoxShape box)
        {
            this.box = box;
        }
    }

    public delegate void BoxEventHandle(Object sender,BoxEventArgs e);
}
