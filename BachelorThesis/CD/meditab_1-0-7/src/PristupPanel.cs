using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using MediTab.InvazivniPristupy;

namespace MediTab
{
    public partial class PristupPanel : FlowLayoutPanel
    {

#region Variables

        /// <summary>
        /// Invazivni pristup, ktery je komponentou zobrazovan
        /// </summary>
        private Pristup pristup;

        /// <summary>
        /// Instance tridy ovladajici invazivni pristupy
        /// </summary>
        private ZavedenePristupy pristupy;

        /// <summary>
        /// Pole seznamu umisteni
        /// </summary>
        private List<string>[] umisteni;

        /// <summary>
        /// Instance karty pacienta
        /// Nutne pro aktualizaci pristupu po pridani
        /// </summary>
        Pacient_Form form;

        /// <summary>
        /// Konstanta pro radkove obarveni
        /// </summary>
        private readonly bool strip;

#endregion

#region Konstruktor

        /// <summary>
        /// Konstruktor komponenty zobrazujici invazivni pristup
        /// </summary>
        /// <param name="pristup">Data o inv. pristupu</param>
        /// <param name="pristupy">Instance tridy InvazivniPristupy.cs pro ovladani pristupu</param>
        public PristupPanel(Pristup pristup, ZavedenePristupy pristupy, Pacient_Form form, bool strip)
        {
            InitializeComponent();

            //mouseFilterAdd();

            this.pristup = pristup;
            this.pristupy = pristupy;
            this.form = form;
            this.strip = strip;

            //pridani komponent do containeru
            addComponents();
            addButtons();

            //inicializace komponent
            initializeText();

            //prida click event comboboxum a textboxum
            addComponentClick();
        }

        /// <summary>
        /// Konstruktor komponenty zobrazujici pole zadani noveho pristupu
        /// </summary>
        /// <param name="pristupy">Instance tridy InvazivniPristupy.cs pro ovladani pristupu</param>
        public PristupPanel(ZavedenePristupy pristupy, Pacient_Form form, bool strip)
        {
            InitializeComponent();

            this.pristupy = pristupy;
            this.form = form;
            this.strip = strip;

            //pridani komponent do containeru
            addComponents();

            //povoleni zapisu do textboxu a vyber z comboboxu
            nazev_ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            cislo_TextBox.ReadOnly = false;
            //umisteni_ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            hloubka_TextBox.ReadOnly = false;
            //datum_DateTimePicker.Enabled = true;
            specifikace_TextBox.ReadOnly = false;
            material_ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            //stav_TextBox.ReadOnly = false;

            //inicializace ciselniku nazvu, ummisteni, materialu
            initCiselniky();

            //init ciselniku umisteni dle vyberu nazvu pristupu
            nazev_ComboBox.SelectionChangeCommitted += new System.EventHandler(nazev_ComboBox_SelectionChangeCommitted);

            addComponentWrite();

            dnu_TextBox.Text = "1";

            //tlacitko pridani
            Button pridat = new Button();
            pridat.Text = "Přidat";
            pridat.AutoSize = true;
            pridat.BackColor = Color.LimeGreen;
            pridat.Click += new EventHandler(pridatClick);
            this.Controls.Add(pridat);
        }
        
