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

            gFrame.newReadyBox += GetNewReadyBox;
        }
        private void GetNewReadyBox(object sender, BoxEventArgs e)
        {
            string path = System.Environment.CurrentDirectory + "/Images/";
            if (e==null)
            {
                path += "emptyPreview.png";
            }
            else
            switch (e.box)
            {
                case BoxShape.I:
                    path += "IBoxPreview.png";
                    break;
                case BoxShape.J:
                    path += "JBoxPreview.png";
                    break;
                case BoxShape.L:
                    path += "LBoxPreview.png";
                    break;
                case BoxShape.T:
                    path += "TBoxPreview.png";
                    break;
                case BoxShape.S:
                    path += "SBoxPreview.png";
                    break;
                case BoxShape.Z:
                    path += "ZBoxPreview.png";
                    break;
                case BoxShape.O:
                    path += "OBoxPreview.png";
                    break;
            }
            if (File.Exists(path))
            {
                img.Source = new BitmapImage(new Uri(path, UriKind.Absolute));
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
