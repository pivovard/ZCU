using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
//using System.Threading.Tasks;
using System.Windows.Forms;

namespace UPS_Scrabble_client
{
    public static class Network
    {
        public const int msg_length = 1024;

        //IP adress and port of the server
        public static byte[] ip;
        public static int port;

        static string nick;
        static int n;

        public static byte[] buffer = new byte[msg_length];
        public static byte[] buffer_in = new byte[msg_length];
        public static byte[] buffer_out = new byte[msg_length];

        /// <summary>
        /// Client socket
        /// </summary>
        public static Socket Socket { get; set; }

        /// <summary>
        /// Thread that receive messages from server
        /// </summary>
        public static Thread Listenner { get; set; }

        /// <summary>
        /// Thread that pings server
        /// </summary>
        private static Thread Pinger { get; set; }

        /// <summary>
        /// Create socket and connect to server
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="nick"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static bool Connect(string ip, string port, string nick, int n)
        {
            //adress of server
            string[] split = ip.Split('.');
            if (split.Count() != 4)
            {
                MessageBox.Show("IP address format must be 255.255.255.255 !");
                return false;
            }

            Network.ip = new byte[4];
            Network.ip[0] = byte.Parse(split[0]);
            Network.ip[1] = byte.Parse(split[1]);
            Network.ip[2] = byte.Parse(split[2]);
            Network.ip[3] = byte.Parse(split[3]);

            //port
            if (!int.TryParse(port, out Network.port))
            {
                MessageBox.Show("Port must be a number!" + Network.port);
                return false;
            }

            //nick
            if(nick.Length == 0)
            {
                MessageBox.Show("Nick must be at least 1 character long!");
                return false;
            }

            Network.nick = nick;
            Network.n = n;

            //IPHostEntry ipHostEntry = Dns.Resolve(Dns.GetHostName());
            //IPAddress ipAddress = new IPAddress(ipHostEntry);
            IPAddress ipAddress = new IPAddress(Network.ip);
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, Network.port);

            //socket
            try
            {
                Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Console.WriteLine("Socket created.");

                Socket.Connect(ipEndPoint);
                Console.WriteLine("Connected to " + Socket.RemoteEndPoint.ToString());
            }
            catch (Exception e)
            {
                MessageBox.Show("Couldn't connect to the server. Error:\n" + e);
                return false;
            }
            
            //send nick to socket
            buffer = Encoding.ASCII.GetBytes("NICK:" + nick + ";" + n.ToString() + "\n");
            Socket.Send(buffer);

            //start recv msg from sever
            Listenner = new Thread(Listen);
            Listenner.Start();

            //start ping server
            Pinger = new Thread(Ping);
            Pinger.Start();

            return true;
        }

        /// <summary>
        /// Close connection.
        /// Abort ping thread.
        /// Set MainForm.
        /// </summary>
        public static void Disconnect()
        {
            Socket.Close();
            Console.WriteLine("Disconnected.");

            try
            {
                Pinger.Abort();
            }
            catch { }

            Program.FormMain.connected = false;
            if (Program.FormMain.Btn_Connect.InvokeRequired)
            {
                Program.FormMain.Btn_Connect.Invoke(new Action(delegate () {
                    Program.FormMain.Btn_Connect.Text = "Connect";
                    Program.FormMain.radioButton1.Enabled = true;
                    Program.FormMain.radioButton2.Enabled = true;
                    Program.FormMain.radioButton3.Enabled = true;
                }));
            }
            else
            {
                Program.FormMain.Btn_Connect.Text = "Connect";
                Program.FormMain.radioButton1.Enabled = true;
                Program.FormMain.radioButton2.Enabled = true;
                Program.FormMain.radioButton3.Enabled = true;
            }

            if (Program.FormMain.Btn_Start.InvokeRequired)
            {
                Program.FormMain.Btn_Start.Invoke(new Action(delegate () { Program.FormMain.Btn_Start.Enabled = false; }));
            }
            else
            {
                Program.FormMain.Btn_Start.Enabled = false;
            }
        }

