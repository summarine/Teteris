using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Tetris
{
    class GameFrame
    {
        public GameFrame(Grid grid,int row,int column)
        {
            boxDropInterval = 500;
            box_Timer = new DispatcherTimer();
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
                    map[i][j] = 0;
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

        public bool UnitAvilible(int x,int y)
        {
            if (x >= row || x < 0 || y >= column || y < 0)
                return false;
            if (map[x][y]==0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Start()
        {
            activeBox = BoxFactory.GetNewBasicBox(this);
            readyBox = BoxFactory.GetNewBasicBox(this);

            box_Timer.Interval = new TimeSpan(boxDropInterval);
            box_Timer.Tick += new EventHandler(BoxTimerTick);
            box_Timer.Start();
            
        }

        public void BoxTimerTick(Object sender,EventArgs e)
        {
            if (!activeBox.MoveDown())
            {
                activeBox = readyBox;
                readyBox = new Box();
            }
        }

        public void MapChanged(Object sender,MoveEventArgs e)
        {
            ClearGrid(e.period);
            UpdateGrid(e.next);
        }

        private void UpdateGrid(List<Square> list)
        {
            Position p;
            for (int i = 0; i < list.Count; i++)
            {
                p = list[i].pos;
                map[p.x][p.y] = list[i].value;
            }
        }

        private void ClearGrid(List<Square> list)
        {
            Position p;
            for (int i=0;i<list.Count;i++)
            {
                p=list[i].pos;
                map[p.x][p.y] = 0;
            }
        }

        public int[][] map;
        public int boxDropInterval;
        public Box activeBox;
        public Box readyBox;
        private DispatcherTimer box_Timer;
    }
}
