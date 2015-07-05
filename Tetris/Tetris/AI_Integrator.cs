using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class AI_Integrator
    {
        public AI_Integrator()
        {
            Init();
        }
        virtual public int CalculateScore()
        {
            return 10000;
        }
        virtual public void Init()
        {
            trueHoles = 0;
            trueFloat = 0;
            cleanedLines = 0;
            holes = 0;
            highDft = new int[40];
            top = 0;
            bottom = 9999;
            floadSqr = new int[40];
        }

        public int trueHoles;
        public int mark;
        public int holes;
        public int[] highDft;
        public int top;
        public int bottom;
        public int[] floadSqr;
        public int cleanedLines;
        public int trueFloat;
    }
    
    public class Simple_AI_Integrator : AI_Integrator
    {
        public override int CalculateScore()
        {
            mark = 100000;
            //mark += highDft[0] * 3;
            //mark += cleanedLines * 30;

            mark -= holes * 700;
            mark -= (top - bottom) * 150;
            for (int i = 1; i < floadSqr.Length; i++)
            {
                mark -= floadSqr[i] * ((i + 2) * (i + 4));
            }

            for (int i = 2; i < highDft.Length; i++)
            {
                mark -= highDft[i] * (i * i * i * 5);
            }
            return mark;
        }
    }
    public class Complex_AI_Integrator : AI_Integrator
    {
        public override int CalculateScore()
        {
            mark = 1000000;
            //mark += cleanedLines * 100 * top;

            mark -= trueHoles * 1000;
            mark -= (holes - trueHoles) * 400;

            //mark -= (top - bottom) * (top - bottom) * (top - bottom);
            mark -= (1<<Math.Max(top-4,0));

            mark -= trueFloat * 300;

            for (int i = 1; i < floadSqr.Length; i++)
            {
                mark -= floadSqr[i] * (i * 40) * trueHoles;
            }

            for (int i = 2; i < highDft.Length; i++)
            {
                mark -= (int)(highDft[i] * (i * i * i * i * 4));
            }

            return mark;
        }
    }
}
