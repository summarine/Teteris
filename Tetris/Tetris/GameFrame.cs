using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows.Media;

namespace Tetris
{
    public class GameFrame
    {
        public BoxEventHandle newReadyBox;
        public GameFrame(Grid grid, int row, int column)
        {
            boxDropInterval = 500;
            this.grid = grid;
            this.row = row;
            this.column = column;
            isPlaying = false;

            for (int i = 0; i < column; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int i = 0; i < row; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition());
            }

            container = new Container(row, column);

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    Label lbl = new Label();
                    container.map[i, j] = new GridData();
                    container.map[i, j].Lbl = lbl;
                    container.map[i, j].Value = 0;
                    lbl.SetValue(Grid.RowProperty, i);
                    lbl.SetValue(Grid.ColumnProperty, j);
                    grid.Children.Add(lbl);
                }
            }

            ClearMap();

        }


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

        private void ClearMap(int v = 0)
        {
            for (int i = 0; i < row; i++)
                for (int j = 0; j < column; j++)
                {
                    container.map[i, j].Value = v;
                }
        }

        public bool UnitAvilible(int x, int y)
        {
            if (x >= row || x < 0 || y >= column || y < 0)
                return false;
            if (container.map[x, y].Value == 0)
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
            if (newReadyBox != null)
            {
                newReadyBox(this, new BoxEventArgs(readyBox.shape));
            }

            isPlaying = true;
            if (!activeBox.Act())
            {
                GameOver();
            }

        }

        public void Pause()
        {
            activeBox.Pause();
        }

        public void Continue()
        {
            activeBox.Continue();
        }

        public void Stop()
        {
            isPlaying = false;
            activeBox.Stop();
            activeBox = null;
            readyBox = null;
            if (newReadyBox != null)
            {
                newReadyBox(this, new BoxEventArgs(readyBox.shape));
            }
        }

        public void GameOver()
        {
            ClearMap(1);
            Stop();
            //score.??
        }

        private void CleanLines(List<int> list)
        {
            //朴素实现，有空来优化
            int r = list.Count();
            int i, j, k, l, t = r;
            for (i = 0; i < r; i++)
            {
                l = list[i];
                for (k = l; k >= 1; k--)
                {
                    for (j = 0; j < column; j++)
                    {
                        container.map[k, j].Value = container.map[k - 1, j].Value;
                    }
                }
                for (; k >= 0; k--)
                {
                    for (j = 0; j < column; j++)
                    {
                        container.map[k, j].Value = 0;
                    }
                }
            }
        }

        private int CheckFullLines()
        {
            List<int> cl = new List<int>();
            //保证cl中的行从下到上
            //从第三行开始算
            for (int i = row - 1; i > 2; i--)
            {
                int j = 0;
                while (j < column && container.map[i, j].Value != 0)
                    j++;
                if (j == column)
                {
                    cl.Add(i);
                }
            }
            if (cl.Count != 0)
                CleanLines(cl);
            return cl.Count();
        }

        public void ActiveBoxCrush(Object sender, EventArgs e)
        {
            //当前方块掉落到底部
            //消行
            int l = CheckFullLines();

            //更新使用的方块
            activeBox = readyBox;
            readyBox = null;

            bool bOK = true;

            if (activeBox != null)
                bOK = activeBox.Act();
            else bOK = false;

            if (bOK)
            {
                readyBox = BoxFactory.GetNewBasicBox(this);
                if (newReadyBox != null)
                {
                    newReadyBox(this, new BoxEventArgs(readyBox.shape));
                }
            }
            else
            {
                GameOver();
            }
        }

        public void MapChanged(Object sender, MoveEventArgs e)
        {
            ClearGrid(e.period);
            UpdateGrid(e.next);
        }

        private void UpdateGrid(List<Square> list)
        {
            if (list == null) return;
            Position p;
            for (int i = 0; i < list.Count; i++)
            {
                p = list[i].pos;
                container.map[p.x, p.y].Value = list[i].value;
            }
        }

        private void ClearGrid(List<Square> list)
        {
            if (list == null) return;
            Position p;
            for (int i = 0; i < list.Count; i++)
            {
                p = list[i].pos;
                container.map[p.x, p.y].Value = 0;
            }
        }

        public Container container;
        public int boxDropInterval;
        public Box activeBox;
        public Box readyBox;
        public bool IsPlaying { get { return isPlaying; } }
        
        private bool isPlaying;
        private readonly Grid grid;
        private readonly int row;
        private readonly int column;
    }
}
