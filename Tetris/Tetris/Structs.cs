using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class BoxEventArgs : EventArgs
    {
        public BoxShape box;

        public BoxEventArgs(BoxShape box)
        {
            this.box = box;
        }
    }
    public delegate void BoxEventHandle(Object sender,BoxEventArgs e);

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

    }
    public delegate void MoveEventHandle(Object sender, MoveEventArgs e);

    public class ScoreEventArgs : EventArgs
    {
        public int value;
        
        public ScoreEventArgs(int value)
        {
            this.value = value;
        }
    }
    public delegate void ScoreEventHandle(Object sender, ScoreEventArgs e);
    /// <summary>
    /// 方块的形状,NULL表示没有形状,BAN表示禁用
    /// </summary>
    public enum BoxShape
    {
        NULL=0, Z=1, S=2, T=3, I=4, O=5, L=6, J=7, BAN=999, BLANK=1000
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
