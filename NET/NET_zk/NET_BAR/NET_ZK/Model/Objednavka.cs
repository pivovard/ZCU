using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET_ZK.Model
{
    public class Objednavka
    {
        public string Nazev { get; set; }
        public int Cena { get; set; }

        public Objednavka(string nazev, int cena)
        {
            Nazev = nazev;
            Cena = cena;
        }

        public Objednavka() { }


        public override string ToString()
        {
            return $"{Nazev}   {Cena}";
        }
    }
}
