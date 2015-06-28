using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Tetris
{
    public class Container
    {
        public Container(int r,int c)
        {
            map = new GridData[r,c];
        }

        public GridData[,] map;

        public void setMapping(int row,int col,Label label)
        {
            
        }
    }

    public class GridData
    {
        public Label Lbl
        {
            get
            {
                return label;
            }
            set
            {
                label = value;
            }
        }
        public BoxShape Value
        {
            set
            {
                if (value<0) return;
                this.value = value;

                Convert(this.value);
            }
            get
            {
                return this.value;
            }
        }

        private void Convert(BoxShape bs)
        {
            label.Background = BoxFactory.Instance().GetBoxImageBrush(bs);
        }

        private BoxShape value;
        private Label label;
    }
}
