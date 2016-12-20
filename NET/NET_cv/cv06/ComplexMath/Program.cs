using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplexMath
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }

    public class Complex
    {
        public double Re { get; set; }
        public double Im { get; set; }

        public double Abs
        {
            get
            {
                return Math.Sqrt(Re * Re + Im * Im);
            }
        }
    }
}
