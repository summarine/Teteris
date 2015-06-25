using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Tetris
{
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
    class Box
    {
        public enum BoxShape
        {
            Z,S,T,I,O,L,J
        }

        public event MoveEventHandle move;
        public event EventHandler onBottom;

        public Box()
        {
            timer1 = new DispatcherTimer();
            timer1.Interval = new TimeSpan(gFrame.boxDropInterval);
            timer1.Tick += timer1_Tick;
            move += gFrame.MapChanged;
            onBottom += gFrame.ActiveBoxCrush;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!MoveDown())
            {
                onBottom(this, null);
            }
        }
        public bool Act()
        {
            //生成
            pos = new Position(0,gFrame.Column/2);
            for (int i = 0;i<4;i++)
            {
                entity[i].pos.x += pos.x;
                entity[i].pos.y += pos.y;
                if (!gFrame.UnitAvilible(entity[i].pos.x, entity[i].pos.y))
                    return false;
            }
            //生成成功
            move(this,new MoveEventArgs(null,entity));
            timer1.Start();
            return true;
        }
        public void Pause()
        {
            timer1.Stop();
        }
        public void Continue()
        {
            timer1.Start();
        }

        public bool Change()
        {
            List<Square> temp = new List<Square>();
            temp.Add(entity[0]);
            int tx, ty, t;
            for (int i = 1; i < 4; i++)
            {
                tx = entity[i].pos.x - entity[0].pos.x;
                ty = entity[i].pos.y - entity[0].pos.y;
                t = tx;
                tx = -ty;
                ty = tx;
                if (gFrame.UnitAvilible(tx, ty))
                    temp.Add(new Square(new Position(tx, ty),1));
                else
                    return false;
            }
            move(this,new MoveEventArgs(entity, temp));
            entity = temp;
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
        public void FastFall()
        {
            int mi = 100;
            int tx,ty;
            for (int i=0;i<4;i++)
            {
                tx = entity[i].pos.x; ty = entity[i].pos.y;
                while (gFrame.UnitAvilible(tx,ty))
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
                if (!gFrame.UnitAvilible(tx, ty))
                    return false;
            }
            List<Square> temp = new List<Square>();
            for (int i=0;i<entity.Count;i++)
            {
                temp.Add(new Square(new Position(entity[i].pos.x+dx, entity[i].pos.y+dy), 1));
            }
            move(this, new MoveEventArgs(entity,temp));
            entity = temp;

            return true;
        }

        protected DispatcherTimer timer1;
        protected GameFrame gFrame;//隶属框架
        protected Position pos;//表示第一个方块的位置
        protected int state;//表示当前是形态多少号
        protected List<Square> entity;//记录整个Box所有方块的相对位置
        public BoxShape shape;


    }

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
