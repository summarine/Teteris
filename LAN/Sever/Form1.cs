using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Text.RegularExpressions;
using Client;
namespace Sever
{
    public partial class Form1 : Form
    {
        private Dictionary<Socket, ClientInfo> clientPool = new Dictionary<Socket, ClientInfo>();
        string been;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //屏蔽线程异常
            CheckForIllegalCrossThreadCalls = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //创建一个新的Socket,这里我们使用最常用的基于TCP的Stream Socket（流式套接字）
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //将该socket绑定到主机上面的某个端口
            socket.Bind(new IPEndPoint(IPAddress.Any, 4530));

            //启动监听，并且设置一个最大的队列长度
            socket.Listen(4);

            //开始接受客户端连接请求
            socket.BeginAccept(new AsyncCallback(ClientAccepted), socket);

            textBox1.Text = "Server is ready!";
            
            Broadcast(socket);
            button1.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (KeyValuePair<Socket, ClientInfo> item in clientPool)
            {                
                    item.Value.handleSocket.Close(); 
            }
        }
        /// <summary>
        /// 在独立线程中不停地向所有客户端广播消息
        /// </summary>
        private void Broadcast(Socket socket)
        {
            Thread broadcast = new Thread(() =>
            {
                while (true)
                {
                    //要发送的消息
                    byte[] msg = Encoding.Unicode.GetBytes("允了" + DateTime.Now.ToString());
                    
                    foreach (KeyValuePair<Socket, ClientInfo> cs in clientPool)
                    {
                        Socket client = cs.Key;
                        if (client.Connected && clientPool.Count==2)
                        {
                            try
                            {
                                MessageBox.Show(clientPool.Count.ToString());
                                //给客户端发送一个欢迎消息
                                client.Send(msg, msg.Length, SocketFlags.None);
                            }
                            catch
                            {
                                MessageBox.Show("客户端已下线");
                            }
                        }
                     }
                }
            });
            broadcast.Start();
        }  

        public void ClientAccepted(IAsyncResult ar)
        {
            var socket = ar.AsyncState as Socket;
            //这就是客户端的Socket实例，我们后续可以将其保存起来
            var client = socket.EndAccept(ar);
            client.Send(Encoding.Unicode.GetBytes("Hi there, I accept you request at " + DateTime.Now.ToString()));
            client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveMessage), client);
            ClientInfo info = new ClientInfo();
            info.Id = client.RemoteEndPoint;
            info.handle = client.Handle;
            info.buffer = buffer;
            info.handleSocket = client;
            //把客户端存入clientPool
            this.clientPool.Add(client, info);
            //准备接受下一个客户端请求
            socket.BeginAccept(new AsyncCallback(ClientAccepted), socket);
            //if (friendMsgType != MsgType.上线应答)
            //{
                //client.Send(Encoding.Unicode.GetBytes("允了" + DateTime.Now.ToString()));
                been = "我就是生成方块的种子";
                textBox3.Text = been;
            //}
        }
        static byte[] buffer = new byte[1024];
        SocketMsg socketmsg = new SocketMsg();

        //分解消息
        public void ReceiveMessage(IAsyncResult ar)
        {
            try
            {
                var socket = ar.AsyncState as Socket;
                var length = socket.EndReceive(ar);
                //读取出来消息内容,获得网络上传输的消息,将消息转换为 字符串
                var msg = Encoding.Unicode.GetString(buffer, 0, length);
                string[] MSGS = msg.Split(' ');
                socketmsg.friendMsgType = (MsgType)int.Parse(MSGS[0]);
                socketmsg.friendName = MSGS[1];
                socketmsg.friendIP = MSGS[2];
                socketmsg.friendProt = int.Parse(MSGS[3]);
                //blockTypes = (Block.BlockTypes)int.Parse(MSGS[4]);
                socketmsg.friendMsg = MSGS[5];
                textBox2.Text = socketmsg.friendName;
                //接收下一个消息(因为这是一个递归的调用，所以这样就可以一直接收消息了）
                socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveMessage), socket);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


    } 
}
