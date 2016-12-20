using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.ViewModel
{
    class Studenti
    {
        ObservableCollection<Model.Student> studenti = new ObservableCollection<Model.Student>();
        public ObservableCollection<Model.Student> ListStudenti
        {
            get { return studenti; }
            private set { studenti = value; }
        }

        public Studenti()
        {
            studenti.Add(new Model.Student() { Jmeno = "AAA", O1 = 1, O2 = 2 });
            studenti.Add(new Model.Student() { Jmeno = "BBB", O1 = 4, O2 = 6 });
            studenti.Add(new Model.Student() { Jmeno = "CCC", O1 = 3, O2 = 2 });
        }
    }
}
