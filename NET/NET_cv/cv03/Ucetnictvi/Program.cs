using System;
using System.Collections.Generic;
using System.Linq;

namespace Ucetnictvi
{
    class Program
    {
        static void Inflace(PolozkaFaktury p)
        {
            p.CenaMJ *= 1.1;
        }

        static void Main(string[] args)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("CS-cz");

            List<Faktura> faktury = new List<Faktura>()
            {
                new Faktura
                {
                    Polozky = new List<PolozkaFaktury>()
                    {
                        new PolozkaFaktury() { Polozka = "Rohlik", Pocet = 5, Jednotky = Jednotky.Ks, CenaMJ = 1.50},
                        new PolozkaFaktury() { Polozka = "Mleko", Pocet = 2, Jednotky = Jednotky.l, CenaMJ = 18 },
                        new PolozkaFaktury() { Polozka = "Maslo", Pocet = 1, Jednotky = Jednotky.Ks, CenaMJ = 35}
                    }
                },

                new Faktura
                {
                    Polozky = new List<PolozkaFaktury>()
                    {
                        new PolozkaFaktury() { Polozka = "Rohlik", Pocet = 5, Jednotky = Jednotky.Ks, CenaMJ = 2},
                        new PolozkaFaktury() { Polozka = "Mleko", Pocet = 2, Jednotky = Jednotky.l, CenaMJ = 26 },
                        new PolozkaFaktury() { Polozka = "Maslo", Pocet = 1, Jednotky = Jednotky.Ks, CenaMJ = 41}
                    }
                }
            };

            foreach (var faktura in faktury)
            {
                Console.WriteLine(faktura.CelkovaCena);
            }

            Console.WriteLine();
            Console.WriteLine(faktury.Maximalni().CelkovaCena);
            Console.WriteLine(faktury.Max(f=>f.CelkovaCena));
        }
    }

    public static class Extenze
    {
        //public static Faktura Maximalni(this List<Faktura> faktury)
        public static Faktura Maximalni(this IEnumerable<Faktura> faktury)
        {
            Faktura nejdrazsi = faktury.First();

            foreach(var faktura in faktury)
            {
                if(nejdrazsi.CelkovaCena < faktura.CelkovaCena)
                {
                    nejdrazsi = faktura;
                }
            }

            return nejdrazsi;
        }
    }
}