        /// <summary>
        /// Konstruktor komponenty zobrazujici labely
        /// </summary>
        public PristupPanel()
        {
            InitializeComponent();

            //prvni radka
            Label cislo = new Label();
            cislo.Text = "Číslo";
            cislo.Width = cislo_TextBox.Width;
            this.Controls.Add(cislo);

            Label nazev = new Label();
            nazev.Text = "Název";
            nazev.Width = nazev_ComboBox.Width;
            this.Controls.Add(nazev);

            Label umisteni = new Label();
            umisteni.Text = "Umístění";
            umisteni.Width = umisteni_ComboBox.Width;
            this.Controls.Add(umisteni);

            Label hloubka = new Label();
            hloubka.Text = "Hloubka zavedení";
            hloubka.Width = hloubka_TextBox.Width + cm_Label.Width + 6;
            this.Controls.Add(hloubka);

            Label datum = new Label();
            datum.Text = "Datum zavedení";
            datum.Width = datum_DateTimePicker.Width;
            this.Controls.Add(datum);

            Label dnu = new Label();
            dnu.Text = "Dnů";
            dnu.Width = dnu_TextBox.Width;
            this.Controls.Add(dnu);

            //nova radka
            this.SetFlowBreak(dnu, true);

            //druha radka
            //odsazeni
            Label beginSpaceLabel = new Label();
            beginSpaceLabel.Width = cislo_TextBox.Width;
            this.Controls.Add(beginSpaceLabel);

            Label specifikace = new Label();
            specifikace.Text = "Specifikace";
            specifikace.Width = specifikace_TextBox.Width;
            specifikace.Height += 5;     //nebylo videt cele 'p'
            this.Controls.Add(specifikace);

            Label material = new Label();
            material.Text = "Materiál";
            material.Width = material_ComboBox.Width;
            this.Controls.Add(material);

            Label stav = new Label();
            stav.Text = "Stav";
            stav.Width = stav_TextBox.Width;
            this.Controls.Add(stav);
        }

#endregion
        
#region Inicializace komponent

        /// <summary>
        /// Metoda prida komponenty do kontaineru
        /// Nutne pro zobrazeni komponent
        /// </summary>
        private void addComponents()
        {
            //prvni radka
            this.Controls.Add(cislo_TextBox);
            this.Controls.Add(nazev_ComboBox);
            //this.Controls.Add(umisteni_TextBox);
            this.Controls.Add(umisteni_ComboBox);
            this.Controls.Add(hloubka_TextBox);
            this.Controls.Add(cm_Label);
            this.Controls.Add(datum_DateTimePicker);
            this.Controls.Add(dnu_TextBox);

            //nova radka
            this.SetFlowBreak(dnu_TextBox, true);

            //druha radka
            //odsazeni
            Label beginSpaceLabel = new Label();
            beginSpaceLabel.Width = cislo_TextBox.Width;
            this.Controls.Add(beginSpaceLabel);

            this.Controls.Add(specifikace_TextBox);
            this.Controls.Add(material_ComboBox);
            this.Controls.Add(stav_TextBox);
        }

        /// <summary>
        /// Metoda prida tlacitka
        /// </summary>
        private void addButtons()
        {
            this.Controls.Add(vymenit_Button);
            this.Controls.Add(aktualizuj_Button);
            this.Controls.Add(vymaz_Button);
        }

        /// <summary>
        /// Inicializuje komponenty
        /// </summary>
        private void initializeText()
        {
            cislo_TextBox.Text = pristup.cislo;
            nazev_ComboBox.Text = pristup.nazev;
            umisteni_ComboBox.Text = pristup.umisteni;
            hloubka_TextBox.Text = pristup.hloubkaZavedeni;
            datum_DateTimePicker.Text = pristup.datumZavedeni.ToString();
            dnu_TextBox.Text = pristup.getDnu();
            specifikace_TextBox.Text = pristup.specifikace;
            material_ComboBox.Text = pristup.kryciMaterial;
            stav_TextBox.Text = pristup.stavMistaVpichu;

            //test pozadavku na vymenu
            if (pristup.vymen == Pristup.VYMEN_TRUE)
            {
                this.BackColor = System.Drawing.Color.Red;
            }
            else
            {
                //radkove obarveni
                if (strip)
                {
                    this.BackColor = System.Drawing.SystemColors.Control;
                }
                else
                {
                    this.BackColor = System.Drawing.Color.WhiteSmoke;
                }
            }
        }

        /// <summary>
        /// Prida click event comboboxum a textboxum
        /// - zobrazi BigMessageBox s obsahem komponenty
        /// </summary>
        private void addComponentClick()
        {
            nazev_ComboBox.Click += new EventHandler(component_Click);
            umisteni_ComboBox.Click += new EventHandler(component_Click);
            material_ComboBox.Click += new EventHandler(component_Click);
            specifikace_TextBox.Click += new EventHandler(component_Click);
            stav_TextBox.Click += new EventHandler(textWrite_Click);
            stav_TextBox.TextChanged += new EventHandler(stav_TextBox_TextChanged);
            datum_DateTimePicker.ValueChanged += new EventHandler(datum_DateTimePicker_ValueChanged);
        }

