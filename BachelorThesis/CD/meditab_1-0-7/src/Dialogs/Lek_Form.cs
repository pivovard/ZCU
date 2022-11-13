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
    public partial class Lek_Form : Form
    {
        /**Ciselnik leku z databaze*/
        private Databaze.State res;

        /**id leku*/
        public string id;

        Keyboard_Form keyboard;

        public Lek_Form(Keyboard_Form key)
        {
            InitializeComponent();
            this.keyboard = key;
        }

#region Buttons

        private void pridej_Button_Click(object sender, EventArgs e)
        {
            DialogResult result = DialogResult.Yes;

            foreach (String[] leky in res.res)
            {
                if (leky[1] == lek_ComboBox.Text)
                {
                    //id leku
                    id = leky[0];

                    //pokud byl vybrán prázdný řetězec, neprovede se nic a upozorní se uživatel
                    if (lek_ComboBox.Text.Equals(""))
                    {
                        BigMessageBox.Show("Nevybral jste lék, který má být pacientovi podáván!");
                        return;
                    }

                    break;
                }
            }

            //test rozparsovatelneho davkovani
            if (!Medikace.Lek.jeRozparsovatelny(davkovani_TextBox.Text))
            {
                result = BigMessageBox.Show("Zadané dávkování není korektni. Opravdu chcete přidat lék s tímto dávkováním?", MessageBoxButtons.YesNo);
            }

            if (result == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        private void zrusit_Button_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void vyhledat_Button_Click(object sender, EventArgs e)
        {

        }

#endregion

#region Text

        private void lek_ComboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            //sbaleni comboboxu
            lek_ComboBox.DroppedDown = false;

            //nacteni ciselniku leku z databaze
            //text z comboboxu + posledni zadany znak (ten jeste neni v cb)
            res = Program.databaze.getCiselnikLeku(lek_ComboBox.Text + e.KeyChar);
            
            //vymazani aktualniho obsahu
            lek_ComboBox.Items.Clear();

            //pridani noveho seznamu leku
            foreach (String[] lek in res.res)
            {
                lek_ComboBox.Items.Add(lek[1]);
            }

            //nastavi coursor na konce textoveho pole
            lek_ComboBox.SelectionStart = lek_ComboBox.Text.Length;

            //rozbaleni comboboxu
            //lek_ComboBox.DroppedDown = true;
        }

        private void lek_ComboBox_Click(object sender, EventArgs e)
        {
            keyboard.showKeyboard(lek_ComboBox.Text);

            if (keyboard.DialogResult == DialogResult.OK)
            {
                lek_ComboBox.Text = keyboard.text_TextBox.Text;

                //nacteni ciselniku leku z databaze
                res = Program.databaze.getCiselnikLeku(lek_ComboBox.Text);

                //vymazani aktualniho obsahu
                lek_ComboBox.Items.Clear();

                //pridani noveho seznamu leku
                foreach (String[] lek in res.res)
                {
                    lek_ComboBox.Items.Add(lek[1]);
                }

                //rozbaleni comboboxu
                lek_ComboBox.DroppedDown = true;
            }
        }

        private void davkovani_TextBox_Click(object sender, EventArgs e)
        {
            keyboard.showKeyboard(davkovani_TextBox.Text);

            if (keyboard.DialogResult == DialogResult.OK)
            {
                davkovani_TextBox.Text = keyboard.text_TextBox.Text;
            }
        }

#endregion
        
    }
}
