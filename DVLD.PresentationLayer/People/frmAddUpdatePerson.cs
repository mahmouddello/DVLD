using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DVLD.BusinessLayer;
using DVLD.EntityLayer;
using DVLD.PresentationLayer.Properties;

namespace DVLD.PresentationLayer.People
{
    public partial class frmAddUpdatePerson : Form
    {

        private Person _person;
        private int _personID;

        public frmAddUpdatePerson(int personID)
        {
            InitializeComponent();
            _personID = personID;
        }

        private void _LoadCountryCombobox()
        {
            cbCountry.Items.Add("None");
            DataTable dt = CountryBusiness.GetCountries();

            foreach (DataRow dr in dt.Rows)
                cbCountry.Items.Add(dr["CountryName"]);
        }

        private void _LoadPersonData()
        {
            _LoadCountryCombobox();
            cbCountry.SelectedIndex = 0; // None, Default
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
            txtAddress.Text = _person.Address;
            dtpDateOfBirth.Value = _person.DateOfBirth;
            cbCountry.SelectedIndex = _person.Nationality.ID;

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
            else
            {
                llImageLink.Text = "Edit Image";
            }
        }

        private void rbMale_CheckedChanged(object sender, System.EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(_person.ImagePath) && File.Exists(_person.ImagePath))
                return;

            _UpdateDefaultImage();
        }

        private void rbFemale_CheckedChanged(object sender, System.EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(_person.ImagePath) && File.Exists(_person.ImagePath))
                return;

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

        private void MapData()
        {
            _person.NationalNo = txtNationalNo.Text.Trim();
            _person.FirstName = txtFirstName.Text.Trim();
            _person.SecondName = txtSecondName.Text.Trim();
            _person.ThirdName = txtThirdName.Text.Trim();
            _person.LastName = txtLastName.Text.Trim();
            _person.DateOfBirth = dtpDateOfBirth.Value;
            _person.Gender = rbMale.Checked ? enGender.Male : enGender.Female;
            _person.Email = txtEmail.Text.Trim();
            _person.Phone = txtPhone.Text.Trim();
            _person.Address = txtAddress.Text.Trim();

            _person.Nationality = new Country
            {
                ID = cbCountry.SelectedIndex,
                Name = cbCountry.SelectedItem?.ToString()
            };
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // This triggers ALL Validating events
            if (!this.ValidateChildren())
                return;

            if (!ValidateRadioGroups())
                return;

            MapData();

            if (PersonBusiness.Save(_person))
                MessageBox.Show($"Saved the person data sucessfully with id {_person.ID}!");
            else
                MessageBox.Show("Failed to save person data to the database!");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void OnlyLettersTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // The pressed key is : space, delete, backspace, ...etc. skips the checks.
            if (char.IsControl(e.KeyChar)) return;

            if (!char.IsLetter(e.KeyChar))
            {
                System.Media.SystemSounds.Beep.Play();
                e.Handled = true;
            }
            else
                e.Handled = false;
        }

        private void OnlyDigitsTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // The pressed key is : space, delete, backspace, ...etc. skips the checks.
            if (char.IsControl(e.KeyChar)) return;

            if (!char.IsDigit(e.KeyChar))
            {
                System.Media.SystemSounds.Beep.Play();
                e.Handled = true;
            }
            else
                e.Handled = false;
        }

        private void RequiredField_Validating(object sender, CancelEventArgs e)
        {
            TextBox senderTextBox = sender as TextBox;

            if (senderTextBox == null)
                return;

            string proccessedText = senderTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(proccessedText))
            {
                e.Cancel = true; // prevent user from leaving textbox.
                senderTextBox.Focus();
                errProviderValidation.SetError(senderTextBox, "This field is required");
            }
            else
            {
                e.Cancel = false;
                errProviderValidation.SetError(senderTextBox, string.Empty);
            }
        }

        private void cbCountry_Validating(object sender, CancelEventArgs e)
        {
            if (cbCountry.SelectedIndex == 0) // None
            {
                e.Cancel = true;
                errProviderValidation.SetError(cbCountry, "This field is required");
            }
            else
            {
                e.Cancel = false;
                errProviderValidation.SetError(cbCountry, string.Empty);
            }
        }

        private bool ValidateRadioGroups()
        {
            bool isValid = true;

            if (!gbGender.Controls.OfType<RadioButton>().Any(rb => rb.Checked))
            {
                errProviderValidation.SetError(gbGender, "Please select a gender");
                isValid = false;
            }
            else
            {
                errProviderValidation.SetError(gbGender, string.Empty);
            }

            return isValid;
        }

        private void frmAddUpdatePerson_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = false; // ensures that user can exit the form if there's an active error.
        }

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            string processedEmail = txtEmail.Text.Trim();

            // Email is not required
            if (processedEmail.Length == 0)
            {
                errProviderValidation.SetError(txtEmail, string.Empty);
                return;
            }

            if (!ValidationHelper.IsValidEmail(processedEmail))
                errProviderValidation.SetError(txtEmail, "Please enter a valid email address!");
            else
                errProviderValidation.SetError(txtEmail, string.Empty);
        }
    }
}
