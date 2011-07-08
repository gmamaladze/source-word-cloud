using System;
using System.Windows.Forms;

namespace Gma.CodeCloud
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            MainForm mainForm;
            if (args != null && args.Length > 0 && !String.IsNullOrEmpty(args[0]))
            {
                mainForm = new MainForm(args[0]);
            } 
            else
            {
                mainForm = new MainForm();
            }
            Application.Run(mainForm);
        }
    }
}
