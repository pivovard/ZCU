using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Databaze;
using System.Configuration;

namespace MediTab
{
    public partial class Login_Form : Form
    {
        /// <summary>
        /// Klavesnice
        /// </summary>
        Keyboard_Form keyboard;

        /// <summary>
        /// login input:
        /// Debug "D", Release "R"
        /// </summary>
        string logIn = ConfigurationManager.AppSettings["logIn"];

        /// <summary>
        /// Konstruktor
        /// Inicializace komponent
        /// </summary>
        public Login_Form()
        {
            InitializeComponent();

            keyboard = new Keyboard_Form();
        }

        /// <summary>
        /// Login
        /// 
        /// Otestuje uspesnost prihlaseni
        /// - Pokud uspesne, nastavi DialogResult OK
        /// - Pokud neuspesne, nastavi DialogResult Retry
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ok_Button_Click(object sender, EventArgs e)
        {
            State res = null;

            //upraveno pro prihlaseni bez vecneho zadavani
            switch (logIn)
            {
                case "R":
                    res = Program.databaze.logInPsw(login_TextBox.Text, pass_TextBox.Text);
                    break;
                case "D":
                    res = Program.databaze.logInPsw("meditab", "MediTab");
                    break;
                default:
                    MessageBox.Show("Neplatné nastavení aplikace: logIn (login input)!");
                    return;
            }

            //vysledek prihlaseni
            if (res.ok)
            {
                if ((bool)res.res.Last())
                {
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    chyba_Label.Text = "Chybné přihlašovací jméno nebo heslo";
                    this.DialogResult = DialogResult.Retry;
                }
            }
            else
            {
                BigMessageBox.Show("Chyba připojení k databázi\n" + res.err);
                this.DialogResult = DialogResult.Retry;
            }
            
        }

        /// <summary>
        /// Vymaze TextBoxy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancel_Button_Click(object sender, EventArgs e)
        {
            login_TextBox.Text = "";
            pass_TextBox.Text = "";
            this.DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// Zobrazeni klavesnice po kliknuti na textbox loginu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void login_TextBox_Click(object sender, EventArgs e)
        {
            //Keyboard_Form key = new Keyboard_Form();
            //keyboard.text_TextBox.Text = login_TextBox.Text;
            DialogResult result = keyboard.showKeyboard(login_TextBox.Text);

            if (result == DialogResult.OK)
            {
                login_TextBox.Text = keyboard.text_TextBox.Text;
            }
        }

        /// <summary>
        /// Zobrazeni klavesnice po kliknuti na textbox hesla a nastaveni sifrovani hesla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pass_TextBox_Click(object sender, EventArgs e)
        {
            //Keyboard_Form key = new Keyboard_Form();
            //keyboard.text_TextBox.Text = "";
            keyboard.text_TextBox.PasswordChar = '*';
            DialogResult result = keyboard.showKeyboard(pass_TextBox.Text);

            if (result == DialogResult.OK)
            {
                pass_TextBox.Text = keyboard.text_TextBox.Text;
            }

            keyboard.text_TextBox.PasswordChar = '\0';
        }
    }
}
