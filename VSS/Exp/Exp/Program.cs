using System;
using MathNet.Numerics.Distributions;

namespace Exp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 4) return;

            int n = int.Parse(args[0]);
            int lam1 = int.Parse(args[1]);
            int lam2 = int.Parse(args[2]);
            int p = int.Parse(args[3]);

            double[] rnd = new double[n];
            int[] rnd_int = new int[n];

            for (int i = 0; i < n; i++)
            {
                rnd[i] = Exponential.Sample(lam1);
                rnd_int[i] = (int)(rnd[i] * 1000);
                Console.WriteLine(rnd);
            }

            Accord.Statistics.Distributions.Univariate.ExponentialDistribution.Random(lam1);
            Accord.Math.ContinuousHistogram hist = new Accord.Math.ContinuousHistogram(rnd_int, new Accord.Range());
            Accord.Controls.HistogramBox.Show(rnd, "Hist");

            Console.ReadKey();
        }
    }
}
