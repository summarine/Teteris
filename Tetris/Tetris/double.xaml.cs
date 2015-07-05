using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tetris
{
    /// <summary>
    /// @double.xaml 的交互逻辑
    /// </summary>
    public partial class DoublePlayer : Window
    {

        public DoublePlayer()
        {
            InitializeComponent();

            int r=19,c=10;
            //myGame = new MyGameFrame(myGrid, r, c);
            myGame = new AIFrame(myGrid, r, c);
            myGame.AIInitialize(new SimpleAI(r, c, myGame,new Complex_AI_Integrator()));

            myAIFrame = new AIFrame(aiGrid, r, c);
            myAIFrame.AIInitialize(new SimpleAI(r,c,myAIFrame,new Simple_AI_Integrator()));

            //设置计分板
            plarSc = new DoubleScoringBoard(PlayerScoreLabel);
            myGame.RowsCleanEvent += plarSc.GetScore;
            aiSc = new DoubleScoringBoard(AIScoreLabel);
            myAIFrame.RowsCleanEvent += aiSc.GetScore;

            plarSc.Threshold = aiSc.Threshold = 800;

            //增加增行函数的事件绑定
            aiSc.CrossThreshold += myGame.OtherCrossThreshold;
            plarSc.CrossThreshold += myAIFrame.OtherCrossThreshold;

            //设置游戏结束
            myGame.GameOverEvent += GameFinish;
            myGame.GameOverEvent += plarSc.WhenGameOver;
            myAIFrame.GameOverEvent += GameFinish;
            myAIFrame.GameOverEvent += aiSc.WhenGameOver;

        }

        private void GameFinish(object sender, EventArgs e)
        {
            if (myAIFrame.State != GameState.Stoped)
                myAIFrame.Stop();
            if (myGame.State != GameState.Stoped)
                myGame.Stop();

            if (sender == myGame)
            {
                MessageBox.Show("你输了");
            }
            else if (sender == myAIFrame)
            {
                MessageBox.Show("你赢了");
            }
        }

        private void PauseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (myGame.State == GameState.Active)
            {
                myGame.Pause();
                myAIFrame.Pause();
            }
        }


        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (myGame.State == GameState.Paused)
            {
                myGame.Continue();
                myAIFrame.Continue();
            }
            else if (myGame.State == GameState.Stoped)
            {
                int seed = (new Random()).Next();
                myGame.SetFactorySeed(seed);
                myAIFrame.SetFactorySeed(seed);
                myGame.Start();
                myAIFrame.Start();
            }   
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            if (myGame.State == GameState.Active || myGame.State==GameState.Paused)
            {
                myGame.Stop();
                myAIFrame.Stop();

                plarSc.Clear();
                aiSc.Clear();
            }
        }
        private AIFrame myGame;
        //private GameFrame myGame;
        private AIFrame myAIFrame;
        private DoubleScoringBoard plarSc, aiSc;

        private void Window_KeyDown(Object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (myGame.State == GameState.Stoped)
                {
                    myGame.Start();
                }
                else if (myGame.State == GameState.Active)
                {
                    myGame.Pause();
                }
                else if (myGame.State == GameState.Paused)
                {
                    myGame.Continue();
                }
            }
            else if (e.Key == Key.Escape)
            {
                if (myGame.State == GameState.Active || myGame.State == GameState.Paused)
                {
                    myGame.Stop();
                }
            }
            else
                if (myGame.activeBox != null)
                {
                    myGame.KeyDown(e.Key);
                }
        }
    }


}
