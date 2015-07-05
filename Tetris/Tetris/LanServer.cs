using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Net.Sockets;
using System.Net;
using Sever;
namespace Tetris
{
    public class LanServer : ServerPlayer
    {
        private Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public void RunServer(int port)
        {          
            socket.Bind(new IPEndPoint(IPAddress.Any, port));
            socket.Listen(4);
            socket.BeginAccept(new AsyncCallback((ar) =>
            {
                m_Client = socket.EndAccept(ar);
                if (m_Client.Connected)
                {
                    BuildWin();
                    Console.WriteLine("server linked");
                    m_Client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveMessage), m_Client);
                }
            }), null);
           // StartReceiveMsg();

        }


    }
}

