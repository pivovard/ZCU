using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stag.ViewModel
{
    class Studenti : Stag.Model.NotifyBase
    {
        private ObservableCollection<Stag.Model.Student> list;

        public ObservableCollection<Stag.Model.Student> List
        {
            get { return list; }
            private set { list = value; }
        }

        private Model.Student selected;

        public Model.Student Selected
        {
            get { return selected; }
            set { SetIfChanged(ref selected, value); }
        }


        public RelayCommand Pridat { get; set; }

        public Studenti()
        {
            list = new ObservableCollection<Model.Student>();
            list.Add(new Model.Student() { Jmeno = "Jarda", Prijmeni = "Navratil", Cislo = 123 });
            list.Add(new Model.Student() { Jmeno = "Marek", Prijmeni = "Novy", Cislo = 147 });
            Selected = list[0];

            Pridat = new RelayCommand(
                (o) => {
                    var student = new Model.Student() { Jmeno = "Jmeno", Prijmeni = "Prijmeni" };
                    list.Add(student);
                    Selected = student;
                    },
                (o) => list.Count < 5);
        }
    }
}
