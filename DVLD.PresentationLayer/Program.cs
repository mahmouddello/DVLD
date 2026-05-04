using System;
using System.Diagnostics;
using System.Windows.Forms;
using DVLD.Infrastructure;
using DVLD.PresentationLayer.GlobalClasses;
using dotenv.net;


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
                Utility.ShowErrorMessage("An error occured when loading .env file, check log for full information");
                Logger.Log("Failed to load .env file in DVLD", EventLogEntryType.Error, ex, nameof(LoadDotEnv));

                Application.Exit();
            }
        }

    }
}
