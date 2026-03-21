using DVLD.BusinessLayer;
using DVLD.EntityLayer;
using DVLD.PresentationLayer.GlobalClasses;
using System;
using System.Windows.Forms;

namespace DVLD.PresentationLayer.Licenses.International_Licenses
{
    public partial class frmInternationalLicenseApplication : Form
    {
        private int _licenseId;
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
                return false;
            }

            // Check 2: Verify if an active, non expired international license already exists for this local license
            InternationalLicense internationalLicense = InternationalLicenseService.GetByLicenseId(license.Id);

            bool existsAndActive = internationalLicense != null && internationalLicense.IsActive;
            if (existsAndActive && !internationalLicense.IsExpired)
            {
                Utility.ShowWarningMessage(
                     message: $"An International Licenese with id ({internationalLicense.Id}) exists and active",
                     title: "International License Exists"
                );
                ResetUIElements();
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
                return;
            }

            License license = LicenseService.FindById(_licenseId);

            if (!IsLicenseValidForInternational(license))
                return;          

            btnIssue.Enabled = true;
            ctrlIntlLicenseDetails1.Enabled = true;
            ctrlIntlLicenseDetails1.FillInfo(_licenseId);
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            Utility.ShowWarningMessage("Not Implemented Yet!", "In Progress...");
        }
    }
}
