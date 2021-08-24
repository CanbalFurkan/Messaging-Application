using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace server
{
    public partial class 
        Form1 : Form
    {

        class messages
        {
            public string sender;
            public string message;


        }

   
        class friend_request
        {
            public string receiver;
            private List<string> sender = new List<string>();
            public List<string> Sender
            {
                get { return sender; }
                set { sender = value; }
            } 
        }
        class notificationForSender
        {
            public string sender;
            private List<string> message = new List<string>();
            public List<string> Message
            {
                get { return message; }
                set { message = value; }
            }
        }
        class userInfo
        {
            public string name;
            private List<string> friends = new List<string>();
            private List<messages> msg =new List<messages> ();
            public List <messages> msgg
            {

                get { return msg; }
                set { msg = value; }

            }
            public List<string> Friends
            {
                get { return friends; }
                set { friends = value; }
            }


        }

        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        List<Socket> clientList = new List<Socket>();
        List<Client> client_list = new List<Client>();

        List<userInfo> UserDB = new List<userInfo>();
        List<friend_request> requests = new List<friend_request>();
        List<notificationForSender> notifications = new List<notificationForSender>();
    

        bool terminating = false;
        bool listening = false;
       

        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            InitializeComponent();


            ////////////
            string line = "";
            var path = Path.Combine(Directory.GetCurrentDirectory(), "user_db.txt");
            System.IO.StreamReader txtReader = new System.IO.StreamReader(path);
            while ((line = txtReader.ReadLine()) != null)
            {
                userInfo temp = new userInfo();
                temp.name = line;
                UserDB.Add(temp);
            }

            txtReader.Close();




            ///////////////



        }

        private void button_listen_Click(object sender, EventArgs e)
        {
            int serverPort;

            if(Int32.TryParse(textBox_port.Text, out serverPort))
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, serverPort);
                serverSocket.Bind(endPoint);
                serverSocket.Listen(100);
              
                listening = true;
                button_listen.Enabled = false;
                textBox_message.Enabled = true;
                button_send.Enabled = true;

                Thread acceptThread = new Thread(Accept);
                acceptThread.Start();

                logs.AppendText("Started listening on port: " + serverPort + "\n");

            }
            else
            {
                logs.AppendText("Please check port number \n");
            }
        }

        private void Accept()
        {
            


                while (listening)
            {
                try
                {
                    Socket newClient = serverSocket.Accept();
                    clientList.Add(newClient);
                    logs.AppendText("A client is trying to connect\n");
                    Byte[] buffer = new Byte[64];
                    newClient.Receive(buffer);

                    string incomingMessage = Encoding.Default.GetString(buffer);
                    incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf("\0"));
                    Client cl = new Client();

                    bool check = false;
                    for(int i = 0; i < UserDB.Count; i++)
                    {
                        if (UserDB[i].name == incomingMessage)
                            check = true;
                    }


                    //if (list.Contains("***"+incomingMessage+"***")&&!client_list.Exists(e=>e.name==incomingMessage))
                    //if (UserDB.Contains(incomingMessage) && !client_list.Exists(e => e.name == incomingMessage))
                    if (check && !client_list.Exists(e => e.name == incomingMessage))
                    {
                       
                       // cl.client_socket = serverSocket.Accept();
                        cl.name = incomingMessage;
                        cl.client_socket = newClient;
                       
                        client_list.Add(cl);
                        byte[] mes = Encoding.Default.GetBytes("Connection is succesfull \n");
                        logs.AppendText(cl.name+ " has connected.\n");
                        newClient.Send(mes);
                        Brodcast(cl," is connected.");

                        Thread receiveThread = new Thread(()=>Receive(cl));
                        receiveThread.Start();
                        Notifications(cl);
                        friendmessage(cl);

                    }

                    else
                    {
                        byte[] error = Encoding.Default.GetBytes("Invalid request!");
                        newClient.Send(error);
                        newClient.Close();
                    }


                }



                catch
                {
                    if (terminating)
                    {
                        listening = false;
                    }
                    else
                    {
                        logs.AppendText("The socket stopped working.\n");
                    }

                }
            }
        }

        private void Receive(Client client)
        {
            
            Socket thisClient = clientList[clientList.Count() - 1];
            bool connected = true;
         

            while (connected && !terminating)
            {


               
                    try

                    {
                        Byte[] buffer = new Byte[64];
                        thisClient.Receive(buffer);

                        string incomingMessage = Encoding.Default.GetString(buffer);
                        incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf("\0"));
                    if (incomingMessage != "Im disconnecting")
                    {
                        if (incomingMessage.Length < 6 || (incomingMessage.Substring(0, 6) != "*#FD#*" && incomingMessage.Substring(0, 6) != "*#RP#*" && incomingMessage.Substring(0, 6) != "#*AC*#" && incomingMessage.Substring(0, 6) != "#*MG*#" && incomingMessage.Substring(0, 6) != "#*FF*#" && incomingMessage.Substring(0, 6) != "*#CA#*"))
                        {
                            Brodcast(client, incomingMessage);
                            logs.AppendText(client.name + ":" + incomingMessage + "\n");
                        }

                        else if(incomingMessage.Substring(0, 6) == "*#CA#*")
                        {
                            incomingMessage = "*#ST#*" + incomingMessage.Substring(6);
                            for (int i = 0; i < notifications.Count; i++)
                            {
                                if (notifications[i].sender == client.name)
                                {
                                    notifications[i].Message.Remove(incomingMessage);
                                }
                            }

                        }
                        
                        else if(incomingMessage.Substring(0, 6) == "#*FF*#")
                        {
                            string sender = client.name;
                            string oldFriend = incomingMessage.Substring(6);
                            
                            foreach(userInfo x in UserDB)
                            {
                                if (x.name == sender)
                                    x.Friends.Remove(oldFriend);
                                else if(x.name == oldFriend)
                                {
                                    x.Friends.Remove(sender);

                                    try
                                    {
                                        foreach (Client y in client_list) {
                                            if(y.name == oldFriend)
                                            {
                                                Byte[] buffer2 = Encoding.Default.GetBytes("#*FF*#" + sender);
                                                y.client_socket.Send(buffer2);
                                            }
                                        }
                                    }
                                    catch { }
                                    string reddedildin = "*#ST#*" + sender;
                                    bool addedNotif = false;
                                    foreach(notificationForSender y in notifications)
                                    {
                                        if (y.sender == oldFriend) { 
                                            y.Message.Add(reddedildin);
                                            addedNotif = true;
                                        }
                                    }
                                    if (!addedNotif)
                                    {
                                        notificationForSender toBeAdded = new notificationForSender();
                                        toBeAdded.sender = oldFriend;
                                        toBeAdded.Message.Add(reddedildin);
                                        notifications.Add(toBeAdded);
                                    }

                                    try
                                    {
                                        foreach (Client y in client_list)
                                        {
                                            if (y.name == oldFriend)
                                            {
                                                Byte[] buffer2 = Encoding.Default.GetBytes(reddedildin);
                                                y.client_socket.Send(buffer2);
                                            }
                                        }
                                    }
                                    catch { }
                                }
                            }

                        }

                        else if (incomingMessage.Substring(0, 6) == "#*MG*#")
                        {

                            incomingMessage = incomingMessage.Substring(6);
                            Byte[] msg = Encoding.Default.GetBytes(client.name+": "+incomingMessage);

                            userInfo sender = new userInfo();

                            foreach (userInfo x in UserDB)
                            {
                                if (client.name == x.name)
                                {
                                    sender = x;

                                }
                            }


                            foreach (String x in sender.Friends)
                            {
                                userInfo rec = new userInfo();
                                Client rec2 = new Client();

                                foreach (userInfo t in UserDB)
                                {
                                    if (t.name == x)
                                    {
                                        rec = t;

                                    }
                                }

                                bool flag = false;

                                foreach (Client y in client_list) {

                                    if (y.name == rec.name)
                                    {
                                        rec2=y;
                                        flag = true;
                                        rec2.client_socket.Send(msg);

                                    }

                                }
                                if (flag == false)
                                {
                                    foreach (userInfo y in UserDB)
                                    {

                                        if (y.name == rec.name)
                                        {
                                            messages t = new messages();
                                            t.message = incomingMessage;
                                            t.sender = client.name;
                                            y.msgg.Add(t);

                                        }

                                    }

                                }
                            
                            }
                        }

                        else if(incomingMessage.Substring(0, 6) == "*#RP#*")
                        {
                            incomingMessage = incomingMessage.Substring(6);
                            char AnsType = incomingMessage.ElementAt(0);
                            string tempSender = incomingMessage.Substring(1);
                            string tempReceiver = client.name;
                            string tobeSent = "";
                            string tobeSentLater = "";
                            if (AnsType == 'A')
                            {
                                for (int i = 0; i < UserDB.Count; i++)
                                {
                                    if (UserDB[i].name == tempSender)
                                        UserDB[i].Friends.Add(tempReceiver);
                                    if (UserDB[i].name == tempReceiver)
                                        UserDB[i].Friends.Add(tempSender);
                                }
                                tobeSent = "You have accepted " + tempSender + "'s friendship invitation.\n";
                                tobeSentLater = "*#RP#*" + tempReceiver + " has accepted your friendship invitation.\n";
                                friendaccept(tempSender, tempReceiver);
                            }

                            else
                            {
                                tobeSent = "You have rejected " + tempSender + "'s friendship invitation.\n";
                                tobeSentLater = "*#RP#*" + tempReceiver + " has rejected your friendship invitation.\n";
                                friendreject(tempSender, tempReceiver,tobeSent, tobeSentLater);
                            }

                            for (int i = 0; i < requests.Count; i++)
                            {
                                if (requests[i].receiver == tempReceiver)   //deletes the request in both cases
                                    requests[i].Sender.Remove(tempSender);
                            }

                            buffer = Encoding.Default.GetBytes(tobeSent);
                            client.client_socket.Send(buffer);
                            bool check = false;
                            for (int i = 0; i < notifications.Count; i++)
                            {
                                if (notifications[i].sender == tempSender)
                                {
                                    check = true;
                                    notifications[i].Message.Add(tobeSentLater);
                                    }
                            }
                            if (!check)
                            {
                                notificationForSender tempNot = new notificationForSender();
                                tempNot.sender = tempSender;
                                tempNot.Message.Add(tobeSentLater);
                                notifications.Add(tempNot);
                            }


                        }

                        else if(incomingMessage.Substring(0, 6) == "#*AC*#")
                        {
                            incomingMessage = "*#RP#*" + incomingMessage.Substring(6);
                            for(int i = 0; i < notifications.Count; i++)
                            {
                                if (notifications[i].sender == client.name)
                                {
                                    notifications[i].Message.Remove(incomingMessage);
                                }
                            }
                        }

                        else
                        {
                            string response = "";
                            string senderofReq = client.name;
                            string receiverofReq = incomingMessage.Substring(6);

                            bool check2 = false;
                            for(int i =0; i < UserDB.Count; i++)
                            {
                                if (UserDB[i].name == receiverofReq)
                                    check2 = true;
                            }

                            if (check2)
                            {

                                bool enough = false;
                                foreach (friend_request x in requests)
                                {
                                    if (x.receiver == senderofReq)
                                    {
                                        if (x.Sender.Contains(receiverofReq))
                                        {
                                            enough = true;
                                        }
                                    }
                                }

                                bool foundReq = false;
                                for (int i = 0; !foundReq && i < requests.Count; i++)
                                {
                                    if (requests[i].receiver == receiverofReq)
                                    {
                                        foundReq = true;

                                        if (requests[i].Sender.Contains(senderofReq))
                                        {
                                            response = "*#FD#*This request is not possible";

                                        }
                                        else
                                        {
                                            if (enough)
                                            {
                                                response = "*#FD#*Already friend with him/her";
                                            }

                                            else
                                            {
                                                requests[i].Sender.Add(senderofReq);
                                                response = "*#FD#*Your friendship request has been sent!";


                                                Client target = new Client();
                                                Client source = new Client();

                                                foreach (Client t in client_list)
                                                {
                                                    if (t.name == receiverofReq)
                                                    {

                                                        target = t;
                                                    }

                                                }

                                                foreach (Client s in client_list)
                                                {
                                                    if (s.name == senderofReq)
                                                    {

                                                        source = s;
                                                    }

                                                }


                                                pm(target, source);





                                            }



                                        }


                                    }
                                }

                                userInfo rec = new userInfo();          //user info mu gönderene eşitledim
                                foreach (userInfo x in UserDB)
                                {
                                    if (x.name == receiverofReq)
                                    {

                                        rec = x;
                                    }

                                }



                                    if (!foundReq&&!rec.Friends.Contains(senderofReq)&&!enough)
                                {
                                    friend_request newReq = new friend_request();
                                    newReq.receiver = receiverofReq;
                                    newReq.Sender.Add(senderofReq);
                                    requests.Add(newReq);
                                    response = "*#FD#*Your friendship request has been sent!";
                                    Client target = new Client();
                                    Client source = new Client();
                                    
                                    foreach (Client t in client_list)
                                    {
                                        if (t.name == receiverofReq) {

                                            target = t;
                                        }
                                        
                                    }

                                    foreach (Client s in client_list)
                                    {
                                        if (s.name == senderofReq) 
                                        {

                                            source = s;
                                        }

                                    }


                                    pm(target, source);


                                }
                                else if(rec.Friends.Contains(senderofReq))
                                {
                                    response = "*#FD#*Already friend with him/her";

                                }
                            }
                            else
                            {

                                response = "*#FD#*This user does not exits";

                            }
                            if (response != "")
                            {
                                Byte[] buffer2 = new Byte[64];
                                buffer2 = Encoding.Default.GetBytes(response);
                                client.client_socket.Send(buffer2);
                            }
                        }/////////////////////////////////////////////////////////////////
                    }
                    else
                    {

                        dc(client);

                    }
                    
                    }
                    catch
                    {
                        if (!terminating)
                        {
                            logs.AppendText(client.name+": has disconnected.\n");
                            //Brodcast(client, ": I got disconnected");
                        }
                    client_list.Remove(client);
                    thisClient.Close();
                        
                        connected = false;
                    }
                
            }
            
        }

        private void Form1_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            listening = false;
            terminating = true;
            Environment.Exit(0);
        }

        private void Brodcast(Client C,string message)
        {

                
            if (message != "" && message.Length <= 64)
            {
                Byte[] buffer = Encoding.Default.GetBytes(C.name+":"+message);
                foreach (Client client in client_list)
                {
                    if (client.name != C.name)
                    {
                        try
                        {
                            client.client_socket.Send(buffer);
                        }
                        catch
                        {
                            logs.AppendText("There is a problem! Check the connection...\n");
                            terminating = true;
                            textBox_message.Enabled = false;
                            button_send.Enabled = false;
                            textBox_port.Enabled = true;
                            button_listen.Enabled = true;
                            serverSocket.Close();
                        }
                    }

                }




            }
        }

        private void friendmessage(Client x)
        {
           
            userInfo rec = new userInfo();
            foreach (userInfo current in UserDB)
            {
               
                if (x.name == current.name)
                {

                    rec = current;

                }



            }

           int sizeoflist=rec.msgg.Count();

            if (sizeoflist > 0)
            {
                for(int l=0; l < sizeoflist; l++)
                {

                    string message = rec.msgg[l].message;
                    string sender = rec.msgg[l].sender;
                    string msg2send = sender + " :" + " " + message;
                    Byte[] buffer = Encoding.Default.GetBytes(msg2send);
                    x.client_socket.Send(buffer);
                    System.Threading.Thread.Sleep(100);


                }
                rec.msgg.Clear();


            }



        }

        private void Notifications(Client C)
        {
            string ClientName = C.name;
            string SenderTemp = "";
            for(int i = 0; i < requests.Count; i++)
            {
                if(requests[i].receiver == ClientName)
                {
                    for(int j = 0; j < requests[i].Sender.Count; j++)
                    {
                        SenderTemp = requests[i].Sender[j];
                        SenderTemp = "*#FR#*" + SenderTemp;
                        Byte[] buffer = new Byte[64];
                        buffer = Encoding.Default.GetBytes(SenderTemp);
                        C.client_socket.Send(buffer);
                        System.Threading.Thread.Sleep(100);

                    }
                }
            }
            for(int i = 0; i < notifications.Count; i++)
            {
                if (notifications[i].sender == ClientName)
                {
                    for (int j = 0; j < notifications[i].Message.Count; j++)
                    {
                        SenderTemp = notifications[i].Message[j];
                        //SenderTemp = "*#RP#*" + SenderTemp;
                        Byte[] buffer = new Byte[64];
                        buffer = Encoding.Default.GetBytes(SenderTemp);
                        C.client_socket.Send(buffer);
                        System.Threading.Thread.Sleep(100);

                    }
                }

            }
            for(int i=0; i < UserDB.Count; i++)
            {
                if (UserDB[i].name == C.name)
                {
                    for(int j=0; j < UserDB[i].Friends.Count; j++)
                    {
                        string FriendTemp = UserDB[i].Friends[j];
                        FriendTemp = "*#FL#*" + FriendTemp;
                        Byte[] buffer = new Byte[64];
                        buffer = Encoding.Default.GetBytes(FriendTemp);
                        C.client_socket.Send(buffer);
                        System.Threading.Thread.Sleep(100);

                    }

                }

            }


        }

        private void pm(Client Target,Client Source)
        {

            String SenderTemp = "";
            try
            {
                SenderTemp = "*#FR#*" + Source.name;
                Byte[] buffer = new Byte[64];
                buffer = Encoding.Default.GetBytes(SenderTemp);
                Target.client_socket.Send(buffer);
                System.Threading.Thread.Sleep(100);
            }
            catch
            {}
        }

        private void pm2(Client Target, Client Source)
        {

            String SenderTemp = "";
            try
            {
                SenderTemp = "*#FR#*" + Source.name;
                Byte[] buffer = new Byte[64];
                buffer = Encoding.Default.GetBytes(SenderTemp);
                Target.client_socket.Send(buffer);
                System.Threading.Thread.Sleep(100);
            }
            catch
            { }
        }

        private void button_send_Click(object sender, EventArgs e)
        {
            string message = textBox_message.Text;
            if(message != "" && message.Length <= 64)
            {
                Byte[] buffer = Encoding.Default.GetBytes(message);
                foreach (Socket client in clientList)
                {
                    try
                    {
                        client.Send(buffer);
                    }
                    catch
                    {
                        logs.AppendText("There is a problem! Check the connection...\n");
                        terminating = true;
                        textBox_message.Enabled = false;
                        button_send.Enabled = false;
                        textBox_port.Enabled = true;
                        button_listen.Enabled = true;
                        serverSocket.Close();
                    }

                }
            }
        }

        private void alreadyfriend(Client c)
        {

            String SenderTemp = "";
            try
            {
                SenderTemp = "The user you are trying to send a request to is already in your friend list!";
                Byte[] buffer = new Byte[64];
                buffer = Encoding.Default.GetBytes(SenderTemp);
                c.client_socket.Send(buffer);
                System.Threading.Thread.Sleep(100);
            }
            catch
            {



            }

        }
        private void dc(Client c)
        {

            Brodcast(c, ": has disconnected");
            c.client_socket.Close();
            client_list.Remove(c);
           


        }

        private void friendaccept(String S,String t)
        {



                Client send = new Client();
            String SenderTemp = "";
            foreach (Client sender in client_list)
            {
                if (sender.name == t) 
                {

                    send = sender;
                }

            }

            
                
                Client rec = new Client();
                String recTemp = "";//user info mu gönderene eşitledim
            foreach (Client reci in client_list)
            {
                if (reci.name == S) 
                {

                    rec = reci;
                }
            }

            try
            {
                SenderTemp = "*#FX#*" + t;
                Byte[] buffer = new Byte[64];
                buffer = Encoding.Default.GetBytes(SenderTemp);
                rec.client_socket.Send(buffer);
                
                System.Threading.Thread.Sleep(100);

            }
            catch
            {

            }


            try
            {
                        recTemp = "*#FX#*" + S;
                        Byte[] buffer = new Byte[64];
                        buffer = Encoding.Default.GetBytes(recTemp);
                        send.client_socket.Send(buffer);
                        System.Threading.Thread.Sleep(100);

                    }
            catch
            {

            }




                

               



            }
        private void friendreject(String tempSender,String tempReceiver,string tobeSent,string tobeSentLater)
        {

            Client send = new Client();
            String SenderTemp = "";
            foreach (Client sender in client_list)
            {
                if (sender.name == tempSender)
                {

                    send = sender;
                }

            }

            Client rec = new Client();
            String recTemp = "";//user info mu gönderene eşitledim
            foreach (Client reci in client_list)
            {
                if (reci.name == tempReceiver)
                {

                    rec = reci;
                }
            }



            try
            {
                recTemp = tobeSentLater;
                Byte[] buffer = new Byte[64];
                buffer = Encoding.Default.GetBytes(recTemp);
                send.client_socket.Send(buffer);
                System.Threading.Thread.Sleep(100);

            }
            catch
            {

            }


        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}


namespace server
{
    class Client
    {

        public Socket client_socket { get; set; }
        private string _name;
        public string name { get { return _name; } set { _name = value; } }
      


    }
}


