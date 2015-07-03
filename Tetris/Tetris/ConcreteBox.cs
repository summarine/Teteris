using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class L_Box : Box
    {
        static protected List<List<Position>> SHAPES = new List<List<Position>>
        {
            new List<Position>()
            {
                new Position(0,0),new Position(0,1),new Position(-1,0),new Position(-2,0)
            },
            new List<Position>()
            {
                new Position(0,0),new Position(0,-1),new Position(0,1),new Position(-1,1)
            },
            new List<Position>()
            {
                new Position(0,0),new Position(-1,0),new Position(-2,0),new Position(-2,-1)
            },
            new List<Position>()
            {
                new Position(0,-1),new Position(-1,-1),new Position(-1,0),new Position(-1,1)
            }
        };
        public override List<List<Position>> Shapes()
        {
            return SHAPES;
        }
        public L_Box(GameFrame gf)
            : base(gf)
        {
            state = 0;
            this.shape = BoxShape.L;
            entity = new List<Square>{
                new Square(new Position(0,0), shape),
                new Square(new Position(0,1), shape),
                new Square(new Position(-1,0), shape),
                new Square(new Position(-2,0), shape),
            };
        }
    }
    public class J_Box : Box
    {
        static protected List<List<Position>> SHAPES = new List<List<Position>>
        {
            new List<Position>()
            {
                new Position(0,0),new Position(0,-1),new Position(-1,0),new Position(-2,0)
            },
            new List<Position>()
            {
                new Position(0,1),new Position(-1,-1),new Position(-1,0),new Position(-1,1)
            },
            new List<Position>()
            {
                new Position(0,0),new Position(-1,0),new Position(-2,0),new Position(-2,1)
            },
            new List<Position>()
            {
                new Position(0,0),new Position(0,1),new Position(0,-1),new Position(-1,-1)
            }
        };
        public override List<List<Position>> Shapes()
        {
            return SHAPES;
        }
        public J_Box(GameFrame gf)
            : base(gf)
        {
            this.shape = BoxShape.J;
            entity = new List<Square>{
                new Square(new Position(0,0), shape),
                new Square(new Position(0,-1), shape),
                new Square(new Position(-1,0), shape),
                new Square(new Position(-2,0), shape)
            };
        }
    }
    public class I_Box : Box
    {
        static protected List<List<Position>> SHAPES = new List<List<Position>>
        {
            new List<Position>()
            {
                new Position(0,0),new Position(-1,0),new Position(-2,0),new Position(-3,0)
            },
            new List<Position>()
            {
                new Position(0,0),new Position(0,1),new Position(0,-1),new Position(0,-2)
            }
        };
        public override List<List<Position>> Shapes()
        {
            return SHAPES;
        }
        public I_Box(GameFrame gf)
            : base(gf)
        {
            this.shape = BoxShape.I;
            entity = new List<Square>{
                new Square(new Position(0,0), shape),
                new Square(new Position(1,0), shape),
                new Square(new Position(-1,0), shape),
                new Square(new Position(-2,0), shape),
            };
        }
    }
    public class Z_Box : Box
    {
        static protected List<List<Position>> SHAPES = new List<List<Position>>
        {
            new List<Position>()
            {
                new Position(0,0),new Position(0,1),new Position(-1,0),new Position(-1,-1)
            },
            new List<Position>()
            {
                new Position(0,-1),new Position(-1,-1),new Position(-1,0),new Position(-2,0)
            }
        };
        public override List<List<Position>> Shapes()
        {
            return SHAPES;
        }
        public Z_Box(GameFrame gf)
            : base(gf)
        {
            this.shape = BoxShape.Z;
            entity = new List<Square>{
                new Square(new Position(0,0), shape),
                new Square(new Position(0,1), shape),
                new Square(new Position(-1,0), shape),
                new Square(new Position(-1,-1), shape),
            };
        }
    }
    public class S_Box : Box
    {
        static protected List<List<Position>> SHAPES = new List<List<Position>>
        {
            new List<Position>()
            {
                new Position(0,0),new Position(0,-1),new Position(-1,0),new Position(-1,1)
            },
            new List<Position>()
            {
                new Position(0,1),new Position(-1,0),new Position(-1,1),new Position(-2,0)
            }
        };
        public override List<List<Position>> Shapes()
        {
            return SHAPES;
        }
        public S_Box(GameFrame gf)
            : base(gf)
        {
            this.shape = BoxShape.S;
            entity = new List<Square>{
                new Square(new Position(0,0), shape),
                new Square(new Position(0,-1), shape),
                new Square(new Position(-1,0), shape),
                new Square(new Position(-1,1), shape),
            };
        }
    }
    public class O_Box : Box
    {
        static protected List<List<Position>> SHAPES = new List<List<Position>>
        {
            new List<Position>()
            {
                new Position(0,0),new Position(0,1),new Position(-1,0),new Position(-1,-1)
            }
        };
        public override List<List<Position>> Shapes()
        {
            return SHAPES;
        }
        public O_Box(GameFrame gf)
            : base(gf)
        {
            this.shape = BoxShape.O;
            entity = new List<Square>{
                new Square(new Position(0,0), shape),
                new Square(new Position(0,1), shape),
                new Square(new Position(-1,0), shape),
                new Square(new Position(-1,1), shape),
            };
        }
        public override bool Change()
        {
            return false;
        }
    }
    public class T_Box : Box
    {
        static protected List<List<Position>> SHAPES = new List<List<Position>>
        {
            new List<Position>()
            {
                new Position(0,0),new Position(0,-1),new Position(-1,0),new Position(0,1)
            },
            new List<Position>()
            {
                new Position(0,0),new Position(-1,0),new Position(-1,-1),new Position(-2,0)
            },
            new List<Position>()
            {
                new Position(0,0),new Position(-1,0),new Position(-1,-1),new Position(-1,1)
            },
            new List<Position>()
            {
                new Position(0,0),new Position(-1,0),new Position(-1,1),new Position(-2,0)
            }
        };
        public override List<List<Position>> Shapes()
        {
            return SHAPES;
        }
        public T_Box(GameFrame gf)
            : base(gf)
        {
            this.shape = BoxShape.T;
            entity = new List<Square>{
                new Square(new Position(0,0), shape),
                new Square(new Position(0,1), shape),
                new Square(new Position(-1,0), shape),
                new Square(new Position(0,-1), shape),
            };
        }
    }
}
