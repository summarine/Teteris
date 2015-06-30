using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
//导入命名空间（文件---硬盘的操作）
using System.IO;
//网络通信 发送 和 接收 数据 
using System.Net.Sockets;
//网络连接 IP协议等
using System.Net;
//线程操作
using System.Threading;
using System.Text.RegularExpressions;

namespace 局域网
{
    public partial class Form1 : Form
    {
        #region data

        //声明 邀请好友界面
        Friend ff = new Friend();
        /// <summary>
        /// 获取本机广播使用的IP组，终结点
        /// </summary>
        private readonly IPEndPoint IPEndBroadcast = new IPEndPoint(IPAddress.Broadcast, 51888);
        /// <summary>
        /// 我的端口号（默认51888）
        /// </summary>
        int myPort = 51888;
        /// <summary>
        /// 我的网络客户端，发送接受数据报
        /// </summary>
        UdpClient myClient;
        /// <summary>
        /// 本地IP
        /// </summary>
        IPAddress IPHost;
        /// <summary>
        /// 本机名
        /// </summary>
        string hostName;
        /// <summary>
        /// 我的接收线程
        /// </summary>
        Thread myThreadReceive;

        #endregion

        #region function

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //准备好好友界面
            ff.Show();
            ff.Visible = false;
            //屏蔽线程异常
            CheckForIllegalCrossThreadCalls = false;
            #region NET设置
            //获得本地IP
            IPHost = Dns.GetHostAddresses(Dns.GetHostName())[0];
            //获得本机名
            hostName = Dns.GetHostName();
            //用正则表达式 对比是否 正确的IP
            if (!Regex.IsMatch(IPHost.ToString(), @"\d+\.\d+\.\d+\.\d"))
            {//不正确则获得2号IP
                IPHost = Dns.GetHostAddresses(Dns.GetHostName())[1];
            }
            try
            {
                // 客户端    设置 端口
                myClient = new UdpClient(myPort);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            //发送上线信息
            SendBroadcastMsg(MsgType.上线, 0, "上线");

            //线程启动时执行的方法〈区别于线程关闭〉
            ThreadStart receiveTreadStart = new ThreadStart(ThreadReceiveMsg);
            //将方法指定给线程
            myThreadReceive = new Thread(receiveTreadStart);
            //设为后台线程
            myThreadReceive.IsBackground = true;
            //接收线程启动
            myThreadReceive.Start();
            #endregion
        }

        //public delegate void friendBeginGameDelegate(string friendMsg, Block.BlockTypes blockTypes);

        private void ThreadReceiveMsg()
        {
            //侦听所有网络接口上的客户端活动
            IPEndPoint IPEndFrom = new IPEndPoint(IPAddress.Any, 51888);

            //好友IP
            string friendIP;
            //好友端口
            int friendProt;
            //好友名
            string friendName;
            
            //好友消息
            string friendMsg;
            //消息类型
            MsgType friendMsgType;
            //程序关闭前一直监听 网络上的信息
            while (true)
            {
                try
                {
                    //获得网络上传输的消息
                    byte[] message = myClient.Receive(ref IPEndFrom);
                    //将消息转换为 字符串
                    string msg = Encoding.Default.GetString(message);
                    //用 : 拆分消息
                    string[] MSGS = msg.Split(':');
                    //将获得的第一个 参数 转换为消息类型
                    friendMsgType = (MsgType)int.Parse(MSGS[0]);
                    //获得用户名
                    friendName = MSGS[1];
                    //获得IP地址
                    friendIP = MSGS[2];
                    //获得端口号
                    friendProt = int.Parse(MSGS[3]);
                    
                    //获得操作消息
                    friendMsg = MSGS[5];

                }
                catch (Exception)
                {
                    //如果发生异常 进行下一次 监听
                    continue;
                }

                //对不同消息类型采取不同的操作
                switch (friendMsgType)
                {
                    case MsgType.上线:
                        //向好友列表添加 好友信息
                        ff.AddFriendInfo(friendIP, friendProt, friendName);
                        //向好友提示 以接受到对方的上线信息
                        SendFriendMsg(MsgType.上线应答, 0, "", new IPEndPoint(IPAddress.Parse(friendIP), friendProt));
                        break;
                    case MsgType.下线:
                        //移出
                        ff.RemoveFriend(friendIP);
                        break;
                }

            }
        }

        public void SendFriendMsg(MsgType type, int blockTypes, string msg, IPEndPoint ipPort)
        {
            //消息格式为  消息类型:用户名:IP地址:端口:方块类型：操作消息
            string massage = string.Format("{0}:{1}:{2}:{3}:{4}:{5}", (int)type, hostName, IPHost.ToString(), myPort, blockTypes, msg);
            //将消息转换为可 网络发送的 字节
            byte[] msgs = Encoding.Default.GetBytes(massage);

            //发送        信息，  长度，   好友IP地址
            myClient.Send(msgs, msgs.Length, ipPort);
        }

        public void SendBroadcastMsg(MsgType type, int blockTypes, string msg)
        {
            //消息格式为  消息类型:用户名:IP地址:端口:方块类型：操作消息
            string massage = string.Format("{0}:{1}:{2}:{3}:{4}:{5}", (int)type, hostName, IPHost.ToString(), myPort, blockTypes, msg);
            //将消息转换为可 网络发送的 字节
            byte[] msgs = Encoding.Default.GetBytes(massage);

            //发送        信息，  长度，    广播地址
            myClient.Send(msgs, msgs.Length, IPEndBroadcast);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //显示邀请好友界面
            ff.ShowDialog();
            this.Focus();
        }

        #endregion
    }
}
