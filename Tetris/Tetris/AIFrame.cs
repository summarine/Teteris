using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Tetris
{
    class AIFrame : GameFrame
    {
        public AIFrame(Grid grid,int row,int col):base(grid,row,col)
        {
            RenewActiveBox += AIWork;
            myAI.Init(row, col);
        }

        private void AIWork(object sender, BoxShapeEventArgs e)
        {
            //myAI
            myAI.NewBoxArrive(activeBox.shape, readyBox.shape);
            List<Key> ope = myAI.GetOperation();
            for (int i=0;i<ope.Count;i++)
            {
                KeyDown(ope[i]);
            }
        }
        public override void Start()
        {
            base.Start();
            if (myAI != null)
                myAI.Init(row, column);
        }
        public override void Stop()
        {
            base.Stop();
            AIStop();
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
    }
}
