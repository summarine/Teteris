using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Tetris
{
    class AIFrame : GameFrame
    {
        DispatcherTimer timer1;
        public AIFrame(Grid grid,int row,int col):base(grid,row,col)
        {
            RenewActiveBox += AIWork;
        }

        private void AIWork(object sender, BoxShapeEventArgs e)
        {
            //myAI
            myAI.NewBoxArrive(activeBox.shape, readyBox.shape);
            ope = myAI.GetOperation();
            
            if (ope != null)
            {
                timer1_i = 0;
                BeginInput();
            }
        }
        private void BeginInput()
        {
            timer1 = new DispatcherTimer();
            timer1.Interval = new TimeSpan((int)1e7 / 10);
            timer1.Tick += timer1_Tick;
            timer1.Start();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (timer1_i >= ope.Count)
            {
                EndInput();
            }
            KeyDown(ope[timer1_i]);
            timer1_i++;
            if (timer1_i >= ope.Count)
            {
                EndInput();
            }
        }

        private void EndInput()
        {
            if (timer1 != null)
            {
                timer1.Tick -= timer1_Tick;
                timer1 = null;
            }
        }

        public override void Start()
        {
            Stop();
            if (myAI != null)
                myAI.Init(row, column);
            base.Start();
        }
        public override void Stop()
        {
            base.Stop();
            EndInput();
            AIStop();
        }
        public override void Pause()
        {
            base.Pause();
            EndInput();
        }
        public override void Continue()
        {
            base.Continue();
            BeginInput();
        }
        public void AIStop()
        {
            if (myAI!=null)
                ;
        }

        public void AIInitialize(AIBase ai)
        {
            myAI = ai;
        }


        private AIBase myAI;
        private List<Key> ope;
        private int timer1_i;
    }
}
