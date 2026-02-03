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
        User user;
        string defaultMessage;

        private enum FormMode
        {
            AddNew,
            Update
        }
        private static FormMode mode;

        public frmAddUpdateUser(int userId)
        {
            InitializeComponent();

            Globals.CurrentUser = UserBusiness.Find(userId);

            ctrlPersonCardWithFilter1.ShowAddPerson = false;
            ctrlPersonCardWithFilter1.FilterEnabled = false;   
            
            mode = FormMode.Update;
            defaultMessage = "Updated the user's data successfully";
        }

        public frmAddUpdateUser()
        {
            InitializeComponent();
            user = new User();

            mode = FormMode.AddNew;
            defaultMessage = $"Added the new user to the database with id {user.Id}";
        }

        private void LoadPersonInfo()
        {
            if (Globals.CurrentUser != null)
            {
                ctrlPersonCardWithFilter1.ctrlPersonCard1.LoadPersonInfo(Globals.CurrentUser.PersonId);
                ctrlPersonCardWithFilter1.QueryText = Globals.CurrentUser.PersonId.ToString();
            }
        }

        private void LoadUserInfo()
        {
            if (Globals.CurrentUser != null)
            {
                lblUserID.Text = Globals.CurrentUser.Id.ToString();
                txtUsername.Text = Globals.CurrentUser.Username;
            }
            else
                this.Dispose();
        }

        private void ctrlPersonCardWithFilter1_OnPersonSelected(int obj)
        {
            int personId = obj;
            if (UserBusiness.IsPersonLinkedToUser(personId))
            {
                MessageBox.Show("Person is already linked to a user!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ctrlPersonCardWithFilter1.ctrlPersonCard1.Enabled = false;
                return;
            }

            ctrlPersonCardWithFilter1.ctrlPersonCard1.Enabled = true;
            btnNext.Enabled = true;
            user.PersonId = personId;
        }

        private void frmAddUpdateUser_Load(object sender, EventArgs e)
        {
            if (mode == FormMode.Update)
            {
                LoadUserInfo();
                LoadPersonInfo();
            }

            lblFormMode.Text = mode == FormMode.Update ? "Update User" : "Add New User";
            btnNext.Enabled = mode == FormMode.Update;
        }

        private bool ValidateRequireField(Control control, string message)
        {
            TextBox senderTextBox = (TextBox)control;

            if (senderTextBox == null)
                return false;

            string text = senderTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(text))
            {
                errProvider.SetError(senderTextBox, message);
                return false;
            }
            else
                errProvider.SetError(senderTextBox, string.Empty);

            return true;
        }

        private void MapUserData()
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            bool isActive = chkIsActive.Checked;

            if (mode == FormMode.Update)
                user = Globals.CurrentUser;

            user.Username = username;
            user.Password = password;
            user.IsActive = isActive;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateChildren())
                return;

            MapUserData();

            if (UserBusiness.Save(user))
            {
                lblUserID.Text = user.Id.ToString();
                Globals.CurrentUser = user; // update the current user after save
                MessageBox.Show(defaultMessage, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void txtUsername_Validating(object sender, CancelEventArgs e)
        {
            if (!ValidateRequireField((TextBox)sender, "This field is required"))
                return;

            string username = txtUsername.Text.Trim();

            if (!Validation.IsUniqueUsername(username) && username != Globals.CurrentUser.Username)
                errProvider.SetError(txtUsername, "This username is already in use");
            else
                errProvider.SetError(txtUsername, string.Empty);
        }

        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            if (!ValidateRequireField((TextBox)sender, "This field is required"))
                return;
        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (!ValidateRequireField((TextBox)sender, "This field is required"))
                return;

            string password = txtPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();

            if (!Validation.DoPasswordsMatch(password, confirmPassword))
            {
                errProvider.SetError(txtConfirmPassword, "Passwords do no match");
                return;
            }

            errProvider.SetError(txtConfirmPassword, string.Empty);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
