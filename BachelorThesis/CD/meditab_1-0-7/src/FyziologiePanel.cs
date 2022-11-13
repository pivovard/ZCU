using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MediTab.TelesneFunkce;
using System.Drawing;

namespace MediTab
{
    public partial class FyziologiePanel : FlowLayoutPanel
    {

#region Properties

        /// <summary>
        /// Zaznam fyziologie zobrazovane ve FyziologiePanel
        /// </summary>
        private ZaznamTelFunkci fyz { get; set; }

        /// <summary>
        /// Instance tridy pracujici se zaznamy fyziologie
        /// Ovladani zaznamu
        /// </summary>
        private Fyziologie fyziologie { get; set; }

        /// <summary>
        /// Instance karty pacienta
        /// Pro volani aktualizace - pridani nove instance FyziologiePanel pridani noveho panelu
        /// </summary>
        private Pacient_Form form { get; set; }

        /// <summary>
        /// Konstanta pro radkove obarveni
        /// </summary>
        private readonly bool strip;

#endregion

#region Konstruktor

        /// <summary>
        /// Konstruktor vytvori panel jednoho existujiciho zaznamu fyziologie
        /// </summary>
        /// <param name="fyz"></param>
        /// <param name="fyziologie"></param>
        /// <param name="form"></param>
        /// <param name="strip"></param>
        public FyziologiePanel(ZaznamTelFunkci fyz, Fyziologie fyziologie, Pacient_Form form, bool strip)
        {
            InitializeComponent();

            this.fyz = fyz;
            this.fyziologie = fyziologie;
            this.form = form;
            this.strip = strip;

            AddComponent();
            AddComponentText();
            AddButtons(false);

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

        /// <summary>
        /// Konstruktor pro vytvoreni panelu fyziologie pro pridani noveho zaznamu
        /// </summary>
        /// <param name="fyziologie"></param>
        /// <param name="form"></param>
        /// <param name="strip"></param>
        public FyziologiePanel(Fyziologie fyziologie, Pacient_Form form, bool strip)
        {
            InitializeComponent();

            this.fyziologie = fyziologie;
            this.form = form;
            this.strip = strip;

            AddComponent();
            AddButtons(true);
        }

        /// <summary>
        /// Konstruktor prvniho panelu fyziologie
        /// Inicializace labelu 
        /// </summary>
        public FyziologiePanel()
        {
            InitializeComponent();

            Label datum = new Label();
            datum.Text = "Datum";
            datum.Width = datum_DateTimePicker.Width;
            this.Controls.Add(datum);

            Label vyska = new Label();
            vyska.Text = "Výška";
            vyska.Width = vyska_TextBox.Width;
            vyska.Height += 5;
            this.Controls.Add(vyska);

            Label hmotnost = new Label();
            hmotnost.Text = "Hmot.";
            hmotnost.Width = hmotnost_TextBox.Width;
            this.Controls.Add(hmotnost);

            Label bmi = new Label();
            bmi.Text = "BMI";
            bmi.Width = bmi_TextBox.Width;
            this.Controls.Add(bmi);

            Label teplota = new Label();
            teplota.Text = "Temp.";
            teplota.Width = teplota_TextBox.Width;
            teplota.Height += 5;     //nebylo videt cele 'p'
            this.Controls.Add(teplota);

            Label stk = new Label();
            stk.Text = "STK";
            stk.Width = stk_TextBox.Width;
            this.Controls.Add(stk);

            Label dtk = new Label();
            dtk.Text = "DTK";
            dtk.Width = dtk_TextBox.Width;
            this.Controls.Add(dtk);

            Label tep = new Label();
            tep.Text = "Tep";
            tep.Width = tep_TextBox.Width;
            tep.Height += 5;
            this.Controls.Add(tep);

            Label dech = new Label();
            dech.Text = "Dech";
            dech.Width = dech_TextBox.Width;
            this.Controls.Add(dech);
        }

#endregion

#region Init komponent

        /// <summary>
        /// Pridani textovych komponent do panelu
        /// </summary>
        private void AddComponent()
        {
            this.Controls.Add(datum_DateTimePicker);
            this.Controls.Add(vyska_TextBox);
            this.Controls.Add(hmotnost_TextBox);
            this.Controls.Add(bmi_TextBox);
            this.Controls.Add(teplota_TextBox);
            this.Controls.Add(stk_TextBox);
            this.Controls.Add(dtk_TextBox);
            this.Controls.Add(tep_TextBox);
            this.Controls.Add(dech_TextBox);
        }

        /// <summary>
        /// Init textu komponent
        /// </summary>
        private void AddComponentText()
        {
            datum_DateTimePicker.Value = fyz.datumZaznamu;
            vyska_TextBox.Text = fyz.vyska;
            hmotnost_TextBox.Text = fyz.hmotnost;
            bmi_TextBox.Text = fyz.BMI;
            teplota_TextBox.Text = fyz.teplota;
            stk_TextBox.Text = fyz.stk;
            dtk_TextBox.Text = fyz.dtk;
            tep_TextBox.Text = fyz.tep;
            dech_TextBox.Text = fyz.dech;
        }

        /// <summary>
        /// Pridani tlacitek do panelu
        /// typ panelu: novy - prida tlacitko pridej; !novy - prida tlacitka upravit a smazat
        /// </summary>
        /// <param name="novy">typ panelu</param>
        private void AddButtons(bool novy)
        {
            //typ panelu: novy - prida tlacitko pridej; !novy - prida tlacitka upravit a smazat
            if (novy)
            {
                this.Controls.Add(pridat_Button);
            }
            else
            {
                this.Controls.Add(upravit_Button);
                this.Controls.Add(smazat_Button);
            }
        }

#endregion

#region Ovladani komponent

        /// <summary>
        /// Odesle do dtb nove hodnty k zaznamu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void upravit_Button_Click(object sender, EventArgs e)
        {
            fyziologie.upravFyzZaznam(fyz, datum_DateTimePicker.Value, teplota_TextBox.Text, stk_TextBox.Text, dtk_TextBox.Text, tep_TextBox.Text, dech_TextBox.Text, vyska_TextBox.Text, hmotnost_TextBox.Text);
            bmi_TextBox.Text = fyz.BMI;
        }

        /// <summary>
        /// Smaze zaznam fyziologie
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void smazat_Button_Click(object sender, EventArgs e)
        {
            //dialog potvrzeni smazani zaznamu
            DialogResult result = BigMessageBox.Show("Smazat záznam?", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                fyziologie.smazFyzZaznam(this.fyz);
                this.Dispose();
            }
        }

        /// <summary>
        /// Prida novy zaznam fyziologie
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pridat_Button_Click(object sender, EventArgs e)
        {
            //Metoda pridani vraci ZaznamTelFunkci
            ZaznamTelFunkci res = fyziologie.pridejFyzZaznam(datum_DateTimePicker.Value, teplota_TextBox.Text, stk_TextBox.Text, dtk_TextBox.Text, tep_TextBox.Text, dech_TextBox.Text, vyska_TextBox.Text, hmotnost_TextBox.Text);

            //test jestli byl zaznam pridan - notnull
            if (res != null)
            {
                this.fyz = res;

                bmi_TextBox.Text = fyz.BMI;

                //odebere tlacitko pridat
                this.Controls.RemoveAt(this.Controls.Count - 1);

                //prida tlacitka upravit a smazat
                AddButtons(false);

                //aktualizace okna - pridani noveho panelu pro pridani zaznamu
                form.AktualizujFyziologie();
            }
        }

        /// <summary>
        /// Zobrazeni numericke klavesnice po kliknut na textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_Click(object sender, EventArgs e)
        {
            //volajici komponenta
            TextBox tb = (TextBox)sender;

            //u teploty lze zadavat desetinna cisla
            if(tb.Equals(teplota_TextBox))
            {
                //klavesnice s desetinnou teckou
                form.NumKeyboard.showKeyboard(tb.Text, true);
            }
            else
            {
                //klavesnice bez desetinne tecky
                form.NumKeyboard.showKeyboard(tb.Text, false);
            }
            

            //pri potvrzeni se prekopiruje text
            if (form.NumKeyboard.DialogResult == DialogResult.OK)
            {
                tb.Text = form.NumKeyboard.value_TextBox.Text;
            }
        }

        /// <summary>
        /// Osetreni vstupu z klavesnice
        /// Lze zadavat pouze cisla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar))
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
