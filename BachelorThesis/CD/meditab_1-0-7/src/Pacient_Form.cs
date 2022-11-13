using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MediTab.BilanceTekutin;
using MediTab.InvazivniPristupy;
using MediTab.Medikace;
using MediTab.TelesneFunkce;
using System.Reflection;
using System.Threading;

namespace MediTab
{
    public partial class Pacient_Form : Form
    {

#region Properties

        private const String LOG = "Přihlášen: ";
        private const String PAC = "Pacient: ";

        /// <summary>
        /// Info o pacientovi
        /// </summary>
        private Pacient Pacient { get; set; }

        /// <summary>
        /// Main Form s vyberem pacientu
        /// </summary>
        private Form MainForm { get; set; }

    #region TabPages

        /// <summary>
        /// instance tridy pracujici s medikacni kartou
        /// </summary>
        private MedikacniKarta Medikace { get; set; }

        /// <summary>
        /// Seznam predepsanych leku
        /// </summary>
        List<Lek> PredepsaneLeky { get; set; }

        /// <summary>
        /// Hodina posledniho nacteni medikace
        /// </summary>
        private int medikaceLoadHour;

        /// <summary>
        /// Hodina, ktera je prvni v medikaci
        /// </summary>
        private int pocatek = int.Parse(System.Configuration.ConfigurationManager.AppSettings["POCATEK"]);

        /// <summary>
        /// Pocet hodin, o ktere se da zadat lek zpetne
        /// </summary>
        private int zpetnePodani = int.Parse(System.Configuration.ConfigurationManager.AppSettings["ZpetnePodani"]);

        private bool clickThread = false;
        private bool click1 = true;
        private bool click2 = false;
        private System.Windows.Forms.Timer timer;
        private int milliseconds = 0;
        private int lekID;
        private int hour;

        /// <summary>
        /// instance tridy pracujici s diurezou
        /// </summary>
        private Diureza Bilance { get; set; }

        /// <summary>
        /// instance tridy pracujici s hodinovou bilanci
        /// </summary>
        private BilanceHod BilanceHod { get; set; }

        /// <summary>
        /// Hodina posledniho nacteni hodinove bilance tekutin
        /// </summary>
        private int bilanceHodLoadHour;

        /// <summary>
        /// Bool byla-li bilance ulozena do dtb
        /// </summary>
        private bool bilanceSaved;

        /// <summary>
        /// instance tridy pracujici s invazivnimi pristupy
        /// </summary>
        private ZavedenePristupy Pristupy { get; set; }

        /// <summary>
        /// Scrollovatelny panel
        /// </summary>
        TouchableFlowLayoutPanel pristupy_TouchableFlowLayoutPanel;

        /// <summary>
        /// instance tridy pracujici s invazivnimi pristupy
        /// </summary>
        private Fyziologie Fyziologie { get; set; }

        /// <summary>
        /// Scrollovatelny panel
        /// </summary>
        TouchableFlowLayoutPanel fyziologie_TouchableFlowLayoutPanel;

        #endregion

    #region Dialogs

        /// <summary>
        /// Klavesnice
        /// </summary>
        public Keyboard_Form Keyboard { get; set; }

        /// <summary>
        /// Numericka klavesnice
        /// </summary>
        public NumKeyboard_Form NumKeyboard { get; set; }

        /// <summary>
        /// Dialog podani leku a infuzi
        /// </summary>
        private Ordinace_Form OrdinaceForm { get; set; }

        /// <summary>
        /// Dialog zadani podani leku
        /// Stary
        /// </summary>
        private Podani_Form PodaniForm { get; set; }

        /// <summary>
        /// Dialog vsech hodin tekutiny hodinove bilance
        /// </summary>
        private BilanceHod_Form BilanceHodForm { get; set; }

    #endregion

#endregion

#region Main

        /// <summary>
        /// Konstruktor karty pacienta
        /// 
        /// Nacteni pacienta
        /// Inicializace trid pro praci s medikacni kartou, diurezou a invazivnimi pristupy
        /// </summary>
        /// <param name="login"></param>
        /// <param name="pacient"></param>
        /// <param name="form"></param>
        public Pacient_Form(Pacient pacient, Form form)
        {
            //vytvoreni nacitaciho okna
            Loading_Form LoadingForm = new Loading_Form();
            LoadingForm.Show();
            LoadingForm.Progress("Inicializace komponent");

            //init komponent
            InitializeComponent();
            this.Icon = new Icon("logo.ico");

            //init klavesnic
            Keyboard = new Keyboard_Form();
            NumKeyboard = new NumKeyboard_Form();

            //nastaveni spravneho rozdeleni okna bilance
            diureza_SplitContainer.SplitterDistance = 650;
            diurezaHod_SplitContainer.SplitterDistance = 650;

            //init scrollovatelneho panelu pristupu
            this.pristupy_TouchableFlowLayoutPanel = new TouchableFlowLayoutPanel();
            pristupy_TouchableFlowLayoutPanel.Dock = DockStyle.Fill;
            pristupy_TouchableFlowLayoutPanel.AutoScroll = true;
            pristupy_TabPage.Controls.Add(pristupy_TouchableFlowLayoutPanel);
            pristupy_TouchableFlowLayoutPanel.BringToFront();

            //init scrollovatelneho panelu fyziologie
            this.fyziologie_TouchableFlowLayoutPanel = new TouchableFlowLayoutPanel();
            fyziologie_TouchableFlowLayoutPanel.Dock = DockStyle.Fill;
            fyziologie_TouchableFlowLayoutPanel.AutoScroll = true;
            fyziologie_TabPage.Controls.Add(fyziologie_TouchableFlowLayoutPanel);
            fyziologie_TouchableFlowLayoutPanel.BringToFront();

            //init pacienta
            this.Pacient = pacient;
            //init Main Formu
            this.MainForm = form;

            //inicializace statusBaru
            prihlasen_ToolStripStatusLabel.Text = LOG + Program.databaze.getUzivatelJmeno();
            pacient_ToolStripStatusLabel.Text = PAC + pacient.jmeno + " " + pacient.prijmeni;
            verze_ToolStripStatusLabel.Text = "MediTab " + Application.ProductVersion;
            
            //inicializace medikacni karty
            LoadingForm.Progress("Inicializace medikační karty");
            LoadMedikace();
            OrdinaceForm = new Ordinace_Form(Keyboard, NumKeyboard, Medikace);
            PodaniForm = new Podani_Form(Keyboard, NumKeyboard);
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 50;
            timer.Tick += new EventHandler(timer_Tick);

            //inicializace zalozky s bilanci tekutin
            LoadingForm.Progress("Inicializace bilance tekutin");
            LoadBilance();
            
            //inicializace zalozky s hodinovou bilanci tekutin
            LoadingForm.Progress("Inicializace hodinové bilance tekutin");
            LoadBilanceHod();
            BilanceHodForm = new BilanceHod_Form(BilanceHod, zpetnePodani, NumKeyboard);

            //inicializace karty fyziologie
            LoadingForm.Progress("Inicializace karty fyziologie");
            LoadFyziologie(true);

            //inicializace invazivnich pristupu
            LoadingForm.Progress("Inicializace invazivních přístupů");
            LoadPristupy(true);
           
            //zavreni nacitaciho okna
            LoadingForm.Close();
            GC.Collect();
        }

