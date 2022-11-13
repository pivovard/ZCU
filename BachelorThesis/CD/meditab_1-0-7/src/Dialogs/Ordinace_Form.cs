using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MediTab.Medikace;

namespace MediTab
{
    public partial class Ordinace_Form : Form
    {

        Keyboard_Form Keyboard { get; set; }
        NumKeyboard_Form NumKeyboard { get; set; }

        Lek Lek { get; set; }
        MedikacniKarta Medikace { get; set; }

        string[] stav = { "O", "P", "N" };

        int ordinaceID = 0;
        bool zmena = false;

        #region Construktors

        public Ordinace_Form(Keyboard_Form key, NumKeyboard_Form num, MedikacniKarta med)
        {
            InitializeComponent();

            this.Keyboard = key;
            this.NumKeyboard = num;
            this.Medikace = med;

            stav_ComboBox.Items.AddRange(new[] { "Ordinace", "Provedená", "Neprovedená" });
            jednotky_ComboBox.Items.AddRange(new[] { "tbl.", "cps.", "ml/h", "mg", "ug", "g", "ml", "gtt.", "sup.", "tbl.eff.", "amp.", "UT" });

            drop();
        }

        public void Show(Lek lek)
        {
            this.Lek = lek;
            lek_Label.Text = $"{Lek.nazev}   -   {Lek.davkovani}";

            foreach (var ordinace in Lek.ordinace)
            {
                ListViewItem item = new ListViewItem(new[] {ordinace.datumOd.ToString(), ordinace.datumDo.ToString(), stav[(int)ordinace.stavPodani], ordinace.davka.ToString()});
                ordinace_ListView.Items.Add(item);
            }

            getDefault();
            zmena = false;

            this.ShowDialog();
        }

        public void Show(Lek lek, int hour)
        {
            this.Lek = lek;
            lek_Label.Text = $"{Lek.nazev}   -   {Lek.davkovani}";

            foreach (var ordinace in Lek.ordinace)
            {
                ListViewItem item = new ListViewItem(new[] { ordinace.datumOd.ToString(), ordinace.datumDo.ToString(), stav[(int)ordinace.stavPodani], ordinace.davka.ToString() });
                ordinace_ListView.Items.Add(item);
            }

            int startH;
            int endH;
            for (int i = 0; i < Lek.ordinace.Count; i++)
            {
                startH = Lek.ordinace[i].datumOd.Hour;

                //test na infuzi
                if (Lek.ordinace[i].kontinual || Lek.ordinace[i].konec)
                {
                    //init endH
                    if (Lek.ordinace[i].konec)
                    {
                        endH = Lek.ordinace[i].datumDo.Value.Hour;
                    }
                    else
                    {
                        endH = DateTime.Now.Hour;
                    }

                    //klasicka infuze
                    if(startH <= endH)
                    {
                        if (hour >= startH && hour <= endH)
                        {
                            ordinace_ListView.Items[i].Selected = true;
                            ordinace_ListView.Select();
                            load(i);
                            break;
                        }
                    }
                    //pres pulnoc
                    else
                    {
                        if (hour >= startH || hour <= endH)
                        {
                            ordinace_ListView.Items[i].Selected = true;
                            ordinace_ListView.Select();
                            load(i);
                            break;
                        }
                    }
                }
                else
                {
                    if (hour == startH)
                    {
                        ordinace_ListView.Items[i].Selected = true;
                        ordinace_ListView.Select();
                        load(i);
                        break;
                    }
                }
            }

            zmena = false;

            this.ShowDialog();
        }

        #endregion

        #region Controling

