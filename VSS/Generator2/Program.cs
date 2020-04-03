using System;

namespace Generator2
{
    class Program
    {
        


        static void Main(string[] args)
        {
            long c = 12345;
            long a = 10103515245;
            double m =Math.Pow(2, 31); 

            double y = 1;
            for(int i = 0; i < 1000; i++)
            {
                y = (a * y +c) % m;
                Console.WriteLine($"{y}");
            }
            Console.ReadLine();
        }
    }
}
