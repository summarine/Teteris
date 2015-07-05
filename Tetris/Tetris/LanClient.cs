using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Net.Sockets;
using System.Net;
using System.Windows.Controls;
using Sever;
namespace Tetris
{
    public class LanClient:NetPlayer
    {       
        bool Isplay = false;

        public void RunClient()
        {
            try
            {
                Isplay = false;
                socket.Connect("192.168.137.1", 4530);
                //socket.Connect("localhost", 4530);

                Console.WriteLine("connect to server");

                socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveMessage), socket);
                
                Console.WriteLine("");
                BuildWin();             
            }
            catch
            {
                Isplay = true;//many 
            }

        }
        public void search() 
        {
            UdpClient myClient = new UdpClient(51888);
            //侦听所有网络接口上的客户端活动
            IPEndPoint IPEndFrom = new IPEndPoint(IPAddress.Any, 51888);
            //好友IP
            string friendIP;
            //好友端口
            int friendProt;
            //好友名
            string friendName;
            byte[] message = myClient.Receive(ref IPEndFrom);
            //将消息转换为 字符串
            string msg = Encoding.Default.GetString(message);
            //string msg = myClient.Receive();
            //用 : 拆分消息
            string[] MSGS = msg.Split(':');
            //获得用户名
            friendName = MSGS[1];
            //获得IP地址
            friendIP = MSGS[2];
            //获得端口号
            friendProt = int.Parse(MSGS[3]);
        }
    }
}
