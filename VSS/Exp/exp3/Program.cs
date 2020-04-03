using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace exp3
{
    class Program
    {
        static void Main(string[] args)
        {

            if (args.Length != 4)
            {
                Console.WriteLine("Wrong parameters input (required 4 params: n, lambda1, lambda2, p)");
                return;
            }

            int n = int.Parse(args[0]);
            double lam1 = double.Parse(args[1]);
            double lam2 = double.Parse(args[2]);
            double p = double.Parse(args[3]);

            double Et = p * 1 / lam1 + (1 - p) * 1 / lam2;
            double Dt = p * 1 / Math.Pow(lam1, 2) + (1 - p) * Math.Pow(lam2, 2);
            double Ev = 0;
            double Dv = 0;

            var r = new Random();

            double[] rnd = new double[n];
            int[] rnd_int = new int[n];

            for (int i = 0; i < n; i++)
            {
                if(r.NextDouble() < p)
                {
                    rnd[i] = Accord.Statistics.Distributions.Univariate.ExponentialDistribution.Random(lam1);
                    Ev += rnd[i];
                    Dv += Math.Pow(rnd[i] - Ev, 2) * p;
                }
                else
                {
                    rnd[i] = Accord.Statistics.Distributions.Univariate.ExponentialDistribution.Random(lam2);
                    Ev += rnd[i];
                    Dv += Math.Pow(rnd[i] - Ev, 2) * (1-p);
                }
            }

            Ev = Ev / n;
            Dv = Math.Sqrt(Dv);

            Console.WriteLine($"{Et}\n{Dt}\n{Ev}\n{Dv}");
            
            Accord.Controls.HistogramBox.Show(rnd, "Hist");
        }

    }
}
