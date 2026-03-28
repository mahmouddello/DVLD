using DVLD.BusinessLayer;
using DVLD.EntityLayer;
using DVLD.PresentationLayer.GlobalClasses;
using DVLD.PresentationLayer.Licenses;
using System;
using System.Linq;
using System.Windows.Forms;
using ATS = DVLD.BusinessLayer.ApplicationTypeService;

namespace DVLD.PresentationLayer.Applications.Replacement
{
    public partial class frmReplacementApplication : Form
    {
        private int _oldLicenseId;
        private License _oldLicense;
        private ApplicationType _applicationType;
        private enLicenseIssueReason _replacementReason;

        // @Cache
        private ApplicationType _damagedType;
        private ApplicationType _lostType;

        public frmReplacementApplication()
        {
            InitializeComponent();
        }

        private void ResetUI()
        {
            // Set all labels in Application Info Groupbox to "???"
            foreach (Control ctrl in gbApplicationInfo.Controls)
                if (ctrl is Label lbl && lbl.Name.StartsWith("lbl"))
                    lbl.Text = "???";

            lnkLicenseHistory.Enabled = false;
        }

        private void frmReplacementApplication_Load(object sender, EventArgs e)
        {
            // Cache variables once
            _damagedType = ATS.FindByType(enApplicationType.ReplaceDamagedDrivingLicense);
            _lostType = ATS.FindByType(enApplicationType.ReplaceLostDrivingLicense);

            // Explicitly set - Default Type
            SetReplacementType(enApplicationType.ReplaceDamagedDrivingLicense);
            rbDamaged.Checked = true;
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int licenseId)
        {
            bool flowControl = HandleLicenseSelection(licenseId);

            if (!flowControl)
            {
                btnReplace.Enabled = false;
                lnkLicenseHistory.Enabled =false;
                return;
            }

            btnReplace.Enabled = true;
            lnkLicenseHistory.Enabled = true;
        }

        private bool HandleLicenseSelection(int licenseId)
        {
            _oldLicenseId = licenseId;

            // Delegate return an id smaller than 0 when the license id is invalid
            if (_oldLicenseId <= 0)
            {
                ResetUI();
                return false;
            }

            _oldLicense = ctrlDriverLicenseInfoWithFilter1.SelectedLicense;

            if (!_oldLicense.IsActive)
            {
                Utility.ShowErrorMessage("License you're trying to replace isn't active");
                return false;
            }

            // Fill application Info
            lblApplicationDate.Text = DateTime.Now.ToShortDateString();
            lblOldLicenseId.Text = _oldLicense.Id.ToString();
            lblCreatedBy.Text = Globals.CurrentUser.Username;
            return true;
        }

        private bool HasSelectedRadioButton(GroupBox groupBox)
        {
            return groupBox.Controls
                           .OfType<RadioButton>()
                           .Any(r => r.Checked);
        }

        private void SetReplacementType(enApplicationType type)
        {
            bool isDamaged = type == enApplicationType.ReplaceDamagedDrivingLicense;

            _applicationType = isDamaged ? _damagedType : _lostType;
            _replacementReason = isDamaged
                ? enLicenseIssueReason.ReplacementForDamaged
                : enLicenseIssueReason.ReplacementForLost;

            lblTitle.Text = isDamaged
                ? "Replacement for damaged license"
                : "Replacement for lost license";

            lblApplicationFees.Text = _applicationType.Fees.ToString();
        }

        private void rbDamaged_CheckedChanged(object sender, EventArgs e)
        {
            SetReplacementType(enApplicationType.ReplaceDamagedDrivingLicense);
        }

        private void rbLost_CheckedChanged(object sender, EventArgs e)
        {
            SetReplacementType(enApplicationType.ReplaceLostDrivingLicense);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lnkLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var person = ctrlDriverLicenseInfoWithFilter1.SelectedLicense.MainApplicationInfo.ApplicantPersonInfo;

            frmShowLicenseHistory form = new frmShowLicenseHistory(person.Id);
            form.ShowDialog();
        }

        private void UpdateUIAfterReplacement(License newLicense)
        {
            lblLRApplicationId.Text = newLicense.ApplicationId.ToString();
            lblNewLicenseId.Text = newLicense.Id.ToString();
            btnReplace.Enabled = false;
            gbApplicationInfo.Enabled = false;

            // New license id in tag to avoid global variable or params
            lnkNewLicenseInfo.Tag = newLicense.Id;
            lnkNewLicenseInfo.Enabled = true;
        }

        private void lnkNewLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (int.TryParse(lnkNewLicenseInfo.Tag.ToString(), out int licenseId))
            {
                var form = frmShowLicenseInfo.CreateByLicenseId(licenseId);
                form.ShowDialog();
            }
            else
                Utility.ShowErrorMessage("The new license id is invalid to process");
        }

        private void ReplaceLicense()
        {
            LicenseService service = new LicenseService(_oldLicense);
            License newLicense = service.Replace(_replacementReason, Globals.CurrentUser.Id);

            if (newLicense == null)
            {
                Utility.ShowErrorMessage($"Failed to issue a replacement for license with id: {_oldLicense.Id}");
                return;
            }

            Utility.ShowSuccessMessage(
                $"Issued a replacement for license with id: {_oldLicense.Id} successfully." +
                $"\nYour new license id: {newLicense.Id}"
            );
            UpdateUIAfterReplacement(newLicense);
        }

        private void btnReplace_Click(object sender, EventArgs e)
        {
            if (!HasSelectedRadioButton(gbReplacementReason))
            {
                Utility.ShowErrorMessage("You must select the replacement reason");
                gbReplacementReason.Focus();
                return;
            }

            ReplaceLicense();
        }
    }
}
