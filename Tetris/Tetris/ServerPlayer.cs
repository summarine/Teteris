using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Sever;
using System.Windows.Controls;
using System.Windows.Input;
namespace Tetris
{
    public class ServerPlayer : NetPlayer
    {
        public Socket m_Client;
        public ServerPlayer()
        {
            m_Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        public override void SendClientMsg(string msg)
        {
            if (m_Client.Connected)
            {
                m_Client.Send(Encoding.Unicode.GetBytes(msg));
            }
            else
            {
                Console.WriteLine("client didn't connect");
            }
        }
        public override void Ready()
        {
            Random rand = new Random();
            int seed = rand.Next();
            SendClientMsg("seed:" + seed.ToString());
            myGame.SetFactorySeed(seed);
            friendGame.SetFactorySeed(seed);

            SendClientMsg("game:start");
            Readycount++;
        }
        public override void Func()
        {
            NetGame win = new NetGame(this);
            win.Show();
        }
    }
}