        /// <summary>
        /// Moznosti psani textu
        /// </summary>
        private void addComponentWrite()
        {
            //povoleni zadani pouze cisel
            cislo_TextBox.KeyPress += new KeyPressEventHandler(keyPress);
            hloubka_TextBox.KeyPress += new KeyPressEventHandler(keyPress);

            umisteni_ComboBox.Click += new EventHandler(umisteni_Click);
            specifikace_TextBox.Click += new EventHandler(textWrite_Click);
            stav_TextBox.Click += new EventHandler(textWrite_Click);
        }

        /// <summary>
        /// Nacte ciselnik nazvu, umisteni, materialu
        /// </summary>
        private void initCiselniky()
        {
            //nacteni ciselniku pristupu a umisteni
            Databaze.State res = Program.databaze.getCiselnikPristupu();

            //inicializace seznamu
            umisteni = new List<string>[20];
            for (int k = 0; k < 20; k++)
            {
                umisteni[k] = new List<string>();
            }

            //pokud i == j, je to nazev pristupu, dalsi prvek je umisteni
            //pokud ne a je to nenulovy retezec, je to umisteni, dalsi prvek je umisteni
            //pokud je to prazdny retezec, dalsi prvek je nazev pristupu
            int i = 0;
            int j = 0;
            foreach (string nazev in res.res.ToArray())
            {
                //nazev pristupu - rozdeleni i a j
                if (i == j)
                {
                    nazev_ComboBox.Items.Add(nazev);
                    i++;
                    continue;
                }

                //pokud je > 0, je to umisteni
                if (nazev.Length > 0)
                {
                    umisteni[j].Add(nazev);
                }
                //kdyz ne, dalsi prvek je nazev pristupu - srovnani i a j
                else
                {
                    j++;
                }
            }

            //nacteni ciselniku materialu
            res = Program.databaze.getCiselnikMaterialu();
            material_ComboBox.Items.AddRange(res.res.ToArray());
        }

#endregion

#region Buttons

        /// <summary>
        /// Po stisku tlacitka se pristup vymaze
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delete_Button_Click(object sender, EventArgs e)
        {
            //potvrzovaci dialog smazani
            //DialogResult result = MessageBox.Show("Smazat invazivní přístup?", "Smazat", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            DialogResult result = BigMessageBox.Show("Smazat invazivní přístup?", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                //smaze pristup z daabaze
                pristupy.delPristup(pristup);

                //odstrani GUI prvek
                this.Dispose();
            }
        }

        /// <summary>
        /// Po stisku tlacitka se odesle pozadavek na vymenu
        /// Pristup se oznaci
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void vymenit_Button_Click(object sender, EventArgs e)
        {
            pristupy.vymenPristup(pristup);

            //test pozadavku na vymenu
            if (pristup.vymen == Pristup.VYMEN_TRUE)
            {
                this.BackColor = System.Drawing.Color.Red;
            }
            else
            {
                //radkove obarveni
                if (strip)
                {
                    this.BackColor = System.Drawing.SystemColors.Control;
                }
                else
                {
                    this.BackColor = System.Drawing.Color.WhiteSmoke;
                }
            }
        }

        /// <summary>
        /// Tlacitko potvrzujici vymenu pristupu
        /// Po stisku tlacitka se nastavi datum na aktualni a pocet dnu na 1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aktualizuj_Button_Click(object sender, EventArgs e)
        {
            pristupy.updatePristup2(pristup);
            //pristupy.updatePristup(pristup.poradi, pristup.datum, pristup.stavMistaVpichu, pristup.datumZavedeni);

            //test pozadavku na vymenu
            if (pristup.vymen == Pristup.VYMEN_FALSE)
            {
                //radkove obarveni
                if (strip)
                {
                    this.BackColor = System.Drawing.SystemColors.Control;
                }
                else
                {
                    this.BackColor = System.Drawing.Color.WhiteSmoke;
                }

                datum_DateTimePicker.Text = pristup.datumZavedeni.ToString();
                dnu_TextBox.Text = pristup.getDnu();
            }
            else
            {
                BigMessageBox.Show("Chyba aktualizace.");
            }
        }

