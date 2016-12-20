using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET_STUDENT_zk.Model
{
    public class Pripominka : NotifyBase
    {
        private TypPripominky typ;
        public TypPripominky Typ
        {
            get { return typ; }
            set { SetIfChanged(ref typ, value); }
        }

        private string oduvodneni;
        public string Oduvodneni
        {
            get { return oduvodneni; }
            set { SetIfChanged(ref oduvodneni, value); }
        }

        private int body;
        public int Body
        {
            get { return body; }
            set { SetIfChanged(ref body, value); }
        }

        public override string ToString()
        {
            return Typ.ToString();
        }

    }
}