        /// <summary>
        /// Pri zavreni karty pacienta
        /// 
        /// Zobrazeni okna s vyberem pacientu
        /// Zavreni aktualniho okna a uvolneni pameti
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pacient_Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            MainForm.Show();
            this.Dispose();
            GC.Collect();
        }

#endregion

#region Buttons

        /// <summary>
        /// Zobrazi okno pro moznosti oprav
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void opravy_Button_Click(object sender, EventArgs e)
        {
            //test byla-li bilance tekutin ulozena
            bilanceTest();

            //okno oprav
            Opravy_Form opravyForm = new Opravy_Form(Pacient.id, tabControl.SelectedIndex + 1);
            opravyForm.ShowDialog();

            //test, jestli byla provedena oprava a je nutny refresh
            if (!opravyForm.oprava) return;

            //znovunacteni karty
            switch (tabControl.SelectedIndex)
            {
                case 0:
                    LoadMedikace();
                    break;
                case 1:
                    LoadBilance();
                    break;
                case 2:
                    LoadBilanceHod();
                    break;
                case 3:
                    tabControl.Visible = false;
                    LoadPristupy(false);
                    tabControl.Visible = true;
                    break;
                case 4:
                    tabControl.Visible = false;
                    LoadFyziologie(false);
                    tabControl.Visible = true;
                    break;
            }

            GC.Collect();
        }

        /// <summary>
        /// Zobrazuje a schovava Buttony v zavislosti na zvolene TabPage
        /// 
        /// Pristupy - scrollovaci Buttony
        /// Bilance - Button zpet bez ulozeni
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl_Selected(object sender, TabControlEventArgs e)
        {
            //test byla-li bilance tekutin ulozena
            bilanceTest();

            if (e.TabPage == hodBilance_TabPage || e.TabPage == diureza_TabPage)
            {
                zpetNo_Button.Visible = true;
            }
            else
            {
                zpetNo_Button.Visible = false;
            }
        }

        /// <summary>
        /// Test, jestli byla bilance tekutin ulozena.
        /// Pri zmene okna.
        /// </summary>
        private void bilanceTest()
        {
            //test byla-li bilance tekutin ulozena
            if (!bilanceSaved)
            {
                DialogResult result = BigMessageBox.Show("Bilance tekutin nebyla uložena. Chcete ji uložit?", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    Bilance.pushToDtb();
                    BilanceHod.pushToDtb();
                }
                else
                {
                    LoadBilance();
                    LoadBilanceHod();
                }

                bilanceSaved = true;
            }
        }

        /// <summary>
        /// Ulozi data
        /// Zavre kartu pacienta a vrati se zpet k vyberu pacienta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void zpet_Button_Click(object sender, EventArgs e)
        {
            //ulozeni dat do dtb
            Bilance.pushToDtb();
            BilanceHod.pushToDtb();
            this.Close();
        }

        /// <summary>
        /// Zavre kartu pacienta bez ulozeni a vrati se zpet k vyberu pacienta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void zpetNo_Button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

#endregion

#region Medikace

   #region load

        /// <summary>
        /// Inicializuje zalozku s medikacni kartou
        /// Prida leky do ListView
        /// </summary>
        private void LoadMedikace()
        {
            //smazani stavajiciho obsahu
            nazev_DataGridView.Rows.Clear();
            medikace_DataGridView.Rows.Clear();

            //disable sort mode
            disableSortMode();

            //init hodiny nacteni
            medikaceLoadHour = DateTime.Now.Hour;

            Medikace = new MedikacniKarta(Pacient.id);

            PredepsaneLeky = Medikace.getLeky();
            
            //vytvoreni radky pro kazdy lek
            foreach(Lek lek in PredepsaneLeky)
            {
                initRowAll(lek);
            }

            ///////////////////////////////////
            //docasne kvuli spatnemu scrollovani na posledni radku
            nazev_DataGridView.Rows.Add();
            medikace_DataGridView.Rows.Add();
            nazev_DataGridView.Rows.Add();
            medikace_DataGridView.Rows.Add();
            nazev_DataGridView.Rows.Add();
            medikace_DataGridView.Rows.Add();
            //////////////////////////////////

            //seradi hodiny od pocatku a odscrolluje na aktualni
            orderHours();
            scrollHours(medikaceLoadHour);
        }

        /// <summary>
        /// Znovu nacte seznam leku
        /// </summary>
        private void ReloadMedikace()
        {
            //smazani stavajiciho obsahu
            nazev_DataGridView.Rows.Clear();
            medikace_DataGridView.Rows.Clear();

            //odstraneni zvyrazneni aktualni hodiny
            medikace_DataGridView.Columns[medikaceLoadHour].DefaultCellStyle.BackColor = Color.White;

            //init hodiny nacteni
            medikaceLoadHour = DateTime.Now.Hour;

            //nacteni novych leku
            PredepsaneLeky = Medikace.getLeky();

            //inicializuje radky DataGridViewu
            foreach (Lek lek in PredepsaneLeky)
            {
                //inicializace jedne radky pro oba viewy
                initRowAll(lek);
            }

            ///////////////////////////////////
            //docasne kvuli spatnemu scrollovani na posledni radku
            nazev_DataGridView.Rows.Add();
            medikace_DataGridView.Rows.Add();
            nazev_DataGridView.Rows.Add();
            medikace_DataGridView.Rows.Add();
            nazev_DataGridView.Rows.Add();
            medikace_DataGridView.Rows.Add();
            //////////////////////////////////

            //odscrollovani na aktualni hodinu
            scrollHours(medikaceLoadHour);
        }

