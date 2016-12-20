using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET_ZK.Model
{
    public class Koktejl : NotifyBase
    {
        private String nazev;

        public String Nazev
        {
            get { return nazev; }
            set { SetIfChanged(ref nazev, value); }
        }

        private List<string> ingredience;

        public List<string> Ingredience
        {
            get { return ingredience; }
            set { SetIfChanged(ref ingredience, value); }
        }

        private int cena;

        public int Cena
        {
            get { return cena; }
            set { SetIfChanged(ref cena, value); }
        }

        public override string ToString()
        {
            return Nazev;
        }

        public Koktejl()
        {
            Ingredience = new List<string>();
        }
    }
}
