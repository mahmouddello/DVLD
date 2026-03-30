using DVLD.BusinessLayer;
using DVLD.EntityLayer;
using DVLD.PresentationLayer.GlobalClasses;
using System;
using System.Windows.Forms;

namespace DVLD.PresentationLayer.Licenses.Detain_License
{
    public partial class frmReleaseDetainedLicense : Form
    {
        private License _license;
        private ApplicationType _releaseApplicationType;
        private DetainLicense _detainRecord;

        public frmReleaseDetainedLicense()
        {
            InitializeComponent();
        }

        public frmReleaseDetainedLicense(License license)
        {
            InitializeComponent();
            _license = license;
        }

        private void frmReleaseDetainedLicense_Load(object sender, EventArgs e)
        {
            // Cache in form launch
            _releaseApplicationType = ApplicationTypeService.FindByType(enApplicationType.ReleaseDetainedLicense);

            if (_license != null)
            {
                ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
                ctrlDriverLicenseInfoWithFilter1.FilterText = _license.Id.ToString();
                ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(_license.Id);
            }
        }

        private void ResetUI()
        {
            // Set all labels in Application Info Groupbox to "???"
            foreach (Control ctrl in gbDetainInfo.Controls)
                if (ctrl is Label lbl && lbl.Name.StartsWith("lbl"))
                    lbl.Text = "???";

            lnkLicenseHistory.Enabled = false;
        }

        private bool HandleLicenseSelection(int licenseId)
        {
            // Delegate return an id smaller than 0 when the license id is invalid
            if (licenseId <= 0)
            {
                ResetUI();
                return false;
            }

            if (_license == null)
                _license = LicenseService.FindById(licenseId);
            else
                ctrlDriverLicenseInfoWithFilter1.ctrlDriverLicenseInfo1.LoadLicenseByLicenseId(_license.Id);

            lnkLicenseHistory.Enabled = true;

            if (!_license.IsDetained)
            {
                Utility.ShowErrorMessage("This license is not detained at the moment");
                return false;
            }

            return true;
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int licenseId)
        {
            bool flowControl = HandleLicenseSelection(licenseId);

            if (!flowControl)
            {
                btnRelease.Enabled = false;
                gbDetainInfo.Enabled = false;
                return;
            }

            // fetch detain record by the license Id
            _detainRecord = DetainLicenseService.FindByLicenseId(licenseId);

            if (_detainRecord == null)
            {
                Utility.ShowErrorMessage("Failed to find the detain record");
                return;
            }

            lblDetainId.Text = _detainRecord.Id.ToString();
            lblCreatedBy.Text = UserService.FindById(_detainRecord.CreatedByUserId).Username;
            lblDetainDate.Text = _detainRecord.DetainDate.ToShortDateString();
            lblLicenseId.Text = _detainRecord.LicenseId.ToString();
            lblFineFees.Text = _detainRecord.FineFees.ToString();
            lblApplicationFees.Text = _releaseApplicationType.Fees.ToString();
            lblTotalFees.Text = $"{_releaseApplicationType.Fees + _detainRecord.FineFees}";

            btnRelease.Enabled = true;
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

        private void btnRelease_Click(object sender, EventArgs e)
        {
            decimal applicationFees = _releaseApplicationType.Fees;

            LicenseService service = new LicenseService(_license);
            bool isReleased = service.Release(applicationFees, Globals.CurrentUser.Id, ref _detainRecord);

            if (!isReleased)
            {
                Utility.ShowErrorMessage("Failed to release the license");
                btnRelease.Enabled = false;
                return;
            }

            Utility.ShowSuccessMessage($"Released the license successfully with application id: {_detainRecord.ReleaseApplicationId}");
            UpdateUIAfterRelease(_detainRecord.ReleaseApplicationId);
        }

        private void UpdateUIAfterRelease(int applicationId)
        {
            lblReleaseApplicationId.Text = applicationId.ToString();
            ctrlDriverLicenseInfoWithFilter1.Enabled = false;
            gbDetainInfo.Enabled = false;
            btnRelease.Enabled = false;
            lnkLicenseHistory.Enabled = true;
        }

    }
}
