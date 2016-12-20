using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinModul
{
    public class SinModul : Main.IModul
    {
        public double Funkce(double x)
        {
            return Math.Sin(x);
        }
    }

    public class Garbage
    {

    }
}
