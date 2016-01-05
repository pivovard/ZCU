using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Krizovatka
{
    public partial class Form1 : Form
    {

        #region Variables

        /// <summary>
        /// Aktualni stav
        /// </summary>
        private int stav = 1;

        /// <summary>
        /// Pozadavek na vjezd do pivovaru
        /// </summary>
        private int pivovar = 0; 

        /// <summary>
        /// Indikace zavreni okna aplikace
        /// </summary>
        private bool isClosed = false;

        #endregion

        /// <summary>
        /// Inicializace komponent okna pri spusteni
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }

        #region Rizeni krizovatky

        /// <summary>
        /// Spusti rizeni krizovatky
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            run();
        }

        /// <summary>
        /// Prechod mezi jednotilivymi stavy
        /// </summary>
        private void run()
        {
            //pocatecni stav S1'
            toS1();

            //cyklus prechodu mezi stavy do zavreni okna
            while (!isClosed)
            {
                //stisknuti tlacitka 4 -> prechod do S4
                if (pivovar == 4)
                {
                    toS4();
                    continue;
                }

                //stisknuti tlacitka 8 -> prechod do S5
                if (pivovar == 8)
                {
                    toS5();
                    continue;
                }

                //prechody mezi stavy dle predchoziho stavu
                if (stav == 1)
                {
                    toS2();
                    continue;
                }

                if (stav == 2)
                {
                    toS3();
                    continue;
                }

                if (stav == 3)
                {
                    toS1();
                    continue;
                }
            }
        }

        /// <summary>
        /// Cekani ve stavu
        /// </summary>
        /// <param name="sec">Pocet sekund</param>
        private void wait(int sec)
        {
            //nasobek defaultniho casu volbou numericUpDown
            sec *= (int)numericUpDown1.Value;

            //cyklus cekani
            DateTime Tthen = DateTime.Now;
            do
            {
                Application.DoEvents();
            } while (Tthen.AddSeconds(sec) > DateTime.Now);
        }

        #endregion

        #region Ovladani semaforu ve stavech

        /// <summary>
        /// Stav pro jizdu v primem smeru
        /// S1' S1 S1'
        /// </summary>
        private void toS1(){
            stav = 1;

            //oranzova
            label1.BackColor = Color.Orange;
            label3.BackColor = Color.Orange;

            wait(1);

            //zelena
            label1.BackColor = Color.Green;
            label3.BackColor = Color.Green;

            wait(8);

            //oranzova
            label1.BackColor = Color.Orange;
            label3.BackColor = Color.Orange;

            wait(1);

            //cervena
            label1.BackColor = Color.Red;
            label3.BackColor = Color.Red;
        }

        /// <summary>
        /// Stav pro odboceni k hornbachu z primeho smeru
        /// S2' S2 S2'
        /// </summary>
        private void toS2()
        {
            stav = 2;

            //oranzova
            label1.BackColor = Color.Orange;
            label2.BackColor = Color.Orange;

            wait(1);

            //zelena
            label1.BackColor = Color.Green;
            label2.BackColor = Color.Green;
            label7.BackColor = Color.Green;

            wait(6);

            //oranzova
            label1.BackColor = Color.Orange;
            label2.BackColor = Color.Orange;
            label7.BackColor = Color.Black;

            wait(1);

            //cervena
            label1.BackColor = Color.Red;
            label2.BackColor = Color.Red;
        }

        /// <summary>
        /// Stav vyjezdu od Hornbachu
        /// S3' S3 S3'
        /// </summary>
        private void toS3()
        {
            stav = 3;

            //oranzova
            label6.BackColor = Color.Orange;

            wait(1);

            //zelena
            label6.BackColor = Color.Green;
            label5.BackColor = Color.Green;

            wait(6);

            //oranzova
            label6.BackColor = Color.Orange;
            label5.BackColor = Color.Black;

            wait(1);

            //cervena
            label6.BackColor = Color.Red;
        }

        /// <summary>
        /// Stav vjezdu do pivovaru
        /// S4' S4 S4'
        /// </summary>
        private void toS4()
        {
            pivovar = 0;

            //oranzova
            label4.BackColor = Color.Orange;
            label2.BackColor = Color.Orange;

            wait(1);

            //zelena
            label4.BackColor = Color.Green;
            label2.BackColor = Color.Green;
            label7.BackColor = Color.Green;

            wait(4);

            //oranzova
            label4.BackColor = Color.Orange;
            label2.BackColor = Color.Orange;
            label7.BackColor = Color.Black;

            wait(1);

            //cervena
            label4.BackColor = Color.Red;
            label2.BackColor = Color.Red;

            //stav, ve kterem se bude pokracovat dle stavu, ze ktereho se preslo do S4
            if (stav == 3)
            {
                toS1();
            }
            else
            {
                toS3();
            }
        }

        /// <summary>
        /// Stav vyjezdu z pivovaru
        /// S5' S5 S5'
        /// </summary>
        private void toS5()
        {
            pivovar = 0;

            //oranzova
            label6.BackColor = Color.Orange;
            label8.BackColor = Color.Orange;

            wait(1);

            //zelena
            label6.BackColor = Color.Green;
            label8.BackColor = Color.Green;

            wait(4);

            //oranzova
            label6.BackColor = Color.Orange;
            label8.BackColor = Color.Orange;

            wait(1);

            //cervena
            label6.BackColor = Color.Red;
            label8.BackColor = Color.Red;

            //stav, ve kterem se bude pokracovat dle stavu, ze kterehos e preslo do S4
            if (stav == 1)
            {
                toS2();
            }
            else
            {
                toS1();
            }
        }

        #endregion

        #region Ovladaci tlacitka semaforu

        /// <summary>
        /// Tlacitko pozadavku vjezdu do pivovaru
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            pivovar = 4;
        }

        /// <summary>
        /// Tlacitko pozadavku vyjezdu z pivovaru
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button8_Click(object sender, EventArgs e)
        {
            pivovar = 8;
        }

        #endregion

        #region Zavreni

        /// <summary>
        /// Zavre okno
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Pri zavreni okna nastavi isClosed hodnotu true -> ukonceni beziciho cyklu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            isClosed = true;
        }
        #endregion
    }
}
