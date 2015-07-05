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
using System.Threading;
namespace Tetris
{
    /// <summary>
    /// tempwin.xaml 的交互逻辑
    /// </summary>
    public partial class tempwin : Window
    {
        LanClient client = new LanClient();
        LanServer server = new LanServer();
        public tempwin()
        {
            InitializeComponent();

            //Thread t2 = new Thread(() =>
            //{
            //    server = new LanServer();
            //    server.RunServer(4530);
            //});
            //t2.Start();

            Thread t1 = new Thread(() =>
            {
                client = new LanClient();
                client.RunClient();
            });
            t1.Start();

            //this.Hide();
        }
    }
}