        private void load(int index)
        {
            var ordinace = Lek.ordinace[index];
            ordinaceID = index;

            datumOd_DateTimePicker.Value = ordinace.datumOd;
            if (ordinace.konec)
            {
                konec_CheckBox.Checked = true;
                datumDo_DateTimePicker.Value = (DateTime)ordinace.datumDo;
                datumDo_DateTimePicker.Enabled = true;
            }
            else
            {
                konec_CheckBox.Checked = false;
                datumDo_DateTimePicker.Value = ordinace.datumOd;
                datumDo_DateTimePicker.Enabled = false;
            }

            stav_ComboBox.SelectedIndex = (int)ordinace.stavPodani;
            if(stav_ComboBox.SelectedIndex == (int)Stav.NEPROVEDENA)
            {
                poznamka_TextBox.ReadOnly = false;
            }
            else
            {
                poznamka_TextBox.ReadOnly = true;
            }

            davka_TextBox.Text = ordinace.davka.ToString();
            jednotky_ComboBox.SelectedItem = Lek.merJednotky;
            if (ordinace.jednotky != "") jednotky_ComboBox.SelectedItem = ordinace.jednotky;
            poznamka_TextBox.Text = $"{ordinace.davka} {ordinace.jednotky} {ordinace.poznamka}";
            kontinual_CheckBox.Checked = ordinace.kontinual;

            podavajici_Label.Text = $"Podávající: {ordinace.uzivatelJmeno}";
        }

        private void getDefault()
        {
            if(Lek.ordinace.Count == 0)
            {
                drop();
                ordinaceID = 0;
            }
            else
            {
                int id = Lek.ordinace.Count - 1;
                ordinace_ListView.Items[id].Selected = true;
                ordinace_ListView.Select();
                load(id);
            }
        }

        private void drop()
        {
            konec_CheckBox.Checked = false;
            datumOd_DateTimePicker.Value = DateTime.Now;
            datumDo_DateTimePicker.Value = DateTime.Now;
            datumDo_DateTimePicker.Enabled = false;

            stav_ComboBox.SelectedIndex = 0;
            davka_TextBox.Text = "1";
            jednotky_ComboBox.SelectedIndex = -1;
            poznamka_TextBox.Text = "";
            poznamka_TextBox.ReadOnly = true;
            kontinual_CheckBox.Checked = false;

            podavajici_Label.Text = $"Podávající: ";
        }

        private void ZadejOrdinaci()
        {
            DateTime? datumDo = null;
            if (konec_CheckBox.Checked) datumDo = datumDo_DateTimePicker.Value;

            Stav stav = (Stav)stav_ComboBox.SelectedIndex;

            float davka;
            
            while (!float.TryParse(davka_TextBox.Text, out davka))
            {
                BigMessageBox.Show("Nebylo vyplněno množství!");

                DialogResult r = NumKeyboard.showKeyboard(davka_TextBox.Text, true);
                if (r == DialogResult.OK)
                {
                    zmena = true;
                    davka_TextBox.Text = NumKeyboard.value_TextBox.Text;
                }
            }

            string id;
            if(ordinaceID < Lek.ordinace.Count())
            {
                id = Lek.ordinace[ordinaceID].apliklekID;
            }
            else
            {
                id = "";
            }

            bool res = Medikace.zadejOrdinaci(Lek, datumOd_DateTimePicker.Value, datumDo, davka, poznamka_TextBox.Text, jednotky_ComboBox.Text, kontinual_CheckBox.Checked, stav, id);
            //Medikace.ukonciInfuzi(Lek.pacientID, Lek, datumOd_DateTimePicker.Value, datumDo_DateTimePicker.Value);
            
            if (res)
            {
                if(Lek.ordinace.Count > ordinace_ListView.Items.Count)
                {
                    var ordinace = Lek.ordinace.Last();
                    ListViewItem item = new ListViewItem(new[] { ordinace.datumOd.ToString(), ordinace.datumDo.ToString(), this.stav[(int)ordinace.stavPodani], ordinace.davka.ToString() });
                    ordinace_ListView.Items.Add(item);
                    item.Selected = true;
                    ordinace_ListView.Select();
                    load(item.Index);
                }
                else
                {
                    zmena = false;
                    updateLV();
                }
            }
            else
            {
                BigMessageBox.Show("Byly zadány nekorektní hodnoty. Změna nebyla provedena!");
            }
        }

        private void updateLV()
        {
            var ordinace = Lek.ordinace[ordinaceID];

            ordinace_ListView.Items[ordinaceID].SubItems[0].Text = ordinace.datumOd.ToString();
            ordinace_ListView.Items[ordinaceID].SubItems[1].Text = ordinace.datumDo.ToString();
            ordinace_ListView.Items[ordinaceID].SubItems[2].Text = stav[(int)ordinace.stavPodani];
            ordinace_ListView.Items[ordinaceID].SubItems[3].Text = ordinace.davka.ToString();
        }

