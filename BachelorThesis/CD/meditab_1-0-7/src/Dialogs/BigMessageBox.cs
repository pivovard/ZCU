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
    public partial class BigMessageBox : Form
    {
        public BigMessageBox()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Staticka tovarni metoda
        /// Inicializuje BigMessageBox a zobrazi ho
        /// </summary>
        /// <param name="text"></param>
        public static void Show(string text)
        {
            //inicializace BigMessageBoxu
            BigMessageBox bmb = new BigMessageBox();

            //init textu
            bmb.label.Text = text;

            //odradkovani pro button ok
            //bmb.label.Text += "\n\n\n";

            //zobrazeni BMB
            bmb.ShowDialog();
        }

        /// <summary>
        /// Staticka tovarni metoda
        /// Inicializuje BigMessageBox s volbou YesNo a zobrazi ho
        /// </summary>
        /// <param name="text"></param>
        public static DialogResult Show(string text, MessageBoxButtons but)
        {
            //inicializace BigMessageBoxu
            BigMessageBox bmb = new BigMessageBox();

            //zmena potvrzovacich tlacitek
            //bmb.ok_Button.Visible = false;
            bmb.initYesNoButtons();
            
            //init textu
            bmb.label.Text = text;

            //odradkovani pro button ok
            //bmb.label.Text += "\n\n\n\n";

            //zobrazeni BMB a ulozeni vysledku
            DialogResult result = bmb.ShowDialog();

            //navrat Dialogresult
            return result;
        }

        public void initYesNoButtons()
        {
            ok_Button.Visible = false;

            FlowLayoutPanel panel = new FlowLayoutPanel();
            panel.AutoSize = true;
            panel.FlowDirection = FlowDirection.RightToLeft;
            panel.WrapContents = false;
            //panel.Padding = new Padding(15);
            panel.Margin = new Padding(15);
            //this.Controls.Add(panel);
            tableLayoutPanel.Controls.Add(panel, 0, 1);
            panel.BringToFront();
            panel.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right)));
            panel.Dock = DockStyle.Bottom;

            Button yesButton = new Button();
            yesButton.Name = "yesButton";
            yesButton.Text = "Ano";
            yesButton.DialogResult = DialogResult.Yes;
            yesButton.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right)));
            yesButton.AutoSize = true;
            yesButton.UseVisualStyleBackColor = true;
            yesButton.Font = new Font("Microsoft Sans Serif", 15.75F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            yesButton.BackColor = Color.LimeGreen;
            yesButton.Margin = new Padding(5);
            yesButton.Size = new Size(105, 35);
            yesButton.Click += new EventHandler(ok_Button_Click);
            
            Button noButton = new Button();
            noButton.Name = "noButton";
            noButton.Text = "Ne";
            noButton.DialogResult = DialogResult.No;
            noButton.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right)));
            noButton.AutoSize = true;
            noButton.UseVisualStyleBackColor = true;
            noButton.Font = new Font("Microsoft Sans Serif", 15.75F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            noButton.BackColor = Color.OrangeRed;
            noButton.Margin = new Padding(5);
            noButton.Size = new Size(105, 35);
            noButton.Click += new EventHandler(ok_Button_Click);

            panel.Controls.Add(noButton);
            panel.Controls.Add(yesButton);

            yesButton.Dock = DockStyle.Fill;
            noButton.Dock = DockStyle.Fill;

            this.AcceptButton = yesButton;
            this.CancelButton = noButton;
        }

        /// <summary>
        /// Zavre BigMessageBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ok_Button_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
