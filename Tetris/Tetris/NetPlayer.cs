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
using System.Threading;
using System.Windows;
namespace Tetris
{
    public class NetPlayer
    {
        public NetPlayer()
        {
            // TODO: Complete member initialization
        }
        public void InitNetPlayer(Grid mygrid, Grid friendgrid, Label mylabel, Label friendlabel, Image myPreviewImage, int row, int column)
        {
            //初始化
            myGame = new MyGameFrame(mygrid, row, column);
            friendGame = new GameFrame(friendgrid, row, column);
            myScoringBoard = new DoubleScoringBoard(mylabel);
            friendScoringBoard = new DoubleScoringBoard(friendlabel);
            myPreviewWindow = new PreviewWindow(myGame, myPreviewImage);

            //行消除事件
            myGame.RowsCleanEvent += myScoringBoard.GetScore;
            friendGame.RowsCleanEvent += friendScoringBoard.GetScore;

            //游戏结束时间
            myGame.GameOverEvent += myScoringBoard.WhenGameOver;
            myGame.GameOverEvent += WhenGameOver;
            friendGame.GameOverEvent += friendScoringBoard.WhenGameOver;
            friendGame.GameOverEvent += WhenGameOver;

            //得足够得分使对面增加无用行,注:生成一个无用行，传给对面用
            friendScoringBoard.Threshold = myScoringBoard.Threshold = 400;
            myScoringBoard.CrossThreshold += new ScoreEventHandler((a,e)=>{
                int holesCnt = 4;
                Random rand = new Random();
                List<int> uselessRow = new List<int>();
                for (int i = 0; i < column; i++)
                    uselessRow.Add(1);
                for (int i = 0; i < holesCnt; i++)
                {
                    int t = rand.Next(column);
                    uselessRow[t] = 0;
                }
                
                string s = "ct:";
                for (int i = 0; i < column; i++)
                    s += uselessRow[i].ToString() + ",";
                SendClientMsg(s);
                friendGame.AddUselessLines(uselessRow);
            });

            //挂载方块移动事件，以发送移动消息
            myGame.ActiveBoxMoving += MyGame_ActiveBox_Moving;
            myGame.ActiveBoxCrushEvent += MyGame_ActiveBoxCrushing;
        }

        private void MyGame_ActiveBoxCrushing(object sender, MoveEventArgs e)
        {
            string msg = "bc:" + e.ToString();
            SendClientMsg(msg);
        }

        private void MyGame_ActiveBox_Moving(object sender, MoveEventArgs e)
        {
            string msg = "mv:" + e.ToString();
            SendClientMsg(msg);
        }

