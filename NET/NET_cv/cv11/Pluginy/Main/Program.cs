using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace Main
{
    public class Program
    {
        static void Main(string[] args)
        {
            var moduly = NactiModuly();

            foreach(var modul in moduly)
            {
                VypisKonfiguraci(modul);
                Console.WriteLine($"{modul.GetType()}: {modul.Funkce(0)}");
            }

            Console.ReadKey();
        }

        public static void VypisKonfiguraci(IModul modul)
        {
            var properties = modul.GetType().GetProperties()
                .Where(p => p.CanRead && p.CanWrite)
                .Where(p => p.GetMethod.IsPublic)
                .Where(p => p.SetMethod.IsPublic);

            foreach(var property in properties)
            {
                Console.WriteLine($"{property.Name}: {property.GetValue(modul)}");

                if(property.PropertyType == typeof(double))
                {
                    property.SetValue(modul, 10);
                    Console.WriteLine($"{property.Name} * 10: {property.GetValue(modul)}");
                }
            }
        }

        public static List<IModul> NactiModuly()
        {
            List<IModul> result = new List<IModul>();

            var files = Directory.GetFiles("./", "*.dll");

            foreach(var file in files)
            {
                try
                {
                    Assembly asm = Assembly.LoadFrom(file);

                    var moduly = asm.GetExportedTypes().Where(t => t.GetInterface("IModul") != null);

                    foreach(var modul in moduly)
                    {
                        result.Add((IModul)Activator.CreateInstance(modul));
                    }
                }
                catch
                {
                    Console.WriteLine($"{file} neni assembly");
                }
            }


            return result;
        }
    }
}
