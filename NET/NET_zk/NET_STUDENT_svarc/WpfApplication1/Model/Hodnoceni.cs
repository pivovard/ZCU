using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1.Model
{
    public class Hodnoceni : NotifyBase, IComparable
    {

        string slovniHodnoceni;

        public string SlovniHodnoceni
        {
            get { return slovniHodnoceni; }
            set { SetIfChanged(ref slovniHodnoceni, value); }
        }

        private DateTime datumHodnoceni;

        public DateTime DatumHodnoceni
        {
            get { return datumHodnoceni; }
            set { SetIfChanged(ref datumHodnoceni, value); }
        }

        int body;

        public int Body
        {
            get { return body; }
            set { SetIfChanged(ref body, value); }
        }

        
        public override string ToString()
        {
            return datumHodnoceni.ToString() + ": " + Body.ToString();
        }

        public int CompareTo(object obj)
        {
            Hodnoceni o = (Hodnoceni)obj;

            return o.DatumHodnoceni.CompareTo(this.datumHodnoceni);
        }
    }
}
