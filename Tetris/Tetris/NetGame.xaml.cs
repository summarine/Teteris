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
    /// ServerNetGame.xaml 的交互逻辑
    /// </summary>
    public partial class NetGame : Window
    {
        public NetGame(NetPlayer np)
        {

            InitializeComponent();
            game = np;
            game.InitNetPlayer(gameGrid1, gameGrid2, Scoring_Board1, Scoring_Board2, PreviewImage1, 19, 10);
        }

       /* private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            //BoxFactory.Instance().setSeed(101);
            game.Start();
        }*/

        private void StopBtn_Click(object sender, RoutedEventArgs e)
        {
            //BoxFactory.Instance().setSeed(101);
            game.Stop();
        }
        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            //BoxFactory.Instance().setSeed(101);
           
            game.Ready();
            //game.Start();
        }
        private NetPlayer game;

        private void Window_KeyDown_1(object sender, KeyEventArgs e)
        {
            game.KeyDown(e.Key);
        }
    }
}
