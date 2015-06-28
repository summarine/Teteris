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
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        GameFrame game;
        PreviewWindow preview;
        ScoringBoard scoreBoard;
        public MainWindow()
        {
            InitializeComponent();
           
            game = new MyGameFrame(gameGrid,19,11);

            scoreBoard = new ScoringBoard(Scoring_Board);

            preview = new PreviewWindow(game, PreviewImage);

            game.RowsCleanEvent += scoreBoard.GetScore;
        }
        
        private void Window_KeyDown(Object sender,KeyEventArgs e)
        {
            if (game.activeBox!=null)
            {
                game.KeyDown(e);
            }
        }

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            if (game.State == GameState.Stoped)
            {
                game.Start();
            }
            else
            {
                MessageBox.Show(this, "已启动游戏");
            }
        }

        private void PauseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (game.State == GameState.Active)
            {
                game.Pause();
            }
            else
            {
                MessageBox.Show(this, "无法暂停");
            }
        }
        private void ContinueBtn_Click(object sender, RoutedEventArgs e)
        {
            if (game.State == GameState.Paused)
            {
                game.Continue();
            }
            else
            {
                MessageBox.Show(this, "无法继续");
            }
        }
        private void StopBtn_Click(object sender, RoutedEventArgs e)
        {
            if (game.State == GameState.Active || game.State== GameState.Paused)
            {
                game.Stop();
            }
            else
            {
                MessageBox.Show(this, "无法停止");
            }
        }
        private void QuitBtn_Click(object sender, RoutedEventArgs e)
        {
            game.Stop();
            this.Close();
        }
        
    }
}
