using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD.BusinessLayer;
using DVLD.EntityLayer;
using DVLD.PresentationLayer.GlobalClasses;

namespace DVLD.PresentationLayer.Users
{
    public partial class frmAddUpdateUser : Form
    {
        private enum FormMode { AddNew, Update }
        private FormMode _mode;

        private int _userId;
        private User _user;

        public frmAddUpdateUser()
        {
            InitializeComponent();
            _mode = FormMode.AddNew;
        }

        public frmAddUpdateUser(int userId)
        {
            InitializeComponent();
            _userId = userId;
            _mode = FormMode.Update;
        }

        private void frmAddUpdateUser_Load(object sender, EventArgs e)
        {
            if (_mode == FormMode.Update)
                LoadUserInfo();
            else
                _user = new User();

            lblFormMode.Text = _mode == FormMode.Update ? "Update User" : "Add New User";
            btnNext.Enabled = _mode == FormMode.Update;
        }

        private void LoadPersonCardInfo()
        {
            ctrlPersonCardWithFilter1.QueryText = _user.PersonId.ToString();
            ctrlPersonCardWithFilter1.ctrlPersonCard1.LoadPersonInfo(_user.PersonId);
        }

        private void LoadUserInfo()
        {
            _user = UserService.FindById(_userId);

            if (_user == null)
            {
                Utility.ShowErrorMessage("User wasn't found!");
                this.Close();
                return;
            }

            lblUserID.Text = _user.Id.ToString();
            txtUsername.Text = _user.Username;
            LoadPersonCardInfo();
        }

        private void ctrlPersonCardWithFilter1_OnPersonSelected(int personId)
        {
            if (UserService.IsPersonLinkedToUser(personId))
            {
                Utility.ShowErrorMessage("This person is already linked to a user");
                btnNext.Enabled = false;
                tabPage2.Enabled = false;
                return;
            }

            _user.LinkedPerson = ctrlPersonCardWithFilter1.SelectedPerson;
            ctrlPersonCardWithFilter1.ctrlPersonCard1.Enabled = true;
            btnNext.Enabled = true;
            tabPage2.Enabled = true;
        }

        private bool ValidateTextBox(TextBox textBox, string message)
        {
            if (textBox == null)
                return false;

            string text = textBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(text))
            {
                errProvider.SetError(textBox, message);
                return false;
            }
            else
                errProvider.SetError(textBox, string.Empty);

            return true;
        }

        private void MapUserData()
        {
            _user.Username = txtUsername.Text.Trim();
            _user.Password = txtPassword.Text;
            _user.IsActive = chkIsActive.Checked;
        }

        private bool IsFormValid()
        {
            if (!ValidateChildren(ValidationConstraints.None))
                return false;

            string username = txtUsername.Text.Trim();

            if (string.IsNullOrWhiteSpace(username))
            {
                errProvider.SetError(txtUsername, "This field is required");
                tabControl1.SelectedIndex = 0;
                return false;
            }

            if (!Validation.IsUniqueUsername(username) && !
                username.Equals(_user.Username, StringComparison.OrdinalIgnoreCase))
            {
                errProvider.SetError(txtUsername, "This username is already in use");
                return false;
            }

            if (!Validation.DoPasswordsMatch(txtPassword.Text, txtConfirmPassword.Text))
            {
                errProvider.SetError(txtConfirmPassword, "Passwords do not match");
                return false;
            }

            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsFormValid()) return;

            MapUserData();

            UserService service = new UserService(_user);
            if (service.Save())
            {
                lblUserID.Text = _user.Id.ToString();
                Utility.ShowSuccessMessage($"Saved successfully with id: {_user.Id}");
            }
            else
                Utility.ShowErrorMessage("Failed to save user!");
        }

        private void txtUsername_Validating(object sender, CancelEventArgs e)
        {
            if (!ValidateTextBox((TextBox)sender, "This field is required"))
                return;

            string username = txtUsername.Text.Trim();

            if (!Validation.IsUniqueUsername(username) &&
                !username.Equals(_user.Username, StringComparison.OrdinalIgnoreCase))
                errProvider.SetError(txtUsername, "This username is already in use");
            else
                errProvider.SetError(txtUsername, string.Empty);
        }

        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            if (!ValidateTextBox((TextBox)sender, "This field is required"))
                return;
        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (!ValidateTextBox((TextBox)sender, "This field is required"))
                return;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