        #endregion

        #region Controls

        private void ordinace_ListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (zmena) ZadejOrdinaci();

            ListView lv = (ListView)sender;
            load(lv.FocusedItem.Index);
        }

        private void davka_TextBox_Click(object sender, EventArgs e)
        {
            DialogResult res = NumKeyboard.showKeyboard(davka_TextBox.Text, true);
            if (res == DialogResult.OK)
            {
                zmena = true;
                davka_TextBox.Text = NumKeyboard.value_TextBox.Text;
            }
        }

        private void poznamka_TextBox_Click(object sender, EventArgs e)
        {
            if (poznamka_TextBox.ReadOnly) return;

            DialogResult res = Keyboard.showKeyboard(poznamka_TextBox.Text);
            if (res == DialogResult.OK)
            {
                zmena = true;
                poznamka_TextBox.Text = Keyboard.text_TextBox.Text;
            }
        }

        private void konec_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (konec_CheckBox.Checked)
            {
                datumDo_DateTimePicker.Enabled = true;
                kontinual_CheckBox.Checked = true;
            }
            else
            {
                datumDo_DateTimePicker.Enabled = false;
                if(Lek.ordinace[ordinaceID].kontinual == false) kontinual_CheckBox.Checked = false;
            }
        }

        private void stav_ComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            if (cb.SelectedIndex == (int)Stav.NEPROVEDENA)
            {
                poznamka_TextBox.ReadOnly = false;
            }
            else
            {
                poznamka_TextBox.ReadOnly = true;
            }
        }

        private void ValueChanged(object sender, EventArgs e)
        {
            zmena = true;
        }

        #endregion

        #region Buttons

        private void podat_Button_Click(object sender, EventArgs e)
        {
            stav_ComboBox.SelectedIndex = (int)Stav.PROVEDENA;
            ZadejOrdinaci();
        }

        private void nepodat_Button_Click(object sender, EventArgs e)
        {
            stav_ComboBox.SelectedIndex = (int)Stav.NEPROVEDENA;
            ZadejOrdinaci();
        }

        private void pridat_Button_Click(object sender, EventArgs e)
        {
            if(zmena) ZadejOrdinaci();

            drop();
            zmena = true;

            ListViewItem item = new ListViewItem(new[] { DateTime.Today.ToString(), "", this.stav[0], "" });
            ordinace_ListView.Items.Add(item);
            item.Selected = true;
            ordinace_ListView.Select();

            ordinaceID = ordinace_ListView.Items.Count - 1;
        }

        private void smazat_Button_Click(object sender, EventArgs e)
        {
            bool res = true;
            if(Lek.ordinace.Count - 1 >= ordinaceID)
            {
                var ordinace = Lek.ordinace[ordinace_ListView.FocusedItem.Index];
                res = Medikace.smazOrdinaci(Lek.pacientID, Lek, ordinace);
            }

            if (res)
            {
                try
                {
                    ordinace_ListView.Items[ordinaceID].Remove();
                }
                catch (Exception ex) { }
                
                zmena = false;
                getDefault();
            }
        }

        private void zavrit_Button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Podani_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (zmena) ZadejOrdinaci();
            drop();
            ordinace_ListView.Items.Clear();
        }

        #endregion

        #region DateTimePicker control hours

        Point startPosition;

        private void DateTimePicker_MouseEnter(object sender, EventArgs e)
        {
            startPosition = Cursor.Position;
        }

        private void DateTimePicker_MouseLeave(object sender, EventArgs e)
        {
            DateTimePicker dtp = (DateTimePicker)sender;
            var endPosition = Cursor.Position;

            if(startPosition.Y > endPosition.Y)
            {
                dtp.Value = dtp.Value.AddHours(1);
            }
            else
            {
                dtp.Value = dtp.Value.AddHours(-1);
            }

            zmena = true;
        }

        #endregion

        
    }
}
