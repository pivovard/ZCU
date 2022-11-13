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
    public partial class NumKeyboard_Form : Form
    {
        public NumKeyboard_Form()
        {
            InitializeComponent();
        }

        public DialogResult showKeyboard()
        {
            this.value_TextBox.Text = "";
            nDot_Button.Enabled = true;
            return this.ShowDialog();
        }

        public DialogResult showKeyboard(bool nDot)
        {
            this.value_TextBox.Text = "";
            nDot_Button.Enabled = nDot;
            return this.ShowDialog();
        }

        public DialogResult showKeyboard(string text)
        {
            this.value_TextBox.Text = text;
            nDot_Button.Enabled = true;
            return this.ShowDialog();
        }

        public DialogResult showKeyboard(string text, bool nDot)
        {
            this.value_TextBox.Text = text;
            nDot_Button.Enabled = nDot;
            return this.ShowDialog();
        }

        

#region buttonsClick

        private void n1_Button_Click(object sender, EventArgs e)
        {
            value_TextBox.Text += "1";
        }

        private void n2_Button_Click(object sender, EventArgs e)
        {
            value_TextBox.Text += "2";
        }

        private void n3_Button_Click(object sender, EventArgs e)
        {
            value_TextBox.Text += "3";
        }

        private void n4_Button_Click(object sender, EventArgs e)
        {
            value_TextBox.Text += "4";
        }

        private void n5_Button_Click(object sender, EventArgs e)
        {
            value_TextBox.Text += "5";
        }

        private void n6_Button_Click(object sender, EventArgs e)
        {
            value_TextBox.Text += "6";
        }

        private void n7_Button_Click(object sender, EventArgs e)
        {
            value_TextBox.Text += "7";
        }

        private void n8_Button_Click(object sender, EventArgs e)
        {
            value_TextBox.Text += "8";
        }

        private void n9_Button_Click(object sender, EventArgs e)
        {
            value_TextBox.Text += "9";
        }

        private void n0_Button_Click(object sender, EventArgs e)
        {
            value_TextBox.Text += "0";
        }

        private void nDot_Button_Click(object sender, EventArgs e)
        {
            if (!value_TextBox.Text.Contains("."))
            {
                value_TextBox.Text += ".";
            }
        }

        private void backspace_Button_Click(object sender, EventArgs e)
        {
            if (value_TextBox.Text.Length > 0)
                value_TextBox.Text = value_TextBox.Text.Substring(0, value_TextBox.Text.Length - 1);
        }

#endregion

        /// <summary>
        /// *****OLD*****
        /// 
        /// Osetreni primeho zapisu do TextBoxu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnozstvi_NumericUpDown_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

    }
}
