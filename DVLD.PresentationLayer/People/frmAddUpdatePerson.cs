using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DVLD.BusinessLayer;
using DVLD.EntityLayer;
using DVLD.PresentationLayer.GlobalClasses;
using DVLD.PresentationLayer.Properties;

namespace DVLD.PresentationLayer.People
{
    public partial class frmAddUpdatePerson : Form
    {

        private enum FormMode { AddNew = 0, Update = 1 }
        private FormMode _mode;

        private Person _person;
        private int _personId;
        private string _originalNationalNo;
        private string _persistedImagePath = null; // saved image
        private string _selectedImageSourcePath = null; // new image (not saved yet)
        private bool _imageMarkedForDeletion;

        public delegate void DataBackEventHandler(object sender, int PersonID);

        // Declare an event using the delegate
        public event DataBackEventHandler DataBack;

        private string FullImagePath
        {
            get
            {
                if (_person == null || string.IsNullOrWhiteSpace(_person.ImagePath))
                    return null;
                return Path.Combine(Globals.ImagesRootDirectory, _person.ImagePath);
            }
        }

        public frmAddUpdatePerson()
        {
            InitializeComponent();
            _mode = FormMode.AddNew;
        }

        public frmAddUpdatePerson(int personID)
        {
            InitializeComponent();

            _personId = personID;
            _mode = FormMode.Update;
        }

        private void LoadCountryCombobox()
        {

            var countries = CountryService.GetAllCountries();
            countries.Insert(0, new Country(-1, "-- Select Country --")); // fake country
            cbCountry.DataSource = countries;

            cbCountry.DisplayMember = "Name";
            cbCountry.ValueMember = "Id";
        }

        private void SetDefaultValues()
        {
            LoadCountryCombobox();
            llRemoveImage.Visible = false;

            if (_mode == FormMode.AddNew)
            {
                lblModeTitle.Text = "Add New Person";
                _person = new Person();
            }

            dtpDateOfBirth.MaxDate = DateTime.Now.AddYears(-18);
            dtpDateOfBirth.MinDate = DateTime.Now.AddYears(-100);
        }

        private void LoadPersonData()
        {
            _person = PersonService.GetById(_personId);

            if (_person == null)
            {
                Utility.ShowErrorMessage($"This form will be closed because there's no person with ID = {_personId}");
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
            cbCountry.SelectedIndex = _person.Nationality.Id;

            if (_person.Gender == Gender.Male)
                rbMale.Checked = true;
            else
                rbFemale.Checked = true;

            if (!string.IsNullOrWhiteSpace(FullImagePath) && File.Exists(FullImagePath))
            {
                _persistedImagePath = _person.ImagePath; // save original guid.png
                pbPerson.Image = Image.FromFile(FullImagePath); // load the image
                llRemoveImage.Visible = true;
            }
            else
                UpdateDefaultImage();
        }

        private void MapFormFields()
        {

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

            _originalNationalNo = _person.NationalNo;
            cbCountry.SelectedValue = _person.Nationality.Id;

            rbMale.Checked = _person.Gender == Gender.Male;
            rbFemale.Checked = _person.Gender == Gender.Female;

            if (!string.IsNullOrWhiteSpace(FullImagePath) && File.Exists(FullImagePath))
            {
                _persistedImagePath = _person.ImagePath; // save original guid.png
                pbPerson.Image = Image.FromFile(FullImagePath); // load the image
                llRemoveImage.Visible = true;
            }
            else
                UpdateDefaultImage();
        }

        private void frmAddUpdatePerson_Load(object sender, System.EventArgs e)
        {
            SetDefaultValues();

            if (_mode == FormMode.Update)
                LoadPersonData();
        }

        private void UpdateDefaultImage()
        {
            if (!string.IsNullOrWhiteSpace(FullImagePath) && File.Exists(FullImagePath))
            {
                llImageLink.Text = "Edit Image";
                return;
            }

            llImageLink.Text = "Set Image";
            pbPerson.Image = rbMale.Checked
                ? Resources.driverMale
                : Resources.driverFemale;
        }

        private void rbGender_CheckedChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(FullImagePath) && File.Exists(FullImagePath))
                return;