        /// <summary>
        /// Receive messages from the server.
        /// Started in thread
        /// </summary>
        private static void Listen()
        {
            int size;
            string msg = "";

            try
            {
                //loop until there is open connection
                while ((size = Socket.Receive(buffer_in, msg_length, SocketFlags.None)) > 0)
                {
                    msg = Encoding.ASCII.GetString(buffer_in, 0, size);
                    int i = msg.IndexOf("\n");
                    if (i != -1)
                        msg = msg.Substring(0, i);
                    Console.WriteLine("Recv: " + msg);
                    Resolve(msg);
                }

                if (size < 1)
                {
                    if (Program.FormGame != null)
                    {
                        if (Program.FormGame.InvokeRequired)
                        {
                            Program.FormGame.Invoke(new Action(delegate () { Program.FormGame.Close(); }));
                        }
                        else
                        {
                            Program.FormGame.Close();
                        }
                    }
                    else Disconnect();

                    MessageBox.Show("Server unavaible.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Resolve message from the server.
        /// </summary>
        /// <param name="msg"></param>
        private static void Resolve(string msg)
        {
            string[] type = msg.Split(':');

            switch (type[0])
            {
                //started new game
                case "GAME":
                    Program.Game = new Game(type[1], type[2], nick, n);
                    Program.FormGame = new Form_Game(Program.Game);
                    Program.Game.Random();
                    
                    if (Program.FormMain.Btn_Start.InvokeRequired)
                    {
                        Program.FormMain.Btn_Start.Invoke(new Action(delegate() { Program.FormMain.Btn_Start.Enabled = true; }));
                    }
                    else
                    {
                        Program.FormMain.Btn_Start.Enabled = true;
                    }
                    
                    break;

                //player on turn
                case "TURN":
                    Program.FormGame.Turn();
                    break;

                //turn of other player
                case "TURNP":
                    Program.Game.RecvTurn(type[1], type[2]);
                    break;

                //nick unavaible
                case "NICK":
                    MessageBox.Show("Nick allready in use.");
                    //Disconnect
                    Disconnect();
                    break;

                //ask for return of player to previous game
                case "RETURN":
                    DialogResult res = MessageBox.Show("Do you want to return to the existing game?", "Return", MessageBoxButtons.YesNo);
                    if (res == DialogResult.Yes)
                    {
                        Send("RETURN");
                    }
                    else if(res == DialogResult.No)
                    {
                        Send("NEW");
                    }
                    break;

                //return to the previous game
                case "GAMER":
                    Console.WriteLine("Reconnected.");
                    if(type.Count() == 4) Program.Game = new Game(type[1], type[2], type[3], nick, n);
                    else Program.Game = new Game(type[1], type[2], nick, n);

                    Program.FormGame = new Form_Game(Program.Game);
                    Program.FormGame.UpdateScore();
                    Program.FormGame.UpdateTurns();
                    Program.Game.Random();
                    //Program.Game.Reconnect();

                    if (Program.FormMain.Btn_Start.InvokeRequired)
                    {
                        Program.FormMain.Btn_Start.Invoke(new Action(delegate () { Program.FormMain.Btn_Start.Enabled = true; }));
                    }
                    else
                    {
                        Program.FormMain.Btn_Start.Enabled = true;
                    }
                    break;

                //player disconnected
                case "DISC":
                    MessageBox.Show("Player " + Program.Game.Players.Where(p => p.ID == int.Parse(type[1])).First().nick + " disconnected.");
                    break;

                //player reconnected
                case "RECN":
                    MessageBox.Show("Player " + Program.Game.Players.Where(p => p.ID == int.Parse(type[1])).First().nick + " reconnected.");
                    break;

                case "PING":
                    break;
                    
                default:
                    Disconnect();
                    MessageBox.Show("Server doesn't react properly.");
                    break;
            }
        }

        /// <summary>
        /// Send message to the server.
        /// Add \n to the end of the message.
        /// </summary>
        /// <param name="msg"></param>
        public static bool Send(string msg)
        {
            Console.WriteLine("Send: " + msg);
            buffer = Encoding.ASCII.GetBytes(msg + "\n");

            try
            {
                Socket.Send(buffer);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Ping server.
        /// If connection timedout, Disconnect()
        /// </summary>
        private static void Ping()
        {
            bool res;
            while (true)
            {
                res = Send("PING");
                if (res == false)
                {
                    MessageBox.Show("Server connection timeout.");
                    if (Program.FormGame != null)
                    {
                        if (Program.FormGame.InvokeRequired)
                        {
                            Program.FormGame.Invoke(new Action(delegate () { Program.FormGame.Close(); }));
                        }
                        else
                        {
                            Program.FormGame.Close();
                        }
                    }
                    else Disconnect();
                    break;
                }
                Thread.Sleep(5000);
            }
        }
    }
}
