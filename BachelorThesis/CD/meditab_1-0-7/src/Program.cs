using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using Databaze;

namespace MediTab
{
    static class Program
    {

        /// <summary>
        /// Trida pristupu k databazi
        /// </summary>
        public static IDtb databaze;

        /// <summary>
        /// Filtr eventu pro dotykovou obrazovku
        /// </summary>
        public static MouseFilter mouseFilter;

        /*
        private Diureza diureza;
        private InvazivniPristupiy invPristupy;
        */
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //nastaveni vyssi priority pro rychlejsi beh programu
            System.Diagnostics.Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.RealTime;

            //init loggeru
            log4net.Config.XmlConfigurator.Configure();

            //nastaveni oddelovaciho desetinneho znaku na tecku
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            //pracoviste, kde se zarizeni nachazi
            String pracoviste = ConfigurationManager.AppSettings["pracoviste"];
            //init tridy pristupu k databazi
            databaze = new Dtb(pracoviste);

            //Filtr eventu pro dotykovou obrazovku
            mouseFilter = new MouseFilter();
            Application.AddMessageFilter(mouseFilter);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //spusteni Main Formu
            Application.Run(new Main_Form());
        }
    }
}
