using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlotTest;

namespace Test
{
    class Program
    {
        static void Vypocet(Dictionary<int, int> cenik, int delka, out int sirka, out int pocet, out float mezera)
        {
            int aktualniCena;
            int aktualniPocet;
            float aktualniMezera;
            int minCena = 0;
            pocet = 0;
            mezera = 0;
            sirka = 0;

            foreach (KeyValuePair<int, int> prkno in cenik)
            {
                aktualniPocet = delka / (prkno.Key +1);
                aktualniCena = prkno.Value * aktualniPocet;
                aktualniMezera = (delka - aktualniPocet * (prkno.Key+1)) / aktualniPocet;
                if (aktualniCena < minCena)
                {
                    minCena = aktualniCena;
                    sirka = prkno.Key;
                    mezera = aktualniMezera;
                    pocet = aktualniPocet;
                }
            }
        }


        static void Main(string[] args)
        {
            Plot plot = new Plot();
            plot.Vypocet(Vypocet, true);
            Console.ReadLine();
        }
    }
}
