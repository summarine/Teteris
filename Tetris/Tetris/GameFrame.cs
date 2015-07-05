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
        public event BoxShapeEventHandler RenewReadyBox;
        public event BoxShapeEventHandler RenewActiveBox;
        /// <summary>
        /// 行被消除时触发
        /// </summary>
        public event RowEventHandler RowsCleanEvent;
        /// <summary>
        /// 活动方块移动是发生,包含新方块的实例
        /// </summary>
        public event BoxEventHandler ActiveBoxMoved;
        /// <summary>
        /// 游戏结束时触发
        /// </summary>
        public event EventHandler GameOverEvent;
        /// <summary>
        /// 活动方块移动时发生,包含前后的位置信息
        /// </summary>
        public event MoveEventHandler ActiveBoxMoving;

        public event MoveEventHandler ActiveBoxCrushEvent;
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
            boxFactory = new BoxFactory();

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
        /// 增加无用的干扰行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OtherCrossThreshold(object sender, ScoreEventArgs e)
        {
            for (int i = 0; i < e.value; i++)
                AddUselessLines();
        }
        public void AddUselessLines(List<int> uselessRow = null)
        {
            if (uselessRow == null)
            {
                int holesCnt = 4;
                Random rand = new Random();
                uselessRow = new List<int>();
                for (int i = 0; i < column; i++)
                    uselessRow.Add(1);
                for (int i = 0; i < holesCnt; i++)
                {
                    int t = rand.Next(column);
                    uselessRow[t] = 0;
                }
            }
            if (activeBox != null)
                for (int i = 0; i < 4; i++)
                {
                    int tx = activeBox.Entity[i].pos.x;
                    int ty = activeBox.Entity[i].pos.y;
                    bool b = true;
                    if (tx != row - 1)
                    {
                        if (!UnitAvilible(tx + 1, ty))
                        {
                            b = activeBox.MoveUp();
                        }
                    }
                    else
                    {
                        if (uselessRow[ty] == 1)
                        {
                            b = activeBox.MoveUp();
                        }
                    }
                    if (!b)
                    {
                        GameOver();
                    }
                }
            for (int r = 0; r < row - 1; r++)
            {
                for (int c = 0; c < column; c++)
                {
                    if (r == 0)
                    {
                        if (!UnitAvilible(r, c))
                        {
                            GameOver();
                        }
                    }
                    BoxShape bs = container.map[r + 1, c].Value;
                    container.map[r, c].Value = bs;
                }
            }

            for (int c = 0; c < column; c++)
            {
                if (uselessRow[c] == 1)
                    container.map[row - 1, c].Value = BoxShape.OBINARY;
                else
                    container.map[row - 1, c].Value = BoxShape.NULL;
            }
            if (activeBox!=null)
                ActiveBoxPositionChanged(null, new MoveEventArgs(null, activeBox.Entity));
        }
        /// <summary>
        /// 列属性
        /// </summary>
        public int Column { get { return column; } }
        /// <summary>
        /// 键盘事件响应方法
        /// </summary>
        /// <param name="e"></param>
        virtual public void KeyDown(Key key)
        {
            if (State != GameState.Active) return;
            if (activeBox == null) return;

            switch (key)
            {
                case Key.W:
                case Key.Up:
                    activeBox.Change();
                    break;
                case Key.A:
                case Key.Left:
                    activeBox.MoveLeft();
                    break;
                case Key.D:
                case Key.Right:
                    activeBox.MoveRight();
                    break;
                case Key.S:
                case Key.Down:
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
        public void ClearMap(int v = 0)
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
            //if (activeBox.UnitInBox(x, y))
            //    return true;
            if (container.map[x, y].Value == BoxShape.NULL)
            {
                return true;
            }
            return false;
        }
        public bool UnitIsEmpty(int x, int y)
        {
            if (x >= row || x < 0 || y >= column || y < 0)
                return false;
            if (container.map[x, y].Value == BoxShape.NULL || container.map[x, y].Value == BoxShape.SHADOW)
            {
                return true;
            }
            return false;
        }

        public void SetFactorySeed(int s)
        {
            boxFactory.SetSeed(s);
        }
        /// <summary>
        /// 开始工作
        /// </summary>
        virtual public void Start()
        {
            Hard = 1;
            boxNum = 1;
            ClearMap();
            State = GameState.Active;

            activeBox = boxFactory.GetNewBasicBox(this);
            activeBox.move += this.ActiveBoxPositionChanged;
            readyBox = boxFactory.GetNewBasicBox(this);
            if (RenewReadyBox != null)
            {
                RenewReadyBox(this, new BoxShapeEventArgs(readyBox.shape));
            }

            if (!activeBox.Act())
            {
                GameOver();
            }

            if (RenewActiveBox != null)
            {
                RenewActiveBox(this, null);
            }

        }
        /// <summary>
        /// 暂停
        /// </summary>
        virtual public void Pause()
        {
            State = GameState.Paused;
            activeBox.Pause();
        }
        /// <summary>
        /// 从暂停状态开始
        /// </summary>
        virtual public void Continue()
        {
            State = GameState.Active;
            activeBox.Continue();
        }
        /// <summary>
        /// 停止
        /// </summary>
        virtual public void Stop()
        {
            State = GameState.Stoped;
            boxNum = 0;
            if (activeBox != null)
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
            //ClearMap(999);
            if (GameOverEvent != null)
            {
                GameOverEvent(this, null);
            }
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
            if (list == null || list.Count == 0)
                return;
            int fillRow = row - 1;
            int n = 0;
            for (int r = row - 1; r >= 0; r--)
            {
                if (n < list.Count && r == list[n])
                {
                    n++;
                }
                else
                {
                    if (fillRow > r)
                    {
                        for (int c = 0; c < column; c++)
                        {
                            container.map[fillRow, c].Value = container.map[r, c].Value;
                        }
                    }
                    fillRow--;
                }
            }
            for (int r = fillRow; r >= 0; r--)
            {
                for (int c = 0; c < column; c++)
                    container.map[r, c].Value = BoxShape.NULL;
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
                while (j < column && container.map[i, j].Value != BoxShape.NULL && container.map[i, j].Value != BoxShape.BAN)
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
                    RowsCleanEvent(this, new RowEventArgs(cl));
            }
            return cl.Count();
        }
        /// <summary>
        /// 用于局域网的沉底时间响应
        /// </summary>
        /// <param name="netPlayer"></param>
        /// <param name="list"></param>
        public void ActiveBoxCrush(Object sender, List<Square> list)
        {
            //绘制当前活动方块
            int tx, ty;
            for (int i = 0; i < 4; i++)
            {
                tx = list[i].pos.x;
                ty = list[i].pos.y;
                container.map[tx, ty].Value = list[i].value;
            }

            //当前方块掉落到底部
            //消行
            int l = CheckFullLines();

            ////停用当前活动方块
            //activeBox.Stop();

            //GenerateActiveBox();
        }
        /// <summary>
        /// 当前方块下落到底部的响应方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ActiveBoxCrush(Object sender, BoxEventArgs e)
        {
            if (ActiveBoxCrushEvent != null)
            {
                ActiveBoxCrushEvent(this, new MoveEventArgs(null, e.box.Entity));
            }
            //绘制当前活动方块
            int tx, ty;
            for (int i = 0; i < 4; i++)
            {
                tx = e.box.Entity[i].pos.x;
                ty = e.box.Entity[i].pos.y;
                container.map[tx, ty].Value = e.box.shape;
            }

            //当前方块掉落到底部
            //消行
            int l = CheckFullLines();

            //停用当前活动方块
            activeBox.Stop();

            GenerateActiveBox();

        }
        /// <summary>
        /// 生成新的活动方块
        /// </summary>
        public void GenerateActiveBox()
        {
            activeBox = readyBox;
            activeBox.move += ActiveBoxPositionChanged;
            readyBox = null;

            bool bOK = true;
            if (activeBox != null)
                bOK = activeBox.Act();
            else bOK = false;

            if (bOK)
            {
                readyBox = boxFactory.GetNewBasicBox(this);

                add_boxNum(1);
                if (RenewReadyBox != null)
                {
                    RenewReadyBox(this, new BoxShapeEventArgs(readyBox.shape));
                }
                if (RenewActiveBox != null)
                {
                    RenewActiveBox(this, null);
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
        public void ActiveBoxPositionChanged(Object sender, MoveEventArgs e)
        {
            ClearGrid(e.period);
            UpdateGrid(e.next);

            if (sender == activeBox)
            {
                if (ActiveBoxMoved != null)
                    ActiveBoxMoved(this, new BoxEventArgs(activeBox));
                if (ActiveBoxMoving != null)
                    ActiveBoxMoving(this, e);
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
                container.map[p.x, p.y].Draw(list[i].value);
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
                container.map[p.x, p.y].Draw(BoxShape.NULL);
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
        /// 每秒掉落多少格
        /// </summary>
        public double Hard
        {
            get { return TimeIntervalToHard(boxDropInterval); }
            set
            {
                boxDropInterval = HardToTimeInterval(value);
            }
        }
        protected int HardToTimeInterval(double hard)
        {
            double t = hard;
            t = 1e3 / t;
            return (int)t;
        }
        protected double TimeIntervalToHard(int interval)
        {
            return 1e3 / (double)interval;
        }

        public int Row
        {
            get { return row; }
        }

        virtual public void add_boxNum(int v = 1)
        {
            boxNum += v;
        }



        public int boxDropInterval;
        public int boxNum;
        protected readonly Grid grid;
        protected readonly int row;
        protected readonly int column;
        protected readonly BoxFactory boxFactory;



    }
}