        public int Readycount
        {
            set
            {
                readycount = value;
                if (readycount == 2)
                {
                    //Console.WriteLine("ReadyCount={0}", readycount);
                    Start();
                    //readycount = 0;
                }
            }
            get
            {
                return readycount;
            }
        }
        private int readycount;
        static protected byte[] buffer = new byte[1024];
        protected Queue<SocketMessage> msgPool = new Queue<SocketMessage>();
        public Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public void ReceiveMessage(IAsyncResult ar)
        {
            var socket1 = ar.AsyncState as Socket;
            var length = socket1.EndReceive(ar);

            try
            {
                var msg = Encoding.Unicode.GetString(buffer, 0, length);
                
                Thread th = new Thread(() =>
                {
                    DealMessage(msg);
                });
                th.Start();

                socket1.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveMessage), socket1);
            }
            catch
            {
                socket1.Disconnect(true);
            }
        }

        public void DealMessage(string msg)
        {
            string[] MSGS = msg.Split(':');
            //将获得的第一个 参数 转换为消息类型
            SocketMessage sm = new SocketMessage();
            sm.et = MSGS[0];
            sm.Message = MSGS[1];
            Console.WriteLine(msg);
            
            //lock (msgPool)
            {
                msgPool.Enqueue(sm);
                //Console.WriteLine(msg);

                while (msgPool.Count != 0)
                {
                    SocketMessage head = msgPool.Dequeue();

                    if (head.et == "operation")
                    {
                        if (head.Message == "1")
                            friendGame.KeyDown(Key.Up);
                        else if (head.Message == "2")
                            friendGame.KeyDown(Key.Down);
                        else if (head.Message == "3")
                            friendGame.KeyDown(Key.Left);
                        else if (head.Message == "4")
                            friendGame.KeyDown(Key.Right);
                    }
                    else if (head.et == "state")
                        friendGame.GameOver();
                    else if (head.et == "allow")
                    {
                        //BuildWin();
                    }
                    else if (head.et == "seed")
                    {
                        int seed = System.Int32.Parse(head.Message);
                        myGame.SetFactorySeed(seed);
                        friendGame.SetFactorySeed(seed);
                    }
                    else if (head.et == "game")
                    {
                        if (head.Message == "start")
                        {
                            Readycount++;
                        }
                    }
                    else if (head.et == "mv")
                    {
                        MoveEventArgs e = new MoveEventArgs(head.Message);
                        friendGame.ActiveBoxPositionChanged(this, e);
                    }
                    else if (head.et == "bc")
                    {
                        MoveEventArgs e = new MoveEventArgs(head.Message);
                        friendGame.ActiveBoxCrush(this, e.next);
                    }
                    else if (head.et == "ct")
                    {
                        string[] s=head.Message.Split(',');
                        List<int> h = new List<int>();
                        for (int i = 0; i < s.Length;i++ )
                        {
                            if (s[i]!="")
                                h.Add(System.Int32.Parse(s[i]));
                        }
                        myGame.AddUselessLines(h);
                    }
                    else
                    {
                        Console.WriteLine("Unknow Message:" + head.et);
                    }
                }
            }
        }

        protected GameFrame myGame;
        protected GameFrame friendGame;
        protected DoubleScoringBoard myScoringBoard;
        protected DoubleScoringBoard friendScoringBoard;
        protected PreviewWindow myPreviewWindow;
        public void Start()
        {
            Console.WriteLine("Game Start");
            friendGame.ClearMap();
            myGame.Start();
            //friendGame.Start();
        }

        public void Stop()
        {
            myGame.Stop();
            //friendGame.Stop();
        }

        public void WhenGameOver(Object sender,EventArgs e)
        {
            Stop();

            if (sender == myGame)
            {
                MessageBox.Show("输");
                SendGameOver();
            }
            else
            {
                MessageBox.Show("赢");
            }

        }

        virtual public void SendClientMsg(string msg)
        {
            var outputBuffer = Encoding.Unicode.GetBytes(msg);
            socket.BeginSend(outputBuffer, 0, outputBuffer.Length, SocketFlags.None, null, null);
        }

        public void SendGameOver()
        {
            string msg = "state:gameover";
            SendClientMsg(msg);
        }

        public void KeyDown(Key key)
        {
            myGame.KeyDown(key);

            //if (Key.Up == key)
            //{
            //    SendClientMsg("operation:1");
            //}
            //else if (Key.Down == key)
            //{
            //    SendClientMsg("operation:2");
            //}
            //else if (Key.Left == key)
            //{
            //    SendClientMsg("operation:3");
            //}
            //else if (Key.Right == key)
            //{
            //    SendClientMsg("operation:4");
            //}
        }

        public void BuildWin()
        {
            System.Windows.Application.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                new DeleFunc(Func));
        }
        public delegate void DeleFunc();
        virtual public void Func()
        {
            NetGame win = new NetGame(this);
            win.Show();
        }

        virtual public void Ready()
        {
            Random rand = new Random();
            int seed = rand.Next();
            SendClientMsg("seed:" + seed.ToString());
            myGame.SetFactorySeed(seed);
            friendGame.SetFactorySeed(seed);
            
            SendClientMsg("game:start");
            Readycount++;
        }
    }
}
