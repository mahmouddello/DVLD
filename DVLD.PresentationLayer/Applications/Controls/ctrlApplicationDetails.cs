using DVLD.BusinessLayer;
using DVLD.EntityLayer;
using DVLD.PresentationLayer.People;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.PresentationLayer.Applications.Controls
{
    public partial class ctrlApplicationDetails : UserControl
    {
        private LocalDrivingLicenseApplication ldla;

        public ctrlApplicationDetails()
        {
            InitializeComponent();
        }

        public void LoadApplicationInfo(int ldlaId)
        {
            ldla = LocalDrivingLicenseApplicationBusiness.Find(ldlaId);

            if (ldla == null)
            {
                ResetApplicationInfo();
                MessageBox.Show(
                    "No details found for ldla id = " + ldlaId.ToString(),
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
            lblAppliedFor.Text = LicenseClassBusiness.Find(ldla.LicenseClassId)?.Name;
            lblPassedTests.Text = $"{LocalDrivingLicenseApplicationBusiness.GetPassedTestCountById(ldla.Id)}/3";

            lblMainApplicationId.Text = ldla.MainApplicationId.ToString();
            lblStatus.Text = ldla.MainApplicationInfo.Status.ToString();
            lblFees.Text = ldla.MainApplicationInfo.PaidFees.ToString();
            lblType.Text = ldla.MainApplicationInfo.ApplicationTypeInfo.Title;
            lblApplicant.Text = ldla.MainApplicationInfo.ApplicantPersonInfo.FullName;
            lblDate.Text = ldla.MainApplicationInfo.Date.ToShortDateString();
            lblLastStatusDate.Text = ldla.MainApplicationInfo.LastStatusDate.ToShortDateString();
            lblCreatedBy.Text = ldla.MainApplicationInfo.CreatorUserInfo.Username;

            lnklblShowPersonInfo.Enabled = true;
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
            MessageBox.Show
            (
                "This feature will be implemented in the future",
                "Stub",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
        }

        private void lnklblShowPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmPersonDetails frm = new frmPersonDetails(ldla.MainApplicationInfo.ApplicantPersonId);
            frm.ShowDialog();
        }
    }
}
