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
    public class Odevzdani : NotifyBase
    {
        private string nazev;
        public string Nazev
        {
            get { return nazev; }
            set { SetIfChanged(ref nazev, value); }
        }

        private int body;
        public int Body
        {
            get { return body; }
            set { SetIfChanged(ref body, value); }
        }

        private ObservableCollection<Pripominka> pripominky;
        public ObservableCollection<Pripominka> Pripominky
        {
            get { return pripominky; }
            set { SetIfChanged(ref pripominky, value); count(); }
        }

        private Pripominka selectedPripominka;
        [XmlIgnore]
        public Pripominka SelectedPripominka
        {
            get { return selectedPripominka; }
            set { SetIfChanged(ref selectedPripominka, value); count(); }
        }

        [XmlIgnore]
        public RelayCommand AddPripominka { get; set; }
        [XmlIgnore]
        public RelayCommand RemPripominka { get; set; }
        

        public Odevzdani()
        {
            Pripominky = new ObservableCollection<Pripominka>();

            AddPripominka = new RelayCommand(
                (o) => {
                    Pripominka x = new Pripominka() { Typ = TypPripominky.Neutralni, Body = 0 };
                    Pripominky.Add(x);
                });

            RemPripominka = new RelayCommand(
                (o) => {
                    Pripominky.Remove(SelectedPripominka);
                });
        }

        public void count()
        {
            int sum = 0;
            foreach (var x in Pripominky) sum += x.Body;
            Body = sum;
        }

        public override string ToString()
        {
            return Nazev;
        }
    }
}
