using System;
using System.IO;
using System.Windows.Forms;
using DVLD.PresentationLayer.Properties;
using DVLD.BusinessLayer;
using DVLD.EntityLayer;
using DVLD.PresentationLayer.GlobalClasses;

namespace DVLD.PresentationLayer.Licenses.Controls
{
    public partial class ctrlDriverLicenseInfo : UserControl
    {
        private LDLA ldla;
        private License license;

        // exposed properties
        public int LicenseId => license?.Id ?? -1;
        public License SelectedLicense => license ?? null;

        public ctrlDriverLicenseInfo()
        {
            InitializeComponent();
        }

        public void LoadLicenseByLocalAppId(int ldlaId)
        {
            // 1. fetch the main application id through the local application
            ldla = LDLAService.FindById(ldlaId);

            if (ldla == null)
            {
                Utility.ShowErrorMessage("Failed to load the application that is associated with this license");
                return;
            }

            // 2. fetch the license id that is associated with the main application id
            license = LicenseService.FindByApplicationId(ldla.MainApplicationId);

            if (license == null)
            {
                Utility.ShowErrorMessage("Failed to find the license that is associated with this application");
                return;
            }

            // 3. Display the info
            FillFormInfo();
        }

        public void LoadLicenseByLicenseId(int licenseId)
        {
            // 1. fetch license
            license = LicenseService.FindById(licenseId);

            if (license == null)
            {
                Utility.ShowErrorMessage("Failed to find the license with id: " + licenseId);
                ResetFormInfo();
                return;
            }

            // 2. Display the info
            FillFormInfo();
        }

        private void ResetFormInfo()
        {
          
            lblClass.Text = "???";
            lblName.Text = "???";
            lblLicenseID.Text = "???";
            lblNationalNo.Text = "???";
            lblGender.Text = "???";
            lblIssueDate.Text = "???";
            lblIssueReason.Text = "???";
            lblNotes.Text = "???";
            lblIsActive.Text = "???";
            lblDateOfBirth.Text = "???";
            lblExpirationDate.Text = "???";
            lblDriverID.Text = "???";
            lblIsDetained.Text = "???";
        }

        private string GetIssueReason(enLicenseIssueReason issueReason)
        {
            switch(issueReason)
            {
                case enLicenseIssueReason.FirstTime:
                    return "First Time";

                case enLicenseIssueReason.Renew:
                    return "Renew";

                case enLicenseIssueReason.ReplacementForLost:
                    return "Replacement for lost";

                case enLicenseIssueReason.ReplacementForDamaged:
                    return "Replacement for damaged";

                default:
                    return null;
            }
        }

        private void FillFormInfo()
        {
            var person = license.MainApplicationInfo.ApplicantPersonInfo;

            lblClass.Text = license.LicenseClassInfo.Name;
            lblName.Text = person.FullName;
            lblLicenseID.Text = license.Id.ToString();
            lblNationalNo.Text = person.NationalNo;
            lblGender.Text = person.Gender.ToString();
            lblIssueDate.Text = license.IssueDate.ToShortDateString();
            lblIssueReason.Text = GetIssueReason(license.IssueReason);
            lblNotes.Text = license.Notes ?? "-"; // if it's not null, left value, null: right value '-'

            lblIsActive.Text = license.IsActive ? "Active" : "Inactive";
            lblDateOfBirth.Text = person.DateOfBirth.ToShortDateString();
            lblExpirationDate.Text = license.ExpirationDate.ToShortDateString();
            lblDriverID.Text = license.DriverId.ToString();
            // implement is detained

            LoadPersonImage();
        }

        private void LoadPersonImage()
        {
            var person = license.MainApplicationInfo.ApplicantPersonInfo;
            string fullImagePath = Path.Combine(Globals.ImagesRootDirectory, person.ImagePath);

            if (!string.IsNullOrWhiteSpace(fullImagePath) && File.Exists(fullImagePath))
            {
                pbImage.ImageLocation = fullImagePath;
                return;
            }

            if (person.Gender == Gender.Male)
                pbImage.Image = Resources.driverMale;
            else
                pbImage.Image = Resources.driverFemale;
        }
    }
}
