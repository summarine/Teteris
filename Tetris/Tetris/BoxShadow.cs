using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class BoxShadow
    {
        public event MoveEventHandler reflect;

        public BoxShadow(GameFrame gf)
        {
            //entity = new List<Square>();
            gFrame = gf;
            reflect += gf.ActiveBoxPositionChanged;
        }

        public void Reflect(Object sender, BoxEventArgs e)
        {
            Box box = e.box;
            if (box == null)
                return;
            List<Square> temp = entity;
            //entity = new List<Square>(box.Entity);
            entity = new List<Square>();
            for (int i = 0; i < 4; i++)
            {
                Square s = new Square(new Position(box.Entity[i].pos.x, box.Entity[i].pos.y), BoxShape.SHADOW);
                entity.Add(s);
            }

            int tx, ty;
            int k = 0;
            bool b = true, over = false;
            while (b)
            {
                for (int i = 0; i < 4; i++)
                {
                    tx = entity[i].pos.x + 1;
                    if (tx >= gFrame.Row)
                    {
                        over = true;
                        break;
                    }
                    ty = entity[i].pos.y;
                    if (!gFrame.UnitAvilible(tx, ty))
                    {
                        b = false; break;
                    }
                }
                if (over)
                    break;
                if (b)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        entity[i].pos.x++;
                    }
                    k++;
                }
            }
            if (k > 4)
            {
                if (reflect != null)
                {
                    for (int i = 0; i < entity.Count; i++)
                    {
                        while (i < entity.Count && box.UnitInBox(entity[i].pos.x, entity[i].pos.y))
                            entity.Remove(entity[i]);
                    }
                    if (temp != null)
                        for (int i = 0; i < temp.Count; i++)
                        {
                            while (i < temp.Count && !gFrame.UnitAvilible(temp[i].pos.x,temp[i].pos.y))
                                temp.Remove(temp[i]);
                        }
                    reflect(this, new MoveEventArgs(temp, entity));
                }
            }
            else
            {
                if (reflect != null)
                {
                    if (temp != null)
                        for (int i = 0; i < temp.Count; i++)
                        {
                            while (i < temp.Count && (box.UnitInBox(temp[i].pos.x, temp[i].pos.y) || !gFrame.UnitAvilible(temp[i].pos.x,temp[i].pos.y) ))
                                temp.Remove(temp[i]);
                        }
                    reflect(this, new MoveEventArgs(temp, null));
                    entity = null;
                }
            }
        }
        private List<Square> entity;
        private GameFrame gFrame;
    }
}
