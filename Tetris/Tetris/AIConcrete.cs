using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Tetris
{
    class SimpleAI : AIBase
    {
        public SimpleAI(int r,int c)
        {
            Init(r, c);
        }
        public override void Init(int row, int col)
        {
            this.row = row;
            this.col = col;
            map = new int[row, col];
            for (int i = 0; i < row; i++)
                for (int j = 0; j < col; j++)
                    map[i,j] = 0;
        }
        public override List<System.Windows.Input.Key> GetOperation()
        {
            return operations;
        }
        public override void NewBoxArrive(BoxShape active, BoxShape ready)
        {
            Box box1 = BoxFactory.Instance().GetBoxFromId((int)active);
            Box box2 = BoxFactory.Instance().GetBoxFromId((int)ready);

            perioty = 0;
            for (int i=0;i<box1.Shapes().Count;i++)
            {
                for ()


                box1.Change();
            }


        }

        int row, col;
        protected AIFrame aiFrame;
        protected int perioty;
        protected List<Key> operations;
        protected int[,] map;
    }
}
