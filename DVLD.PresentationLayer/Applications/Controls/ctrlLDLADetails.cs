using DVLD.PresentationLayer.People;
using System;
using System.Windows.Forms;
using DVLD.BusinessLayer;
using DVLD.EntityLayer;
using DVLD.PresentationLayer.Licenses;

namespace DVLD.PresentationLayer.Applications.Controls
{
    public partial class ctrlLDLADetails : UserControl
    {
        private LDLA ldla;

        public ctrlLDLADetails()
        {
            InitializeComponent();
        }

        public void LoadApplicationInfo(int _ldlaId)
        {
            ldla = LDLAService.FindById(_ldlaId);

            if (ldla == null)
            {
                ResetApplicationInfo();
                MessageBox.Show(
                    "No details found for ldla id = " + _ldlaId.ToString(),
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            FillApplicationInfo();
        }

        private void FillApplicationInfo()
        {
            lblLdlaId.Text = ldla.Id.ToString();
            lblAppliedFor.Text = LicenseClassService.FindById(ldla.LicenseClassId)?.Name;

            int passedTests = TestService.GetPassedTestCount(ldla.Id);
            lblPassedTests.Text = $"{passedTests}/3";

            lblMainApplicationId.Text = ldla.MainApplicationId.ToString();
            lblStatus.Text = ldla.MainApplicationInfo.Status.ToString();
            lblFees.Text = ldla.MainApplicationInfo.PaidFees.ToString();
            lblType.Text = ldla.MainApplicationInfo.ApplicationTypeInfo.Title;
            lblApplicant.Text = ldla.MainApplicationInfo.ApplicantPersonInfo.FullName;
            lblDate.Text = ldla.MainApplicationInfo.ApplicationDate.ToShortDateString();
            lblLastStatusDate.Text = ldla.MainApplicationInfo.LastStatusDate.ToShortDateString();
            lblCreatedBy.Text = ldla.MainApplicationInfo.CreatorUserInfo.Username;

            var associatedLicense = LicenseService.FindByApplicationId(ldla.MainApplicationId);
            lnklblShowLicenseInfo.Enabled = associatedLicense != null;
        }

        private void ResetApplicationInfo()
        {
            lblLdlaId.Text = "???";
            lblAppliedFor.Text = "???";
            lblPassedTests.Text = "???";
            lnklblShowLicenseInfo.Enabled = false;

            lblMainApplicationId.Text = "???";
            lblStatus.Text = "???";
            lblFees.Text = "???";
            lblType.Text = "???";
            lblApplicant.Text = "???";
            lblDate.Text = "???";
            lblLastStatusDate.Text = "???";
            lblCreatedBy.Text = "???";
            lnklblShowPersonInfo.Enabled = false;
        }

        private void lnklblShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            int _ldlaId = Convert.ToInt32(lblLdlaId.Text);
            frmShowLicenseInfo.CreateByLdlaId(_ldlaId).ShowDialog();
        }

        private void lnklblShowPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmPersonDetails frm = new frmPersonDetails(ldla.MainApplicationInfo.ApplicantPersonId);
            frm.ShowDialog();
        }
    }
}
