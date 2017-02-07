using System;
using System.Collections.Generic;
using System.Linq;
//using System.Threading.Tasks;
using System.Windows.Forms;

namespace UPS_Scrabble_client
{
    static class Program
    {
        /// <summary>
        /// Instance of Main Form
        /// </summary>
        public static Form_Main FormMain { get; set; }

        /// <summary>
        /// Instance of Game Form
        /// </summary>
        public static Form_Game FormGame { get; set; }

        /// <summary>
        /// Instance of game
        /// </summary>
        public static Game Game { get; set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(FormMain = new Form_Main());
        }
    }
}
