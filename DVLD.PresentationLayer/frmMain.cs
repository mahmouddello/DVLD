using System;
using System.Windows.Forms;
using dotenv.net;
using DVLD.PresentationLayer.People;

namespace DVLD.PresentationLayer
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void peopleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListPeople form = new frmListPeople();
            form.MdiParent = this;
            form.Show();
        }

        private void driversToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show
            (
                "This feature will be implemented in the future",
                "Stub",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show
            (
                "This feature will be implemented in the future",
                "Stub",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
        }

        private void _LoadDotEnv()
        {
            // Load .env once
            string envPath = "../../../.env";
            var options = new DotEnvOptions(envFilePaths: new[] { envPath }, overwriteExistingVars: true);
            DotEnv.Load(options);
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            _LoadDotEnv();
        }
    }
}
