using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 局域网
{
    public partial class Friend : Form
    {
        public Friend()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 添加好友信息
        /// </summary>
        /// <param name="friendIP">好友IP</param>
        /// <param name="friendProt">好友端口</param>
        /// <param name="friendName">好友名</param>
        public void AddFriendInfo(string friendIP, int friendProt, string friendName)
        {
            //查找IP地址是否存在
            ListViewItem lvi = lvFriend.FindItemWithText(friendIP);
            //如果没有找到对应的IP地址
            if (lvi == null)
            {
                lvi = new ListViewItem(friendIP);
                lvi.SubItems.Add(friendName);
                lvi.Tag = friendProt;

                //添加到 界面
                lvFriend.Items.Add(lvi);
            }
        }

        public void RemoveFriend(string friendIP)
        {
            //查找IP地址是否存在
            ListViewItem lvi = lvFriend.FindItemWithText(friendIP);
            //如果没有找到对应的IP地址
            if (lvi != null)
            {
                lvFriend.FindItemWithText(friendIP).Remove();
            }
        }
    }
}
