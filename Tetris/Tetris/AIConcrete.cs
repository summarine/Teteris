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
        public SimpleAI(int r, int c, AIFrame frame)
        {
            this.aiFrame = frame;
            Init(r, c);
            integrator = new AI_Integrator();
        }
        public override void Init(int row, int col)
        {
            this.row = row;
            this.col = col;
            map = new int[row, col];
            for (int i = 0; i < row; i++)
                for (int j = 0; j < col; j++)
                    map[i, j] = 0;
        }
        public override List<System.Windows.Input.Key> GetOperation()
        {
            return operations;
        }
        public override void NewBoxArrive(BoxShape active, BoxShape ready)
        {
            Console.WriteLine();
            Console.WriteLine();
            //Console.WriteLine("New Box Type is " + ready.ToString()+"   Box Id:{0}    ========================",aiFrame.boxNum);

            perioty = 0;
            operations = new List<Key>();
            for (int j = 0; j < aiFrame.activeBox.Shapes().Count; j++)
            {
                //Console.WriteLine("     Shape Change Time is {0}",j);
                for (int i = col/2; i < col; i++)
                {
                    int mark = MakeTry(i, j, active);
                    //Console.WriteLine("          Position {0} : Mark is {1}",i,mark);
                    //if (mark < -10)   PaintMap();
                    if (mark == -1) break;
                    if (mark > perioty)
                    {
                        perioty = mark;
                        operations = tempOpr;
                    }
                }
                for (int i=col/2-1;i>=0;i--)
                {
                    int mark = MakeTry(i, j, active);
                   // Console.WriteLine("          Position {0} : Mark is {1}", i, mark);
                    if (mark == -1) break;
                    if (mark > perioty)
                    {
                        perioty = mark;
                        operations = tempOpr;
                    }
                }
            }

        }
        protected int MakeTry(int center_col, int changeTimes, BoxShape bshape)
        {
            tempOpr = new List<Key>();
            int i, j, k, t;
            Key tKey;
            int ty = col / 2, tx = 2;
            Box box = BoxFactory.Instance().GetBoxFromId((int)bshape,aiFrame);
            for (i = 0; i < 4; i++)
            {
                box.Entity[i].pos.x += tx;
                box.Entity[i].pos.y += ty;
            }
            for (i = 0; i < changeTimes; i++)
            {
                box.Change();
                tempOpr.Add(Key.Up);
            }
            MoveHandle moveWay;
            if (center_col < col / 2)
            {
                k = -1;
                tKey = Key.Left;
                moveWay = new MoveHandle(box.MoveLeft);
            }
            else
            {
                k = 1;
                tKey = Key.Right;
                moveWay = new MoveHandle(box.MoveRight);
            }

            for (; ty != center_col; )
            {
                ty += k;
                bool b = moveWay();
                if (!b) return -1;
                tempOpr.Add(tKey);
            }
            box.FastFall();
            tempOpr.Add(Key.Down);
            //当方块掉落完成后绘制地图
            for (i = 0; i < row; i++)
                for (j = 0; j < col; j++)
                {
                    t = (int)(aiFrame.container.map[i, j].Value);
                    if (t > 0 && t < 8 && ! aiFrame.activeBox.UnitInBox(i,j))
                        map[i, j] = 1;
                    else
                        map[i, j] = 0;
                }
            for (i = 0; i < 4; i++)
            {
                tx = box.Entity[i].pos.x;
                ty = box.Entity[i].pos.y;
                map[tx, ty] = 1;
            }
            ProcessMap();
            int rtn = integrator.CalculateScore();

            //if (bshape == BoxShape.L && changeTimes == 2) rtn *= 10; 

            return rtn;
        }
        private void ProcessMap()
        {
            //消行算法已优化
            integrator.Init();

            int fillRow = row-1;

            for (int r=row-1;r>=0;r--)
            {
                int sum=0;
                for (int c=0;c<col;c++)
                {
                    sum += map[r, c];
                }
                if (sum==col)
                {
                    integrator.cleanedLines++;
                }
                else
                {
                    if (fillRow<r)
                    {
                        for (int c=0;c<col;c++)
                        {
                            map[fillRow, c] = map[r, c];
                        }
                    }
                    fillRow--;
                }
            }
            for (int r=fillRow;r>=0;r--)
            {
                for (int c = 0; c < col; c++)
                    map[r, c] = 0;
            }


            int ghole=0,h,lh=0,gsqr=0,thole;
            //提取各个关键数据
            for (int c=0;c<col;c++)
            {
                ghole = 0;
                gsqr = 0;
                h = 0;
                thole = 0;
                for (int r=row-1;r>=2;r--)
                {
                    if (map[r, c] == 0)
                    {
                        integrator.floadSqr[thole] += gsqr;
                        gsqr = 0;

                        ghole++;
                        thole++;
                    }
                    if (map[r, c] != 0)
                    {
                        gsqr++;
                        h = row - r;

                        integrator.holes += ghole;
                        ghole = 0;
                    }
                    integrator.floadSqr[thole] += gsqr;
                }
                if (c!=0)
                {
                    integrator.highDft[Math.Abs(h - lh)]++;
                }
                lh = h;
                if (h > integrator.top) integrator.top = h;
                if (h < integrator.bottom) integrator.bottom = h;
            }
        }

        private void PaintMap()
        {
            Console.WriteLine("Paint Map ---------------------------");
            for (int r=0;r<row;r++)
            {
                for (int c=0;c<col;c++)
                {
                    char cc = '_';
                    if (map[r, c] == 1)
                        cc = '#';
                    Console.Write(cc);
                }
                Console.WriteLine();
            }
            Console.WriteLine("Paint Map End -------------------------");
        }

        private delegate bool MoveHandle();

        int row, col;
        protected AIFrame aiFrame;
        protected int perioty;
        protected List<Key> operations, tempOpr;
        protected int[,] map;
        protected AI_Integrator integrator;
    }
}
