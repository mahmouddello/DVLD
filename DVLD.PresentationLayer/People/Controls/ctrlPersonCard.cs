using System;
using System.IO;
using System.Windows.Forms;
using DVLD.BusinessLayer;
using DVLD.EntityLayer;
using DVLD.PresentationLayer.GlobalClasses;
using DVLD.PresentationLayer.Properties;

namespace DVLD.PresentationLayer.People
{
    public partial class ctrlPersonCard : UserControl
    {
        private Person person;

        public int PersonID => person?.Id ?? -1;
        public Person SelectedPerson => person;

        private string fullImagePath =>
            person == null ? null : Path.Combine(Globals.ImagesRootDirectory, person.ImagePath);

        public ctrlPersonCard()
        {
            InitializeComponent();
        }

        public void LoadPersonInfo(int personID)
        {
            person = PersonService.GetById(personID);

            if (person == null)
            {
                ResetPersonInfo();
                Utility.ShowErrorMessage($"No person found with ID = {personID}");
                return;
            }

            FillPersonInfo();
        }

        public void LoadPersonInfo(string nationalNo)
        {
            person = PersonService.GetByNationalNo(nationalNo);

            if (person == null)
            {
                ResetPersonInfo();
                Utility.ShowErrorMessage($"No person found with national no = {nationalNo}");
                return;
            }

            FillPersonInfo();
        }

        public void ResetPersonInfo()
        {
            person = null;  // clear the object
            lblPersonID.Text = "[????]";
            lblNationalNo.Text = "[????]";
            lblName.Text = "[????]";
            lblGender.Text = "[????]";
            lblEmail.Text = "[????]";
            lblPhone.Text = "[????]";
            lblDateOfBirth.Text = "[????]";
            lblCountry.Text = "[????]";
            lblAddress.Text = "[????]";
            pbImage.Image = Resources.driverMale;
        }

        private void FillPersonInfo()
        {
            llEditInfo.Enabled = true;
            lblPersonID.Text = person.Id.ToString();
            lblNationalNo.Text = person.NationalNo;
            lblName.Text = person.FullName;
            lblGender.Text = person.Gender == Gender.Male ? "Male" : "Female";
            lblEmail.Text = person.Email;
            lblPhone.Text = person.Phone;
            lblDateOfBirth.Text = person.DateOfBirth.ToShortDateString();
            lblCountry.Text = person.Nationality.Name;
            lblAddress.Text = person.Address;
            LoadPersonImage();
        }

        private void LoadPersonImage()
        {
            pbImage.Image = person.Gender == Gender.Male
                ? Resources.driverMale
                : Resources.driverFemale;

            if (!string.IsNullOrWhiteSpace(fullImagePath) && File.Exists(fullImagePath))
                pbImage.ImageLocation = fullImagePath;
        }

        private void llEditInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAddUpdatePerson form = new frmAddUpdatePerson(person.Id);
            form.ShowDialog();
            LoadPersonInfo(person.Id); // refresh
        }
    }
}
