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

            // Check 1: Not Ordinary License
            if (!IsOrdinaryLicense(license))
            {
                Utility.ShowWarningMessage(
                   message: "Only \"Ordinary Driving License\" is accepted",
                   title: "License isn't eligible"
                );
                ResetUIElements();
                return;
            }

            // TODO: Check 2: Check if the license id, has an international license issued already

            btnIssue.Enabled = true;
            ctrlIntlLicenseDetails1.Enabled = true;
            ctrlIntlLicenseDetails1.FillInfo(_licenseId);
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {

        }
    }
}
