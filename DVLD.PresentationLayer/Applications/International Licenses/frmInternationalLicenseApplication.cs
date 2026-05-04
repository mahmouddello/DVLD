using System;
using System.Windows.Forms;
using DVLD.BusinessLayer;
using DVLD.EntityLayer;
using DVLD.PresentationLayer.GlobalClasses;
using ILS = DVLD.BusinessLayer.InternationalLicenseService;

namespace DVLD.PresentationLayer.Licenses.International_Licenses
{
    public partial class frmInternationalLicenseApplication : Form
    {
        private int _licenseId;
        private License _license;

        public frmInternationalLicenseApplication()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmInternationalLicenseApplication_Load(object sender, EventArgs e)
        {
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = true;
        }

        private void ResetUIElements()
        {
            ctrlIntlLicenseDetails1.ResetInfo();
            ctrlIntlLicenseDetails1.Enabled = false;
            btnIssue.Enabled = false;
        }

        private bool IsOrdinaryLicense(License license)
        {
            return (enLicenseClass)license.LicenseClassID == enLicenseClass.C3_Ordinary;
        }

        private bool IsLicenseValidForInternational(License license)
        {
            // Check 1: Not Ordinary License
            if (!IsOrdinaryLicense(license))
            {
                Utility.ShowWarningMessage(
                   message: "Only \"Ordinary Driving License\" is accepted",
                   title: "License isn't eligible"
                );
                ResetUIElements();
                lnkLicenseHistory.Enabled = true;
                return false;
            }

            // Check 2: Active non-expired international license already exists
            if (ILS.ExistsActiveByLocalLicenseId(license.Id))
            {
                Utility.ShowWarningMessage(
                     message: "An active international license already exists for this local license",
                     title: "International License Exists"
                );
                ResetUIElements();
                lnkLicenseHistory.Enabled = true;
                return false;
            }

            return true;
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            _licenseId = obj;

            // Invalid license id
            if (_licenseId <= 0)
            {
                ResetUIElements();
                lnkLicenseHistory.Enabled = false;
                return;
            }

            _license = LicenseService.FindById(_licenseId);

            if (!IsLicenseValidForInternational(_license))
                return;          

            btnIssue.Enabled = true;
            ctrlIntlLicenseDetails1.Enabled = true;
            ctrlIntlLicenseDetails1.FillInfo(_licenseId);
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            int intlLicenseValidityLength = Globals.IntrLicenseValidityLength;
            var intlLicense = ILS.IssueInternationalLicense(_license, Globals.CurrentUser.Id, intlLicenseValidityLength);

            if (intlLicense != null)
            {
                Utility.ShowSuccessMessage($"Issued the new international license with id: {intlLicense.Id} successfully");
                ctrlIntlLicenseDetails1.IntlAppId = intlLicense.ApplicationId.ToString();
                ctrlIntlLicenseDetails1.IntLicenseId = intlLicense.Id.ToString();
            }
            else
                Utility.ShowErrorMessage("Failed to issue the license");


        }

        private void lnkLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Driver driver = DriverService.FindById(_license.DriverId);
            
            frmShowLicenseHistory frm = new frmShowLicenseHistory(driver.PersonId);
            frm.Show();
        }
    }
}
