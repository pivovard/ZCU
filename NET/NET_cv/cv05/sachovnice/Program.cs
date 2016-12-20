using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using SachovniceLib;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = new Sachovnice(Kresli, true);
            Console.ReadKey();
        }

        static int n;

        /// <summary>
        /// Nastavi velikost okna a vykresli sachovnici o rozmerech n*n,
        /// s pismennymi popisky (uppercase) nahore a dole a ciselnymi vlevo a vpravo.
        /// Kazde pole i popisek ma rozmer 3x3, popisek musi byt 
        /// vycentrovany (v pripade cisel >9 zacina cislo uprostred a 
        /// jde doprava, pr: 12|).
        /// Pole 1A ma cernou barvu. 
        /// Na konci okna by nemela byt prazdna radka.
        /// </summary>
        /// <param name="n"></param>
        public static void Kresli(int velikost)
        {
            n = velikost;
            Okno();
            Popisky();
            Plan();
        }

        /// <summary>
        /// Nastavi velikost okna
        /// </summary>
        static void Okno()
        {
            Console.WindowWidth = (3 * n) +6;
            Console.WindowHeight = (3 * n) +6;

            //Console.SetWindowSize(3 * n, 3 * n);
        }

        /// <summary>
        /// Vypise popisky
        /// </summary>
        static void Popisky()
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.White;
                for (int i = 0; i < n + 6; i++)
                {
                    Console.CursorLeft = (3 * i) + 1;
                    Console.CursorTop = 1;
                    switch (i)
                    {
                        case 1: Console.Write("A"); break;
                        case 2: Console.Write("B"); break;
                        case 3: Console.Write("C"); break;
                        case 4: Console.Write("D"); break;
                        case 5: Console.Write("E"); break;
                        case 6: Console.Write("F"); break;
                        case 7: Console.Write("G"); break;
                        case 8: Console.Write("H"); break;
                    }

                    Console.CursorLeft = (3 * i) + 1;
                    Console.CursorTop = (3 * n) + 1 + 3;
                    switch(i)
                    {
                        case 1: Console.Write("A"); break;
                        case 2: Console.Write("B"); break;
                        case 3: Console.Write("C"); break;
                        case 4: Console.Write("D"); break;
                        case 5: Console.Write("E"); break;
                        case 6: Console.Write("F"); break;
                        case 7: Console.Write("G"); break;
                        case 8: Console.Write("H"); break;
                    }

                    //Console.Write(i);

                }

                for (int i = 1; i <= n; i++)
                {
                    
                    Console.CursorTop = (3 * i) + 1;
                    Console.CursorLeft = 1;
                    Console.Write(i);

                    Console.CursorTop = (3 * i) + 1;
                    Console.CursorLeft = (3 * n) + 1 + 3;
                    Console.Write(i);
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// Nakresli jednotliva pole
        /// </summary>
        static void Plan()
        {
            try
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if((i*j)%3 == 0)
                            Pole(i, j, ConsoleColor.White);
                        else
                            Pole(i, j, ConsoleColor.Black);
                    }
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// Nakresli pole dane barvy na zadanych souradnicich. Vsechna pole musi byt 
        /// vykreslena pomoci znaku ' ' (mezera).
        /// </summary>
        /// <param name="i">1. souradnice</param>
        /// <param name="j">2. souradnice</param>
        /// <param name="barva">barva</param>
        static void Pole(int i, int j, ConsoleColor barva)
        {
            Console.BackgroundColor = barva;
            for (int k = 0; k < 3; k++)
            {
                Console.CursorLeft = 3 * i;
                Console.CursorTop = 3 * j;
                Console.Write("   ");
            }
        }

    }

}
