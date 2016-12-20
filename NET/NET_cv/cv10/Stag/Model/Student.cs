using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stag.Model
{
    class Student : NotifyBase
    {
        private string jmeno;

        public string Jmeno
        {
            get { return jmeno; }
            set { SetIfChanged(ref jmeno, value); }
        }

        private string prijmeni;

        public string Prijmeni
        {
            get { return prijmeni; }
            set { SetIfChanged(ref prijmeni, value); }
        }

        private int cislo;

        public int Cislo
        {
            get { return cislo; }
            set { SetIfChanged(ref cislo, value); }
        }

        public override string ToString()
        {
            return $"{Jmeno} {Prijmeni}";
        }

    }
}
