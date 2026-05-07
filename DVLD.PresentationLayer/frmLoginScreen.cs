using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Win32;
using DVLD.BusinessLayer;
using DVLD.EntityLayer;
using DVLD.PresentationLayer.GlobalClasses;
using DVLD.Infrastructure;

namespace DVLD.PresentationLayer
{
    public partial class frmLoginScreen : Form
    {
        private static readonly string KEY_PATH = @"HKEY_CURRENT_USER\Software\DVLD";
        private string _username, _password;
        private bool _remeberMe;

        public frmLoginScreen()
        {
            InitializeComponent();
        }

        private void frmLoginScreen_Load(object sender, EventArgs e)
        {
            LoadSavedCredentials();

            if (_remeberMe)
            {
                txtUsername.Text = _username;
                txtPassword.Text = _password;
                chkRememberMe.Checked = true;
            }
        }

        private void LoadSavedCredentials()
        {
            try
            {
                _username = Registry.GetValue(KEY_PATH, "username", null) as string;

                string strA = Registry.GetValue(KEY_PATH, "rememberMe", null) as string;
                _remeberMe = strA == "1";
            }
            catch (Exception ex)
            {
                Logger.Log("Failed to load saved credentials", EventLogEntryType.Error, ex, nameof(LoadSavedCredentials));
            }
        }

        private bool SaveCredentials(string username, bool rememberMe)
        {
            if (string.IsNullOrWhiteSpace(username))
                return false;

            try
            {
                Registry.SetValue(KEY_PATH, "username", username, RegistryValueKind.String);
                Registry.SetValue(KEY_PATH, "rememberMe", rememberMe ? "1" : "0", RegistryValueKind.String);

                return true;
            }
            catch (Exception ex)
            {
                Logger.Log("Failed to save credentials in the registry", EventLogEntryType.Error, ex, nameof(SaveCredentials));
                return false;
            }
        }

        private bool DeleteCredentials(params string[] values)
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\DVLD", writable: true))
                {
                    if (key == null) return false;

                    foreach (string valueName in values)
                        key.DeleteValue(valueName, throwOnMissingValue: false);
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.Log("Failed to delete the saved credntials", EventLogEntryType.Error, ex, nameof(DeleteCredentials));
                return false;
            }
        }

        private void SetRememberMe(bool flag)
        {
            try
            {
                Registry.SetValue(KEY_PATH, "rememberMe", flag ? "1" : "0", RegistryValueKind.String);
            }
            catch (Exception ex)
            {
                Logger.Log("Failed to change remember me state", EventLogEntryType.Error, ex, nameof(SetRememberMe));
            }
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

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (!ValidateChildren())
                return;

            _username = txtUsername.Text.Trim();
            _password = txtPassword.Text.Trim();

            User user = UserService.Login(_username, _password);

            if (user == null)
            {
                Utility.ShowErrorMessage("Login failed: Invalid credntials!");
                return;
            }

            HandleSuccessfulLogin(user);
        }

        private void HandleSuccessfulLogin(User user)
        {
            Logger.Log($"User '{user.Username}' logged in successfully",
               EventLogEntryType.Information,
               null,
               nameof(HandleSuccessfulLogin));

            if (chkRememberMe.Checked)
                SaveCredentials(_username, true);
            else
            {
                DeleteCredentials("username");
                SetRememberMe(false);
            }

            Globals.CurrentUser = user;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