        /// <summary>
        /// Prida inv pristup
        /// Deaktivuje zapis do komponent a aktuaizuje zobrazeni
        /// Odstrani button pridat
        /// Prida buttony k pristupu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pridatClick(object sender, EventArgs e)
        {
            //pokud byl vybrán prázdný řetězec, neprovede se nic a upozorní se uživatel
            if (nazev_ComboBox.Text.Equals(""))
            {
                BigMessageBox.Show("Nevybral jste typ invazivního přístupu!");
                return;
            }

            //pridani pristupu
            pristup = pristupy.addPristup(cislo_TextBox.Text, nazev_ComboBox.Text, umisteni_ComboBox.Text, hloubka_TextBox.Text, specifikace_TextBox.Text, material_ComboBox.Text, stav_TextBox.Text, datum_DateTimePicker.Value);
            //pristup = pristupy.zavedenePristupy.ElementAt(pristupy.zavedenePristupy.Count() - 1);

            //pokud se nepodari pridat pristup do dtb
            if (pristup == null)
            {
                return;
            }

            //aktualizace zobrazeni - deaktivace komponent
            nazev_ComboBox.DropDownStyle = ComboBoxStyle.DropDown;
            nazev_ComboBox.Items.Clear();
            cislo_TextBox.ReadOnly = true;
            umisteni_ComboBox.Items.Clear();
            hloubka_TextBox.ReadOnly = true;
            //datum_DateTimePicker.Enabled = false;
            specifikace_TextBox.ReadOnly = true;
            material_ComboBox.DropDownStyle = ComboBoxStyle.DropDown;
            material_ComboBox.Items.Clear();
            //stav_TextBox.ReadOnly = true;

            //odebere tlacitko pridat
            this.Controls.RemoveAt(this.Controls.Count - 1);

            addButtons();
            addComponentClick();

            //radkove obarveni
            if (strip)
            {
                this.BackColor = System.Drawing.SystemColors.Control;
            }
            else
            {
                this.BackColor = System.Drawing.Color.WhiteSmoke;
            }

            //radek pro pridani dalsiho pristupu
            form.AktualizujPristupy();
        }

#endregion

#region Ovladani komponent

        /// <summary>
        /// Inicializuje ciselnik umisteni dle vyberu nazvu pristupu
        /// Pri zmene smaze predchozi hodnoty
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nazev_ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //ziskani indexu zvoleneho nazvu
            string nazev = nazev_ComboBox.Text;
            int index = nazev_ComboBox.Items.IndexOf(nazev);
            
            //smazani predchozich hodnot
            umisteni_ComboBox.Items.Clear();
            umisteni_ComboBox.Text = "";

