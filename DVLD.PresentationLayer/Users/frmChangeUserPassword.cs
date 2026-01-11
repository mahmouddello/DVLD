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
    public partial class frmChangeUserPassword : Form
    {
        private User user;

        public frmChangeUserPassword(int userId)
        {
            InitializeComponent();

            ctrlUserLoginInfo1.LoadUserInfo(userId);
            user = ctrlUserLoginInfo1.SelectedUser;

            ctrlPersonCard1.LoadPersonInfo(user.PersonId);
        }

        public frmChangeUserPassword(User user)
        {
            InitializeComponent();
            ctrlUserLoginInfo1.LoadUserInfo(user);
            ctrlPersonCard1.LoadPersonInfo(user.PersonId);
        }

        private void RequiredField_Validating(object sender, CancelEventArgs e)
        {
            TextBox senderTextBox = sender as TextBox;

            if (senderTextBox == null)
                return;

            string text = senderTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(text))
            {
                e.Cancel = true; // prevent user from leaving textbox.
                senderTextBox.Focus();
                errProvider.SetError(senderTextBox, "This field is required");
            }
            else
            {
                e.Cancel = false;
                errProvider.SetError(senderTextBox, string.Empty);
            }
        }

        private void ShowPasswordMismatchErrors()
        {
            errProvider.SetError(txtNewPassword, "Passwords do not match.");
            errProvider.SetError(txtNewPasswordConfirmation, "Passwords do not match.");
        }

        private void ClearPasswordErrors()
        {
            errProvider.SetError(txtNewPassword, string.Empty);
            errProvider.SetError(txtNewPasswordConfirmation, string.Empty);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateChildren())
                return;

            string newPass = txtNewPassword.Text.Trim();
            string newPassConfirm = txtNewPasswordConfirmation.Text.Trim();

            if (!Validation.DoPasswordsMatch(newPass, newPassConfirm))
            {
                ShowPasswordMismatchErrors();
                return;
            }

            ClearPasswordErrors();

            int currentUserId = Globals.CurrentUser.UserId;
            string currentPassword = txtCurrentPassword.Text.Trim();

            if (UserBusiness.ChangePassword(currentUserId, currentPassword, newPass))
                MessageBox.Show("Updated password successfully!");
            else
                MessageBox.Show("Current password is incorrect!");
        }
    }
}
