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
        /// <summary>
        /// 当准备的方块更新时发出
        /// </summary>
        public event BoxShapeEventHandle RenewReadyBox;
        public event ScoreEventHandle RowsCleanEvent;
        public event BoxEventHandle ActiveBoxMoved;
        /// <summary>
        /// 构造函数，参数为主体网格,以及行数列数
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        public GameFrame(Grid grid, int row, int column)
        {
            boxDropInterval = 500;
            this.grid = grid;
            this.row = row;
            this.column = column;
            State = GameState.Stoped;
            boxNum = 0;

            Hard = 1;

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
                    container.map[i, j].Value = BoxShape.NULL;
                    lbl.SetValue(Grid.RowProperty, i);
                    lbl.SetValue(Grid.ColumnProperty, j);
                    grid.Children.Add(lbl);
                }
            }

            ClearMap();

        }

        /// <summary>
        /// 列属性
        /// </summary>
        public int Column { get { return column; } }
        /// <summary>
        /// 键盘事件响应方法
        /// </summary>
        /// <param name="e"></param>
        virtual public void KeyDown(KeyEventArgs e)
        {
            if (State != GameState.Active) return;
            if (activeBox == null) return;

            switch (e.Key)
            {
                case Key.W:case Key.Up:
                    activeBox.Change();
                    break;
                case Key.A:case Key.Left:
                    activeBox.MoveLeft();
                    break;
                case Key.D:case Key.Right:
                    activeBox.MoveRight();
                    break;
                case Key.S:case Key.Down:
                    activeBox.FastFall();
                    break;
                default:
                    break;
            }
           
        }
        /// <summary>
        /// 清空地图，默认参数为0——清空
        /// </summary>
        /// <param name="v"></param>
        protected void ClearMap(int v = 0)
        {
            for (int i = 0; i < row; i++)
                for (int j = 0; j < column; j++)
                {
                    container.map[i, j].Value = (BoxShape)v;
                }
        }
        /// <summary>
        /// 网格单元是否可用
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool UnitAvilible(int x, int y)
        {
            if (x >= row || x < 0 || y >= column || y < 0)
                return false;
            if (container.map[x, y].Value == BoxShape.NULL || container.map[x,y].Value == BoxShape.SHADOW)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 开始工作
        /// </summary>
        public void Start()
        {
            boxNum = 0;
            ClearMap();

            activeBox = BoxFactory.Instance().GetNewBasicBox(this);
            readyBox = BoxFactory.Instance().GetNewBasicBox(this);
            if (RenewReadyBox != null)
            {
                RenewReadyBox(this, new BoxShapeEventArgs(readyBox.shape));
            }
            State = GameState.Active;
            if (!activeBox.Act())
            {
                GameOver();
            }

        }
        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause()
        {
            State = GameState.Paused;
            activeBox.Pause();
        }
        /// <summary>
        /// 从暂停状态开始
        /// </summary>
        public void Continue()
        {
            State = GameState.Active;
            activeBox.Continue();
        }
        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            State = GameState.Stoped;
            activeBox.Stop();
            activeBox = null;
            readyBox = null;
            if (RenewReadyBox != null)
            {
                RenewReadyBox(this, null);
            }
        }
        /// <summary>
        /// 游戏结束
        /// </summary>
        public void GameOver()
        {
            ClearMap(999);
            Stop();
            //score.??
        }
        /// <summary>
        /// 清除行，并使其上方块下落
        /// </summary>
        /// <param name="list"></param>
        protected void CleanLines(List<int> list)
        {
            //朴素实现，有空来优化
            int r = list.Count();
            int i, j, k, l, t = r;
            for (i = 0; i < r; i++)
            {
                l = list[i]+i;
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
        /// <summary>
        /// 检查放满的行
        /// </summary>
        /// <returns></returns>
        protected int CheckFullLines()
        {
            List<int> cl = new List<int>();
            //保证cl中的行从下到上
            //从第三行开始算
            for (int i = row - 1; i > 2; i--)
            {
                int j = 0;
                while (j < column && container.map[i, j].Value != BoxShape.NULL && container.map[i,j].Value!=BoxShape.BAN)
                    j++;
                if (j == column)
                {
                    cl.Add(i);
                }
            }
            if (cl.Count != 0)
            {
                CleanLines(cl);
                if (RowsCleanEvent != null)
                    RowsCleanEvent(this, new ScoreEventArgs(cl.Count));
            }
            return cl.Count();
        }
        /// <summary>
        /// 当前方块下落到底部的响应方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ActiveBoxCrush(Object sender, EventArgs e)
        {
            //当前方块掉落到底部
            //消行
            int l = CheckFullLines();

            //更新使用的方块
            activeBox.Stop();
            activeBox = readyBox;
            readyBox = null;

            bool bOK = true;

            if (activeBox != null)
                bOK = activeBox.Act();
            else bOK = false;

            if (bOK)
            {
                readyBox = BoxFactory.Instance().GetNewBasicBox(this);
                if (RenewReadyBox != null)
                {
                    RenewReadyBox(this, new BoxShapeEventArgs(readyBox.shape));
                }
            }
            else
            {
                GameOver();
            }
        }
        /// <summary>
        /// 活动方块位置改变响应方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MapChanged(Object sender, MoveEventArgs e)
        {
            ClearGrid(e.period);
            UpdateGrid(e.next);

            if (sender == activeBox)
            {
                if (ActiveBoxMoved != null)
                    ActiveBoxMoved(this,new BoxEventArgs(activeBox));
            }
        }
        /// <summary>
        /// 更新网格
        /// </summary>
        /// <param name="list"></param>
        protected void UpdateGrid(List<Square> list)
        {
            if (list == null) return;
            Position p;
            for (int i = 0; i < list.Count; i++)
            {
                p = list[i].pos;
                container.map[p.x, p.y].Value = list[i].value;
            }
        }
        /// <summary>
        /// 清除网格
        /// </summary>
        /// <param name="list"></param>
        protected void ClearGrid(List<Square> list)
        {
            if (list == null) return;
            Position p;
            for (int i = 0; i < list.Count; i++)
            {
                p = list[i].pos;
                container.map[p.x, p.y].Value = BoxShape.NULL;
            }
        }

        public Container container;
        public Box activeBox;
        public Box readyBox;
        
        public GameState State 
        { 
            get { return state; }
            private set
            {
                state = value;
                
            }
        }
        protected GameState state;
        /// <summary>
        /// set:输入的是每秒掉落多少格
        /// get:输出的是掉落一格用多少毫微秒
        /// </summary>
        public double Hard
        {
            get { return boxDropInterval; }
            set
            {
                double t = value;
                t = 1e7 / t;
                boxDropInterval = (int)t;
            }
        }

        public int Row
        {
            get { return row; }
        }

        protected int boxDropInterval;
        protected int boxNum;
        protected readonly Grid grid;
        protected readonly int row;
        protected readonly int column;
    }
}