            //inicializace ciselniku
            umisteni_ComboBox.Items.AddRange(umisteni[index].ToArray());
        }

        /// <summary>
        /// Pokud combobox umisteni obsahuje polozky, rozbali se
        /// Pokud neobsahuje nebo je rozbalen, zobrazi se klavesnice
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void umisteni_Click(object sender, EventArgs e)
        {
            ComboBox tb = (ComboBox)sender;

            if (tb.Items.Count == 0)
            {
                form.Keyboard.showKeyboard(tb.Text);

                if (form.Keyboard.DialogResult == DialogResult.OK)
                {
                    tb.Text = form.Keyboard.text_TextBox.Text;
                }
            }
            else
            {
                tb.DroppedDown = true;
            }
        }

        /// <summary>
        /// Zobrazi numerickou klavesnici pro zapis
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numericTextBox_Click(object sender, EventArgs e)
        {
            //oznaceni textu v textboxu
            TextBox tb = (TextBox)sender;

            //test, jestli se muze hodnota menit (pro pridani noveho pristupu)
            if (tb.ReadOnly == true)
            {
                return;
            }

            form.NumKeyboard.showKeyboard(false);

            //pri potvrzeni se prekopiruje text
            if (form.NumKeyboard.DialogResult == DialogResult.OK)
            {
                tb.Text = form.NumKeyboard.value_TextBox.Text;
            }
        }

        /// <summary>
        /// Zobrazeni klavesnice
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textWrite_Click(object sender, EventArgs e)
        {
            Control tb = (Control)sender;

            form.Keyboard.showKeyboard(tb.Text);

            if (form.Keyboard.DialogResult == DialogResult.OK)
            {
                tb.Text = form.Keyboard.text_TextBox.Text;
            }
        }

        private void stav_TextBox_TextChanged(object sender, EventArgs e)
        {
            pristupy.updatePristupStavDate(pristup, stav_TextBox.Text, datum_DateTimePicker.Value);
            
            stav_TextBox.Text = pristup.stavMistaVpichu;
        }

        private void datum_DateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            pristupy.updatePristupStavDate(pristup, stav_TextBox.Text, datum_DateTimePicker.Value);

            datum_DateTimePicker.Value = pristup.datumZavedeni;
            dnu_TextBox.Text = pristup.getDnu();
        }

        /// <summary>
        /// Zobrazi BigMessageBox s obsahem komponenty
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void component_Click(object sender, EventArgs e)
        {
            Control tb = (Control)sender;
            BigMessageBox.Show(tb.Text);
        }

        private void nazev_ComboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        /// <summary>
        /// Test zadavani cisel
        /// Lza zadat jen cislo
        /// </summary>
        private void keyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

#endregion

#region mousefilter

        private bool _doTouchScroll;
        private Point _mouseStartPoint = Point.Empty;
        private Point _PanelStartPoint = Point.Empty;

        private void mouseFilterAdd()
        {
            Program.mouseFilter.MouseFilterDown += mouseFilter_MouseFilterDown;
            Program.mouseFilter.MouseFilterMove += mouseFilter_MouseFilterMove;
            Program.mouseFilter.MouseFilterUp += mouseFilter_MouseFilterUp;
        }

        /// <summary>
        ///     Handles the MouseFilterDown event of the mudmFilter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">
        ///     The <see cref="MouseFilterEventArgs" /> instance containing the event data.
        /// </param>
        private void mouseFilter_MouseFilterDown(object sender, MouseFilterEventArgs e)
        {
            if (!_doTouchScroll && e.Button == MouseButtons.Left)
            {
                _mouseStartPoint = new Point(e.X, e.Y);
                _PanelStartPoint = new Point(this.Parent.AutoScrollOffset.X,
                                                     this.Parent.AutoScrollOffset.Y);
            }
        }

        /// <summary>
        ///     Handles the MouseFilterMove event of the mudmFilter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">
        ///     The <see cref="MouseFilterEventArgs" /> instance containing the event data.
        /// </param>
        private void mouseFilter_MouseFilterMove(object sender, MouseFilterEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!_mouseStartPoint.Equals(Point.Empty))
                {
                    int dx = (e.X - _mouseStartPoint.X);
                    int dy = (e.Y - _mouseStartPoint.Y);

                    if (_doTouchScroll)
                    {
                        this.Parent.AutoScrollOffset = new Point(_PanelStartPoint.X - dx,
                                                                     _PanelStartPoint.Y - dy);
                    }
                    else if (Math.Abs(dx) > 10 || Math.Abs(dy) > 10)
                    {
                        _doTouchScroll = true;
                    }
                }
            }
        }

        /// <summary>
        ///     Handles the MouseFilterUp event of the mudmFilter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">
        ///     The <see cref="MouseFilterEventArgs" /> instance containing the event data.
        /// </param>
        private void mouseFilter_MouseFilterUp(object sender, MouseFilterEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (_doTouchScroll && !this.Parent.AutoScrollOffset.Equals(_PanelStartPoint) &&
                    !_PanelStartPoint.Equals(Point.Empty))
                {
                    // dont fire Click-Event
                    e.Handled = true;
                }
                _doTouchScroll = false;
                _mouseStartPoint = Point.Empty;
                _PanelStartPoint = Point.Empty;
            }
        }

#endregion

        
    }
}
