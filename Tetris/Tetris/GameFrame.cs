using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Tetris
{
    class GameFrame
    {
        public GameFrame(Grid grid,int row,int column)
        {
            this.grid = grid;
            this.row = row;
            this.column = column;

            for (int i = 0; i < row; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int i = 0; i < column; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition());
            }

            map = new int[row][];
            for (int i = 0; i < row; i++)
                map[i] = new int[column];
            
            for (int i=0;i<row;i++)
                for (int j=0;j<column;j++)
                {
                     
                }

        }

        private readonly Grid grid;
        private readonly int row;
        private readonly int column;

        internal void KeyDown(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                    activeBox.Change();
                    break;
                case Key.A:
                    activeBox.MoveLeft();
                    break;
                case Key.D:
                    activeBox.MoveRight();
                    break;
                case Key.S:
                    activeBox.FastFall();
                    break;
                default:
                    break;
            }
        }

        public bool 

        public int[][] map;
        public Box activeBox; 

    }
}
