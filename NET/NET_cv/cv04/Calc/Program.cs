using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc
{
    class Program
    {
        static void Main(string[] args)
        {
            Menu menu = new Menu();

            menu.Polozky['+'] = new PolazkaMenu() { Operace = (op1, op2) => op1 + op2, Popis = "Scitani" };
            menu.Polozky['-'] = new PolazkaMenu() { Operace = (op1, op2) => op1 - op2, Popis = "Odcitani" };
            menu.Polozky['*'] = new PolazkaMenu() { Operace = (op1, op2) => op1 * op2, Popis = "Nasobeni" };
            menu.Polozky['/'] = new PolazkaMenu() { Operace = Podil, Popis = "Podil" };

            int a = NactiCislo("a");
            int b = NactiCislo("b");

            menu.Vykresli();

            var klavesa = Console.ReadKey();

            menu.Spust(klavesa.KeyChar, a, b);
        }

        static int Podil(int a, int b)
        {
            if (b != 0) return a / b;

            return 0;
        }

        private static int NactiCislo(string popis)
        {
            int a;
            Console.Write($"Zadej {popis}: ");
            int.TryParse(Console.ReadLine(), out a);
            return a;
        }

    }

    public delegate int Operace(int a, int b);

    public class PolazkaMenu
    {
        public char Zkratka { get; set; }

        public string Popis { get; set; }

        public Operace Operace { get; set; }
    }

    public class Menu
    {
        public Dictionary<char, PolazkaMenu> Polozky { get; set; } = new Dictionary<char, PolazkaMenu>();

        public void Vykresli()
        {
            foreach(var kv in Polozky)
            {
                Console.WriteLine($"{kv.Key} - {kv.Value.Popis}");
            }
        }

        public bool Spust(char operace, int a, int b)
        {
            if (Polozky.ContainsKey(operace))
            {
                Console.WriteLine("Vysledek = " + Polozky[operace].Operace(a, b));

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
