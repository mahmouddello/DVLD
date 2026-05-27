using System;
using System.Windows.Forms;

namespace DVLD.PresentationLayer
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

            // Show login dialog first
            frmLoginScreen loginForm = new frmLoginScreen();

            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                // Login successful, run main form as the application form
                Application.Run(new frmMain());
            }
        }
    }
}
