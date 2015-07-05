using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public int et;
        //好友消息
        public string friendMsg;
        
    }

    public class SocketMessage
    {
        public string et { get; set; }
        public ClientInfo Client { get; set; }
        public string Message { get; set; }
        public DateTime Time { get; set; }
    }
}
