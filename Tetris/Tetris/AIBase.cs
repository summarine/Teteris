using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Tetris
{
    public abstract class AIBase
    {
        virtual public void Init(int row, int col)
        {

        }
        virtual public void NewBoxArrive(BoxShape active, BoxShape ready)
        {

        }

        virtual public List<Key> GetOperation()
        {
            return null;
        }

    }

    public class AI_Integrator
    {
        public AI_Integrator()
        {
            Init();
        }
        virtual public int CalculateScore()
        {
            mark = 100000;
            mark += highDft[0] * 3;
            mark += cleanedLines * 30;

            mark -= holes * 700;
            mark -= (top - bottom) * 150;
            for (int i = 1; i < floadSqr.Length; i++)
            {
                mark -= floadSqr[i] * ((i + 2) * (i + 4));
            }

            for (int i = 2; i < highDft.Length; i++)
            {
                mark -= highDft[i] * (i * i * i* 5);
            }
            return mark;
        }
        virtual public void Init()
        {
            cleanedLines = 0;
            holes = 0;
            highDft = new int[40];
            top = 0;
            bottom = 9999;
            floadSqr = new int[40];
        }

        public int mark;
        public int holes;
        public int[] highDft;
        public int top;
        public int bottom;
        public int[] floadSqr;
        public int cleanedLines;
    }
}
