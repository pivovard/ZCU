using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NET_ZK.ViewModel;
using System.Xml.Serialization;
using System.Xml;

namespace NET_ZK.Model
{
    class Serializace
    {
        

        public static void ToXML(Bar bar, string path)
        {
            var serializer = new XmlSerializer(typeof(Bar));

            using (var writer = XmlWriter.Create(path, new XmlWriterSettings() { Indent = true }))
            {
                serializer.Serialize(writer, bar);
            }
        }

        public static Bar FromXML(string path)
        {
            Bar bar;

            var serializer = new XmlSerializer(typeof(Bar));

            using (var reader = XmlReader.Create(path))
            {
                bar = (Bar)serializer.Deserialize(reader);
            }

            return bar;
        }
    }
}
