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

namespace MediTab
{
    public partial class Main_Form : Form
    {

        private const String LOG = "Přihlášen: ";

        /// <summary>
        /// List vsech pacientu na oddeleni
        /// </summary>
        private List<Pacient> pacienti;

#region Main

        public Main_Form()
        {
            InitializeComponent();

            this.Icon = new Icon("logo.ico");
            this.verze_ToolStripStatusLabel.Text = "MediTab " + Application.ProductVersion;
        }

        /// <summary>
        /// Ukonci aplikaci
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void konec_Button_Click(object sender, EventArgs e)
        {
            DialogResult result = BigMessageBox.Show("Chcete program ukončit?", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        /// <summary>
        /// Uvolneni pameti pri zavreni okna
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
            GC.Collect();
        }

        /// <summary>
        /// Zobrazeni napovedy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void napoveda_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        #endregion

#region Login + Pacienti

        /// <summary>
        /// Uvodni prihlaseni - dialog pro prihlaseni
        /// Nacteni pacientu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_Form_Load(object sender, EventArgs e)
        {
            //dialog pro prihlaseni
            Login_Form loginForm = new Login_Form();

            //pokus o prihlaseni
            Login:
            switch(loginForm.ShowDialog(this))
            {
                case DialogResult.OK:
                    pacienti_ListView.Enabled = true;
                    break;
                case DialogResult.Retry:
                    //novy pokus o prihlaseni
                    goto Login;
                case DialogResult.Cancel:
                    BigMessageBox.Show("Pro práci s MediTabem musíte být přihlášen!");
                    //zruseni prihlaseni
                    goto Not;
            }
            
            //inicializace loginu
            prihlasen_ToolStripStatusLabel.Text = LOG + Program.databaze.getUzivatelJmeno();

            //nacteni paientu
            State res = Program.databaze.getPacienti();

            if (res.ok)
            {
                this.pacienti = new List<Pacient>(res.res.Cast<Pacient>());
            }
            else
            {
                BigMessageBox.Show("Chyba připojení k databázi\n" + res.err);
            }

            //serazeni pacientu podle prijmeni
            if (pacienti.Count != 0) 
            { 
                pacienti.Sort(); 
            }

            //pridani pacientu do listview
            foreach (Pacient pacient in pacienti)
            {
                //ListViewItem row = new ListViewItem(new [] {pacient.id, pacient.prijmeni.ToUpper(), pacient.jmeno, //pacient.rodneCislo});

                //vytvoreni radky ListView
                ListViewItem row = new ListViewItem(pacient.prijmeni.ToUpper());
                row.SubItems.Add(pacient.jmeno);
                row.SubItems.Add(pacient.rodneCislo);
                row.SubItems.Add(pacient.cisloChorobopisu);

                //pridani radky pacienta do ListView
                pacienti_ListView.Items.Add(row);
            }

            //zrusene prihlaseni
            Not:
            return;
        }

        /// <summary>
        /// Odhlasi prihlaseneho uzivatele
        /// Vymaze ListView
        /// Zobrazi prihlasovaci okno
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void odhlasit_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //odhlaseni aktualniho uzivatele
            Program.databaze.logOut();

            //smazani stareho seznamu pacientu pokud byl inicializovan drive (pro Click event ze StripMenu)
            pacienti_ListView.Items.Clear();
            prihlasen_ToolStripStatusLabel.Text = LOG;

            //nove prihlaseni a nacteni pacientu
            //stejna metoda jako pri spusteni Main_Form
            Main_Form_Load(sender, e);
        }

        /// <summary>
        /// Vyber pacienta
        /// Prechod na kartu pacienta
        /// Schovani okna s vyberem pacienta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pacienti_ListView_Click(object sender, EventArgs e)
        {
            //info o vyberu z ListView
            ListView lv = (ListView)sender;
            String id; //= lv.SelectedItems[0].Text;
            id = pacienti[lv.SelectedIndices[0]].id;

            //vytvoreni okna s kartou pacienta
            Form form = new Pacient_Form(getPacient(id), this);
            form.Show();
            
            //schova okno s vyberem pacientu
            this.Hide();
        }

        /// <summary>
        /// Metoda vyhleda pacienta v seznamu pacientu podle ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private Pacient getPacient(String id)
        {
            foreach (Pacient pacient in pacienti)
            {
                if (pacient.id == id)
                {
                    return pacient;
                }
            }

            return null;
        }

        #endregion

    }
}
