using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Studenti
{
    class XmlRW
    {
        public static void Uloz(Studenti studenti, string soubor)
        {
            using(var w = XmlWriter.Create(soubor, new XmlWriterSettings() { Indent = true }))
            {
                w.WriteStartDocument();
                w.WriteStartElement("Studenti");

                foreach(var student in studenti.List)
                {
                    w.WriteStartElement("Student");
                    w.WriteElementString("Jmeno", student.Jmeno);
                    w.WriteElementString("Prijmeni", student.Prijmeni);
                    w.WriteElementString("OsCislo", student.OsCislo);
                    w.WriteEndElement();
                }

                w.WriteEndElement();
            }
        }

        public static Studenti Nacti(string soubor)
        {
            Studenti studenti = new Studenti();

            using(var r = XmlReader.Create(soubor))
            {
                while (r.Read())
                {
                    if (r.Name == "Studenti")
                    {
                        while (r.Read())
                        {
                            if (r.Name == "Student")
                            {
                                var student = new Student();
                                r.Read();

                                while (r.IsStartElement())
                                {
                                    
                                    switch (r.Name)
                                    {
                                        case "Jmeno": r.Read(); student.Jmeno = r.Value; break;
                                        case "Prijmeni": r.Read(); student.Prijmeni = r.Value; break;
                                        case "OsCislo": r.Read(); student.OsCislo = r.Value; break;
                                        default: r.Skip(); break;
                                    }

                                    r.Read();
                                    r.Read();
                                }

                                studenti.List.Add(student);
                            }
                        }
                    }
                }
            }

            return studenti;
        }
    }
}
