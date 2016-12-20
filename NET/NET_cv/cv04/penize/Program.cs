using System;
using System.Collections.Generic;

using PenizeLib;

namespace PenizeTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Bar bar = new Bar();

            bar.Vypocet(VhodnaBankovka, VycetDrobnych, SumaDrobnych, false);

            Console.ReadLine();
        }

        #region 1. cast (1 bod)
        /// <summary>
        /// Funkce najde nejmensi bankovku, kterou lze zaplatit danou castku.
        /// </summary>
        /// <param name="castka">Castka k zaplaceni</param>
        /// <param name="bankovky">Seznam existujicich bankovek</param>
        /// <returns>Nejmensi vhodna bankovka</returns>
        /// <exception cref="ApplicationException">Vyjimka v pripade, ze je castka vetsi, nez nejvetsi bankovka</exception>
        static int VhodnaBankovka(int castka, List<int> bankovky)
        {
            // TODO: dopsat funkci tak, aby ze seznamu bankovek vratila nejmensi moznou bankovku, kterou lze zaplatit castka 
            foreach(var bank in bankovky)
            {
                if (bank >= castka) return bank;
            }

            //return 100;
            throw new ApplicationException("Tak velkou bankovku nemame");
        }

        /// <summary>
        /// Vrati slovnik ve tvaru [hodnota bankovky, pocet], kde bude ulozen nejmensi pocet bankovek, potrebnych pro 
        /// vyplaceni castky. 
        /// </summary>
        /// <param name="castka">Castka, ktera se ma vyplatit</param>
        /// <param name="bankovky">Seznam existujiich bankovek</param>
        /// <returns>Vycet bankovek</returns>
        static SortedDictionary<int, int> VycetDrobnych(int castka, List<int> bankovky)
        {
            // TODO: dopsat funkci, ktera ulozi do slovniku dvojice [hodnota bankovky, pocet] tak, aby na vzplaceni castky byl potreba minimalni pocet bankovek
            SortedDictionary<int, int> vycet = new SortedDictionary<int, int>();

            for (int i = bankovky.Count - 1; i > -1; i--)
            {
                int key = bankovky[i];
                if (key <= castka)
                {
                    if (vycet.ContainsKey(key))
                    {
                        vycet[key] += 1;
                    }
                    else
                    {
                        vycet[key] = 1;
                    }

                    castka -= key;
                    i++;
                }
            }

            //vycet[100] = 1;
            return vycet;
        }
        #endregion

        #region 2. cast (1 bod)
        /// <summary>
        /// Secte pocet jednotlivych hodnot bankovek z jednotlivych vyctu.
        /// </summary>
        /// <param name="vycty">Seznamy vracenych bankovek viz <see cref="VycetDrobnych(int, List{int})"/></param>
        /// <returns>Soucty jednotlivych typu bankovek</returns>
        public static SortedDictionary<int, int> SumaDrobnych(List<SortedDictionary<int, int>> vycty)
        {
            // TODO: dopsat funkci tak, aby secetla pocet bankovek jednotlivych hodnot ze vsech vyctu
            SortedDictionary<int, int> soucet = new SortedDictionary<int, int>();

            foreach(var vycet in vycty)
            {
                foreach(var key in vycet.Keys)
                {
                    if (soucet.ContainsKey(key))
                    {
                        soucet[key] += vycet[key];
                    }
                    else
                    {
                        soucet.Add(key, vycet[key]);
                    }
                }                
            }
            
            return soucet;
        }
        #endregion
    }
}
