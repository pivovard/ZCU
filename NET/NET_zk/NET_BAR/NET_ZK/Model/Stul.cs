using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NET_ZK.Model
{
    public class Stul : NotifyBase
    {
        private int id;
        public int ID
        {
            get { return id; }
            set { SetIfChanged(ref id, value); }
        }

        private ObservableCollection<Objednavka> objednavky;
        public ObservableCollection<Objednavka> Objednavky
        {
            get { return objednavky; }
            set { SetIfChanged(ref objednavky, value); }
        }

        private int cena;
        public int Cena
        {
            get { return cena; }
            set { SetIfChanged(ref cena, value); }
        }

        public Stul()
        {
            Objednavky = new ObservableCollection<Objednavka>();
        }

        public override string ToString()
        {
            return "Stul " + ID.ToString();
        }
    }
}
