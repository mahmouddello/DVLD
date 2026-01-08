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

        private bool ArePasswordsMatch()
        {
            string newPassword = txtNewPassword.Text.Trim();
            string newPasswordConfirmation = txtNewPasswordConfirmation.Text.Trim();

            if (!(newPassword == newPasswordConfirmation))
            {
                errProvider.SetError(txtNewPassword, "Passwords doesn't match!");
                errProvider.SetError(txtNewPasswordConfirmation, "Passwords doesn't match!");
                return false;
            }
            else
            {
                errProvider.SetError(txtNewPassword, string.Empty);
                errProvider.SetError(txtNewPasswordConfirmation, string.Empty);
                return true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateChildren())
                return;

            if (!ArePasswordsMatch())
                return;

            int currentUserId = Globals.CurrentUser.UserId;
            string currentPassword = txtCurrentPassword.Text.Trim();
            string newPassword = txtNewPassword.Text.Trim();

            if (UserBusiness.ChangePassword(currentUserId, currentPassword, newPassword))
                MessageBox.Show("Updated password successfully!");
            else
                MessageBox.Show("Current password is incorrect!");
        }
    }
}
