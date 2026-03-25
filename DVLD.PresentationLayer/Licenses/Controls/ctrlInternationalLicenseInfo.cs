using System.IO;
using System.Windows.Forms;
using DVLD.EntityLayer;
using DVLD.PresentationLayer.GlobalClasses;
using DVLD.PresentationLayer.Properties;
using ILS = DVLD.BusinessLayer.InternationalLicenseService;

namespace DVLD.PresentationLayer.Licenses.Controls
{
    public partial class ctrlInternationalLicenseInfo : UserControl
    {
        private InternationalLicense _internationalLicense;
        public InternationalLicense SelectedlLicense => _internationalLicense;

        public ctrlInternationalLicenseInfo()
        {
            InitializeComponent();
        }

        public void LoadLicense(int interLicenseId)
        {
            _internationalLicense = ILS.FindById(interLicenseId);

            if (_internationalLicense == null)
            {
                Utility.ShowErrorMessage($"International License with id: {interLicenseId} wasn't found");
                ResetInfo();
                return;
            }

            LoadInfo();
        }

        private void LoadInfo()
        {
            var person = _internationalLicense.ApplicationInfo.ApplicantPersonInfo;

            // Set labels
            lblName.Text = person.FullName;
            lblNationalNo.Text = person.NationalNo;
            lblDateOfBirth.Text = person.DateOfBirth.ToShortDateString();
            lblGender.Text = person.Gender == Gender.Male ? "Male" : "Female";
            lblIntlLicenseId.Text = _internationalLicense.Id.ToString();
            lblLocalLicenseId.Text = _internationalLicense.LocalLicenseId.ToString();
            lblApplicationId.Text = _internationalLicense.ApplicationId.ToString();
            lblDriverId.Text = _internationalLicense.DriverId.ToString();
            lblIssueDate.Text = _internationalLicense.IssueDate.ToShortDateString();
            lblExpirationDate.Text = _internationalLicense.ExpirationDate.ToShortDateString();
            lblIsActive.Text = _internationalLicense.IsActive ? "Active" : "Inactive";

            // Set Person Image
            LoadPersonImage();
        }

        private void LoadPersonImage()
        {
            var person = _internationalLicense.ApplicationInfo.ApplicantPersonInfo;
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

        private void ResetInfo()
        {
            foreach (Control ctrl in this.Controls)
                if (ctrl is Label lbl)
                    lbl.Text = "???";
        }
    }
}
