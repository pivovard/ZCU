using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;

namespace UPS_Scrabble_client
{
    public partial class Form_Main : Form
    {
        private object _lock = new object();
        public bool connected = false;

        public Form_Main()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Connect/Disconnect to the server
        /// Enable/Disable controls (cross-thread)
        /// </summary>
        public void Connect_Disconnect()
        {
            lock (_lock)
            {
                if (!connected)
                {
                    //# of players
                    int n;
                    if (radioButton1.Checked)
                    {
                        n = 2;
                    }
                    else if (radioButton2.Checked)
                    {
                        n = 3;
                    }
                    else
                    {
                        n = 4;
                    }

                    //connect to server
                    connected = Network.Connect(Tb_IP.Text, Tb_Port.Text, Tb_Nick.Text, n);
                    if (connected)
                        if (Btn_Connect.InvokeRequired)
                        {
                            Btn_Connect.Invoke(new Action(delegate () {
                                Btn_Connect.Text = "Disconnect";
                                radioButton1.Enabled = false;
                                radioButton2.Enabled = false;
                                radioButton3.Enabled = false;
                            }));
                        }
                        else
                        {
                            Btn_Connect.Text = "Disconnect";
                            radioButton1.Enabled = false;
                            radioButton2.Enabled = false;
                            radioButton3.Enabled = false;
                        }
                }
                else
                {
                    //disconnect from server
                    Network.Disconnect();
                } 
            }
        }

        /// <summary>
        /// Call Connect_Disconnect();
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Connect_Click(object sender, EventArgs e)
        {
            this.Connect_Disconnect();
        }

        /// <summary>
        /// Show Game Form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Start_Click(object sender, EventArgs e)
        {
            if (!connected) return;
            this.Hide();

            //GameForm must be created first
            try
            {
                Program.FormGame.Show();
            }
            //Create new GameForm
            catch (Exception ex)
            {
                Program.FormGame = new Form_Game(Program.Game);
                Program.FormGame.UpdateScore();
                Program.FormGame.UpdateTurns();
                Program.Game.Random();
                //Program.Game.Reconnect();
                Program.FormGame.Show();
            }
        }

        /// <summary>
        /// On close of app.
        /// Disconnect from the server if connected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (connected) Network.Disconnect();
        }
    }
}