        /// <summary>
        /// Disable sorting mode in both DataGridViews
        /// </summary>
        private void disableSortMode()
        {
            foreach (DataGridViewColumn column in nazev_DataGridView.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            foreach (DataGridViewColumn column in medikace_DataGridView.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        #endregion

   #region init hours

        /// <summary>
        /// Seradi hodiny od pocatku dne
        /// Indexy sloupcu zustavaji stejne, pouze se zmeni poradi
        /// </summary>
        private void orderHours()
        {
            for (int i = 0; i < 24; i++)
            {
                int j = pocatek + i;

                if (j > 23)
                {
                    j -= 24;
                }

                medikace_DataGridView.Columns[j].DisplayIndex = i;
            }
        }

        /// <summary>
        /// Odscrolluje na aktualni hodinu - zpetne podani
        /// </summary>
        private void scrollHours(int hour)
        {
            int aktualniHodina = hour;
            
            int zobrazenaHodina = aktualniHodina - zpetnePodani;

            //osetreni zapornych hodnot
            if (zobrazenaHodina < 0)
            {
                zobrazenaHodina += 24;
            }

            //pri zpetnem podani 12 a vetsim to blbne
            if (zpetnePodani < 12)
            {
                //pokud je aktualni hodina vetsi jak pocatek a zobrazovana mensi, zobrazi se aktualni
                if (aktualniHodina >= pocatek && zobrazenaHodina < pocatek)
                {
                    medikace_DataGridView.FirstDisplayedScrollingColumnIndex = pocatek;
                }
                else
                {
                    medikace_DataGridView.FirstDisplayedScrollingColumnIndex = zobrazenaHodina;
                }
            }
            else
            {
                medikace_DataGridView.FirstDisplayedScrollingColumnIndex = aktualniHodina;
            }
            

            //zvyrazneni aktualni hodiny
            //medikace_DataGridView.Columns[aktualniHodina].DefaultCellStyle.BackColor = Color.Khaki;
            /*foreach (DataGridViewRow row in medikace_DataGridView.Rows)
            {
                row.Cells[aktualniHodina].Style.BackColor = Color.LightYellow;
            }*/
        }

   #endregion

   #region init rows

        /// <summary>
        /// Vytvori radku medikace pro oba DataGridView
        /// </summary>
        private void initRowAll(Lek lek)
        {
            //pro nazev_DataGridView
            string[] row = initRowNazev(lek);
            nazev_DataGridView.Rows.Add(row);
            colorRowNazev(nazev_DataGridView.Rows[nazev_DataGridView.Rows.Count - 1], lek);

            //pro medikace_DataGridView
            row = initRowMedikace(lek);
            medikace_DataGridView.Rows.Add(row);
            colorRowMedikace(medikace_DataGridView.Rows[medikace_DataGridView.Rows.Count - 1], lek);
        }

        /// <summary>
        /// Vytvori radku pro nazev_DataGridView s nazvem a podanim leku
        /// </summary>
        /// <param name="lek"></param>
        /// <returns></returns>
        private string[] initRowNazev(Lek lek)
        {
            string[] row = new string[2];
            
            row[0] = lek.nazev;
            row[1] = lek.davkovani;
            
            return row;
        }

        /// <summary>
        /// Obarvi radku nazev_DataGridView podle poznamky u podani
        /// </summary>
        /// <param name="row"></param>
        /// <param name="lek"></param>
        private void colorRowNazev(DataGridViewRow row, Lek lek)
        {
            //test na opiat
            if (lek.jeOpiat)
            {
                row.Cells[0].Style.BackColor = Color.Red;
            }
            
            //davkovani ma poznamku
            if (lek.maPoznamku)
            {
                row.Cells[1].Style.BackColor = Color.LightBlue;
            }
        }

        /// <summary>
        /// Vytvori radku medikace_DataGridView s podanim leku
        /// </summary>
        /// <param name="lek"></param>
        /// <returns></returns>
        private string[] initRowMedikace(Lek lek)
        {
            string[] row = new string[24];

            foreach(var ordinace in lek.ordinace)
            {
                row[ordinace.datumOd.Hour] = ordinace.davka.ToString();
            }
            //inicializace davkovani
            /*for (int i = 0; i < 24; i++)
            {
                if (lek.davkovaniCas[i].davka > 0)
                {
                    row[i] = lek.davkovaniCas[i].davka.ToString();
                }
            }*/

            return row;
        }

        /// <summary>
        /// Obarvi radku medikace_DataGridView podle podani leku
        /// </summary>
        /// <param name="row"></param>
        /// <param name="lek"></param>
        private void colorRowMedikace(DataGridViewRow row, Lek lek)
        {
            //aktualni hodina
            row.Cells[medikaceLoadHour].Style.BackColor = Color.Khaki;

            int startH;
            int endH;

            foreach (var ordinace in lek.ordinace)
            {
                startH = ordinace.datumOd.Hour;

                switch (ordinace.stavPodani)
                {
                    case Stav.ORDINACE: row.Cells[startH].Style.BackColor = Color.LightGray; break;
                    case Stav.PROVEDENA: row.Cells[startH].Style.BackColor = Color.Green; break;
                    case Stav.NEPROVEDENA: row.Cells[startH].Style.BackColor = Color.Purple; break;
                }

                //infuze
                if ((ordinace.konec || ordinace.kontinual) && (ordinace.stavPodani == Stav.PROVEDENA || ordinace.stavPodani == Stav.ORDINACE || ordinace.stavPodani == Stav.NEPROVEDENA))
                {
                    //init endH
                    if (ordinace.konec)
                    {
                        DateTime date = (DateTime)ordinace.datumDo;
                        endH = date.Hour;
                    }
                    else
                    {
                        endH = medikaceLoadHour;
                    }

                    //obarveni infuzi
                    if(startH <= endH)
                    {
                        colorKontinual(ordinace, row, startH, endH);
                    }
                    //pres pulnoc
                    else
                    {
                        colorKontinual(ordinace, row, startH, 23);
                        colorKontinual(ordinace, row, -1, endH);
                    }
                }
            }
        }

        private void colorKontinual(Ordinace ordinace, DataGridViewRow row, int startH, int endH)
        {
            while (startH < endH)
            {
                startH++;
                if (ordinace.stavPodani == Stav.PROVEDENA)
                {
                    row.Cells[startH].Style.BackColor = Color.LightGreen;
                }
                if (ordinace.stavPodani == Stav.ORDINACE)
                {
                    row.Cells[startH].Style.BackColor = Color.LightGray;
                }
                if (ordinace.stavPodani == Stav.NEPROVEDENA)
                {
                    row.Cells[startH].Style.BackColor = Color.Purple;
                }
            }
        }

        #endregion

   #region click DataGridView

        /// <summary>
        /// Obsluha kliknuti na polozku v tabulce nazev_DataGridView_CellClick
        /// Zobrazi se BigMessageBox s nazvem leku a davkovanim
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nazev_DataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //click na header
            if (e.RowIndex < 0) return;

            ///////////////////////////////
            //docasne kvuli spatnemu scrollovani na posledni radku
            if (e.RowIndex > medikace_DataGridView.Rows.Count - 4) return;
            ///////////////////////////////

            OrdinaceForm.Show(PredepsaneLeky[e.RowIndex]);
            //BigMessageBox.Show(PredepsaneLeky[e.RowIndex].nazev + "\n" + PredepsaneLeky[e.RowIndex].davkovani);
            nazev_DataGridView.ClearSelection();

            for (int i = 0; i < 24; i++)
            {
                medikace_DataGridView.Rows[e.RowIndex].Cells[i].Value = null;
                medikace_DataGridView.Rows[e.RowIndex].Cells[i].Style.BackColor = Color.White;
            }

            foreach (var ordinace in PredepsaneLeky[e.RowIndex].ordinace)
            {
                medikace_DataGridView.Rows[e.RowIndex].Cells[ordinace.datumOd.Hour].Value = ordinace.davka.ToString();
            }
            colorRowMedikace(medikace_DataGridView.Rows[e.RowIndex], PredepsaneLeky[e.RowIndex]);
        }

        
        /// <summary>
        /// Obsluha kliknuti na polozku v tabulce medikace_DataGridView_CellClick
        /// Jedno klikuti otevre okno s ordinacemi
        /// Double klik provede podani v danou hodinu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void medikace_DataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //click na header
            if (e.RowIndex < 0) return;

            ///////////////////////////////
            //docasne kvuli spatnemu scrollovani na posledni radku
            if (e.RowIndex > medikace_DataGridView.Rows.Count - 4) return;
            //////////////////////////////

            //index leku
            lekID = e.RowIndex;
            //hodina, na kterou se kliklo
            hour = e.ColumnIndex;
            Console.WriteLine("ted");
            //Cekani na druhy klik - timer
            if (click1)
            {
                click1 = false;
                Invalidate();
                timer.Start();
            }
            else
            {
                if(milliseconds < SystemInformation.DoubleClickTime)
                {
                    click2 = true;
                }
            }

            /*
            //Cekani na druhy klik - vlakno
            if (clickThread)
            {
                clickThread = false;
                bool res = Medikace.zadejPodani(lekID, hour);

                if (res)
                {
                    medikace_DataGridView.Rows[lekID].Cells[hour].Style.BackColor = Color.Green;
                    if(medikace_DataGridView.Rows[lekID].Cells[hour].Value == null)
                    {
                        medikace_DataGridView.Rows[lekID].Cells[hour].Value = 1;
                    }
                }

                medikace_DataGridView.ClearSelection();
                return;
            }
            clickThread = true;

            //Zapne cekani
            //await WaitClick();
            await Task.Run(() => System.Threading.Thread.Sleep(1000));

            if (!clickThread)
            {
                medikace_DataGridView.ClearSelection();
                return;
            }
            clickThread = false;

            OrdinaceShow(lekID, hour);
            */
        }

        private Task WaitClick()
        {
            return Task.Run(() =>
            {
                System.Threading.Thread.Sleep(1000);
            });
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            milliseconds += 50;
            if(milliseconds >= SystemInformation.DoubleClickTime)
            {
                timer.Stop();
                
                if (click2)
                {
                    bool res = Medikace.zadejPodani(lekID, hour);

                    if (res)
                    {
                        medikace_DataGridView.Rows[lekID].Cells[hour].Style.BackColor = Color.Green;
                        if (medikace_DataGridView.Rows[lekID].Cells[hour].Value == null)
                        {
                            medikace_DataGridView.Rows[lekID].Cells[hour].Value = 1;
                        }
                    }

                    medikace_DataGridView.ClearSelection();
                }
                else
                {
                    OrdinaceShow(lekID, hour);
                    medikace_DataGridView.ClearSelection();
                }

                click1 = true;
                click2 = false;
                milliseconds = 0;
            }
        }

        private void OrdinaceShow(int lekID, int hour)
        {
            OrdinaceForm.Show(PredepsaneLeky[lekID], hour);
            medikace_DataGridView.ClearSelection();

            for (int i = 0; i < 24; i++)
            {
                medikace_DataGridView.Rows[lekID].Cells[i].Value = null;
                medikace_DataGridView.Rows[lekID].Cells[i].Style.BackColor = Color.White;
            }

            foreach (var ordinace in PredepsaneLeky[lekID].ordinace)
            {
                medikace_DataGridView.Rows[lekID].Cells[ordinace.datumOd.Hour].Value = ordinace.davka.ToString();
            }
            colorRowMedikace(medikace_DataGridView.Rows[lekID], PredepsaneLeky[lekID]);
        }

        private void PodaniShow(int lekID, int hour)
        {
            //aktualni cas
            int aktualniHodina = DateTime.Now.Hour;

            // pokud se kliklo na uz vyplnene podani leku
            if (medikace_DataGridView.Rows[lekID].Cells[hour].Style.BackColor == Color.Green ||
                medikace_DataGridView.Rows[lekID].Cells[hour].Style.BackColor == Color.LightGreen || medikace_DataGridView.Rows[lekID].Cells[hour].Style.BackColor == Color.Purple)
            {
                string datum;
                //existence datumDo - test podle defaultni hodnoty DateTime
                if (PredepsaneLeky[lekID].davkovaniCas[hour].datumDo.Equals(new DateTime()))
                {
                    datum = "\nDatum od: \t" + PredepsaneLeky[lekID].davkovaniCas[hour].datumOd;
                }
                else {
                    datum = "\nDatum od - do: \t" + PredepsaneLeky[lekID].davkovaniCas[hour].datumOd + " - " + PredepsaneLeky[lekID].davkovaniCas[hour].datumDo;
                }


                BigMessageBox.Show("Název: \t" + PredepsaneLeky[lekID].nazev + "\nDávka: \t" + PredepsaneLeky[lekID].davkovaniCas[hour].davka + " " + PredepsaneLeky[lekID].davkovaniCas[hour].poznamka + datum + "\nPodal: \t" + PredepsaneLeky[lekID].davkovaniCas[hour].uzivatelJmeno);

                medikace_DataGridView.ClearSelection();
                return;
            }

            //kontrola, zda se kliklo v povolenou hodinu
            if (hour <= aktualniHodina && hour >= aktualniHodina - zpetnePodani)
            {
                //kontrola, zda vybraná hodina nepatří k zítřku, pokud ne, podmínka projde
                if (aktualniHodina <= pocatek || hour >= pocatek)
                {
                    decimal zadaneMnozstvi;
                    //test predvyplneneho mnozstvi podani
                    if (medikace_DataGridView.Rows[lekID].Cells[hour].Value != null && decimal.TryParse(medikace_DataGridView.Rows[lekID].Cells[hour].Value.ToString(), out zadaneMnozstvi))
                    {
                        PodaniForm.showPodani(zadaneMnozstvi, PredepsaneLeky[lekID]);
                    }
                    else
                    {
                        PodaniForm.showPodani(PredepsaneLeky[lekID]);
                    }

                    //vysledek zadani do dtb - false je ok, true obnova seznamu
                    bool reload = false;

                    //provedene podani
                    if (PodaniForm.DialogResult == DialogResult.OK)
                    {
                        String mnozstvi = PodaniForm.mnozstvi_NumericUpDown.Value.ToString();
                        mnozstvi = mnozstvi.Replace(',', '.');

                        //reload = Medikace.zadejPodani(Pacient.id, PredepsaneLeky[lekID].idLeku, PredepsaneLeky[lekID].ordlekID, hodina,
                        //    float.Parse(mnozstvi, System.Globalization.CultureInfo.InvariantCulture), PodaniForm.poznamka_TextBox.Text, "N" , DateTime.Today.AddHours(hodina), "MJ");//"MJ" sou merny jednotky, mas na ne vytvorenej ciselnik

                        //reload seznamu leku
                        if (reload)
                        {
                            BigMessageBox.Show("Podání nebylo provedeno, je nutné aktualizovat seznam léků.");
                            ReloadMedikace();
                            return;
                        }

                        medikace_DataGridView.Rows[lekID].Cells[hour].Value = PodaniForm.mnozstvi_NumericUpDown.Value.ToString();

                        //Barva oznaceni polozky
                        if (medikace_DataGridView.Rows[lekID].Cells[hour].Style.BackColor == Color.Gray)
                        {
                            medikace_DataGridView.Rows[lekID].Cells[hour].Style.BackColor = Color.Green;
                        }
                        else
                        {
                            medikace_DataGridView.Rows[lekID].Cells[hour].Style.BackColor = Color.LightGreen;
                        }
                    }

                    //neprovedene podani
                    if (PodaniForm.DialogResult == DialogResult.Ignore)
                    {
                        //reload = Medikace.zadejPodani(Pacient.id, PredepsaneLeky[lekID].idLeku, PredepsaneLeky[lekID].ordlekID, hodina, 0, PodaniOldForm.poznamka_TextBox.Text, "N", DateTime.Today.AddHours(hodina), "MJ");

                        //reload seznamu leku
                        if (reload)
                        {
                            BigMessageBox.Show("Podání nebylo provedeno, je nutné aktualizovat seznam léků.");
                            ReloadMedikace();
                            return;
                        }

                        medikace_DataGridView.Rows[lekID].Cells[hour].Value = "";
                        medikace_DataGridView.Rows[lekID].Cells[hour].Style.BackColor = Color.Purple;
                    }
                }
                //pokud podmínka selhala, hodina patří k zítřku, tudíž ještě nenastala
                else
                {
                    BigMessageBox.Show("Tato hodina ještě nenastala, patří k zítřku.");
                }
            }
            else
            {
                //pokud je hodina, na kterou se kliklo větší, než hodina aktuální, taková hodina ještě nebyla
                if (hour > aktualniHodina)
                {
                    BigMessageBox.Show("Tato hodina ještě nenastala.");
                }
                //jinak hodina neprošla předchozí podmínkou jelikož byla příliš daleko a nastavené zpětné podání akci nepustilo
                else
                {
                    BigMessageBox.Show("Do této hodiny není možný kvůli konfiguraci aplikace již zapisovat.");
                }

            }

            medikace_DataGridView.ClearSelection();
        }

        #endregion

   #region scroll DataGridViews
        /// <summary>
        /// Scrolluje synchronizovane medikace_DataGridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nazev_DataGridView_Scroll(object sender, ScrollEventArgs e)
        {
            //medikace_DataGridView.VerticalScrollingOffset = e.NewValue;

            PropertyInfo verticalOffset = medikace_DataGridView.GetType().GetProperty("VerticalOffset", BindingFlags.NonPublic | BindingFlags.Instance);
            verticalOffset.SetValue(this.medikace_DataGridView, nazev_DataGridView.VerticalScrollingOffset, null);
        }

        /// <summary>
        /// Scrolluje synchronizovane nazev_DataGridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void medikace_DataGridView_Scroll(object sender, ScrollEventArgs e)
        {
            //nazev_DataGridView.VerticalScrollingOffset = e.NewValue;

            PropertyInfo verticalOffset = nazev_DataGridView.GetType().GetProperty("VerticalOffset", BindingFlags.NonPublic | BindingFlags.Instance);
            //if (medikace_DataGridView.VerticalScrollingOffset.Equals(null))
            try
            {
                verticalOffset.SetValue(this.nazev_DataGridView, medikace_DataGridView.VerticalScrollingOffset, null);
            }
            catch (Exception exc) { }
        }

    #endregion

        /// <summary>
        /// Prida novy lek
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pridej_Button_Click(object sender, EventArgs e)
        {
            //okno pro pridani leku
            Lek_Form lekForm = new Lek_Form(Keyboard);

            if (lekForm.ShowDialog() == DialogResult.OK)
            {
                Lek lek = null;

                //pridani leku do databaze
                //pokud uspesne, init lek, jinak null
                lek = Medikace.addLek(lekForm.lek_ComboBox.Text, lekForm.davkovani_TextBox.Text, lekForm.id);

                //test uspesneho pridani leku
                if (lek != null)
                {
                    ///////////////////////////////////
                    //docasne kvuli spatnemu scrollovani na posledni radku
                    int r = nazev_DataGridView.Rows.Count;
                    nazev_DataGridView.Rows.RemoveAt(r - 1);
                    nazev_DataGridView.Rows.RemoveAt(r - 2);
                    nazev_DataGridView.Rows.RemoveAt(r - 3);
                    medikace_DataGridView.Rows.RemoveAt(r - 1);
                    medikace_DataGridView.Rows.RemoveAt(r - 2);
                    medikace_DataGridView.Rows.RemoveAt(r - 3);
                    ///////////////////////////////////

                    //prida lek do DataGridView
                    initRowAll(lek);

                    ///////////////////////////////////
                    //docasne kvuli spatnemu scrollovani na posledni radku
                    nazev_DataGridView.Rows.Add();
                    medikace_DataGridView.Rows.Add();
                    nazev_DataGridView.Rows.Add();
                    medikace_DataGridView.Rows.Add();
                    nazev_DataGridView.Rows.Add();
                    medikace_DataGridView.Rows.Add();
                    //////////////////////////////////                    
                }
                else
                {
                    BigMessageBox.Show("Neexistuje klinická událost. Není možné naordinovat nový lék.");
                }
            }
        }

#endregion

#region Bilance

        /// <summary>
        /// Inicializuje zalozku s diurezou
        /// </summary>
        private void LoadBilance()
        {
            //inicializace instance tridy zpracovavajici diurezu
            Bilance = new Diureza(Pacient.id);
            bilanceSaved = true;

            //aktualni datum
            bilance_datum.Text = Bilance.datumDiurezy.ToShortDateString();

            //prijmy
            initHodnotyPrijem();
            //vydaje
            initHodnotyVydej();
            //inicializace indexu
            initIndexy();
        }

        /// <summary>
        /// Inicializuje zalozku s diurezou
        /// </summary>
        private void reloadBilance()
        {
            //aktualni datum
            bilance_datum.Text = Bilance.datumDiurezy.ToShortDateString();

            //prijmy
            initHodnotyPrijem();
            //vydaje
            initHodnotyVydej();
        }

   #region init

        /// <summary>
        /// Inicializuje hodnoty pro prijem
        /// </summary>
        private void initHodnotyPrijem()
        {
            perOs_TextBox.Text = Bilance.getHodnota(Tekutiny.PER_OS);
            jinyEnteralni_TextBox.Text = Bilance.getHodnota(Tekutiny.JINY_ENTERALNI);
            parenteralni_TextBox.Text = Bilance.getHodnota(Tekutiny.PARENTERALNI);
            jinyParenteralni_TextBox.Text = Bilance.getHodnota(Tekutiny.JINY_PARENTERALNI);
            mlp_TextBox.Text = Bilance.getHodnota(Tekutiny.MLP);
            eryRes_TextBox.Text = Bilance.getHodnota(Tekutiny.ERY_RES);
            ostatKrevDerivaty_TextBox.Text = Bilance.getHodnota(Tekutiny.OSTATNI_KREVNI_DERIVATY);
            prijemCelkem_TextBox.Text = Bilance.getPrijem();
        }

        /// <summary>
        /// Inicializuje hodnoty pro Vydej
        /// </summary>
        private void initHodnotyVydej()
        {
            diureza_TextBox.Text = Bilance.getHodnota(Tekutiny.DIUREZA);
            sonda_TextBox.Text = Bilance.getHodnota(Tekutiny.SONDA);
            jineZtraty_TextBox.Text = Bilance.getHodnota(Tekutiny.JINE_ZTRATY);
            ultrafiltrace_TextBox.Text = Bilance.getHodnota(Tekutiny.ULTRAFILTRACE);
            dreny_TextBox.Text = Bilance.getHodnota(Tekutiny.DRENY);
            vydejCelkem_TextBox.Text = Bilance.getVydej();
        }

        /// <summary>
        /// Inicializuje indexy TextBoxu
        /// </summary>
        private void initIndexy()
        {
            //indexy celkove
            perOs_TextBox.TabIndex = (int)Tekutiny.PER_OS;
            jinyEnteralni_TextBox.TabIndex = (int)Tekutiny.JINY_ENTERALNI;
            parenteralni_TextBox.TabIndex = (int)Tekutiny.PARENTERALNI;
            jinyParenteralni_TextBox.TabIndex = (int)Tekutiny.JINY_PARENTERALNI;
            mlp_TextBox.TabIndex = (int)Tekutiny.MLP;
            eryRes_TextBox.TabIndex = (int)Tekutiny.ERY_RES;
            ostatKrevDerivaty_TextBox.TabIndex = (int)Tekutiny.OSTATNI_KREVNI_DERIVATY;
            diureza_TextBox.TabIndex = (int)Tekutiny.DIUREZA;
            sonda_TextBox.TabIndex = (int)Tekutiny.SONDA;
            jineZtraty_TextBox.TabIndex = (int)Tekutiny.JINE_ZTRATY;
            ultrafiltrace_TextBox.TabIndex = (int)Tekutiny.ULTRAFILTRACE;
            dreny_TextBox.TabIndex = (int)Tekutiny.DRENY;

            //indexy pro +   (posunuti o +100)
            perOsPlus_TextBox.TabIndex = (int)Tekutiny.PER_OS + 100;
            jinyEnteralniPlus_TextBox.TabIndex = (int)Tekutiny.JINY_ENTERALNI + 100;
            parenteralniPlus_TextBox.TabIndex = (int)Tekutiny.PARENTERALNI + 100;
            jinyParenteralniPlus_TextBox.TabIndex = (int)Tekutiny.JINY_PARENTERALNI + 100;
            mlpPlus_TextBox.TabIndex = (int)Tekutiny.MLP + 100;
            eryResPlus_TextBox.TabIndex = (int)Tekutiny.ERY_RES + 100;
            ostatKrevDerivatyPlus_TextBox.TabIndex = (int)Tekutiny.OSTATNI_KREVNI_DERIVATY + 100;
            diurezaPlus_TextBox.TabIndex = (int)Tekutiny.DIUREZA + 100;
            sondaPlus_TextBox.TabIndex = (int)Tekutiny.SONDA + 100;
            jineZtratyPlus_TextBox.TabIndex = (int)Tekutiny.JINE_ZTRATY + 100;
            ultrafiltracePlus_TextBox.TabIndex = (int)Tekutiny.ULTRAFILTRACE + 100;
            drenyPlus_TextBox.TabIndex = (int)Tekutiny.DRENY + 100;
        }

        #endregion

   #region Ovladani komponent

        /// <summary>
        /// Zadani hodnoty bilance tekutin
        /// Implementu ji vsechny aktivni textboxy pro prijem a vydej (vcetne +)
        /// 
        /// Zobrazeni numericke klavesnice
        /// Ulozeni hodnoty
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bilance_TextBox_Enter(object sender, EventArgs e)
        {
            //oznaceni textu v textboxu
            TextBox tb = (TextBox)sender;
            tb.SelectAll();
            
            //zobrazeni numericke klavesnice jako dialog
            NumKeyboard.showKeyboard(false);

            if (NumKeyboard.DialogResult == DialogResult.OK)
            {
                //pro celkove hodnoty nebo pro +hodnoty
                if (tb.TabIndex < 100)
                {
                    tb.Text = Bilance.zadejHodnotu(tb.TabIndex, NumKeyboard.value_TextBox.Text);
                    prijemCelkem_TextBox.Text = Bilance.getPrijem();
                    vydejCelkem_TextBox.Text = Bilance.getVydej();
                }
                else
                {
                    if (!NumKeyboard.value_TextBox.Text.Equals(""))
                    {
                        Bilance.pridejHodnotu(tb.TabIndex - 100, NumKeyboard.value_TextBox.Text);
                        tb.Text = "";
                        initHodnotyPrijem();
                        initHodnotyVydej();
                    }
                }
                bilanceSaved = false;
            }
        }

        /// <summary>
        /// Odesle data do databaze
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uloz_Button_Click(object sender, EventArgs e)
        {
            Bilance.pushToDtb();
            bilanceSaved = true;
        }

        #endregion

#endregion

#region Hod Bilance

        /// <summary>
        /// Inicializuje hodinovou bilanci
        /// </summary>
        private void LoadBilanceHod()
        {
            //inicializace instance tridy zpracovavajici hodinovou bilanci
            BilanceHod = new BilanceHod(Pacient.id);
            bilanceSaved = true;

            //aktualni datum
            bilanceHod_datum.Text = BilanceHod.datumBilanceHod.ToShortDateString();
            //aktualni hodina
            bilanceHodLoadHour = DateTime.Now.Hour;

            //prijmy
            initHodnotyPrijemHod(bilanceHodLoadHour);
            //vydaje
            initHodnotyVydejHod(bilanceHodLoadHour);
            //inicializace indexu
            initIndexyHod();
        }

        /// <summary>
        /// Inicializuje hodinovou bilanci
        /// </summary>
        private void reloadBilanceHod()
        {
            //aktualni datum
            bilanceHod_datum.Text = BilanceHod.datumBilanceHod.ToShortDateString();
            //aktualni hodina
            bilanceHodLoadHour = DateTime.Now.Hour;
            //prijmy
            initHodnotyPrijemHod(bilanceHodLoadHour);
            //vydaje
            initHodnotyVydejHod(bilanceHodLoadHour);
        }

   #region init

        /// <summary>
        /// Inicializuje hodnoty pro prijem
        /// </summary>
        private void initHodnotyPrijemHod(int hour)
        {
            prijemHod_hodina.Text = hour.ToString() + " h";

            perOsHod_TextBox.Text = BilanceHod.getHodnota(Tekutiny.PER_OS, hour);
            jinyEnteralniHod_TextBox.Text = BilanceHod.getHodnota(Tekutiny.JINY_ENTERALNI, hour);
            parenteralniHod_TextBox.Text = BilanceHod.getHodnota(Tekutiny.PARENTERALNI, hour);
            jinyParenteralniHod_TextBox.Text = BilanceHod.getHodnota(Tekutiny.JINY_PARENTERALNI, hour);
            mlpHod_TextBox.Text = BilanceHod.getHodnota(Tekutiny.MLP, hour);
            eryResHod_TextBox.Text = BilanceHod.getHodnota(Tekutiny.ERY_RES, hour);
            ostatKrevDerivatyHod_TextBox.Text = BilanceHod.getHodnota(Tekutiny.OSTATNI_KREVNI_DERIVATY, hour);
            prijemHod_TextBox.Text = BilanceHod.getPrijemHod(hour);
            prijemCelkemHod_TextBox.Text = BilanceHod.getCelkovyPrijem();
        }

        /// <summary>
        /// Inicializuje hodnoty pro Vydej
        /// </summary>
        private void initHodnotyVydejHod(int hour)
        {
            vydejHod_hodina.Text = hour.ToString() + " h";

            diurezaHod_TextBox.Text = BilanceHod.getHodnota(Tekutiny.DIUREZA, hour);
            sondaHod_TextBox.Text = BilanceHod.getHodnota(Tekutiny.SONDA, hour);
            jineZtratyHod_TextBox.Text = BilanceHod.getHodnota(Tekutiny.JINE_ZTRATY, hour);
            ultrafiltraceHod_TextBox.Text = BilanceHod.getHodnota(Tekutiny.ULTRAFILTRACE, hour);
            drenyHod_TextBox.Text = BilanceHod.getHodnota(Tekutiny.DRENY, hour);
            vydejHod_TextBox.Text = BilanceHod.getVydejHod(hour);
            vydejCelkemHod_TextBox.Text = BilanceHod.getCelkovyVydej();
        }

        /// <summary>
        /// Inicializuje indexy TextBoxu a Buttonu
        /// </summary>
        private void initIndexyHod()
        {
            //indexy celkove
            perOsHod_TextBox.TabIndex = (int)Tekutiny.PER_OS;
            jinyEnteralniHod_TextBox.TabIndex = (int)Tekutiny.JINY_ENTERALNI;
            parenteralniHod_TextBox.TabIndex = (int)Tekutiny.PARENTERALNI;
            jinyParenteralniHod_TextBox.TabIndex = (int)Tekutiny.JINY_PARENTERALNI;
            mlpHod_TextBox.TabIndex = (int)Tekutiny.MLP;
            eryResHod_TextBox.TabIndex = (int)Tekutiny.ERY_RES;
            ostatKrevDerivatyHod_TextBox.TabIndex = (int)Tekutiny.OSTATNI_KREVNI_DERIVATY;
            diurezaHod_TextBox.TabIndex = (int)Tekutiny.DIUREZA;
            sondaHod_TextBox.TabIndex = (int)Tekutiny.SONDA;
            jineZtratyHod_TextBox.TabIndex = (int)Tekutiny.JINE_ZTRATY;
            ultrafiltraceHod_TextBox.TabIndex = (int)Tekutiny.ULTRAFILTRACE;
            drenyHod_TextBox.TabIndex = (int)Tekutiny.DRENY;

            //indexy pro Buttony   (posunuti o +100)
            perOsHod_Button.TabIndex = (int)Tekutiny.PER_OS + 100;
            jinyEnteralniHod_Button.TabIndex = (int)Tekutiny.JINY_ENTERALNI + 100;
            parenteralniHod_Button.TabIndex = (int)Tekutiny.PARENTERALNI + 100;
            jinyParenteralniHod_Button.TabIndex = (int)Tekutiny.JINY_PARENTERALNI + 100;
            mlpHod_Button.TabIndex = (int)Tekutiny.MLP + 100;
            eryResHod_Button.TabIndex = (int)Tekutiny.ERY_RES + 100;
            ostatKrevDerivatyHod_Button.TabIndex = (int)Tekutiny.OSTATNI_KREVNI_DERIVATY + 100;
            diurezaHod_Button.TabIndex = (int)Tekutiny.DIUREZA + 100;
            sondaHod_Button.TabIndex = (int)Tekutiny.SONDA + 100;
            jineZtratyHod_Button.TabIndex = (int)Tekutiny.JINE_ZTRATY + 100;
            ultrafiltraceHod_Button.TabIndex = (int)Tekutiny.ULTRAFILTRACE + 100;
            drenyHod_Button.TabIndex = (int)Tekutiny.DRENY + 100;
        }

   #endregion

   #region Ovladani komponent

        /// <summary>
        /// Oznaceni celeho textu pri aktivovani textboxu
        /// Zobrazeni numericke klavesnice
        /// Implementu ji vsechny aktivni textboxy - udalosti Enter a Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bilanceHod_TextBox_Enter(object sender, EventArgs e)
        {
            //kontrola aktualni hodiny
            if (bilanceHodLoadHour != DateTime.Now.Hour)
            {
                BigMessageBox.Show("Změnila se aktuální hodnota od posledního načtení bilance tekutin.\nBilance tekutin bude znova načtena.\nPokračujte v zadání k " + DateTime.Now.Hour + ". hodině.");
                reloadBilanceHod();
            }

            //oznaceni textu v textboxu
            TextBox tb = (TextBox)sender;
            tb.SelectAll();

            //kontrola vicenasobneho zadani
            if (!tb.Text.Equals("0"))
            {
                BigMessageBox.Show("Tato hodnota již byla vyplněna.\nNelze zadat dvě hodnoty v jednu hodin.");
                return;
            }

            //zobrazeni numericke klavesnice jako dialog
            NumKeyboard.showKeyboard(false);

            //pri potvrzeni se ulozi hodnota
            if (NumKeyboard.DialogResult == DialogResult.OK)
            {
                //ulozeni hodnoty
                tb.Text = BilanceHod.zadejHodnotu(tb.TabIndex, NumKeyboard.value_TextBox.Text, bilanceHodLoadHour);

                //soucet
                prijemHod_TextBox.Text = BilanceHod.getPrijemHod(bilanceHodLoadHour);
                prijemCelkemHod_TextBox.Text = BilanceHod.getCelkovyPrijem();
                vydejHod_TextBox.Text = BilanceHod.getVydejHod(bilanceHodLoadHour);
                vydejCelkemHod_TextBox.Text = BilanceHod.getCelkovyVydej();

                bilanceSaved = false;
            }
        }

        /// <summary>
        /// Zobrazi dialog hodnot vsech hodin k tekutine
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonHod_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            BilanceHodForm.showBilance((Tekutiny)(button.TabIndex - 100));
            reloadBilanceHod();
        }

