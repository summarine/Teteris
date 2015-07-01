using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class Resources
    {
        public static string GetUnitImgFromInt(int v)
        {
            return "Blank.png";
        }
        public static string GetStringfromBoxShape(BoxShape bs)
        {
            //前面添加了BoxUnit_
            switch (bs)
            {
                case BoxShape.OBINARY:
                    return "Obinary.png";
                case BoxShape.BLANK:
                    return "Blank.png";
                case BoxShape.I:
                    return "I.png";
                case BoxShape.J:
                    return "J.png";
                case BoxShape.L:
                    return "L.png";
                case BoxShape.O:
                    return "O.png";
                case BoxShape.S:
                    return "S.png";
                case BoxShape.Z:
                    return "Z.png";
                case BoxShape.T:
                    return "T.png";
                case BoxShape.SHADOW:
                    return "Shadow.png";
                case BoxShape.BAN:
                    return "Ban.png";
                default:
                    return "Blank.png";
            }
        }
    }
}
