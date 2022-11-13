using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MediTab.Opravy;

namespace MediTab
{
    public partial class Opravy_Form : Form
    {

        /// <summary>
        /// Instance tridy provadejici opravy
        /// </summary>
        private NavratAkci navratAkci;

        /// <summary>
        /// Promena pro obarveni radek
        /// </summary>
        private bool strip = true;

        /// <summary>
        /// True byla-li provedena oprava.
        /// </summary>
        public bool oprava = false;

        public Opravy_Form(String pacientID, int index)
        {
            //init tridy vracejici akce
            navratAkci = new NavratAkci(pacientID, index);

            InitializeComponent();

            //init scrollovatelneho panelu
            this.panel = new TouchableFlowLayoutPanel();
            panel.Dock = DockStyle.Fill;
            panel.AutoScroll = true;
            this.Controls.Add(panel);
            panel.BringToFront();

            //pridani oprav
            foreach (Akce akce in navratAkci.seznamAkci)
            {
                addAkci(akce);

                strip = !strip;
            }
        }

        /// <summary>
        /// Prida panel s akci, kterou lze vratit
        /// </summary>
        /// <param name="akce"></param>
        private void addAkci(Akce akce)
        {
            //Panel s komponentami
            FlowLayoutPanel panelAkce = new FlowLayoutPanel();
            panelAkce.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            panelAkce.Padding = new Padding(10);
            panelAkce.AutoSize = true;

            //obarveni radku
            if (strip)
            {
                panelAkce.BackColor = Color.WhiteSmoke;
            }

            //Label s ID akce
            Label id = new Label();
            id.Text = akce.id.ToString();
            id.Width = 50;
            id.Margin = new Padding(0, 10, 0, 0);
            panelAkce.Controls.Add(id);

            //Label s casem provedeni 
            Label cas = new Label();
            //cas: DD.MM.YYYY "  " HH:MM
            cas.Text = akce.cas.ToShortDateString() + "  " + akce.cas.ToShortTimeString();
            cas.Width = 200;
            cas.Margin = new Padding(0, 10, 0, 0);
            panelAkce.Controls.Add(cas);

            //Label s popisem akce
            Label info = new Label();
            info.Text = akce.ToString();
            info.Width = 500;
            info.Height = info.Height + 5;
            info.Margin = new Padding(0, 10, 0, 0);
            panelAkce.Controls.Add(info);

            //Button pro zruseni akce
            Button del = new Button();
            del.Text = "X";
            del.Width = 50;
            del.AutoSize = true;
            del.ForeColor = Color.DarkRed;
            del.Click += new EventHandler(del_Button_Click);

            //TabIndexu priradine id
            //-> diky tomu budeme mit id kdispozici v ClickEventu a budeme moct zavolat metodu opravakci()
            del.TabIndex = akce.id;
            panelAkce.Controls.Add(del);

            //pridani akce do okna
            this.panel.Controls.Add(panelAkce);
            panelAkce.Dock = DockStyle.Top;

        }

        /// <summary>
        /// Provede vraceni opravy
        /// Odstrani panel s akci z dialogu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void del_Button_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;

            //navrat akce - id akce v TabIndexu
            //navratAkci.opravAkci(bt.TabIndex);
            navratAkci.seznamAkci[bt.TabIndex].vratAkci();

            //smazani panelu z okna
            bt.Parent.Dispose();

            //nastaveni provedene opravy
            oprava = true;
        }

        /// <summary>
        /// Zavre okno oprav
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void zavrit_Button_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
