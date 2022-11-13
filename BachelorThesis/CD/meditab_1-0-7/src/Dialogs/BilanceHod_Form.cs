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

namespace MediTab
{
    public partial class BilanceHod_Form : Form
    {

#region Variables

        /// <summary>
        /// Instnace tridy pracujici s hodinovou bilanci
        /// </summary>
        private BilanceHod bilanceHod;

        /// <summary>
        /// ID tekutiny
        /// </summary>
        private Tekutiny tekutina;

        /// <summary>
        /// Pocet hodin pro zpetne zadani
        /// </summary>
        private int zpetnePodani;

        /// <summary>
        /// Hodina nacteni
        /// </summary>
        private int loadHour;

        NumKeyboard_Form numKeyboard;

#endregion

        public BilanceHod_Form(BilanceHod bilanceHod, int zpetnePodani, NumKeyboard_Form num)
        {
            InitializeComponent();

            this.numKeyboard = num;

            this.bilanceHod = bilanceHod;
            this.zpetnePodani = zpetnePodani;
        }

        public DialogResult showBilance(Tekutiny tekutina)
        {
            this.tekutina = tekutina;
            load();

            return this.ShowDialog();
        }

        /// <summary>
        /// Inicializuje hodnoty
        /// </summary>
        private void load()
        {
            loadHour = DateTime.Now.Hour;

            //vymaze DataGridview, pokud tam neco je
            hodnoty_DataGridView.Rows.Clear();

            //radka DataGridView
            string[] row = new string[2];

            //pro hodinu 0 - 23
            for (int i = 0; i < 24; i++)
            {
                row[0] = i.ToString();
                row[1] = bilanceHod.getHodnota(tekutina, i);

                hodnoty_DataGridView.Rows.Add(row);
            }

            //zvyrazni radku s aktualni hodinou
            hodnoty_DataGridView.Rows[loadHour].DefaultCellStyle.BackColor = Color.Khaki;

            //odscrollovani na radku s aktualni hodinou
            hodnoty_DataGridView.FirstDisplayedScrollingRowIndex = loadHour;

            //init celkoveho souctu tekutiny
            celkem_TextBox.Text = bilanceHod.getSoucty()[(int)tekutina];
        }

        /// <summary>
        /// Zavre okno
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void zavrit_Button_Click(object sender, EventArgs e)
        {
            bilanceHod.pushToDtb();
        }

        private void hodnoty_DataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //kontrola zapisu v aktualni hodinu (- zpetne zadani)
            if (e.RowIndex > DateTime.Now.Hour || e.RowIndex < DateTime.Now.Hour - zpetnePodani)
            {
                BigMessageBox.Show("Do této hodiny nelze zapisovat.");
                return;
            }

            DataGridViewCell cell = hodnoty_DataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];
            
            //kontrola vicenasobneho zadani
            if (!cell.Value.ToString().Equals("0"))
            {
                BigMessageBox.Show("Tato hodnota již byla vyplněna.\nNelze zadat dvě hodnoty v jednu hodin.");
                return;
            }

            numKeyboard.showKeyboard(false);

            //pri potvrzeni se prekopiruje text
            if (numKeyboard.DialogResult == DialogResult.OK)
            {
                cell.Value = numKeyboard.value_TextBox.Text;

                //pridani nove hodnoty
                bilanceHod.zadejHodnotu((int)tekutina, numKeyboard.value_TextBox.Text, e.RowIndex);

                //init celkoveho souctu tekutiny
                celkem_TextBox.Text = bilanceHod.getSoucty()[(int)tekutina];
            }
        }
    }
}
