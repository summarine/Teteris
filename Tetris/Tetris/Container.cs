using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Tetris
{
    class Container
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

    class GridData
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
        public int Value
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

        private void Convert(int v)
        {
            if (v == 0)
                label.Background = new SolidColorBrush(Colors.Aqua);
            else
                label.Background = new SolidColorBrush(Colors.Yellow);
        }

        private int value;
        private Label label;
    }
}
