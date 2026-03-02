using System;
using System.Windows.Forms;
using DVLD.PresentationLayer.Applications;
using DVLD.PresentationLayer.Applications.LocalDrivingLicense;
using DVLD.PresentationLayer.ApplicationTypes;
using DVLD.PresentationLayer.Drivers;
using DVLD.PresentationLayer.GlobalClasses;
using DVLD.PresentationLayer.People;
using DVLD.PresentationLayer.Tests.TestTypes;
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
            frmListDrivers form = new frmListDrivers();
            form.MdiParent = this;

            form.Show();
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListUsers form = new frmListUsers();
            form.MdiParent = this;
            form.Show();
        }

        private void currentUserInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUserDetails frm = new frmUserDetails(Globals.CurrentUser);
            frm.ShowDialog();
        }

        private void signOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utility.Logout();
            this.Hide(); // hide main form

            // Show login dialog first
            frmLoginScreen loginForm = new frmLoginScreen();

            if (loginForm.ShowDialog() == DialogResult.OK)
                this.Show(); // unhide after successfull login
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmChangeUserPassword frm = new frmChangeUserPassword(Globals.CurrentUser);
            frm.ShowDialog();
        }

        private void manageApplicationTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmApplicationTypes frm = new frmApplicationTypes();
            frm.ShowDialog();
        }

        private void manageTestTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListTestTypes frm = new frmListTestTypes();
            frm.ShowDialog();
        }

        private void localLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddEditLocalDLA frm = new frmAddEditLocalDLA();
            frm.ShowDialog();
        }

        private void localDrivingLicenseApplicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListLocalDLA frm = new frmListLocalDLA();
            frm.ShowDialog();
        }
    }
}