        /// <summary>
        /// Odesle data do databaze
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ulozHod_Button_Click(object sender, EventArgs e)
        {
            BilanceHod.pushToDtb();
            bilanceSaved = true;
        }

   #endregion

#endregion

#region Invazivni pristupy

        /// <summary>
        /// Inicializuje zalazku s invazivnimi pristupy
        /// </summary>
        private void LoadPristupy(bool first)
        {
            //smazani stareho obsahu panelu
            pristupy_TouchableFlowLayoutPanel.Controls.Clear();
            //pristupy_TabPage.Controls.Clear();

            //init tridy pracujici s pristupy
            Pristupy = new InvazivniPristupy.ZavedenePristupy(Pacient.id);

            //panel jednoho pristupu
            PristupPanel pristupPanel;

            if (first)
            {
                //labely
                pristupPanel = new PristupPanel();
                //pristupy_TouchableFlowLayoutPanel.Controls.Add(pristupPanel);
                pristupy_TabPage.Controls.Add(pristupPanel);
                //pristupy_TabPage.Controls.SetChildIndex(pristupPanel, 0);
            }

            bool strip = true;
            //zavedene pristupy
            foreach (Pristup pristup in Pristupy.zavedenePristupy)
            {
                pristupPanel = new PristupPanel(pristup, Pristupy, this, strip);
                pristupy_TouchableFlowLayoutPanel.Controls.Add(pristupPanel);
                //pristupy_TabPage.Controls.Add(pristupPanel);
                //pristupy_TabPage.Controls.SetChildIndex(pristupPanel, 0);

                strip = !strip;
            }

            //novy pristup
            pristupPanel = new PristupPanel(Pristupy, this, strip);
            pristupy_TouchableFlowLayoutPanel.Controls.Add(pristupPanel);
            //pristupy_TabPage.Controls.Add(pristupPanel);
            //pristupy_TabPage.Controls.SetChildIndex(pristupPanel, 0);

            //***OLD***
            //tlacitka pro scrollovani v pristupy_TabPage
            //initScrollButtons();
        }

