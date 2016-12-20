using NET_STUDENT_zk.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NET_STUDENT_zk.Model
{
    public class Student : NotifyBase
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

        private string osobniCislo;
        public string OsobniCislo
        {
            get { return osobniCislo; }
            set { SetIfChanged(ref osobniCislo, value); }
        }

        private int body;
        public int Body
        {
            get { return body; }
            set { SetIfChanged(ref body, value); }
        }

        private ObservableCollection<Odevzdani> odevzdani;
        public ObservableCollection<Odevzdani> Odevzdani
        {
            get { return odevzdani; }
            set { SetIfChanged(ref odevzdani, value); count(); }
        }

        private Odevzdani selectedOdevzdani;
        [XmlIgnore]
        public Odevzdani SelectedOdevzdani
        {
            get { return selectedOdevzdani; }
            set { SetIfChanged(ref selectedOdevzdani, value); count(); }
        }

        [XmlIgnore]
        public RelayCommand AddOdevzdani { get; set; }
        [XmlIgnore]
        public RelayCommand RemOdevzdani { get; set; }


        public Student()
        {
            Odevzdani = new ObservableCollection<Model.Odevzdani>();

            AddOdevzdani = new RelayCommand(
                (o) => {
                    Odevzdani x = new Odevzdani() { Nazev = "Nazev", Body = 0 };
                    Odevzdani.Add(x);
                });

            RemOdevzdani = new RelayCommand(
                (o) => {
                    Odevzdani.Remove(SelectedOdevzdani);
                });
        }

        public void count()
        {
            int sum = 0;
            foreach (var x in Odevzdani) sum += x.Body;
            Body = sum;
        }

        public override string ToString()
        {
            return $"{Jmeno} {Prijmeni}";
        }

    }
}
