using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosModul
{
    public class CosModul : Main.IModul
    {
        [Description("Nasobici konstanta")]
        [Category("MulCat")]
        public double Mult { get; set; } = 1;

        public double Funkce(double x)
        {
            return Mult * Math.Cos(x);
        }
    }
}
