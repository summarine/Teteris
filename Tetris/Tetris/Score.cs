using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace Tetris
{
    public class ScoringBoard : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event ScoreEventHandle ScoringEnded;
        /// <summary>
        /// 计分板,必须传入作为UI的Label
        /// </summary>
        /// <param name="Scoring_Board"></param>
        public ScoringBoard(Label scoring_Board,int l1=100,int l2=250,int l3=400,int l4=600)
        {
            Binding binding = new Binding("Score");
            binding.Source = this;
            binding.Mode = BindingMode.OneWay;
            binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            scoring_Board.SetBinding(Label.ContentProperty, binding);

            threshold = 500;
            L1 = l1; L2 = l2; L3 = l3; L4 = l4;
        }

        public void GetScore(Object sender,RowEventArgs e)
        {
            int r = e.value.Count;
            int s = 0;
            switch (r)
            {
                case 0: s = 0;
                    break;
                case 1: s = L1;
                    break;
                case 2: s = L2;
                    break;
                case 3: s = L3;
                    break;
                case 4: s = L4;
                    break;
                default: s = 0;
                    break;
            }
            Score += s;
        }

        public void Clear()
        {
            if (ScoringEnded!=null)
            {
                ScoringEnded(this,new ScoreEventArgs(Score));
            }
            this.score = 0;
        }
        public void WhenGameOver(Object sender,EventArgs e)
        {
            //排行榜
            //....
            Clear();
        }
        virtual public int Score
        {
            get { return score; }
            set
            {
                
                this.score = value;
                
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Score"));
                }
            }
        }

        protected int threshold;
        protected int score;
        protected int L1, L2, L3, L4;
    }

    public class DoubleScoringBoard : ScoringBoard
    {
        /// <summary>
        /// 到达阀值后触发
        /// </summary>
        public event ScoreEventHandle CrossThreshold;
        public DoubleScoringBoard(Label sc):base(sc)
        {
            threshold = 400;
        }

        public override int Score
        {
            get
            {
                return base.Score;
            }
            set
            {
                int temp = value;
                if (temp > threshold)
                {
                    if (CrossThreshold != null)
                    {
                        CrossThreshold(this, new ScoreEventArgs(1));
                    }
                    temp -= threshold;
                }
                base.Score = temp;
            }
        }
        
    }
}
