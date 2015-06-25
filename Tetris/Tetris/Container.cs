using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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
        public Label Lbl { get; set; }
        public int Value { get; set; }
    }
}
