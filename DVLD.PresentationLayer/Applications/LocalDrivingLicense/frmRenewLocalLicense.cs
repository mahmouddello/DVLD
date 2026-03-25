using System;
using System.Windows.Forms;
using DVLD.BusinessLayer;
using DVLD.EntityLayer;
using DVLD.PresentationLayer.GlobalClasses;
using DVLD.PresentationLayer.Licenses;
using ATS = DVLD.BusinessLayer.ApplicationTypeService;
using LCS = DVLD.BusinessLayer.LicenseClassService;

namespace DVLD.PresentationLayer.Applications.LocalDrivingLicense
{
    public partial class frmRenewLocalLicense : Form
    {
        private int _oldLicenseId;
        private License _oldLicense, _newLicense;

        public frmRenewLocalLicense()
        {
            InitializeComponent();
        }

        private void frmRenewLocalLicense_Load(object sender, EventArgs e)
        {
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = true;
        }

        private bool IsLicenseValidForRenewal(License license)
        {
            // 1. If license not expired, popup a warning message
            if (!license.IsExpired)
            {
                Utility.ShowWarningMessage
                (
                    $"This license will expire in: {license.ExpirationDate.ToShortDateString()}",
                    "License not expired yet"
                );
                return false;
            }

            // 2. Check if license is active
            if (!license.IsActive)
            {
                ResetUIElements();
                Utility.ShowWarningMessage
                (
                    $"This license isn't active",
                    "License not active"
                );
                lnkLicenseHistory.Enabled = true;
                return false;
            }

            return true;
        }

        private void ResetUIElements()
        {
            btnNext.Enabled = false;
            btnRenew.Enabled = false;
            tpApplicationInfo.Enabled = false;

            // Set all labels in Application Info tab to : ???
            foreach (Control ctrl in tpApplicationInfo.Controls)
                if (ctrl is Label lbl && lbl.Name.StartsWith("lbl"))
                    lbl.Text = "???";

            lnkLicenseHistory.Enabled = false;
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            _oldLicenseId = obj;

            // Delegate return an id smaller than 0 when the license id is invalid
            if (_oldLicenseId <= 0)
            {
                ResetUIElements();
                return;
            }

            _oldLicense = ctrlDriverLicenseInfoWithFilter1.SelectedLicense;
            lnkLicenseHistory.Enabled = true;

            if (!IsLicenseValidForRenewal(_oldLicense))
                return;

            btnNext.Enabled = true;
            FillApplicationTabInfo();
        }

        private void FillApplicationTabInfo()
        {
            // We fetch application type and license incase their fees did change 
            var applicationType = ATS.FindByType(enApplicationType.RenewDrivingLicense);
            var licenseClass = LCS.FindById(_oldLicense.LicenseClassID);

            decimal licenseFees = licenseClass.Fees;
            decimal applicationFees = applicationType.Fees;

            // general info
            lblRLApplicationId.Text = "???";
            lblNewLicenseId.Text = "???";
            lblApplicationDate.Text = DateTime.Now.ToShortDateString();
            lblIssueDate.Text = DateTime.Now.ToShortDateString();
            lblExpirationDate.Text = DateTime.Now.AddYears(licenseClass.DefaultValidityLength).ToShortDateString();
            lblOldLicenseId.Text = _oldLicenseId.ToString();
            lblCreatedBy.Text = Globals.CurrentUser.Username;

            // fees
            lblApplicationFees.Text = applicationFees.ToString();
            lblLicenseFees.Text = licenseFees.ToString();
            lblTotalFees.Text = (licenseFees + applicationFees).ToString();
        }

        private void lnkLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var person = ctrlDriverLicenseInfoWithFilter1.SelectedLicense.MainApplicationInfo.ApplicantPersonInfo;

            frmShowLicenseHistory form = new frmShowLicenseHistory(person.Id);
            form.ShowDialog();
        }

        private void lnkNewLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var form = frmShowLicenseInfo.CreateByLicenseId(_newLicense.Id);
            form.ShowDialog();
        }

        private void btnRenew_Click(object sender, EventArgs e)
        {
            LicenseService service = new LicenseService(_oldLicense);
            _newLicense = service.Renew(txtNotes.Text.Trim(), Globals.CurrentUser.Id);

            if (_newLicense == null)
            {
                Utility.ShowErrorMessage("Failed to renew the license");
                this.Close();
                return;
            }

            Utility.ShowSuccessMessage($"Renewed license successfully with id: {_newLicense.Id}");
            lblRLApplicationId.Text = _newLicense.ApplicationId.ToString();
            lblNewLicenseId.Text = _newLicense.Id.ToString();
            tpApplicationInfo.Enabled = false;
            lnkNewLicenseInfo.Enabled = true;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            tabControl.SelectedTab = tpApplicationInfo;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
