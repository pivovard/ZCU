using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.Model
{
    class Student : BaseModel
    {
        string jmeno;
        public string Jmeno
        {
            get { return jmeno; }
            set
            {
                jmeno = value;
                NotifyPropertyChanged();
            }
        }

        int o1;
        public int O1
        {
            get { return o1; }
            set
            {
                o1 = value;
                NotifyPropertyChanged();
            }
        }

        int o2;
        public int O2
        {
            get { return o2; }
            set
            {
                o2 = value;
                NotifyPropertyChanged();
            }
        }

        public int Celkem
        {
            get { return o1 + o2; }
        }

        public override string ToString()
        {
            return Jmeno;
        }
    }
}
