using System;
using System.Windows.Forms;

namespace Task_1_2__template
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new formWordAutomatisationPart2());
        }
    }
}
