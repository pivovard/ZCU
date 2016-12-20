using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NET_STUDENT_zk.Model;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace NET_STUDENT_zk.ViewModel
{
    public class NET : NotifyBase
    {
        private ObservableCollection<Student> studenti;
        public ObservableCollection<Student> Studenti
        {
            get { return studenti; }
            private set { studenti = value; }
        }

        private Student selectedStudent;
        [XmlIgnore]
        public Student SelectedStudent
        {
            get { return selectedStudent; }
            set { SetIfChanged(ref selectedStudent, value); }
        }

        [XmlIgnore]
        public RelayCommand LoadCSV { get; set; }
        [XmlIgnore]
        public RelayCommand LoadXML { get; set; }
        [XmlIgnore]
        public RelayCommand ExportXML { get; set; }

        public NET()
        {
            Studenti = new ObservableCollection<Student>();

            Studenti = Serializace.LoadCSV();

            LoadCSV = new RelayCommand(
                (o) => {
                    this.Studenti.Clear();
                    this.Studenti = Serializace.LoadCSV();
                });
            LoadXML = new RelayCommand(
                (o) => {
                    NET net = Serializace.FromXML("export.xml");
                    this.Studenti.Clear();
                    this.Studenti = net.Studenti;
                });
            ExportXML = new RelayCommand(
                (o) => {
                    Serializace.ToXML(this, "export.xml");
                });
        }
    }
}