            UpdateDefaultImage();
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
            if (_imageMarkedForDeletion)
            {
                if (!string.IsNullOrWhiteSpace(_persistedImagePath))
                    Utility.DeleteImageFromDirectory(_persistedImagePath);

                _person.ImagePath = null;
                _persistedImagePath = null;
                _imageMarkedForDeletion = false;
                return;
            }

            // Case 2: Image unchanged
            if (string.IsNullOrWhiteSpace(_selectedImageSourcePath))
                return;

            // Case 3: Image replaced
            string newImagePath = Utility.CopyImageToDirectory(_selectedImageSourcePath);

            if (!string.IsNullOrWhiteSpace(_persistedImagePath))
                Utility.DeleteImageFromDirectory(_persistedImagePath);

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
            _person.Gender = rbMale.Checked ? Gender.Male : Gender.Female;
            _person.Email = txtEmail.Text.Trim();
            _person.Phone = txtPhone.Text.Trim();
            _person.Address = txtAddress.Text.Trim();
            _person.Nationality = (Country)cbCountry.SelectedItem;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
                return;
            if (!ValidateRadioGroups())
                return;

            MapPersonFields();
            HandleImage();

            PersonService service = new PersonService(_person);

            if (service.Save())
            {
                Utility.ShowSuccessMessage($"Saved the person data successfully with id: {_person.Id}");
                _mode = FormMode.Update;
                lblModeTitle.Text = "Edit Person Details";
                DataBack?.Invoke(this, _person.Id);
            }
            else
                Utility.ShowErrorMessage("Failed to save person data to the database");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            _selectedImageSourcePath = null; // restore
            this.Close();
        }

        private void OnlyLettersTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar)) return;
            if (!char.IsLetter(e.KeyChar))
                Utility.HandleWrongKey(e);
        }

        private void OnlyDigitsTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar)) return;
            if (!char.IsDigit(e.KeyChar))
                Utility.HandleWrongKey(e);
        }

        private void RequiredField_Validating(object sender, CancelEventArgs e)
        {
            if (!(sender is TextBox senderTextBox)) return;

            string fieldValue = senderTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(fieldValue))
                errProviderValidation.SetError(senderTextBox, "This field is required");
            else
                errProviderValidation.SetError(senderTextBox, string.Empty);
        }

        private void cbCountry_Validating(object sender, CancelEventArgs e)
        {
            if (cbCountry.SelectedItem == null)
                errProviderValidation.SetError(cbCountry, "Please select a country");
            else
                errProviderValidation.SetError(cbCountry, string.Empty);
        }

        private bool ValidateRadioGroups()
        {
            bool isValid = gbGender.Controls.OfType<RadioButton>().Any(rb => rb.Checked);

            errProviderValidation.SetError(gbGender, isValid
                ? string.Empty
                : "Please select a gender");

            return isValid;
        }

        private void frmAddUpdatePerson_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = false; // ensures that user can exit the form if there's an active error.
        }

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            string email = txtEmail.Text.Trim();

            // email is not required field
            if (email.Length == 0)
            {
                errProviderValidation.SetError(txtEmail, string.Empty);
                return;
            }

            errProviderValidation.SetError(txtEmail, Validation.IsValidEmail(email)
                ? string.Empty
                : "Please enter a valid email address");
        }

        private void txtNationalNo_Validating(object sender, CancelEventArgs e)
        {
            string nationalNo = txtNationalNo.Text.Trim();

            if (string.IsNullOrWhiteSpace(nationalNo))
            {
                errProviderValidation.SetError(txtNationalNo, "This field is required");
                return;
            }

            // only check uniqueness if it changed from original
            bool isUnchanged = nationalNo.Equals(_person.NationalNo, StringComparison.OrdinalIgnoreCase);

            if (!isUnchanged && !Validation.IsUniqueNationalNo(nationalNo))
                errProviderValidation.SetError(txtNationalNo, "This national number already exists");
            else
                errProviderValidation.SetError(txtNationalNo, string.Empty);
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
