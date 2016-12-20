using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace NET_ZK.Model
{
    class Loading
    {
        static ObservableCollection<Student> studenti;

        public static ObservableCollection<Student> FromCSV()
        {
            studenti = new ObservableCollection<Student>();
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
                        TitulPred = values[3].Trim('"'),
                        TitulZa = values[4].Trim('"'),
                        Stav = values[5].Trim('"'),
                        Username = values[6].Trim('"'),
                        Obor = values[17].Trim('"'),
                        Program = values[7].Trim('"')
                    };

                    studenti.Add(student);
                }
            }
            //odstraneni prvni radky
            studenti.RemoveAt(0);

            return studenti;
        }

        public static ObservableCollection<Student> FromXML()
        {
            ObservableCollection<Student> list = new ObservableCollection<Student>();

            var serializer = new XmlSerializer(typeof(ObservableCollection<Student>));

            using(var reader = XmlReader.Create("zanamy.xml"))
            {
                studenti = (ObservableCollection<Student>)serializer.Deserialize(reader);
            }

            return list;
        }

        public static void ToXML(ObservableCollection<Student> list)
        {
            var serializer = new XmlSerializer(typeof(ObservableCollection<Student>));

            using (var writer = XmlWriter.Create("zaznamy.xml", new XmlWriterSettings() { Indent = true }))
            {
                serializer.Serialize(writer, list);
            }
        }
    }
}
