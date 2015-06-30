using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client;
namespace Sever
{
    class SocketMsg
    { 
        //好友IP
        public string friendIP;
        //好友端口
        public int friendProt;
        //好友名
        public string friendName;
        //方块类型
        //Block.BlockTypes blockTypes;
        //好友消息
        public string friendMsg;
        //消息类型
        public MsgType friendMsgType;
    }
}
