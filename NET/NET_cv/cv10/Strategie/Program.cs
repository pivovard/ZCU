using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strategie
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Probiha vysadek...");
            Mapa mapa = new Mapa();
            mapa.VysaditJednotku(1, 1, new Jednotka() { Nazev = "Sectoid", Zdravi = 30 });
            mapa.VysaditJednotku(1, 5, new Jednotka() { Nazev = "Snakeman", Zdravi = 55 });
            mapa.VysaditJednotku(1, 5, new Jednotka() { Nazev = "Silacoid", Zdravi = 114 });
            mapa.VysaditJednotku(2, 5, new Jednotka() { Nazev = "Snakeman", Zdravi = 50 });

            Console.WriteLine("Klonuji z 1;5 na 4;2 a upravuji zdravi...");
            try
            {
                mapa.ZkopirovatOblast(1, 5, 4, 2);
                //Console.WriteLine(mapa.Jednotky[new Oblast() { Delka = 4, Sirka = 2 }].ToString());
                mapa.Jednotky[new Point() { X = 4, Y = 2 }].First().Zdravi = 45;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("Vysazene jednotky:");
            foreach (var kv in mapa.Jednotky)
            {
                Console.WriteLine("V sektoru {0}; {1} jsou: {2}", kv.Key.X, kv.Key.Y, 
                    string.Join(", ", kv.Value.Select(a=>string.Format("{0} [{1}]",a.Nazev, a.Zdravi)).ToArray()));
            }

            Console.WriteLine("Konec vysadku... ENTER");
            Console.ReadLine();
        }
    }

}
