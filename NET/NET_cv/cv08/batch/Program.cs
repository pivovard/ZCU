using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.IO;

namespace BatchProcess
{
    /// <summary>
    /// Program spousti davkove zpracovani aplikace Process.exe.
    /// Process.exe se spousti s jednim int parametrem, vsechny vypocty vypisuje do konzole.
    /// Protoze vypocet trva pomerne dlouho, spousti se vse ve vlaknech, ktera zapisuji 
    /// vysledek do souboru vystupN.txt, kde N je parametr, se kterym byl dany proces spusten.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            string processFile = "Process.exe";
            string outputFileBase = "vystup";
            string outputFileExtension = ".txt";
            int processes = 20;

            List<Thread> threads = new List<Thread>();

            for (int p = 0; p < processes; p++)
            {
                int ap = p;
                Thread t = new Thread(delegate() {
                    try
                    {
                        var outputFile = File.CreateText(outputFileBase + p + outputFileExtension);
                        Process proc = new Process();
                        proc.StartInfo.FileName = processFile;
                        proc.StartInfo.Arguments = p.ToString();
                        proc.StartInfo.UseShellExecute = false;
                        proc.StartInfo.RedirectStandardOutput = true;
                        proc.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
                        {
                            outputFile.WriteLine(e.Data);
                            
                        }
                        );
                        
                        proc.Start();
                        proc.BeginOutputReadLine();
                        
                        proc.WaitForExit();
                        proc.Close();
                        outputFile.Close();
                    } catch {

                    }
                });
                threads.Add(t);
                t.Start();
            }

            Console.WriteLine("Processes started.");

            foreach (var thread in threads)
                thread.Join();

            Console.WriteLine("Processes done.");
        }
    }
}
