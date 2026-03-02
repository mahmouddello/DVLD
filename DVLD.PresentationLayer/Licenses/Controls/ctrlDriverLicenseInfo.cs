using DVLD.BusinessLayer;
using DVLD.EntityLayer;
using DVLD.PresentationLayer.GlobalClasses;
using DVLD.PresentationLayer.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.PresentationLayer.Licenses.Controls
{
    public partial class ctrlDriverLicenseInfo : UserControl
    {
        private LocalDrivingLicenseApplication ldla;
        private clsLicense license;

        public ctrlDriverLicenseInfo()
        {
            InitializeComponent();
        }

        public void LoadLicenseByLocalAppId(int ldlaId)
        {
            // 1. fetch the main application id through the local application
            ldla = LocalDrivingLicenseApplicationBusiness.Find(ldlaId);

            if (ldla == null)
            {
                Utility.ShowErrorMessage("Failed to load the application that is associated with this license!");
                return;
            }

            // 2. fetch the license id that is associated with the main application id
            license = clsLicenseBusiness.FindByApplicationId(ldla.MainApplicationId);

            if (license == null)
            {
                Utility.ShowErrorMessage("Failed to find the license that is associated with this application!");
                return;
            }

            // 3. Display the info
            FillFormInfo();
        }

        public void LoadLicenseByLicenseId(int licenseId)
        {
            // 1. fetch license
            license = clsLicenseBusiness.FindByLicenseId(licenseId);

            if (license == null)
            {
                Utility.ShowErrorMessage("Failed to find the license with id: " + licenseId);
                return;
            }

            // 2. fetch the ldla that is associated with the license id
            ldla = LocalDrivingLicenseApplicationBusiness.FindByMainApplicationId(license.ApplicationId);

            if (ldla == null)
            {
                Utility.ShowErrorMessage("Failed to load the application that is associated with this license!");
                return;
            }

            // 3. Display the info
            FillFormInfo();
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
            enGender gender = ldla.MainApplicationInfo.ApplicantPersonInfo.Gender;
            DateTime dob = ldla.MainApplicationInfo.ApplicantPersonInfo.DateOfBirth;
            var driver = clsDriverBusiness.FindByPersonId(ldla.MainApplicationInfo.ApplicantPersonId);

            lblClass.Text = ldla.LicenseClassInfo.Name;
            lblName.Text = ldla.MainApplicationInfo.ApplicantPersonInfo.FullName;
            lblLicenseID.Text = license.Id.ToString();
            lblNationalNo.Text = ldla.MainApplicationInfo.ApplicantPersonInfo.NationalNo;
            lblGender.Text = gender.ToString();
            lblIssueDate.Text = license.IssueDate.ToShortDateString();
            lblIssueReason.Text = GetIssueReason(license.IssueReason);
            lblNotes.Text = license.Notes ?? "-"; // if it's not null, left value, null: right value '-'

            lblIsActive.Text = license.IsActive ? "Yes" : "No";
            lblDateOfBirth.Text = dob.ToShortDateString();
            lblExpirationDate.Text = license.ExpirationDate.ToShortDateString();
            lblDriverID.Text = driver.Id.ToString();
            // implement is detained

            LoadPersonImage();
        }

        private void LoadPersonImage()
        {
            var person = ldla.MainApplicationInfo.ApplicantPersonInfo;
            string fullImagePath = Path.Combine(Globals.ImagesRootDirectory, person.ImagePath);

            if (!string.IsNullOrWhiteSpace(fullImagePath) && File.Exists(fullImagePath))
            {
                pbImage.ImageLocation = fullImagePath;
                return;
            }

            if (person.Gender == enGender.Male)
                pbImage.Image = Resources.driverMale;
            else
                pbImage.Image = Resources.driverFemale;
        }
    }
}
