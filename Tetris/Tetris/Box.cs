using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class Box
    {
        virtual public void Change()
        { }
        public void MoveRight()
        { }
        public void MoveLeft()
        { }
        public void FastFall()
        { }
        protected bool TryMove(int dx,int dy)
        {
            int tx, ty;
            for (int i = 0; i < entity.Count;i++ )
            {
                
            }
                return false;    
        }

        protected GameFrame gFrame;
        protected Position pos;
        protected int state;//表示当前状态
        protected List<Position> entity;//记录整个Box所有方块的相对位置
    }

    class Position
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
}
