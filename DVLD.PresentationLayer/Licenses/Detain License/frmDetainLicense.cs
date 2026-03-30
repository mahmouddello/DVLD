using System;
using System.Windows.Forms;
using DVLD.BusinessLayer;
using DVLD.EntityLayer;
using DVLD.PresentationLayer.GlobalClasses;

namespace DVLD.PresentationLayer.Licenses.Detain_License
{
    public partial class frmDetainLicense : Form
    {
        private License _license;

        public frmDetainLicense()
        {
            InitializeComponent();
        }

        private void ResetUI()
        {
            // Set all labels in Application Info Groupbox to "???"
            foreach (Control ctrl in gbDetainInfo.Controls)
                if (ctrl is Label lbl && lbl.Name.StartsWith("lbl"))
                    lbl.Text = "???";

            lnkLicenseHistory.Enabled = false;
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int licenseId)
        {
            bool flowControl = HandleLicenseSelection(licenseId);

            if (!flowControl)
            {
                btnDetain.Enabled = false;
                gbDetainInfo.Enabled = false;
                return;
            }

            btnDetain.Enabled = true;
            lnkLicenseHistory.Enabled = true;
            gbDetainInfo.Enabled = true;
            txtFees.Clear();
            txtFees.Focus();
        }

        private bool HandleLicenseSelection(int licenseId)
        {
            // Delegate return an id smaller than 0 when the license id is invalid
            if (licenseId <= 0)
            {
                ResetUI();
                return false;
            }

            lnkLicenseHistory.Enabled = true;
            _license = LicenseService.FindById(licenseId);


            if (_license.IsDetained)
            {
                Utility.ShowErrorMessage("This license is detained right now");
                return false;
            }

            if (!_license.IsActive)
            {
                Utility.ShowErrorMessage("The License you're trying to detain isn't active");
                return false;
            }

            if (_license.IsExpired)
            {
                Utility.ShowErrorMessage("The License you're trying to detain is expired");
                return false;
            }


            // fill detain info
            lblDetainDate.Text = DateTime.Now.ToShortDateString();
            lblLicenseId.Text = _license.Id.ToString();
            lblCreatedBy.Text = Globals.CurrentUser.Username;
            return true;
        }

        private void txtFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
                return;

            if (!char.IsDigit(e.KeyChar))
            {
                Utility.HandleWrongKey(e);
                return;
            }
        }

        private void btnDetain_Click(object sender, EventArgs e)
        {
            int detainId = -1;

            if (!DetainLicense(ref detainId))
            {
                Utility.ShowErrorMessage("Failed to detain the license");
                return;
            }

            Utility.ShowSuccessMessage("Detained the license Successfully");
            UpdateUIAfterDetain(detainId);
        }

        private bool DetainLicense(ref int detainId)
        {
            if (string.IsNullOrWhiteSpace(txtFees.Text.Trim()))
            {
                Utility.ShowWarningMessage("Please enter fee amount", "Specify fee amount");
                return false;
            }

            if (!(decimal.TryParse(txtFees.Text.Trim(), out decimal fees)))
            {
                Utility.ShowErrorMessage("Fine value couldn't convert to number");
                return false;
            }

            LicenseService service = new LicenseService(_license);
            bool isDetained = service.Detain(fees, Globals.CurrentUser.Id, ref detainId);

            return isDetained;
        }

        private void UpdateUIAfterDetain(int detainId)
        {
            lblDetainId.Text = detainId.ToString();
            
            ctrlDriverLicenseInfoWithFilter1.Enabled = false;
            gbDetainInfo.Enabled = false;
            btnDetain.Enabled = false;
            lnkLicenseHistory.Enabled = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lnkLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Person person = _license.MainApplicationInfo.ApplicantPersonInfo;

            frmShowLicenseHistory form = new frmShowLicenseHistory(person.Id);
            form.ShowDialog();
        }

        private void lnkNewLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowLicenseInfo form = frmShowLicenseInfo.CreateByLicenseId(_license.Id);
            form.ShowDialog();
        }
    }
}
