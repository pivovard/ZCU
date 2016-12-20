using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1.ViewModel
{
    class Studenti : Model.NotifyBase
    {
        private ObservableCollection<Model.Student> list;

        public ObservableCollection<Model.Student> List
        {
            get { return list; }
            private set { list = value; }
        }

        private ObservableCollection<Model.Student> filtrovanyList;

        public ObservableCollection<Model.Student> FiltrovanyList
        {
            get { return filtrovanyList; }
            private set { filtrovanyList = value; filtrovanyList.Sort(); }
        }

        private Model.Student selected;

        public Model.Student Selected
        {
            get { return selected; }
            set { SetIfChanged(ref selected, value); }
        }

        private string filtr;

        public string Filtr
        {
            get { return filtr; }
            set
            {
                filtr = value;
                
            }
        }

        public RelayCommand Pridat { get; set; }
        public RelayCommand Ulozit { get; set; }
        public RelayCommand Vybrat { get; set; }
        public RelayCommand Smazat { get; set; }

        public Studenti()
        {
            filtr = "";

            if (list == null)
            {
                //list = Model.PraceSDaty.nactiStudentyZCsv();
                list = Model.PraceSDaty.nactiZeXml();
                //Console.WriteLine(list.Count + " " + list.ElementAt(0).Prijmeni);

                filtrovanyList = new ObservableCollection<Model.Student>();

                foreach (Model.Student stud in list)
                {
                    stud.Hodnoceni.Sort();
                    filtrovanyList.Add(stud);
                }

                filtrovanyList.Sort();


            }

            //list = new ObservableCollection<Model.Student>();
            //list.Add(new Model.Student() { Jmeno = "Jarda", Prijmeni = "Navratil", StudijniCislo = "A13N5594P" });
            //list.Add(new Model.Student() { Jmeno = "Marek", Prijmeni = "Novy", StudijniCislo = "A13N4994P" });

            sestrojPridat();
            sestrojUlozit();
            sestrojVybrat();
            sestrojSmazat();
        }

        private void sestrojSmazat()
        {
            Smazat = new RelayCommand(
                            (o) =>
                            {
                                list.Remove(Selected);
                                filtrovanyList.Remove(Selected);
                            }
                            );
        }

        private void sestrojVybrat()
        {
            Vybrat = new RelayCommand(
                            (o) =>
                            {

                                filtrovanyList.Clear();

                                foreach (Model.Student stud in list)
                                {
                                    if (stud.CeleJmeno.ToLower().Contains(filtr.ToLower()))
                                    {
                                        filtrovanyList.Add(stud);
                                    }

                                }

                                filtrovanyList.Sort();
                            }
                            );
        }

        private void sestrojUlozit()
        {
            Ulozit = new RelayCommand(
                            (o) =>
                            {
                                Model.PraceSDaty.zapisDoXml(list);
                            }
                            );
        }

        private void sestrojPridat()
        {
            Pridat = new RelayCommand(
                            (o) =>
                            {
                                var student = new Model.Student() { Jmeno = "jméno", Prijmeni = "příjmení", OsobniCislo = "osobní číslo" };
                                list.Add(student);
                                Selected = student;

                                filtrovanyList.Add(student);
                                filtrovanyList.Sort();

                            }//,
                             //podminka pro nemoznost pridani dalsiho
                             //(o) => list.Count < 5
                            );
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
