using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//网络通信 发送 和 接收 数据 
using System.Net.Sockets;
//网络连接 IP协议等
using System.Net;
//线程操作
using System.Threading;
using System.Text.RegularExpressions;

namespace Client
{
    public partial class Form1 : Form
    {
        private bool dropdown = false;//判断方块是否倒底
        IPAddress IPHost;
        string hostName="localhost";

        private static byte[] buffer = new byte[1024];

        public Form1()
        {
            InitializeComponent();
        }

        //创建一个Socket
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        private void button1_Click(object sender, EventArgs e)
        {
            //连接到指定服务器的指定端口发送链接信息
            socket.Connect("localhost", 4530);
            string msg = "havedropeddown";
            int blockTypes = 1;
            MsgType type = MsgType.上线应答;
            SendClientMsg(type, blockTypes, msg);
            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveMessage), socket);
        }

        //给服务器发送上线信息
        public void SendClientMsg(MsgType type, int blockTypes, string msg)
        {

                //发送开始信息
                var message = (int)type + " " + hostName + " " + IPHost.ToString() + " " + "4530"+" "+blockTypes + " " + msg;

                //将消息发送给服务器端
                var outputBuffer = Encoding.Unicode.GetBytes(message);
                socket.BeginSend(outputBuffer, 0, outputBuffer.Length, SocketFlags.None, null, null);
                textBox1.Text = "connect to the server";                  
        }

        public void ReceiveMessage(IAsyncResult ar)
        {
            try
            {
                var socket = ar.AsyncState as Socket;

                var length = socket.EndReceive(ar);
                //读取出来消息内容
                var message = Encoding.Unicode.GetString(buffer, 0, length);
                //显示消息
                textBox2.Text=message;
                //接收下一个消息(因为这是一个递归的调用，所以这样就可以一直接收消息了）
                socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveMessage), socket);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Drop()
        {
            dropdown = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //屏蔽线程异常
            CheckForIllegalCrossThreadCalls = false;
            //获得本地IP
            IPHost = Dns.GetHostAddresses(Dns.GetHostName())[0];
        }      
    }
}
