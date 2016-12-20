using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kiv.Net.cv02;

namespace Ucetnictvi
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Faktura> faktury = new List<Faktura>();
            faktury.Add(new Faktura() { CisloFaktury = 0, DatumSplatnosti = DateTime.Now, DatumVystaveni = DateTime.Now });
            faktury.Add(new Faktura() { CisloFaktury = 1, DatumSplatnosti = DateTime.Now.AddDays(3), DatumVystaveni = DateTime.Now });
            faktury.Add(new Faktura() { CisloFaktury = 2, DatumSplatnosti = DateTime.Now.AddDays(30), DatumVystaveni = DateTime.Now });

            foreach (Faktura f in faktury)
            {
                Console.WriteLine(f);
            }
        }
    }
}
