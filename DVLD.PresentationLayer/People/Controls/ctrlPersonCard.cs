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
        private int personID = -1;
        private string fullImagePath
        {
            get
            {
                if (person == null)
                    return null;

                return Path.Combine(Globals.ImagesRootDirectory, person.ImagePath);
            }
        }

        public int PersonID { get { return personID; } }
        public Person SelectedPerson { get { return person; } }

        public ctrlPersonCard()
        {
            InitializeComponent();
        }

        public void LoadPersonInfo(int personID)
        {
            person = PersonBusiness.Find(personID);

            if (person == null)
            {
                ResetPersonInfo();
                MessageBox.Show("No Person with PersonID = " + personID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            FillPersonInfo();
        }

        public void LoadPersonInfo(string nationalNo)
        {
            person = PersonBusiness.Find(nationalNo);

            if (person == null)
            {
                ResetPersonInfo();
                MessageBox.Show("No Person with national no = " + nationalNo.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            FillPersonInfo();
        }

        public void ResetPersonInfo()
        {
            personID = -1;
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
            personID = person.Id;
            lblPersonID.Text = person.Id.ToString();
            lblNationalNo.Text = person.NationalNo;
            lblName.Text = person.FullName;
            lblGender.Text = person.Gender == enGender.Male ? "Male" : "Female";
            lblEmail.Text = person.Email;
            lblPhone.Text = person.Phone;
            lblDateOfBirth.Text = person.DateOfBirth.ToShortDateString();
            lblCountry.Text = CountryBusiness.Find(person.Nationality.Id).Name;
            lblAddress.Text = person.Address;

            LoadPersonImage();
        }

        private void LoadPersonImage()
        {
            if (person.Gender == enGender.Male)
                pbImage.Image = Resources.driverMale;
            else
                pbImage.Image = Resources.driverFemale;


            if (!string.IsNullOrWhiteSpace(fullImagePath) && File.Exists(fullImagePath))
                pbImage.ImageLocation = fullImagePath;  
        }

        private void llEditInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            int.TryParse(lblPersonID.Text, out int personID);

            frmAddUpdatePerson form = new frmAddUpdatePerson(personID);
            form.ShowDialog();

            LoadPersonInfo(personID); // refresh
        }
    }
}
