using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace WpfApplication1.Model
{
    static class PraceSDaty
    {
        static ObservableCollection<Student> studenti;
        static Student tmp;
        static Student student;
        static Hodnoceni hodnoceni;

        public static ObservableCollection<Student> nactiStudentyZCsv()
        {
            studenti = new ObservableCollection<Student>();

            var reader = new StreamReader(File.OpenRead(@"C:\skola\kivnet.csv"));


            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(';');

                tmp = new Student() { Jmeno = values[1].Trim('"'), Prijmeni = values[2].Trim('"'), OsobniCislo = values[0].Trim('"'),
                    TitulPred = values[3].Trim('"'), TitulZa = values[4].Trim('"'), Stav = values[5].Trim('"'),
                    Username = values[6].Trim('"'), Obor = values[17].Trim('"'), Program = values[7].Trim('"')
                };
                studenti.Add(tmp);
            }

            reader.Close();

            //odstraneni prvni radky
            studenti.RemoveAt(0);

            return studenti;
        }


        public static ObservableCollection<Student> nactiZeXml()
        {
            studenti = new ObservableCollection<Student>();

            String tag = "";

            XmlTextReader reader = new XmlTextReader("C:\\skola\\zaznamy.xml");
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element: // Tento uzel je prvek.
                            tag = reader.Name;
                        if (tag.Equals("Student"))
                        {
                            student = new Student();
                            student.Hodnoceni = new ObservableCollection<Hodnoceni>();
                        }
                        if (tag.Equals("Hodnoceni"))
                        {
                            hodnoceni = new Hodnoceni();
                        }
                        break;

                    case XmlNodeType.Text: //Zobrazení textu v jednotlivých prvcích.
                        zapisUdaj(tag, reader.Value);
                        break;

                    case XmlNodeType.EndElement: //Zobrazení konce prvku.
                        if (reader.Name.Equals("Student"))
                        {
                            studenti.Add(student);
                        }
                        if (reader.Name.Equals("Hodnoceni"))
                        {
                            student.Hodnoceni.Add(hodnoceni);
                        }
                        break;

                    default: break;
                }
            }
            reader.Close();

            return studenti;
        }

        private static void zapisUdaj(String tag, String udaj)
        {

            switch (tag)
            {
               
                case "Jmeno":
                    student.Jmeno = udaj;
                       
                    break;

                case "Prijmeni":
                    student.Prijmeni = udaj;

                    break;

                case "OsobniCislo":
                    student.OsobniCislo = udaj;

                    break;

                case "TitulPred":
                    student.TitulPred = udaj;

                    break;

                case "TitulZa":
                    student.TitulZa = udaj;

                    break;

                case "Stav":
                    student.Stav = udaj;

                    break;

                case "Username":
                    student.Username = udaj;

                    break;

                case "Program":
                    student.Program = udaj;

                    break;

                case "Obor":
                    student.Obor = udaj;

                    break;

                case "DatumHodnoceni":
                    hodnoceni.DatumHodnoceni = Convert.ToDateTime(udaj);

                    break;

                case "Body":
                    hodnoceni.Body = Convert.ToInt32(udaj);

                    break;

                case "SlovniHodnoceni":
                    hodnoceni.SlovniHodnoceni = udaj;

                    break;


                default: break;
            }


        }



        public static void zapisDoXml(ObservableCollection<Student> zaznamy)
        {
            //kvuli nacitani pak musim smazat notaci xml
            using (var sw = new StringWriter())
            {
                using (var writer = System.Xml.XmlWriter.Create(sw))
                {
                    writer.WriteStartElement("Studenti");

                    foreach (Student zaznam in zaznamy)
                    {
                        
                        writer.WriteStartElement("Student");

                        writer.WriteStartElement("Jmeno");
                        writer.WriteString(zaznam.Jmeno);
                        writer.WriteEndElement();

                        writer.WriteStartElement("Prijmeni");
                        writer.WriteString(zaznam.Prijmeni);
                        writer.WriteEndElement();

                        writer.WriteStartElement("OsobniCislo");
                        writer.WriteString(zaznam.OsobniCislo);
                        writer.WriteEndElement();

                        writer.WriteStartElement("TitulPred");
                        writer.WriteString(zaznam.TitulPred);
                        writer.WriteEndElement();

                        writer.WriteStartElement("TitulZa");
                        writer.WriteString(zaznam.TitulZa);
                        writer.WriteEndElement();

                        writer.WriteStartElement("Stav");
                        writer.WriteString(zaznam.Stav);
                        writer.WriteEndElement();

                        writer.WriteStartElement("Username");
                        writer.WriteString(zaznam.Username);
                        writer.WriteEndElement();

                        writer.WriteStartElement("Program");
                        writer.WriteString(zaznam.Program);
                        writer.WriteEndElement();

                        writer.WriteStartElement("Obor");
                        writer.WriteString(zaznam.Obor);
                        writer.WriteEndElement();

                        
                        foreach (Hodnoceni h in zaznam.Hodnoceni)
                        {
                            writer.WriteStartElement("Hodnoceni");

                            writer.WriteStartElement("DatumHodnoceni");
                            writer.WriteString(h.DatumHodnoceni.ToString());
                            writer.WriteEndElement();

                            writer.WriteStartElement("Body");
                            writer.WriteString(h.Body.ToString());
                            writer.WriteEndElement();

                            writer.WriteStartElement("SlovniHodnoceni");
                            writer.WriteString(h.SlovniHodnoceni);
                            writer.WriteEndElement();

                            writer.WriteEndElement();
                        }                        

                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();

                }

                File.WriteAllText(@"C:\skola\zaznamy.xml", sw.ToString().Substring(39, sw.ToString().Length - 39));

            }

            


        }

    }
}
