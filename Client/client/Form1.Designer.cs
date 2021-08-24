namespace client
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_ip = new System.Windows.Forms.TextBox();
            this.textBox_port = new System.Windows.Forms.TextBox();
            this.button_connect = new System.Windows.Forms.Button();
            this.logs = new System.Windows.Forms.RichTextBox();
            this.textBox_message = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button_send = new System.Windows.Forms.Button();
            this.user_name = new System.Windows.Forms.TextBox();
            this.name = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.text_add_friend = new System.Windows.Forms.TextBox();
            this.button_add_friend = new System.Windows.Forms.Button();
            this.labelF1 = new System.Windows.Forms.Label();
            this.labelF2 = new System.Windows.Forms.Label();
            this.show_friends = new System.Windows.Forms.Button();
            this.show_notif = new System.Windows.Forms.Button();
            this.Invite_Panel = new System.Windows.Forms.Panel();
            this.FriendsList = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.Mess2Friend = new System.Windows.Forms.TextBox();
            this.Send2Friend = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(59, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(45, 95);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Port:";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // textBox_ip
            // 
            this.textBox_ip.Location = new System.Drawing.Point(89, 60);
            this.textBox_ip.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox_ip.Name = "textBox_ip";
            this.textBox_ip.Size = new System.Drawing.Size(120, 22);
            this.textBox_ip.TabIndex = 2;
            this.textBox_ip.TextChanged += new System.EventHandler(this.textBox_ip_TextChanged);
            // 
            // textBox_port
            // 
            this.textBox_port.Location = new System.Drawing.Point(89, 95);
            this.textBox_port.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox_port.Name = "textBox_port";
            this.textBox_port.Size = new System.Drawing.Size(120, 22);
            this.textBox_port.TabIndex = 3;
            this.textBox_port.TextChanged += new System.EventHandler(this.textBox_port_TextChanged);
            // 
            // button_connect
            // 
            this.button_connect.Location = new System.Drawing.Point(89, 206);
            this.button_connect.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_connect.Name = "button_connect";
            this.button_connect.Size = new System.Drawing.Size(93, 28);
            this.button_connect.TabIndex = 4;
            this.button_connect.Text = "connect";
            this.button_connect.UseVisualStyleBackColor = true;
            this.button_connect.Click += new System.EventHandler(this.button_connect_Click);
            // 
            // logs
            // 
            this.logs.Location = new System.Drawing.Point(331, 60);
            this.logs.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.logs.Name = "logs";
            this.logs.ReadOnly = true;
            this.logs.Size = new System.Drawing.Size(249, 320);
            this.logs.TabIndex = 5;
            this.logs.Text = "";
            this.logs.TextChanged += new System.EventHandler(this.logs_TextChanged);
            // 
            // textBox_message
            // 
            this.textBox_message.Enabled = false;
            this.textBox_message.Location = new System.Drawing.Point(89, 297);
            this.textBox_message.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox_message.Name = "textBox_message";
            this.textBox_message.Size = new System.Drawing.Size(129, 22);
            this.textBox_message.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 300);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 17);
            this.label3.TabIndex = 7;
            this.label3.Text = "Message:";
            // 
            // button_send
            // 
            this.button_send.Enabled = false;
            this.button_send.Location = new System.Drawing.Point(227, 292);
            this.button_send.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_send.Name = "button_send";
            this.button_send.Size = new System.Drawing.Size(87, 32);
            this.button_send.TabIndex = 8;
            this.button_send.Text = "send";
            this.button_send.UseVisualStyleBackColor = true;
            this.button_send.Click += new System.EventHandler(this.button_send_Click);
            // 
            // user_name
            // 
            this.user_name.Location = new System.Drawing.Point(89, 130);
            this.user_name.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.user_name.Name = "user_name";
            this.user_name.Size = new System.Drawing.Size(120, 22);
            this.user_name.TabIndex = 9;
            this.user_name.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // name
            // 
            this.name.AutoSize = true;
            this.name.Location = new System.Drawing.Point(35, 130);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(49, 17);
            this.name.TabIndex = 11;
            this.name.Text = "Name:";
            this.name.Click += new System.EventHandler(this.label4_Click);
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(89, 240);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 28);
            this.button1.TabIndex = 12;
            this.button1.Text = "disconnect";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // text_add_friend
            // 
            this.text_add_friend.Location = new System.Drawing.Point(611, 80);
            this.text_add_friend.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.text_add_friend.Name = "text_add_friend";
            this.text_add_friend.Size = new System.Drawing.Size(120, 22);
            this.text_add_friend.TabIndex = 15;
            this.text_add_friend.Visible = false;
            // 
            // button_add_friend
            // 
            this.button_add_friend.Location = new System.Drawing.Point(740, 80);
            this.button_add_friend.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_add_friend.Name = "button_add_friend";
            this.button_add_friend.Size = new System.Drawing.Size(85, 23);
            this.button_add_friend.TabIndex = 16;
            this.button_add_friend.Text = "Add Friend";
            this.button_add_friend.UseVisualStyleBackColor = true;
            this.button_add_friend.Visible = false;
            this.button_add_friend.Click += new System.EventHandler(this.Button2_Click);
            // 
            // labelF1
            // 
            this.labelF1.AutoSize = true;
            this.labelF1.Location = new System.Drawing.Point(611, 60);
            this.labelF1.Name = "labelF1";
            this.labelF1.Size = new System.Drawing.Size(89, 17);
            this.labelF1.TabIndex = 17;
            this.labelF1.Text = "Add a Friend";
            this.labelF1.Visible = false;
            // 
            // labelF2
            // 
            this.labelF2.AutoSize = true;
            this.labelF2.Location = new System.Drawing.Point(613, 114);
            this.labelF2.Name = "labelF2";
            this.labelF2.Size = new System.Drawing.Size(81, 17);
            this.labelF2.TabIndex = 19;
            this.labelF2.Text = "Friends List";
            this.labelF2.Visible = false;
            // 
            // show_friends
            // 
            this.show_friends.Enabled = false;
            this.show_friends.Location = new System.Drawing.Point(676, 386);
            this.show_friends.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.show_friends.Name = "show_friends";
            this.show_friends.Size = new System.Drawing.Size(149, 25);
            this.show_friends.TabIndex = 20;
            this.show_friends.Text = "Friends Panel";
            this.show_friends.UseVisualStyleBackColor = true;
            this.show_friends.Click += new System.EventHandler(this.show_friends_Click);
            // 
            // show_notif
            // 
            this.show_notif.Location = new System.Drawing.Point(676, 386);
            this.show_notif.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.show_notif.Name = "show_notif";
            this.show_notif.Size = new System.Drawing.Size(149, 25);
            this.show_notif.TabIndex = 21;
            this.show_notif.Text = "Notifications Panel";
            this.show_notif.UseVisualStyleBackColor = true;
            this.show_notif.Visible = false;
            this.show_notif.Click += new System.EventHandler(this.show_notif_Click);
            // 
            // Invite_Panel
            // 
            this.Invite_Panel.AutoScroll = true;
            this.Invite_Panel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Invite_Panel.Location = new System.Drawing.Point(600, 60);
            this.Invite_Panel.Margin = new System.Windows.Forms.Padding(11, 10, 11, 10);
            this.Invite_Panel.Name = "Invite_Panel";
            this.Invite_Panel.Size = new System.Drawing.Size(329, 320);
            this.Invite_Panel.TabIndex = 22;
            this.Invite_Panel.Paint += new System.Windows.Forms.PaintEventHandler(this.Invite_Panel_Paint);
            // 
            // FriendsList
            // 
            this.FriendsList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.FriendsList.Location = new System.Drawing.Point(611, 135);
            this.FriendsList.Margin = new System.Windows.Forms.Padding(11, 10, 11, 10);
            this.FriendsList.Name = "FriendsList";
            this.FriendsList.Size = new System.Drawing.Size(308, 238);
            this.FriendsList.TabIndex = 23;
            this.FriendsList.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 353);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 17);
            this.label4.TabIndex = 24;
            this.label4.Text = "All Friends:";
            // 
            // Mess2Friend
            // 
            this.Mess2Friend.Enabled = false;
            this.Mess2Friend.Location = new System.Drawing.Point(89, 350);
            this.Mess2Friend.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Mess2Friend.Name = "Mess2Friend";
            this.Mess2Friend.Size = new System.Drawing.Size(129, 22);
            this.Mess2Friend.TabIndex = 25;
            // 
            // Send2Friend
            // 
            this.Send2Friend.Enabled = false;
            this.Send2Friend.Location = new System.Drawing.Point(227, 350);
            this.Send2Friend.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Send2Friend.Name = "Send2Friend";
            this.Send2Friend.Size = new System.Drawing.Size(87, 32);
            this.Send2Friend.TabIndex = 26;
            this.Send2Friend.Text = "send";
            this.Send2Friend.UseVisualStyleBackColor = true;
            this.Send2Friend.Click += new System.EventHandler(this.Send2Friend_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 333);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 17);
            this.label5.TabIndex = 27;
            this.label5.Text = "Message to";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(949, 441);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Send2Friend);
            this.Controls.Add(this.Mess2Friend);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.FriendsList);
            this.Controls.Add(this.show_friends);
            this.Controls.Add(this.labelF2);
            this.Controls.Add(this.labelF1);
            this.Controls.Add(this.button_add_friend);
            this.Controls.Add(this.text_add_friend);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.name);
            this.Controls.Add(this.user_name);
            this.Controls.Add(this.button_send);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox_message);
            this.Controls.Add(this.logs);
            this.Controls.Add(this.button_connect);
            this.Controls.Add(this.textBox_port);
            this.Controls.Add(this.textBox_ip);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Invite_Panel);
            this.Controls.Add(this.show_notif);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_ip;
        private System.Windows.Forms.TextBox textBox_port;
        private System.Windows.Forms.Button button_connect;
        private System.Windows.Forms.RichTextBox logs;
        private System.Windows.Forms.TextBox textBox_message;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button_send;
        private System.Windows.Forms.TextBox user_name;
        private System.Windows.Forms.Label name;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TextBox text_add_friend;
        private System.Windows.Forms.Button button_add_friend;
        private System.Windows.Forms.Label labelF1;
        private System.Windows.Forms.Label labelF2;
        private System.Windows.Forms.Button show_friends;
        private System.Windows.Forms.Button show_notif;
        private System.Windows.Forms.Panel Invite_Panel;
        private System.Windows.Forms.Panel FriendsList;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox Mess2Friend;
        private System.Windows.Forms.Button Send2Friend;
        private System.Windows.Forms.Label label5;
    }
}

