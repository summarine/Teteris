namespace 局域网
{
    partial class Friend
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lvFriend = new System.Windows.Forms.ListView();
            this.friendIP = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.friendName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // lvFriend
            // 
            this.lvFriend.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.friendIP,
            this.friendName});
            this.lvFriend.Location = new System.Drawing.Point(2, 12);
            this.lvFriend.Name = "lvFriend";
            this.lvFriend.Size = new System.Drawing.Size(506, 281);
            this.lvFriend.TabIndex = 18;
            this.lvFriend.UseCompatibleStateImageBehavior = false;
            this.lvFriend.View = System.Windows.Forms.View.Details;
            // 
            // friendIP
            // 
            this.friendIP.Text = "IPAddress";
            this.friendIP.Width = 300;
            // 
            // friendName
            // 
            this.friendName.Text = "好友名";
            this.friendName.Width = 318;
            // 
            // Friend
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 296);
            this.Controls.Add(this.lvFriend);
            this.Name = "Friend";
            this.Text = "Friend";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvFriend;
        private System.Windows.Forms.ColumnHeader friendIP;
        private System.Windows.Forms.ColumnHeader friendName;
    }
}