using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strategie
{
    /// <summary>
    /// Mapa jednotek
    /// </summary>
    public class Mapa
    {
        /// <summary>
        /// Rozmisteni jednotek v mape
        /// </summary>
        public Dictionary<Point, List<Jednotka>> Jednotky { get; set; }

        public Mapa()
        {
            Jednotky = new Dictionary<Point, List<Jednotka>>();
        }

        /// <summary>
        /// Vysazeni jednotky do oblasti
        /// </summary>
        /// <param name="sirka">Sirka</param>
        /// <param name="delka">Delka</param>
        /// <param name="jednotka">Jednotka</param>
        public void VysaditJednotku(int sirka, int delka, Jednotka jednotka)
        {
            Point oblast = new Point() { X = sirka, Y = delka };
            if (!Jednotky.ContainsKey(oblast))
            {
                Jednotky.Add(oblast, new List<Jednotka>());
            }
            Jednotky[oblast].Add(jednotka);
        }

        /// <summary>
        /// Vycisteni oblasti - vymazani celeho seznamu
        /// </summary>
        /// <param name="sirka">Sirka</param>
        /// <param name="delka">Delka</param>
        public void VycistitOblast(int sirka, int delka)
        {
            Point oblast = new Point() { X = sirka, Y = delka };

            if (Jednotky.ContainsKey(oblast))
            {
                Jednotky.Remove(oblast);
            }
        }

        public void ZkopirovatOblast(int odkudSirka, int odkudDelka, int kamSirka, int kamDelka)
        {
            Point odkud = new Point() { X = odkudSirka, Y = odkudDelka };
            Point kam = new Point() { X = kamSirka, Y = kamDelka };

            if (Jednotky.ContainsKey(odkud))
            {
                if (!Jednotky.ContainsKey(kam))
                {
                    Jednotky.Add(kam, new List<Jednotka>());
                }

                foreach (var jednotka in Jednotky[odkud])
                {
                    Jednotky[kam].Add(new Jednotka() { Nazev = jednotka.Nazev, Zdravi = jednotka.Zdravi });
                }

                //Jednotky[kam] = Jednotky[odkud];
            }
            else
            {
                if (!Jednotky.ContainsKey(kam))
                {
                    Jednotky.Add(kam, new List<Jednotka>());
                }
                //Jednotky[kam] = new List<Jednotka>();
            }
        }

        /// <summary>
        /// Vrati seznam jednotek v oblasti
        /// </summary>
        /// <param name="sirka">Sirka</param>
        /// <param name="delka">Delka</param>
        /// <returns>Seznam jednotek</returns>
        public List<Jednotka> JednotkyVOblasti(int sirka, int delka)
        {
            Point oblast = new Point() { X = sirka, Y = delka };

            if (Jednotky.ContainsKey(oblast))
            {
                return Jednotky[oblast];
            }
            else
            {
                return new List<Jednotka>();
            }
        }
    }
}
