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
using System.Windows.Shapes;

namespace Tetris
{
    /// <summary>
    /// AIWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AIWindow : Window
    {
        public AIWindow()
        {
            InitializeComponent();
            game = new AIFrame(gameGrid,19,10);
            game.AIInitialize(new SimpleAI(19,10, game,new Simple_AI_Integrator()));

            ScoringBoard sb = new ScoringBoard(Scoring_Board_AI);
            game.RowsCleanEvent += sb.GetScore;
           

        }

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            game.SetFactorySeed(103);
            game.Start();
        }

        private AIFrame game;

        private void PauseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (game.State == GameState.Active)
                game.Pause();
            else
                game.Continue();
        }

    }
}
