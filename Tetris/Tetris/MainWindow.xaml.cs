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
        public MainWindow()
        {
            InitializeComponent();
           
            game = new GameFrame(gameGrid,10,15);

            gameGrid.ShowGridLines = true;
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
            if (game.IsPlaying)
            {
                MessageBox.Show(this, "已启动游戏");
            }
            else
            {
                game.Start();
            }
        }

        
    }
}
