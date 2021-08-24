using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace client
{
    public partial class Form1 : Form
    {



        bool terminating = false;
        bool connected = false;
        Socket clientSocket;

        List<string> Notifs = new List<string>();
        //List<string> FriendList = new List<string>();
        String who = "";




        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            InitializeComponent();
        }

        private void button_connect_Click(object sender, EventArgs e)

        {
            Update_Panel();
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            string IP = textBox_ip.Text;
            string userName = user_name.Text;
            who = userName;         

            int portNum;
            if(Int32.TryParse(textBox_port.Text, out portNum))
            {
               
                try
                {
                    if (IP == "") { IP = "123131"; }
                    clientSocket.Connect(IP, portNum);
                  
              
                   

                    
                    if (userName != "" && userName.Length <= 64)
                    {
                        Byte[] buffer = new Byte[64];
                        buffer = Encoding.Default.GetBytes(userName);
                        clientSocket.Send(buffer);
                    }


                    Byte[] bufferrec = new Byte[64];
                    clientSocket.Receive(bufferrec);

                    string incomingMessage = Encoding.Default.GetString(bufferrec);
                    incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf("\0"));

                    if (incomingMessage == "Invalid request!")
                    {

                        logs.AppendText("Invalid Request \n" );
                        button_connect.Enabled = true;
                        textBox_message.Enabled = false;
                        button_send.Enabled = false;
                        connected = false;
                    }
                    else
                    {
                        button_connect.Enabled = false;
                        button1.Enabled = true;
                        textBox_message.Enabled = true;
                        button_send.Enabled = true;
                        connected = true;
                        textBox_ip.Enabled = false;
                        textBox_port.Enabled = false;
                        user_name.Enabled = false;
                        show_friends.Enabled = true;
                        Mess2Friend.Enabled = true;
                        Send2Friend.Enabled = true;
                        logs.AppendText("Connected to the server!\n");
                        Thread receiveThread = new Thread(Receive);
                        receiveThread.Start();
                    }



                }
                catch
                {
                    logs.AppendText("Could not connect to the server!\n");
                }
            }
            else
            {
                logs.AppendText("Check the port\n");
            }

        }

        private void Receive()
        {
            while(connected)
            {
                try
                {
                    Byte[] buffer = new Byte[64];
                    clientSocket.Receive(buffer);

                    string incomingMessage = Encoding.Default.GetString(buffer);
                    incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf("\0"));

                    if (incomingMessage.Length < 6 || (incomingMessage.Substring(0, 6) != "*#FD#*" && incomingMessage.Substring(0, 6) != "*#FR#*" && incomingMessage.Substring(0, 6) != "*#RP#*" && incomingMessage.Substring(0, 6) != "*#FL#*" && incomingMessage.Substring(0, 6) != "*#FX#*" && incomingMessage.Substring(0, 6) != "#*FF*#" && incomingMessage.Substring(0, 6) != "*#ST#*")) {
                        logs.AppendText(incomingMessage + "\n");
                    }

                    else if (incomingMessage.Substring(0, 6) == "*#ST#*")
                    {
                        Notifs.Add(incomingMessage);
                        Update_Panel();
                    }

                    else if(incomingMessage.Substring(0, 6) == "#*FF*#")
                    {
                        string sender = incomingMessage.Substring(6);

                        sender = "*#FL#*" + sender;

                        Notifs.Remove(sender);
                        Update_Panel();

                    }

                    else if (incomingMessage.Substring(0, 6) == "*#FR#*")
                    {


                        string temp = incomingMessage;
                        int PreviousEnd = 0;
                        for (int i = 6; i < temp.Length; i++)
                        {
                            if (temp.ElementAt(i) == '*')                               //ayni anda birden fazla gelirse
                            {
                                Notifs.Add(temp.Substring(PreviousEnd, i - PreviousEnd));
                                PreviousEnd = i;
                                i = i + 5;
                                
                            }
                        }
                        Notifs.Add(temp.Substring(PreviousEnd));
                        Update_Panel();
                    }
                    else if (incomingMessage.Substring(0, 6) == "*#RP#*")
                    {
                        Notifs.Add(incomingMessage);
                        Update_Panel();
                    }

                    else if (incomingMessage.Substring(0, 6) == "*#FL#*")
                    {
                        Notifs.Add(incomingMessage);
                        Update_Panel();

                    }
                    else if (incomingMessage.Substring(0, 6) == "*#FX#*")
                    {
                        Notifs.Add("*#FL#*"+incomingMessage.Substring(6));
                        Update_Panel();
                        logs.AppendText(incomingMessage.Substring(6) +" become friend with you \n");
                    }

                    else
                    {
                        incomingMessage = incomingMessage.Substring(6);
                        logs.AppendText(incomingMessage + "\n");
                    }
                }
                catch
                {
                    if (!terminating)
                    {
                        logs.AppendText("Disconnected from server \n");
                        button_connect.Enabled = true;
                        textBox_message.Enabled = false;
                        button_send.Enabled = false;
                        Mess2Friend.Enabled = false;
                        Send2Friend.Enabled = false;
                    }

                    clientSocket.Close();
                    connected = false;
                }

            }
        }

        private void Form1_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            connected = false;
            terminating = true;
            Environment.Exit(0);
        }

        private void button_send_Click(object sender, EventArgs e)
        {
            string message = textBox_message.Text;

            if(message != "" && message.Length <= 64)
            {
                Byte[] buffer = new Byte[64];
                buffer = Encoding.Default.GetBytes(message);
                clientSocket.Send(buffer);
                logs.SelectionColor = Color.Red;
                logs.AppendText("You: ");
                logs.SelectionColor = Color.Black;
                logs.AppendText(message);
                logs.AppendText("\n");
            }

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox_port_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox_ip_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
          
            button_connect.Enabled = true;
            textBox_message.Enabled = false;
            button_send.Enabled = false;
            connected = false;
            button1.Enabled = false;
            textBox_ip.Enabled = true;
            textBox_port.Enabled = true;
            user_name.Enabled = true;
            Mess2Friend.Enabled = false;
            Send2Friend.Enabled = false;

            labelF1.Hide();
            labelF2.Hide();
            text_add_friend.Hide();
            button_add_friend.Hide();
            FriendsList.Hide();
            show_friends.Show();
            show_notif.Hide();
            Invite_Panel.Show();
            show_friends.Enabled = false;

            Byte[] buffer = new Byte[64];
            buffer = Encoding.Default.GetBytes("Im disconnecting");

            try
            {
                clientSocket.Send(buffer);
            }

            catch
            {


            }
            clientSocket.Close();

            this.Invoke((MethodInvoker)delegate ()
            {
                Invite_Panel.Controls.Clear();
            });
            Notifs.Clear();

        }

        private void logs_TextChanged(object sender, EventArgs e)
        {

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            string message = "*#FD#*" + text_add_friend.Text;

            bool flag = Notifs.Contains("*#FR*#" + text_add_friend.Text);

            if (who==text_add_friend.Text)
            {

                logs.AppendText("you can't send request to yourself!\n");

            }

            else if (Notifs.Contains("*#FR#*"+text_add_friend.Text))
            {

                logs.AppendText("This is not allowed");


            }

           else if (message != "*#FD#*" && message.Length <= 64-12)
            {
                try
                {
                    Byte[] buffer = new Byte[64];
                    buffer = Encoding.Default.GetBytes(message);
                    clientSocket.Send(buffer);

                    String x = "2";
                    
                }

                catch(Exception)
                {

                    String x = "";

                }
                    
             
            }

        }

        private void show_friends_Click(object sender, EventArgs e)
        {
            labelF1.Show();
            labelF2.Show();
            text_add_friend.Show();
            button_add_friend.Show();
            FriendsList.Show();
            show_friends.Hide();
            show_notif.Show();
            Invite_Panel.Hide();
        }

        private void show_notif_Click(object sender, EventArgs e)
        {
            labelF1.Hide();
            labelF2.Hide();
            text_add_friend.Hide();
            button_add_friend.Hide();
            FriendsList.Hide();
            show_friends.Show();
            show_notif.Hide();
            Invite_Panel.Show();
        }

        private void Invite_Panel_Paint(object sender, PaintEventArgs e)
        {
            /*

            for (int i = 0; i < 20; i++)
            {

                CheckBox chb = new CheckBox();
                chb.Text = i.ToString();
                chb.Location = new Point(10, Invite_Panel.Controls.Count * 20);

                Invite_Panel.Controls.Add(chb);
            }
            
            //
            if (Notifs != null)
            {
                for (int i = 0; i < Notifs.Count; i++)
                {
                    if (Notifs[i].Substring(0, 6) == "*#FR#*")
                    {
                        Label lbl = new Label();
                        lbl.Text = Notifs[i].Substring(6);
                        lbl.Location = new Point(10, Invite_Panel.Controls.Count * 20);

                        Button AcceptInv = new Button();
                        AcceptInv.Text = "Accept";
                        AcceptInv.Location = new Point(110, (Invite_Panel.Controls.Count - 1) * 20);


                    }          
        }
        }
        */

        }

        private void Update_Panel()
        {
            int numFriends = 0;
            this.Invoke((MethodInvoker)delegate ()
            {
                Invite_Panel.Controls.Clear();
                FriendsList.Controls.Clear();
            });

            for (int i = 0; i < Notifs.Count; i++)
            {
                if (Notifs[i].Substring(0, 6) == "*#FR#*")
                {
                    Label lbl = new Label();
                    lbl.Text = Notifs[i].Substring(6) + " has sent you a friend request!";
                    lbl.Location = new Point(10, Invite_Panel.Controls.Count * 20);
                    lbl.AutoSize = true;

                    Button AcceptInv = new Button();
                    AcceptInv.Text = "Accept";
                    AcceptInv.Location = new Point(10, (Invite_Panel.Controls.Count+1)  * 20);
                    AcceptInv.Name = 'A' + Notifs[i].Substring(6);
                    AcceptInv.Click += Button_Click;

                    Button DeclineInv = new Button();
                    DeclineInv.Text = "Decline";
                    DeclineInv.Location = new Point(80, (Invite_Panel.Controls.Count+1) * 20);
                    DeclineInv.Name = 'D' + Notifs[i].Substring(6);
                    DeclineInv.Click += Button_Click;


                    this.Invoke((MethodInvoker)delegate ()
                    {


                        Invite_Panel.Controls.Add(lbl);
                        Invite_Panel.Controls.Add(AcceptInv);
                        Invite_Panel.Controls.Add(DeclineInv);

                    });
                }
                if (Notifs[i].Substring(0, 6) == "*#RP#*")
                {
                    Label lbl = new Label();
                    lbl.Text = Notifs[i].Substring(6);
                    lbl.Location = new Point(3, Invite_Panel.Controls.Count * 20);
                    lbl.AutoSize = true;

                    Button Affir = new Button();
                    Affir.Text = "X";
                    Affir.Location = new Point(3, (Invite_Panel.Controls.Count + 1) * 20);
                    Affir.Name = 'X' + Notifs[i].Substring(6);
                    Affir.Click += Button_Click2;
                    Size desiredSize = new Size(23, 23);
                    Affir.Size = desiredSize;
                    this.Invoke((MethodInvoker)delegate ()
                    {


                        Invite_Panel.Controls.Add(lbl);
                        Invite_Panel.Controls.Add(Affir);

                    });
                }
                if (Notifs[i].Substring(0, 6) == "*#ST#*")
                {
                    string sender = Notifs[i].Substring(6);

                    string reddedilmissin = sender + " has removed you from their friend list.";



                    Label lbl = new Label();
                    lbl.Text = reddedilmissin;
                    lbl.Location = new Point(3, Invite_Panel.Controls.Count * 20);
                    lbl.AutoSize = true;

                    Button Affir = new Button();
                    Affir.Text = "X";
                    Affir.Location = new Point(3, (Invite_Panel.Controls.Count + 1) * 20);
                    Affir.Name = 'X' + Notifs[i].Substring(6);
                    Affir.Click += Button_Click4;
                    Size desiredSize = new Size(23, 23);
                    Affir.Size = desiredSize;
                    this.Invoke((MethodInvoker)delegate ()
                    {


                        Invite_Panel.Controls.Add(lbl);
                        Invite_Panel.Controls.Add(Affir);

                    });

                }
                if(Notifs[i].Substring(0, 6) == "*#FL#*")
                {
                    Label lbl = new Label();
                    lbl.Text = Notifs[i].Substring(6);
                    lbl.Location = new Point(10, numFriends * 20);
                    lbl.AutoSize = true;

                    Button removeFriend = new Button();
                    removeFriend.Text = "Remove";
                    removeFriend.Location = new Point(123, (numFriends) * 20);
                    removeFriend.Name = 'X' + Notifs[i].Substring(6);
                    removeFriend.Click += Button_Click3;

                    numFriends++;

                    this.Invoke((MethodInvoker)delegate ()
                    {
                        FriendsList.Controls.Add(lbl);
                        FriendsList.Controls.Add(removeFriend);
                    });


                }

            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string s1 = btn.Name;

            //char AnsType = s1.ElementAt(0);


            s1 = "*#RP#*" + s1;


            Byte[] buffer = new Byte[64];
            buffer = Encoding.Default.GetBytes(s1);
            clientSocket.Send(buffer);

            s1 = "*#FR#*" + s1.Substring(7);
            Notifs.Remove(s1);
            Update_Panel();


        }

        private void Button_Click2(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string s1 = btn.Name;

            s1 = "#*AC*#" + s1.Substring(1);

            Byte[] buffer = new Byte[64];
            buffer = Encoding.Default.GetBytes(s1);
            clientSocket.Send(buffer);

            s1 = "*#RP#*" + s1.Substring(6);
            Notifs.Remove(s1);
            Update_Panel();
        }
        private void Button_Click3(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string s2 =  btn.Name.Substring(1);

            string s1 = "#*FF*#" + s2;

            s2 = "*#FL#*" + s2;

            Byte[] buffer = new Byte[64];
            buffer = Encoding.Default.GetBytes(s1);
            clientSocket.Send(buffer);

            Notifs.Remove(( s2));
            Update_Panel();
        }

        private void Button_Click4(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string s1 = "*#CA#*" + btn.Name.Substring(1);

            Byte[] buffer = new Byte[64];
            buffer = Encoding.Default.GetBytes(s1);
            clientSocket.Send(buffer);

            s1 = "*#ST#*" + s1.Substring(6);
            Notifs.Remove(s1);
            Update_Panel();
        }



        private void Send2Friend_Click(object sender, EventArgs e)
        {
            string message = "#*MG*#" + Mess2Friend.Text;

            Byte[] buffer = new Byte[64];
            buffer = Encoding.Default.GetBytes(message);
            clientSocket.Send(buffer);
        }
    }
}
