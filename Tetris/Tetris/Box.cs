using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Tetris
{
    public enum BoxShape
    {
        Z, S, T, I, O, L, J
    }
    public class MoveEventArgs : EventArgs
    {
        public List<Square> period;
        public List<Square> next;

        public MoveEventArgs(List<Square> a, List<Square> b)
        {
            // TODO: Complete member initialization
            this.period = a;
            this.next = b;
        }

    }
    public delegate void MoveEventHandle(Object sender, MoveEventArgs e);
    
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

            timer1 = new DispatcherTimer();
            timer1.Interval = new TimeSpan(6000000);//gFrame.boxDropInterval);
            timer1.Tick += timer1_Tick;

            isActive = false;
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
                timer1.Tick -= timer1_Tick;
            }
        }
        public bool Act()
        {
            //生成
            center = new Position(2,gFrame.Column/2);

            for (int i = 0;i<4;i++)
            {
                entity[i].pos.x += center.x;
                entity[i].pos.y += center.y;
                if (!gFrame.UnitAvilible(entity[i].pos.x, entity[i].pos.y))
                    return false;
            }
            //生成成功
            
            onBottom += gFrame.ActiveBoxCrush;
            move += gFrame.MapChanged;

            move(this,new MoveEventArgs(null,entity));
            timer1.Start();
            isActive = true;
            return true;
        }
        public void Stop()
        {
            isActive = false;
            timer1.Tick -= timer1_Tick;
            timer1.Stop();
        }
        public void Pause()
        {
            isActive = false;
            timer1.Stop();
        }
        public void Continue()
        {
            timer1.Start();
            isActive = true;
        }

        protected virtual List<List<Position>> Shapes()
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
                if (gFrame.UnitAvilible(tx, ty) || UnitInBox(tx, ty, entity))
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
                move(this, new MoveEventArgs(entity, temp));
                entity = temp;
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
        protected bool UnitInBox(int x,int y,List<Square> list)
        {
            bool b=false;
            for (int i=0;i<list.Count;i++)
            {
                if (x==list[i].pos.x && y==list[i].pos.y)
                {
                    b = true;
                    break;
                }
            }
            return b;
        }
        public void FastFall()
        {
            int mi = 100;
            int tx,ty;
            for (int i=0;i<4;i++)
            {
                tx = entity[i].pos.x; ty = entity[i].pos.y;
                while (gFrame.UnitAvilible(tx,ty) || UnitInBox(tx,ty,entity))
                {
                    tx++;
                }
                mi = Math.Min(tx - entity[i].pos.x, mi);
            }
            mi--;
            TryMove(mi, 0);
            
        }
        protected bool TryMove(int dx,int dy)
        {

            int tx, ty;
            for (int i = 0; i < entity.Count;i++ )
            {
                tx = entity[i].pos.x + dx;
                ty = entity[i].pos.y + dy;
                bool bIn = false;
                for (int j = 0; j < 4; j++)
                {
                    if (entity[j].pos.x == tx && entity[j].pos.y == ty)
                    {
                        bIn = true; break;
                    }
                }
                if (!bIn && !gFrame.UnitAvilible(tx, ty))
                    return false;
            }
            List<Square> temp = new List<Square>();
            for (int i=0;i<entity.Count;i++)
            {
                temp.Add(new Square(new Position(entity[i].pos.x+dx, entity[i].pos.y+dy), 1));
            }
            move(this, new MoveEventArgs(entity,temp));
            entity = temp;

            center.x += dx;
            center.y += dy;
            return true;
        }

        protected DispatcherTimer timer1;
        protected Position center;
        protected GameFrame gFrame;//隶属框架
        protected List<Square> entity;//记录整个Box所有方块的相对位置
        public BoxShape shape;
        protected int state;
        public bool isActive;



    }

    /// <summary>
    /// 坐标 : x行,y列
    /// </summary>
    public class Position
    {
        public Position(int x,int y)
        {
            this.x=x;
            this.y=y;
        }
        public Position()
        {

        }
        public int x;
        public int y;
    }
    public class Square
    {
        public Square(Position p, int v)
        {
            this.pos = p;
            this.value = v;
        }
        public Position pos;//位置
        public int value;//种类？
    }
}
