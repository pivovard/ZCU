using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediTab
{

    public partial class Podani_Form : Form
    {

        decimal mnozstvi;

        Keyboard_Form keyboard;
        NumKeyboard_Form numKeyboard;

        /// <summary>
        /// Inicializace dialogu podani
        /// init klavesnic
        /// </summary>
        /// <param name="key"></param>
        /// <param name="num"></param>
        public Podani_Form(Keyboard_Form key, NumKeyboard_Form num)
        {
            InitializeComponent();

            this.keyboard = key;
            this.numKeyboard = num;
        }

        /// <summary>
        /// Zobrazeni dialogu podani
        /// Prednastavene podani
        /// </summary>
        /// <param name="mnozstvi"></param>
        /// <param name="lek"></param>
        /// <returns></returns>
        public DialogResult showPodani(decimal mnozstvi, Medikace.Lek lek)
        {
            //init nazvu leku a davkovani
            nazev_Label.Text = lek.nazev;
            davkovani_Label.Text = lek.davkovani;
            this.mnozstvi = mnozstvi;
            poznamka_TextBox.Text = "";

            //init predepsaneho mnozstvi
            //test davkovani - desetinna cisla
            if (mnozstvi % 1 == 0)
            {
                mnozstvi_NumericUpDown.Value = mnozstvi;
            }
            //nastaveni desetinnych cisel
            else
            {
                mnozstvi_NumericUpDown.Increment = mnozstvi;
                mnozstvi_NumericUpDown.DecimalPlaces = 2;
                mnozstvi_NumericUpDown.Value = mnozstvi;
            }

            return this.ShowDialog();
        }

        /// <summary>
        /// Zobrazeni dialogu podani
        /// Dafaultni podani
        /// </summary>
        /// <param name="mnozstvi"></param>
        /// <param name="lek"></param>
        /// <returns></returns>
        public DialogResult showPodani(Medikace.Lek lek)
        {
            //init nazvu leku a davkovani
            nazev_Label.Text = lek.nazev;
            davkovani_Label.Text = lek.davkovani;

            poznamka_TextBox.Text = "";

            //defaultni davkovani 1
            mnozstvi_NumericUpDown.Value = 1;

            //disable Nepodat Button
            nepodat_Button.Enabled = false;

            return this.ShowDialog();
        }

#region UpDown

        /// <summary>
        /// Zvetseni hodnoty NumericUpDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void up_Button_Click(object sender, EventArgs e)
        {
            mnozstvi_NumericUpDown.Value += mnozstvi_NumericUpDown.Increment;
        }

        /// <summary>
        /// Zmenseni hodnoty NumericUpDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void down_Button_Click(object sender, EventArgs e)
        {
            if (mnozstvi_NumericUpDown.Value - mnozstvi_NumericUpDown.Increment > 0)
            {
                mnozstvi_NumericUpDown.Value -= mnozstvi_NumericUpDown.Increment;
            }
        }
#endregion

#region Zapis

        private void mnozstvi_NumericUpDown_Click(object sender, EventArgs e)
        {
            //komponenta textboxu
            NumericUpDown tb = (NumericUpDown)sender;

            //zobrazeni numericke klavesnice jako dialog
            numKeyboard.showKeyboard(tb.Text);
            
            //pri potvrzeni se prekopiruje text
            if (numKeyboard.DialogResult == DialogResult.OK)
            {
                string text = numKeyboard.value_TextBox.Text;

                if (text.Equals("0"))
                {
                    if (mnozstvi > 1)
                    {
                        text = "1";
                    }
                    else
                    {
                        text = mnozstvi.ToString();
                    }

                    BigMessageBox.Show("Nelze zadat množství 0, minimum je " + text);
                }

                if (text.Contains('.'))
                {
                    //zobrazeni desetinych cisel
                    mnozstvi_NumericUpDown.DecimalPlaces = 2;
                }
                
                tb.Value = decimal.Parse(text);
            }
        }

        private void poznamka_TextBox_Click(object sender, EventArgs e)
        {
            keyboard.showKeyboard(poznamka_TextBox.Text);

            if (keyboard.DialogResult == DialogResult.OK)
            {
                poznamka_TextBox.Text = keyboard.text_TextBox.Text;
            }
        }

        /// <summary>
        /// Osetreni primeho zapisu do NumericUpDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnozstvi_NumericUpDown_KeyPress(object sender, KeyPressEventArgs e)
        {
            //povoleni zapisu cislic, punktace a pouziti konntrolnich klaves
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !e.KeyChar.Equals('.'))
            {
                e.Handled = true;
                return;
            }            
        }

#endregion

#region OLD constructors

        /// <summary>
        /// *****OLD*****
        /// Inicializace dialogu podani s ordinaci
        /// Prednastaveni naordinovaneho podani
        /// </summary>
        /// <param name="mnozstvi"></param>
        /// <param name="lek"></param>
        public Podani_Form(decimal mnozstvi, Medikace.Lek lek)
        {
            InitializeComponent();

            //init nazvu leku a davkovani
            nazev_Label.Text = lek.nazev;
            davkovani_Label.Text = lek.davkovani;
            this.mnozstvi = mnozstvi;

            //init predepsaneho mnozstvi
            //test davkovani - desetinna cisla
            if (mnozstvi % 1 == 0)
            {
                mnozstvi_NumericUpDown.Value = mnozstvi;
            }
            //nastaveni desetinnych cisel
            else
            {
                mnozstvi_NumericUpDown.Increment = mnozstvi;
                mnozstvi_NumericUpDown.DecimalPlaces = 2;
                mnozstvi_NumericUpDown.Value = mnozstvi;
            }
        }

        /// <summary>
        /// *****OLD*****
        /// Inicializace dialogu podani bez ordinace
        /// Disable Botton Nepodat
        /// </summary>
        /// <param name="mnozstvi"></param>
        /// <param name="lek"></param>
        public Podani_Form(Medikace.Lek lek)
        {
            InitializeComponent();

            //init nazvu leku a davkovani
            nazev_Label.Text = lek.nazev;
            davkovani_Label.Text = lek.davkovani;

            //defaultni davkovani 1
            mnozstvi_NumericUpDown.Value = 1;

            //disable Nepodat Button
            nepodat_Button.Enabled = false;
        }

        #endregion

        
    }
}
