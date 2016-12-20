using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace ConsoleApplication4
{
    class Program
    {
        static FileWatcher fw;        

        static void Main(string[] args)
        {
            DeleteDatFiles();
            FileSystemWatcher newFiles = new FileSystemWatcher("./", "*.dat");
            newFiles.Created += newFile_Created;
            newFiles.EnableRaisingEvents = true;

            Console.WriteLine("Zacinam merit.");

            Process p = new Process();
            p.StartInfo = new ProcessStartInfo();
            p.StartInfo.FileName = "Measurement.exe";
            p.StartInfo.UseShellExecute = true;
            p.Start();

            while (!p.HasExited)
            {
                
            }

            Console.WriteLine("Mereni dokonceno.");
            
            Console.ReadLine();
            DeleteDatFiles();
        }


        /// <summary>
        /// Smaze .dat soubory
        /// </summary>
        static void DeleteDatFiles()
        {
            Console.WriteLine("Mazu datove soubory");
            string[] filesToDelete = Directory.GetFiles("./");
            foreach (var file in filesToDelete)
            {
                try
                {
                    if (Path.GetExtension(file) == ".dat")
                        File.Delete(file);
                }
                catch { }
            }
        }

        /// <summary>
        /// Kdyz se objevi novy soubor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void newFile_Created(object sender, FileSystemEventArgs e)
        {
                Console.WriteLine("Novy soubor: {0}", e.Name);
            fw = new FileWatcher(e.Name);
            if (fw != null)
            {
                fw.Stop();
                fw.Changed -= fw_Changed;
            }
            
            fw.Changed += fw_Changed;
        }

        /// <summary>
        /// Kdyz se zmeni sledovany soubor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="evetArgs"></param>
        static void fw_Changed(object sender, FileWatcherChangedEventArgs evetArgs)
        {
            if(evetArgs.Type==FileWatcherChangedType.Modified) {
                int pos = evetArgs.ChangePosition;
                Console.WriteLine("Prvni zmena na bytu {0} z {1} na {2}", pos, evetArgs.OriginalData[pos], evetArgs.NewData[pos]);
            }
        }
    }

}
