using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ucetnictvi
{
    public enum Jednotky { Ks, Kg, l};

    public class Faktura
    {
        public List<PolozkaFaktury> Polozky { get; set; } = new List<PolozkaFaktury>();
        public double CelkovaCena => Polozky.Sum(p => p.CelkovaCena);
        //public double CelkovaCena { get { return Polozky.Sum(p => p.CelkovaCena); } }
        public DateTime DatumVystaveni { get; set; }
    }

    /// <summary>
    /// Faktura
    /// </summary>
    public class PolozkaFaktury
    {
        public string Polozka { get; set; }
        public Jednotky Jednotky { get; set; }
        public double CenaMJ { get; set; }
        public double Pocet { get; set; }
        public double DPH
        {
            get { return CenaMJ * 0.21 * Pocet; }
        }
        public double CelkovaCena
        {
            get { return CenaMJ * Pocet * 1.21; }
        }


        public override string ToString()
        {
            return $"{Polozka}\n{Pocet}\t{Jednotky}\t{CenaMJ}\t{DPH:C}\t{CelkovaCena}";
        }

        /// <summary>
        /// Inflace
        /// </summary>
        public void Inflace(double kolik)
        {

        }
    }
}
