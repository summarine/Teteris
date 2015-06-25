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
            this.grid = grid;
            this.row = row;
            this.column = column;
            isPlaying = false;

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

            container = new Container(row,column);

            for (int i = 0; i < row; i++) 
            {
                for (int j = 0; j < column; j++) 
                {
                    container.map[i,j].La
                }
            }

                ClearMap();

        }

        private readonly Grid grid;
        private readonly int row;
        private readonly int column;

        public int Column { get { return column; } }

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

        private void ClearMap()
        {
            for (int i = 0; i < row; i++)
                for (int j = 0; j < column; j++)
                {
                    map[i][j] = 0;
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
            ClearMap();

            activeBox = BoxFactory.GetNewBasicBox(this);
            readyBox = BoxFactory.GetNewBasicBox(this);

            isPlaying = true;
            activeBox.Act();
            
        }

        public void Pause()
        {
            activeBox.Pause();
            isPlaying = false;
        }

        public void Continue()
        {
            activeBox.Continue();
            isPlaying = true;
        }

        public void Stop()
        {
            isPlaying = false;
            activeBox = null;
            readyBox = null;
        }

        public void GameOver()
        {
            Stop();
            //score.??
        }

        private void CleanLines(List<int> list)
        {
            int r = list.Count();
            for (int i=0;i<r;i++)
            {
                for (int j=0;j<column;j++)
                {
                    if (list[i] - r >= 0)
                        map[i][j] = map[i - r][j];
                    else
                        map[i][j] = 0;
                }
            }
        }

        private int CheckFullLines()
        {
            List<int> cl = new List<int>();
            for (int i=0;i<row;i++)
            {
                int j = 0;
                while (j < column && map[i][j] != 0)
                    j++;
                if (j==column)
                {
                    cl.Add(i);
                }
            }
            CleanLines(cl);
            return cl.Count();
        }

        public void ActiveBoxCrush(Object sender,EventArgs e)
        {
            //当前方块掉落到底部
                //消行
            int l = CheckFullLines();

            //更新使用的方块
            activeBox = readyBox;
            readyBox = null;
            if (!activeBox.Act())
            {
                GameOver();
            }
            readyBox = new Box();
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

        public Container container;
        private int[][] map;
        public int boxDropInterval;
        public Box activeBox;
        public Box readyBox;
        public bool IsPlaying { get { return isPlaying; } }
        private bool isPlaying;

    }
}
