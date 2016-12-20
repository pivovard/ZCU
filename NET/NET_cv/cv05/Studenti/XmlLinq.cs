using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Studenti
{
    class XmlLinq
    {
        public static void Uloz(Studenti studenti, string soubor)
        {
            XDocument xDocument = new XDocument();

            XElement xStudenti = new XElement("Studenti");

            foreach(var student in studenti.List)
            {
                xStudenti.Add( new XElement("Student",
                    new XElement("Jmeno", student.Jmeno),
                    new XElement("Prijmeni", student.Prijmeni),
                    new XElement("OsCislo", student.OsCislo)
                ));
                
            }

            xDocument.Add(xStudenti);
            xDocument.Save(soubor);
        }

        public static Studenti Nacti(string soubor)
        {
            XDocument xDocument = XDocument.Load(soubor);

            Studenti studenti = new Studenti();

            studenti.List = xDocument.Descendants("Student").Select(s =>
                new Student()
                {
                    OsCislo = s.Element("OsCislo").Value,
                    Jmeno = s.Element("Jmeno").Value,
                    Prijmeni = s.Element("Prijmeni").Value
                }).ToList();

            return studenti;
        }
    }
}
