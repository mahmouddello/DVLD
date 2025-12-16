using DVLD.BusinessLayer;
using DVLD.EntityLayer;
using DVLD.PresentationLayer.Properties;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DVLD.PresentationLayer.People
{
    public partial class frmAddUpdatePerson : Form
    {
        private enum enFormMode
        {
            AddNew = 1,
            Update = 2
        }
        private enFormMode _formMode;

        private Person _person;
        private int _personID;

        public frmAddUpdatePerson(int personID)
        {
            InitializeComponent();
            _personID = personID;
            _formMode = _personID == -1 ? enFormMode.AddNew : enFormMode.Update;
        }

        private void _LoadCountryCombobox()
        {
            DataTable dt = CountryBusiness.GetCountries();

            foreach (DataRow dr in dt.Rows)
                cbCountry.Items.Add(dr["CountryName"]);
        }

        private void _LoadPersonData()
        {
            _LoadCountryCombobox();
            cbCountry.SelectedIndex = 168; // Syria, Default
            dtpDateOfBirth.MaxDate = DateTime.Now.AddYears(-18);

            if (_personID == -1)
            {
                _person = new Person();
                lblModeTitle.Text = "Add New Person";
                return;
            }

            _person = PersonBusiness.Find(_personID);

            if (_person == null) 
            {
                MessageBox.Show($"This form will be closed because there's no Person with ID = {_personID}");
                this.Close();
                return;
            }

            lblModeTitle.Text = "Edit Person Details";

            txtFirstName.Text = _person.FirstName;
            txtSecondName.Text = _person.SecondName;
            txtThirdName.Text = _person.ThirdName;
            txtLastName.Text = _person.LastName;
            txtNationalNo.Text = _person.NationalNo;
            txtEmail.Text = _person.Email;
            txtPhone.Text = _person.Phone;
            rtxtAddress.Text = _person.Address;
            dtpDateOfBirth.Value = _person.DateOfBirth;

            if (_person.Gender == enGender.Male)
                rbMale.Checked = true;
            else
                rbFemale.Checked = true;

            if (!string.IsNullOrWhiteSpace(_person.ImagePath) && File.Exists(_person.ImagePath))
                pbPersonImage.Image = Image.FromFile(_person.ImagePath);
            else
                _UpdateDefaultImage();

        }

        private void frmAddUpdatePerson_Load(object sender, System.EventArgs e)
        { 
            _LoadPersonData();
        }

        private void _UpdateDefaultImage()
        {
            if (_personID == -1)
            {
                llImageLink.Text = "Set Image";

                if (rbMale.Checked)
                {
                    pbPersonImage.Image = Resources.driverMale;
                    return;
                }

                if (rbFemale.Checked)
                {
                    pbPersonImage.Image = Resources.driverFemale;
                    return;
                }
            }
            else
            {
                llImageLink.Text = "Edit Image";
            }
        }

        private void rbMale_CheckedChanged(object sender, System.EventArgs e)
        {
            _UpdateDefaultImage();
        }

        private void rbFemale_CheckedChanged(object sender, System.EventArgs e)
        {
            _UpdateDefaultImage();
        }

        private void llImageLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            dialogSetImage.InitialDirectory = @"C:\Photos";
            dialogSetImage.Title = "Choose an Image";

            dialogSetImage.DefaultExt = "png";
            dialogSetImage.Filter = "Image Files|*.jpg;*.jpeg;*.png;";

            if (dialogSetImage.ShowDialog() == DialogResult.OK)
            {
                pbPersonImage.Image = Image.FromFile(dialogSetImage.FileName);
                _person.ImagePath = dialogSetImage.FileName;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _person.NationalNo = txtNationalNo.Text;
            _person.FirstName = txtFirstName.Text;
            _person.SecondName = txtSecondName.Text;
            _person.ThirdName = txtThirdName.Text;
            _person.LastName = txtLastName.Text;
            _person.DateOfBirth = dtpDateOfBirth.Value;
            _person.Gender = rbMale.Checked ? enGender.Male : enGender.Female;
            _person.Email = txtEmail.Text;
            _person.Phone = txtPhone.Text;
            _person.Nationality = new Country
            {
                ID = cbCountry.SelectedIndex + 1,
                Name = cbCountry.SelectedItem.ToString()
            };
            _person.Address = rtxtAddress.Text;

            if (PersonBusiness.Save(ref _person))
                MessageBox.Show($"Saved the person data sucessfully with id {_person.ID}!");
            else
                MessageBox.Show("Failed!");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
