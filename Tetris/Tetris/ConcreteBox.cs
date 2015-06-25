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
        public L_Box(GameFrame gf) 
            : base(gf)
        {
            this.shape = BoxShape.L;
            entity = new List<Square>{
                new Square(new Position(0,0), 1),
                new Square(new Position(0,1), 1),
                new Square(new Position(-1,0), 1),
                new Square(new Position(-2,0), 1),
            };
        }
    }
    class J_Box : Box
    {
        public J_Box(GameFrame gf)
            : base(gf)
        {
            this.shape = BoxShape.J;
            entity = new List<Square>{
                new Square(new Position(0,0), 1),
                new Square(new Position(0,-1), 1),
                new Square(new Position(-1,0), 1),
                new Square(new Position(-2,0), 1),
            };
        }
    }
    class I_Box : Box
    {
        public I_Box(GameFrame gf)
            : base(gf)
        {
            this.shape = BoxShape.I;
            entity = new List<Square>{
                new Square(new Position(0,0), 1),
                new Square(new Position(1,0), 1),
                new Square(new Position(-1,0), 1),
                new Square(new Position(-2,0), 1),
            };
        }
    }
    class Z_Box : Box
    {
        public Z_Box(GameFrame gf)
            : base(gf)
        {
            this.shape = BoxShape.Z;
            entity = new List<Square>{
                new Square(new Position(0,0), 1),
                new Square(new Position(0,1), 1),
                new Square(new Position(-1,0), 1),
                new Square(new Position(-1,-1), 1),
            };
        }
    }
    class S_Box : Box
    {
        public S_Box(GameFrame gf)
            : base(gf)
        {
            this.shape = BoxShape.S;
            entity = new List<Square>{
                new Square(new Position(0,0), 1),
                new Square(new Position(0,-1), 1),
                new Square(new Position(-1,0), 1),
                new Square(new Position(-1,1), 1),
            };
        }
    }
    class O_Box : Box
    {
        public O_Box(GameFrame gf)
            : base(gf)
        {
            this.shape = BoxShape.O;
            entity = new List<Square>{
                new Square(new Position(0,0), 1),
                new Square(new Position(0,1), 1),
                new Square(new Position(-1,0), 1),
                new Square(new Position(-1,1), 1),
            };
        }
    }
    class T_Box : Box
    {
        public T_Box(GameFrame gf)
            : base(gf)
        {
            this.shape = BoxShape.T;
            entity = new List<Square>{
                new Square(new Position(0,0), 1),
                new Square(new Position(0,1), 1),
                new Square(new Position(-1,0), 1),
                new Square(new Position(0,-1), 1),
            };
        }
    }
}
