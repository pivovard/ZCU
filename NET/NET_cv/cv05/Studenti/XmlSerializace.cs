using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Studenti
{
    class XmlSerializace
    {

        public static void Uloz(Studenti studenti, string soubor)
        {
            var xs = new XmlSerializer(typeof(Studenti));

            using(var w = XmlWriter.Create(soubor,new XmlWriterSettings() { Indent = true }))
            {
                xs.Serialize(w, studenti);
            }
        }

        public static Studenti Nacti(string soubor)
        {
            Studenti studenti = new Studenti();
            var xs = new XmlSerializer(typeof(Studenti));

            using (var r = XmlReader.Create(soubor))
            {
                studenti = (Studenti)xs.Deserialize(r);
            }

            return studenti;
        }


    }
}
