using System;
using System.Windows.Forms;
using DVLD.BusinessLayer;
using DVLD.EntityLayer;
using DVLD.PresentationLayer.GlobalClasses;
using Application = DVLD.EntityLayer.Application;

namespace DVLD.PresentationLayer.Licenses
{
    public partial class frmIssueDrivingLicense : Form
    {
        private int _ldlaId;

        public frmIssueDrivingLicense(int ldlaId)
        {
            InitializeComponent();
            this._ldlaId = ldlaId;
        }

        private void frmIssueDrivingLicense_Load(object sender, EventArgs e)
        {
            this.ctrlApplicationDetails1.LoadApplicationInfo(_ldlaId);
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            License license = LicenseService.IssueDLFirstTime(_ldlaId, txtNotes.Text.Trim(), Globals.CurrentUser.Id);

            if (license != null)
                Utility.ShowSuccessMessage($"Issued the new license successfully with id: {license.Id}");
            else
                Utility.ShowErrorMessage("Failed to issue the license");

            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
