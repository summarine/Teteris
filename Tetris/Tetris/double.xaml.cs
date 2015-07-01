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
            myGame = new MyGameFrame(myGrid, r, c);
            myAIFrame = new AIFrame(aiGrid, r, c);
            myAIFrame.AIInitialize(new SimpleAI(r,c,myAIFrame));

            plarSc = new DoubleScoringBoard(PlayerScoreLabel);
            myGame.RowsCleanEvent += plarSc.GetScore;
            aiSc = new DoubleScoringBoard(AIScoreLabel);
            myAIFrame.RowsCleanEvent += aiSc.GetScore;

            //增加增行时间的绑定
            aiSc.CrossThreshold += myGame.AddUselessRows;
            plarSc.CrossThreshold += myAIFrame.AddUselessRows;
        }

        private void PauseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (myGame.State == GameState.Active)
            {
                myAIFrame.Pause();
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
            }
        }
        
        private GameFrame myGame;
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
