using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET_ZK.Model
{
    class Student : NotifyBase, IComparable
    {
        public Student()
        {
            SmazatHodnoceni = new ViewModel.RelayCommand(
                (o) => {
                    hodnoceni.Remove(SelectedHodnoceni);
                }
                );

            PridatHodnoceni = new ViewModel.RelayCommand(
                (o) => {
                    Hodnoceni nove = new Hodnoceni() { SlovniHodnoceni = "slovní hodnocení", Body = 0 };
                    hodnoceni.Add(nove);
                    SelectedHodnoceni = nove;
                    hodnoceni.Sort();

                });
        }


        private ObservableCollection<Hodnoceni> hodnoceni;

        public ObservableCollection<Hodnoceni> Hodnoceni
        {
            get { return hodnoceni; }
            set { SetIfChanged(ref hodnoceni, value); }
        }

        private Hodnoceni selectedHodnoceni;

        public Hodnoceni SelectedHodnoceni
        {
            get { return selectedHodnoceni; }
            set { SetIfChanged(ref selectedHodnoceni, value); }
        }

        public ViewModel.RelayCommand SmazatHodnoceni { get; set; }
        public ViewModel.RelayCommand PridatHodnoceni { get; set; }

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

        private string titulPred;

        public string TitulPred
        {
            get { return titulPred; }
            set { SetIfChanged(ref titulPred, value); }
        }

        private string titulZa;

        public string TitulZa
        {
            get { return titulZa; }
            set { SetIfChanged(ref titulZa, value); }
        }

        private string stav;

        public string Stav
        {
            get { return stav; }
            set { SetIfChanged(ref stav, value); }
        }

        private string username;

        public string Username
        {
            get { return username; }
            set { SetIfChanged(ref username, value); }
        }

        private string program;

        public string Program
        {
            get { return program; }
            set { SetIfChanged(ref program, value); }
        }

        private string obor;

        public string Obor
        {
            get { return obor; }
            set { SetIfChanged(ref obor, value); }
        }

        private DateTime datumNarozeni;

        public DateTime DatumNarozeni
        {
            get { return datumNarozeni; }
            set { SetIfChanged(ref datumNarozeni, value); }
        }

        public string CeleJmeno
        {
            get { return Jmeno + " " + Prijmeni; }
        }

        public override string ToString()
        {
            return Jmeno + " " + Prijmeni;
        }

        public int CompareTo(object obj)
        {
            Student o = (Student)obj;

            return (-1) * o.CeleJmeno.CompareTo(this.CeleJmeno);
        }
    }

    static class Extensions
    {
        public static void Sort<T>(this ObservableCollection<T> collection) where T : IComparable
        {
            List<T> sorted = collection.OrderBy(x => x).ToList();
            for (int i = 0; i < sorted.Count(); i++)
            {
                collection.Move(collection.IndexOf(sorted[i]), i);
            }

        }
    }
}
