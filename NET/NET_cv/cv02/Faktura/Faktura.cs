using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kiv.Net.cv02
{
    public class Faktura
    {
        public int CisloFaktury { get; set; }

        public DateTime DatumVystaveni { get; set; }

        public DateTime DatumSplatnosti { get; set; }

        public override string ToString()
        {
            return string.Format("cislo: {0} \n dat vystaveni {1:dd.MM.yyyy} \n dat. splatnosti {2:dd.MM.yyyy}",
                CisloFaktury,
                DatumVystaveni,
                DatumSplatnosti);
        }
    }
}
