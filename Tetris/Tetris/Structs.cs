using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class BoxEventArgs : EventArgs
    {
        public Box box;

        public BoxEventArgs(Box box)
        {
            this.box = box;
        }

    }
    public delegate void BoxEventHandler(Object sender, BoxEventArgs e);

    public class BoxShapeEventArgs : EventArgs
    {
        public BoxShape box;

        public BoxShapeEventArgs(BoxShape box)
        {
            this.box = box;
        }
    }
    public delegate void BoxShapeEventHandler(Object sender,BoxShapeEventArgs e);

    public class MoveEventArgs : EventArgs
    {
        public List<Square> period;
        public List<Square> next;

        public MoveEventArgs(List<Square> a, List<Square> b)
        {
            // TODO: Complete member initialization
            this.period = a;
            this.next = b;
        }

        public override string ToString()
        {
            string str = "";

            if (period == null)
            {
                str += "N";
            }
            else
            {
                for (int i=0;i<4;i++)
                {
                    str += period[i].ToString() + ",";
                }
            }
            str += "|";
            if (next == null)
            {
                str += "N";
            }
            else
            {
                for (int i=0;i<4;i++)
                {
                    str += next[i].ToString() + ",";
                }
            }

            //Console.WriteLine(str);

            return str;
        }

        public MoveEventArgs(string str)
        {
            string[] pe = str.Split('|');
            if (pe[0]=="N")
            {
                period = null;
            }
            else
            {
                string[] sq = pe[0].Split(',');
                period = new List<Square>();
                for (int i = 0; i < 4; i++)
                    period.Add(new Square(sq[i]));
            }
            if (pe[1]=="N")
            {
                next = null;
            }
            else
            {
                string[] sq = pe[1].Split(',');
                next = new List<Square>();
                for (int i = 0; i < 4; i++)
                    next.Add(new Square(sq[i]));
            }

        }

    }
    public delegate void MoveEventHandler(Object sender, MoveEventArgs e);

    public class RowEventArgs : EventArgs
    {
        public List<int> value;
        
        public RowEventArgs(List<int> value)
        {
            this.value = value;
        }
    }
    public delegate void RowEventHandler(Object sender, RowEventArgs e);

    public class ScoreEventArgs : EventArgs
    {
        public int value;
        public ScoreEventArgs(int value)
        { 
            this.value = value;
        }
    }
    public delegate void ScoreEventHandler(Object sender,ScoreEventArgs e);
    /// <summary>
    /// 方块的形状,NULL表示没有形状,BAN表示禁用
    /// </summary>
    public enum BoxShape
    {
        NULL = 0, Z = 1, S = 2, T = 3, I = 4, O = 5, L = 6, J = 7, OBINARY = 8, BAN = 9, SHADOW = 10, BLANK = 1000
    }
    /// <summary>
    /// 游戏状态
    /// </summary>
    public enum GameState
    {
        Stoped=0, Active, Paused
    }
    /// <summary>
    /// 坐标 : x行,y列
    /// </summary>
    public class Position
    {
        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public Position()
        {

        }
        public int x;
        public int y;
    }
    
}