        /// <summary>
        /// aktualizuje zobrazeni pristupu
        /// </summary>
        public void AktualizujPristupy()
        {
            //novy pristup + umisteni na konec
            PristupPanel pristup = new PristupPanel(Pristupy, this, true);
            pristupy_TouchableFlowLayoutPanel.Controls.Add(pristup);
            //pristupy_TouchableFlowLayoutPanel.Controls.SetChildIndex(pristup, 0);
        }

        #endregion

#region Fyziologie

        /// <summary>
        /// Inicializuje zalazku s fyziologii
        /// </summary>
        /// <param name="first">prvni inicializace</param>
        private void LoadFyziologie(bool first)
        {
            //smazani stareho obsahu panelu
            fyziologie_TouchableFlowLayoutPanel.Controls.Clear();

            //init tridy pracujici s pristupy
            Fyziologie = new Fyziologie(Pacient.id);

            //panel jednoho pristupu
            FyziologiePanel fyziologiePanel;

            //pri prvni inicializaci init labelu
            if (first)
            {
                //labely
                fyziologiePanel = new FyziologiePanel();
                fyziologiePanel.Dock = DockStyle.Top;
                fyziologie_TabPage.Controls.Add(fyziologiePanel);
            }

            bool strip = true;
            //zavedene pristupy
            foreach (ZaznamTelFunkci fyz in Fyziologie.zaznamy)
            {
                fyziologiePanel = new FyziologiePanel(fyz, Fyziologie, this, strip);
                fyziologie_TouchableFlowLayoutPanel.Controls.Add(fyziologiePanel);

                strip = !strip;
            }

            //novy pristup
            fyziologiePanel = new FyziologiePanel(Fyziologie, this, strip);
            fyziologie_TouchableFlowLayoutPanel.Controls.Add(fyziologiePanel);
        }

        /// <summary>
        /// Aktualizuje zobrazeni fyziologie
        /// </summary>
        public void AktualizujFyziologie()
        {
            //novy pristup + umisteni na konec
            FyziologiePanel fyziologiePanel = new FyziologiePanel(Fyziologie, this, true);
            fyziologie_TouchableFlowLayoutPanel.Controls.Add(fyziologiePanel);
        }

#endregion

        /// <summary>
        /// Test zadavani cisel
        /// Lze zadat jen cislo
        /// 
        /// Pro Bilanci a BilanciHod
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void diurezaHod_SplitContainer_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
