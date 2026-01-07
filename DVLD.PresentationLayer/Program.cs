using dotenv.net;
using DVLD.PresentationLayer.Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            // load .env once at application startup
            LoadDotEnv();

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

        private static void LoadDotEnv()
        {
            try
            {
                string envPath = "../../../.env";
                var options = new DotEnvOptions(
                    envFilePaths: new[] { envPath },
                    overwriteExistingVars: true
                );
                DotEnv.Load(options);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to load .env file: {ex.Message}",
                    "Configuration Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                Application.Exit();
            }
        }

    }
}
