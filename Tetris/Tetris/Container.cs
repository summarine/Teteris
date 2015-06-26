using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Tetris
{
    public class Container
    {
        public Container(int r,int c)
        {
            map = new GridData[r,c];
        }

        public GridData[,] map;

        public void setMapping(int row,int col,Label label)
        {
            
        }
    }

    public class GridData
    {
        public Label Lbl
        {
            get
            {
                return label;
            }
            set
            {
                label = value;
            }
        }
        public BoxShape Value
        {
            set
            {
                if (value<0) return;
                this.value = value;

                Convert(this.value);
            }
            get
            {
                return this.value;
            }
        }

        private void Convert(BoxShape bs)
        {
            if (bs==BoxShape.NULL)
                label.Background = null;
            else
            {
                string path = System.Environment.CurrentDirectory + "/images/BoxUnit_";
                path += Resources.GetUnitImgFromBoxShape(bs);
                if (File.Exists(path))
                    label.Background = new ImageBrush(new BitmapImage(new Uri(path, UriKind.Absolute)));
                else
                    label.Background = null;
            }
        }

        private BoxShape value;
        private Label label;
    }
}
