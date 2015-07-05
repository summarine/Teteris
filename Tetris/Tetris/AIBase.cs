using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Tetris
{
    public abstract class AIBase
    {
        virtual public void Init(int row, int col)
        {

        }
        virtual public void NewBoxArrive(BoxShape active, BoxShape ready)
        {

        }

        virtual public List<Key> GetOperation()
        {
            return null;
        }

    }

    
}
