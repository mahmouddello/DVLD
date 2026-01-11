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

namespace DVLD.PresentationLayer
{
    public partial class frmLoginScreen : Form
    {

        public frmLoginScreen()
        {
            InitializeComponent();
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

        private void MapDefaultValues()
        {
            // Set the values of the saved username, password, and remember me state
            if (chkRememberMe.Checked)
            {
                Properties.Settings.Default.RemeberMe = true;
                Properties.Settings.Default.Username = txtUsername.Text;
                Properties.Settings.Default.Password = txtPassword.Text; // ⚠ see note below
            }
            else
            {
                Properties.Settings.Default.RemeberMe = false;
                Properties.Settings.Default.Username = "";
                Properties.Settings.Default.Password = "";
            }

            Properties.Settings.Default.Save();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (!ValidateChildren())
                return;

            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            User user = UserBusiness.Login(username, password);

            if (user == null)
            {
                MessageBox.Show("Invalid credntials!", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                this.DialogResult = DialogResult.OK;  // Signal success
                Globals.CurrentUser = user;
                MapDefaultValues();
                this.Close();  // Now safe to close
            }
        }

        private void frmLoginScreen_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.RemeberMe)
            {
                txtUsername.Text = Properties.Settings.Default.Username;
                txtPassword.Text = Properties.Settings.Default.Password;
                chkRememberMe.Checked = true;
            }
        }
    }
}
