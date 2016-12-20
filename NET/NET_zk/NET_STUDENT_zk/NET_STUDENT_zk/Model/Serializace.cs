using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NET_STUDENT_zk.ViewModel;
using System.Xml.Serialization;
using System.Xml;
using System.Collections.ObjectModel;
using NET_STUDENT_zk.Model;
using System.IO;

namespace NET_STUDENT_zk.Model
{
    class Serializace
    {
        public static ObservableCollection<Student> LoadCSV()
        {
            ObservableCollection<Student>  studenti = new ObservableCollection<Student>();
            Student student;

            using (var reader = new StreamReader(File.OpenRead("kivnet.csv")))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');

                    student = new Student()
                    {
                        Jmeno = values[1].Trim('"'),
                        Prijmeni = values[2].Trim('"'),
                        OsobniCislo = values[0].Trim('"'),
                        //TitulPred = values[3].Trim('"'),
                        //TitulZa = values[4].Trim('"'),
                        //Stav = values[5].Trim('"'),
                        //Username = values[6].Trim('"'),
                        //Obor = values[17].Trim('"'),
                        //Program = values[7].Trim('"')
                    };

                    studenti.Add(student);
                }
            }
            //odstraneni prvni radky
            studenti.RemoveAt(0);


            return studenti;
        }




        public static void ToXML(NET net, string path)
        {
            var serializer = new XmlSerializer(typeof(NET));

            using (var writer = XmlWriter.Create(path, new XmlWriterSettings() { Indent = true }))
            {
                serializer.Serialize(writer, net);
            }
        }

        public static NET FromXML(string path)
        {
            NET net;

            var serializer = new XmlSerializer(typeof(NET));

            using (var reader = XmlReader.Create(path))
            {
                net = (NET)serializer.Deserialize(reader);
            }

            return net;
        }
    }
}
