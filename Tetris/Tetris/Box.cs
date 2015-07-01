using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Tetris
{

    /// <summary>
    /// 一次次掉下的方块
    /// </summary>
    public class Box
    {
        public event MoveEventHandle move;
        public event EventHandler onBottom;

        public Box(GameFrame gf)
        {
            this.gFrame = gf;
            isActive = false;
            center = new Position(2, gFrame.Column / 2);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!isActive)
            {
                return;
            }
            if (!MoveDown())
            {
                onBottom(this, null);
            }
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (!isActive)
            {
                timer2.Stop();
                return;
            }
            if (!MoveDown())
            {
                onBottom(this, null);
            }
            else
            {
                if (timer1!=null)
                    timer1.Start();
                timer2.Stop();
            }
        }
        public bool Act()
        {
            //生成

            for (int i = 0; i < 4; i++)
            {
                entity[i].pos.x += center.x;
                entity[i].pos.y += center.y;
                if (!gFrame.UnitIsEmpty(entity[i].pos.x, entity[i].pos.y))
                    return false;
            }
            //生成成功
            timer1 = new DispatcherTimer();
            timer1.Interval = new TimeSpan(gFrame.boxDropInterval);//gFrame.boxDropInterval);
            timer1.Tick += timer1_Tick;

            onBottom += gFrame.ActiveBoxCrush;

            move(this, new MoveEventArgs(null, entity));
            timer1.Start();
            isActive = true;
            return true;
        }
        public void Stop()
        {
            isActive = false;
            if (timer1 != null)
            {
                timer1.Tick -= timer1_Tick;
                timer1.Stop();
            }
            if (timer2 != null)
            {
                timer2.Tick -= gFrame.ActiveBoxCrush;
                timer2.Stop();
            }
        }
        public void Pause()
        {
            isActive = false;
            if (timer1 != null)
                timer1.Stop();
        }
        public void Continue()
        {
            if (timer1 != null)
                timer1.Start();
            isActive = true;
        }

        public virtual List<List<Position>> Shapes()
        {
            return null;
        }

        public virtual bool Change()
        {
            state++;
            if (state == Shapes().Count) state = 0;
            bool b = true;
            List<Square> temp = new List<Square>();
            int tx, ty;
            for (int i = 0; i < 4; i++)
            {
                tx = center.x + Shapes()[state][i].x;
                ty = center.y + Shapes()[state][i].y;
                if (gFrame.UnitAvilible(tx, ty) || UnitInBox(tx, ty))
                {
                    temp.Add(new Square(new Position(tx, ty), entity[i].value));
                }
                else
                {
                    b = false;
                    break;
                }
            }
            if (!b)
            {
                state--;
                if (state < 0) state = Shapes().Count - 1;
            }
            else
            {
                List<Square> sw = temp; temp = entity; entity = sw;
                if (isActive && move!=null)
                    move(this, new MoveEventArgs(temp, entity));
                //entity = temp;
            }
            return true;
        }
        public bool MoveRight()
        {
            return TryMove(0, 1);
        }
        public bool MoveLeft()
        {
            return TryMove(0, -1);
        }
        public bool MoveDown()
        {
            return TryMove(1, 0);
        }
        public bool UnitInBox(int x, int y)
        {
            bool b = false;
            for (int i = 0; i < entity.Count; i++)
            {
                if (x == entity[i].pos.x && y == entity[i].pos.y)
                {
                    b = true;
                    break;
                }
            }
            return b;
        }
        public void FastFall()
        {
            if (timer1 != null)
                timer1.Stop();

            int mi = 100;
            int tx, ty;
            for (int i = 0; i < 4; i++)
            {
                tx = entity[i].pos.x; ty = entity[i].pos.y;
                while (gFrame.UnitAvilible(tx, ty) || UnitInBox(tx, ty))
                {
                    tx++;
                }
                mi = Math.Min(tx - entity[i].pos.x, mi);
            }
            mi--;
            TryMove(mi, 0);

            if (timer2 == null)
            {
                timer2 = new DispatcherTimer();
                timer2.Interval = new TimeSpan((long)2e6);
                timer2.Tick += timer2_Tick;
            }
            timer2.Start();
        }

        protected bool TryMove(int dx, int dy)
        {

            int tx, ty;
            for (int i = 0; i < entity.Count; i++)
            {
                tx = entity[i].pos.x + dx;
                ty = entity[i].pos.y + dy;
                
                if (!UnitInBox(tx,ty) && !gFrame.UnitAvilible(tx, ty))
                    return false;
            }
            List<Square> temp = new List<Square>();
            for (int i = 0; i < entity.Count; i++)
            {
                temp.Add(new Square(new Position(entity[i].pos.x + dx, entity[i].pos.y + dy), shape));
            }
            List<Square> sw = temp; temp = entity; entity = sw;
            center.x += dx;
            center.y += dy;
            if (isActive && move!=null)
                move(this, new MoveEventArgs(temp, entity));

            return true;
        }

        protected DispatcherTimer timer1;//匀速下落
        protected DispatcherTimer timer2;//快速下落后销毁方块
        protected Position center;
        protected GameFrame gFrame;//隶属框架
        protected List<Square> entity;//记录整个Box所有方块的相对位置
        public List<Square> Entity
        {
            get { return entity; }
        }
        public BoxShape shape;
        protected int state;
        public bool isActive;



    }


    public class Square
    {
        public Square(Position p, BoxShape v)
        {
            this.pos = p;
            this.value = v;
        }
        public Position pos;//位置
        public BoxShape value;//种类
    }
}
