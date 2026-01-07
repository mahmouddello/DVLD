using System;
using System.Windows.Forms;
using dotenv.net;
using DVLD.PresentationLayer.Globals;
using DVLD.PresentationLayer.People;
using DVLD.PresentationLayer.Users;

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
            frmListUsers form = new frmListUsers();
            form.MdiParent = this;
            form.Show();
        }

        private void currentUserInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUserDetails frm = new frmUserDetails(SharedGlobals.CurrentUser);
            frm.ShowDialog();
        }

        private void signOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SharedGlobals.Logout();
            this.Hide(); // hide main form

            // Show login dialog first
            frmLoginScreen loginForm = new frmLoginScreen();

            if (loginForm.ShowDialog() == DialogResult.OK)
                this.Show(); // unhide after successfull login
        }
    }
}
