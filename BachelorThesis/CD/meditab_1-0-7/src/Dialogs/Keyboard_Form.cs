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
    public partial class Keyboard_Form : Form
    {

        bool shift = false;

        public Keyboard_Form()
        {
            InitializeComponent();
        }

        public DialogResult showKeyboard(string text)
        {
            text_TextBox.Text = text;
            return this.ShowDialog();
        }

        private void backspace_Button_Click(object sender, EventArgs e)
        {
            if (text_TextBox.Text.Length > 0)
                text_TextBox.Text = text_TextBox.Text.Substring(0, text_TextBox.Text.Length - 1);
        }

        private void shift_Button_Click(object sender, EventArgs e)
        {
            shift = !shift;

            if (shift)
            {
                lShift_Button.BackColor = SystemColors.GradientActiveCaption;
                rShift_Button.BackColor = SystemColors.GradientActiveCaption;
            }
            else
            {
                lShift_Button.BackColor = SystemColors.Control;
                rShift_Button.BackColor = SystemColors.Control;
            }
        }

        private void button_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            if (!shift)
            {
                text_TextBox.Text += button.Text;
            }
            else
            {
                text_TextBox.Text += button.Text.ToUpper();
            }
        }

        private void space_Button_Click(object sender, EventArgs e)
        {
            text_TextBox.Text += " ";
        }
    }
}
