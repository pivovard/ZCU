using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Stl2Obj
{
    public struct Facet
    {
        public int V1 { get; set; }
        public int V2 { get; set; }
        public int V3 { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", V1, V2, V3);
        }
    }

    class Program
    {
        static string inputFilename;
        static string outputFilename = "";
        
        static List<Facet> facets = new List<Facet>();
        static Dictionary<string, int> vertices = new Dictionary<string, int>();

        static int lastVertex = 0;

        static void ParseArgs(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Chybny pocet parametru. Parametrem musi byt nazev ascii stl souboru, pripadne nazev vystupniho souboru.");
            }
            else
            {
                inputFilename = args[0];
                if (args.Length > 1)
                    outputFilename = args[1];
            }
        }

        static int ResolveVertex(string line)
        {
            line = line.Substring(7);
            if (!vertices.ContainsKey(line))
            {
                vertices[line] = ++lastVertex;
            }
            return vertices[line];
        }

        static Facet ReadFacet(TextReader file)
        {
            Facet result = new Facet();
            string line;
            int index = 0;
            do
            {
                line = file.ReadLine();

                if (line.StartsWith("vertex"))
                {
                    int vertex = ResolveVertex(line);
                    switch (index++)
                    {
                        case 0: result.V1 = vertex; break;
                        case 1: result.V2 = vertex; break;
                        case 2: result.V3 = vertex; break;
                    }
                }
            } while (!line.StartsWith("endfacet"));
            return result;
        }

        static void ReadSolid(TextReader file)
        {
            string line;
            do
            {
                line = file.ReadLine().Trim();
                if (line.StartsWith("facet"))
                {
                    facets.Add(ReadFacet(file));
                }
            } while (!line.StartsWith("endsolid"));
        }

        static void ReadFile()
        {
            try
            {
                using (TextReader file = File.OpenText(inputFilename))
                {
                    ReadSolid(file);
                }
            }
            catch 
            {                
            }
        }

        static void SaveData(TextWriter stream)
        {
            try
            {
                vertices.OrderBy(kw => kw.Value).ToList().ForEach(v => stream.WriteLine("v " + v));
                facets.ForEach(f => stream.WriteLine("f "+ f));
            }
            catch
            {

            }
        }

        static void SaveFile()
        {
            try
            {
                if (outputFilename != "")
                {
                    TextWriter stream = File.CreateText(outputFilename);
                    SaveData(stream);
                    stream.Close();
                }
                else
                {
                    SaveData(Console.Out);
                }
            } catch {

            }
        }

        static void Main(string[] args)
        {
            ParseArgs(args);
            ReadFile();
            SaveFile();
        }
    }
}
