using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DVLD.BusinessLayer;
using DVLD.EntityLayer;
using DVLD.PresentationLayer.Globals;
using DVLD.PresentationLayer.Properties;

namespace DVLD.PresentationLayer.People
{
    public partial class frmAddUpdatePerson : Form
    {

        private enum enMode { AddNew = 0, Update = 1 }
        private enMode _mode;

        private Person _person;
        private int _personID;
        private string _originalNationalNo;
        private string _persistedImagePath = null; // saved image
        private string _selectedImageSourcePath = null; // new image (not saved yet)
        private bool _imageMarkedForDeletion;

        public delegate void DataBackEventHandler(object sender, int PersonID);

        // Declare an event using the delegate
        public event DataBackEventHandler DataBack;

        private string _fullImagePath
        {
            get
            {
                if (_person == null)
                    return null;

                return Path.Combine(SharedGlobals.ImagesRootDirectory, _person.ImagePath);
            }
        }

        public frmAddUpdatePerson()
        {
            InitializeComponent();
            _mode = enMode.AddNew;
        }

        public frmAddUpdatePerson(int personID)
        {
            InitializeComponent();

            _personID = personID;
            _mode = enMode.Update;
        }

        private void _LoadCountryCombobox()
        {
            cbCountry.Items.Add("None");
            DataTable dt = CountryBusiness.GetCountries();

            foreach (DataRow dr in dt.Rows)
                cbCountry.Items.Add(dr["CountryName"]);
        }

        private void _SetDefaultValues()
        {
            _LoadCountryCombobox();
            cbCountry.SelectedIndex = 0; // None, Default
            llRemoveImage.Visible = false;

            if (_mode  == enMode.AddNew)
            {
                lblModeTitle.Text = "Add New Person";
                _person = new Person();
            }

            dtpDateOfBirth.MaxDate = DateTime.Now.AddYears(-18);
            dtpDateOfBirth.MinDate = DateTime.Now.AddYears(-100);
        }

        private void _LoadPersonData()
        {
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
            _originalNationalNo = _person.NationalNo; // save original national no to a local variable
            txtEmail.Text = _person.Email;
            txtPhone.Text = _person.Phone;
            txtAddress.Text = _person.Address;
            dtpDateOfBirth.Value = _person.DateOfBirth;
            cbCountry.SelectedIndex = _person.Nationality.ID;

            if (_person.Gender == enGender.Male)
                rbMale.Checked = true;
            else
                rbFemale.Checked = true;

            if (!string.IsNullOrWhiteSpace(_fullImagePath) && File.Exists(_fullImagePath))
            {
                _persistedImagePath = _person.ImagePath; // save original guid.png
                pbPerson.Image = Image.FromFile(_fullImagePath); // load the image
                llRemoveImage.Visible = true;
            }
            else
                _UpdateDefaultImage();
        }

        private void frmAddUpdatePerson_Load(object sender, System.EventArgs e)
        {
            _SetDefaultValues();

            if (_mode == enMode.Update)
                _LoadPersonData();
        }

        private void _UpdateDefaultImage()
        {
            if (!string.IsNullOrWhiteSpace(_fullImagePath) && File.Exists(_fullImagePath))
                return;

            llImageLink.Text = "Set Image";

            if (rbMale.Checked)
            {
                pbPerson.Image = Resources.driverMale;
                return;
            }

            if (rbFemale.Checked)
            {
                pbPerson.Image = Resources.driverFemale;
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
            dialogSetImage.InitialDirectory = @"E:\Photos";
            dialogSetImage.Title = "Choose an Image";

            dialogSetImage.DefaultExt = "png";
            dialogSetImage.Filter = "Image Files|*.jpg;*.jpeg;*.png;";

            if (dialogSetImage.ShowDialog() == DialogResult.OK)
            {
                pbPerson.Image?.Dispose();
                pbPerson.Image = null;

                _selectedImageSourcePath = dialogSetImage.FileName;
                pbPerson.Image = Image.FromFile(_selectedImageSourcePath);

                _imageMarkedForDeletion = false;
                llRemoveImage.Visible = true;
            }
        }

        private void HandleImage()
        {
            // Case 1: Image removed
            if (_imageMarkedForDeletion)
            {
                if (!string.IsNullOrWhiteSpace(_persistedImagePath))
                    UtilityHelper.DeleteImageFromDirectory(_persistedImagePath);

                _person.ImagePath = null;
                _persistedImagePath = null;
                _imageMarkedForDeletion = false;
                return;
            }

            // Case 2: Image unchanged
            if (string.IsNullOrWhiteSpace(_selectedImageSourcePath))
                return;

            // Case 3: Image replaced
            string newImagePath = UtilityHelper.CopyImageToDirectory(_selectedImageSourcePath);

            if (!string.IsNullOrWhiteSpace(_persistedImagePath))
                UtilityHelper.DeleteImageFromDirectory(_persistedImagePath);

            _person.ImagePath = newImagePath;
            _persistedImagePath = newImagePath;
            _selectedImageSourcePath = null;
        }

        private void MapPersonFields()
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
             (
                countryId: cbCountry.SelectedIndex,
                countryName: cbCountry.SelectedItem.ToString()
             );
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // This triggers ALL Validating events
            if (!this.ValidateChildren())
                return;

            if (!ValidateRadioGroups())
                return;

            MapPersonFields();
            HandleImage();

            if (PersonBusiness.Save(_person))
            {
                MessageBox.Show($"Saved the person data sucessfully with id {_person.ID}!");
                _mode = enMode.Update;
                lblModeTitle.Text = "Edit Person Details";

                // Trigger the event to send data back to the caller form.
                DataBack?.Invoke(this, _person.ID);
            }
            else
                MessageBox.Show("Failed to save person data to the database!");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            _selectedImageSourcePath = null; // restore
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

            string nationalNo = senderTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(nationalNo))
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

        private void txtNationalNo_Validating(object sender, CancelEventArgs e)
        {
            string nationalNo = txtNationalNo.Text.Trim();

            if (string.IsNullOrWhiteSpace(nationalNo))
            {
                e.Cancel = true; // prevent user from leaving textbox.
                txtNationalNo.Focus();
                errProviderValidation.SetError(txtNationalNo, "This field is required");
                return;
            }

            if (!ValidationHelper.IsUniqueNationalNo(nationalNo) && nationalNo.ToLower() != _originalNationalNo.ToLower())
            {
                e.Cancel = true; // prevent user from leaving textbox.
                txtNationalNo.Focus();
                errProviderValidation.SetError(txtNationalNo, "This field is required");
            }

            else
            {
                e.Cancel = false;
                errProviderValidation.SetError(txtNationalNo, string.Empty);
            }
        }

        private void llRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pbPerson.Image?.Dispose();
            pbPerson.Image = null;

            _selectedImageSourcePath = null;
            _imageMarkedForDeletion = true;

            llRemoveImage.Visible = false;
        }
    }
}
