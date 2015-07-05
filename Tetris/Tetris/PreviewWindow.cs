using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Tetris
{
    public class PreviewWindow
    {
        public PreviewWindow(GameFrame gf,Image img)
        {
            gFrame = gf;
            this.img = img;
            string path = System.Environment.CurrentDirectory + "/images/" + "emptyPreview.png";
            
            if (File.Exists(path))
            {
                img.Source = new BitmapImage(new Uri(path, UriKind.Absolute));
            }
            else
            {
                img.Source = null;
            }

            gFrame.RenewReadyBox += GetNewReadyBox;
        }
        private void GetNewReadyBox(object sender, BoxShapeEventArgs e)
        {
            string path = System.Environment.CurrentDirectory + "/Images/BoxPreview_";
            if (e==null)
            {
                path += "empty.png";
            }
            else
            {
                path += Resources.GetStringfromBoxShape(e.box);
            }
            if (File.Exists(path))
            {
                img.Dispatcher.Invoke(
                               new Action(
                                          delegate
                                          {
                                              img.Source = new BitmapImage(new Uri(path, UriKind.Absolute));
                                          }));
            }
            else
            {
                img.Source = null;
            }
        }
        public GameFrame gFrame;
        public Image img;
    }
}
