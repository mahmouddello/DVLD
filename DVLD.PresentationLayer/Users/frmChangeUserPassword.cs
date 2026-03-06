using System;
using System.ComponentModel;
using System.Windows.Forms;
using DVLD.BusinessLayer;
using DVLD.PresentationLayer.GlobalClasses;

namespace DVLD.PresentationLayer.Users
{
    public partial class frmChangeUserPassword : Form
    {
        private int _userId;

        private frmChangeUserPassword()
        {
            InitializeComponent();
        }

        public static frmChangeUserPassword CreateForCurrentUser()
        {
            var form = new frmChangeUserPassword();
            form._userId = Globals.CurrentUser.Id;
            return form;
        }

        public static frmChangeUserPassword CreateById(int userId)
        {
            var form = new frmChangeUserPassword();
            form._userId = userId;
            return form;
        }

        private void frmChangeUserPassword_Load(object sender, EventArgs e)
        {
            ctrlUserLoginInfo1.LoadUserInfo(_userId);
            ctrlPersonCard1.LoadPersonInfo(ctrlUserLoginInfo1.SelectedUser.PersonId);
        }

        private void RequiredField_Validating(object sender, CancelEventArgs e)
        {
            TextBox senderTextBox = sender as TextBox;

            if (senderTextBox == null)
                return;

            string text = senderTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(text))
                errProvider.SetError(senderTextBox, "This field is required");
            else
                errProvider.SetError(senderTextBox, string.Empty);
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

            string newPassword = txtNewPassword.Text;
            string newPasswordConfirm = txtNewPasswordConfirmation.Text;

            if (!Validation.DoPasswordsMatch(newPassword, newPasswordConfirm))
            {
                ShowPasswordMismatchErrors();
                return;
            }

            ClearPasswordErrors();

            string enteredCurrentPassword = txtCurrentPassword.Text;
            var selectedUser = ctrlUserLoginInfo1.SelectedUser;

            if (UserService.ChangePassword(selectedUser, enteredCurrentPassword, newPassword))
                Utility.ShowSuccessMessage("Updated password successfully!");
            else
                Utility.ShowErrorMessage("Current password is incorrect!");
        }
    }
}
