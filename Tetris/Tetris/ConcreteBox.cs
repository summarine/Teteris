using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class ConcreteBox
    {
    }
    class L_Box : Box
    {
        public L_Box(GameFrame gf) : base()
        {
            this.gFrame = gf;
            List<Position> lp = new List<Position>{
                new Position(0,0),
                new Position(0,1),
                new Position(-1,0),
                new Position(-2,0)
            };
            for (int i=0;i<4;i++)
            {
                entity.Add(new Square(lp[i], 1));
            }
        }
    }
    class J_Box : Box
    {
        public J_Box(GameFrame gf)
            : base()
        {
            this.gFrame = gf;
            List<Position> lp = new List<Position>{
                new Position(0,0),
                new Position(0,-1),
                new Position(-1,0),
                new Position(-2,0)
            };
            for (int i = 0; i < 4; i++)
            {
                entity.Add(new Square(lp[i], 1));
            }
        }
    }
    class I_Box : Box
    {
        public I_Box(GameFrame gf)
            : base()
        {
            this.gFrame = gf;
            List<Position> lp = new List<Position>{
                new Position(0,0),
                new Position(1,0),
                new Position(-1,0),
                new Position(-2,0)
            };
            for (int i = 0; i < 4; i++)
            {
                entity.Add(new Square(lp[i], 1));
            }
        }
    }
    class Z_Box : Box
    {
        public Z_Box(GameFrame gf)
            : base()
        {
            this.gFrame = gf;
            List<Position> lp = new List<Position>{
                new Position(0,0),
                new Position(0,1),
                new Position(-1,0),
                new Position(-1,-1)
            };
            for (int i = 0; i < 4; i++)
            {
                entity.Add(new Square(lp[i], 1));
            }
        }
    }
    class S_Box : Box
    {
        public S_Box(GameFrame gf)
            : base()
        {
            this.gFrame = gf;
            List<Position> lp = new List<Position>{
                new Position(0,0),
                new Position(0,-1),
                new Position(-1,0),
                new Position(-1,1)
            };
            for (int i = 0; i < 4; i++)
            {
                entity.Add(new Square(lp[i], 1));
            }
        }
    }
    class O_Box : Box
    {
        public O_Box(GameFrame gf)
            : base()
        {
            this.gFrame = gf;
            List<Position> lp = new List<Position>{
                new Position(0,0),
                new Position(0,1),
                new Position(-1,0),
                new Position(-1,1)
            };
            for (int i = 0; i < 4; i++)
            {
                entity.Add(new Square(lp[i], 1));
            }
        }
    }
    class T_Box : Box
    {
        public T_Box(GameFrame gf)
            : base()
        {
            this.gFrame = gf;
            List<Position> lp = new List<Position>{
                new Position(0,0),
                new Position(0,1),
                new Position(-1,0),
                new Position(0,-1)
            };
            for (int i = 0; i < 4; i++)
            {
                entity.Add(new Square(lp[i], 1));
            }
        }
    }
}
