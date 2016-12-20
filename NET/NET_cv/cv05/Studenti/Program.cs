using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Studenti
{
    class Program
    {
        static void Main(string[] args)
        {
            Studenti studenti = new Studenti();
            
            Encoding cp = Encoding.GetEncoding(1250);

            FileStream f = new FileStream("studenti.csv", FileMode.Open);
            StreamReader r = new StreamReader(f, Encoding.GetEncoding(1250));
            r.ReadLine();

            string line;
            while((line = r.ReadLine()) != null)
            {
                string lineCP = cp.GetString(cp.GetBytes(line)).Trim('"');
                string[] lineSplit = line.Split(';');

                Console.WriteLine($"Jmeno: {lineSplit[1]}, Prijmeni: {lineSplit[2]}, Rok: {lineSplit[0].Substring(2,2)}, cislo: {lineSplit[0]}");

                studenti.List.Add(new Student() { Jmeno = lineSplit[1], Prijmeni = lineSplit[2], OsCislo = lineSplit[0] });
            }

            XmlSerializace.Uloz(studenti, "studenti.serializace.xml");
            studenti = XmlSerializace.Nacti("studenti.serializace.xml");

            XmlRW.Uloz(studenti, "studenti.rw.xml");
            studenti = XmlRW.Nacti("studenti.rw.xml");

            XmlLinq.Uloz(studenti, "studenti.linq.xml");
            studenti = XmlLinq.Nacti("studenti.linq.xml");

            Console.ReadKey();
        }
    }

    public class Studenti
    {
        public List<Student> List { get; set; } = new List<Student>();
        public string neco { get; set; }
    }

    public class Student
    {
        [XmlAttribute("id")]
        public string OsCislo { get; set; }

        [XmlIgnore]
        public string Jmeno { get; set; }

        [XmlElement("Name")]
        public string Prijmeni { get; set; }
    }
}
