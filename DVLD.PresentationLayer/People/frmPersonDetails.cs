using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using DVLD.EntityLayer;
using DVLD.BusinessLayer;
using DVLD.PresentationLayer.Properties;

namespace DVLD.PresentationLayer.People
{
    public partial class frmPersonDetails : Form
    {
        private Person person;
        private int _personID;
        public frmPersonDetails(int personID)
        {
            InitializeComponent();
            _personID = personID;
        }

        private void btnCloseForm_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void _LoadPersonData()
        {
            person = PersonBusiness.Find(_personID);

            if (person == null)
                return;

            var ctrl = ctrlPersonInformation1;

            ctrl.lblPersonID.Text = person.ID.ToString();
            ctrl.lblName.Text = person.FullName;
            ctrl.lblNationalNo.Text = person.NationalNo;
            ctrl.lblGender.Text = person.Gender.ToString();
            ctrl.lblEmail.Text = person.Email;
            ctrl.lblPhone.Text = person.Phone;
            ctrl.lblDateOfBirth.Text = person.DateOfBirth.ToString("dd/mm/yyyy");
            ctrl.lblAddress.Text = person.Address;
            ctrl.lblCountry.Text = person.Nationality.Name;


            if (!string.IsNullOrEmpty(person.ImagePath) && File.Exists(person.ImagePath))
                ctrl.pbImage.Image = Image.FromFile(person.ImagePath);
            else
                switch (person.Gender)
                {
                    case enGender.Male:
                        ctrl.pbImage.Image = Resources.driverMale;
                        break;
                    case enGender.Female:
                        ctrl.pbImage.Image = Resources.driverFemale;
                        break;
                }
        }

        private void frmPersonDetails_Load(object sender, EventArgs e)
        {
            _LoadPersonData();
        }
    }
}
