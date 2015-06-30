using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public enum MsgType
    {
        上线 = 0,
        下线 = 1,
        上线应答 = 2,
        邀请游戏 = 3,
        同意游戏 = 4,
        操作信息 = 5,
        游戏结束 = 6,
        游戏中 = 7,
        新方块 = 8,
    }
}
