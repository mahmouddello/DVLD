using DVLD.BusinessLayer;
using DVLD.EntityLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.PresentationLayer.Applications.ApplicationTypes
{
    public partial class frmUpdateApplicationType : Form
    {
        private int _applicationTypeId;
        private ApplicationType _applicationType;

        public frmUpdateApplicationType(int applicationTypeId)
        {
            InitializeComponent();
            _applicationTypeId = applicationTypeId;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateChildren())
                return;

            string applicationTypeTitle = txtTitle.Text.Trim();
            if (!decimal.TryParse(txtFees.Text.Trim(), out decimal applicationFees))
                return;

            _applicationType.Title = applicationTypeTitle;
            _applicationType.Fees = applicationFees;

            if (ApplicationTypeBusiness.Save(_applicationType))
            {
                MessageBox.Show("Updated Successfully!");
                FillApplicationInfo();
            }
            else
                MessageBox.Show("Failed to save and update the application type!");
        }

        private void frmUpdateApplicationType_Load(object sender, EventArgs e)
        {
            LoadApplicationInfo();
        }

        private void LoadApplicationInfo()
        {
            _applicationType = ApplicationTypeBusiness.Find(_applicationTypeId);

            if (_applicationType == null)
            {
                MessageBox.Show(
                    "Application Type not found, this form will be closed",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                this.Dispose();
            }

            FillApplicationInfo();
        }

        private void FillApplicationInfo()
        {
            lblID.Text = _applicationTypeId.ToString();
            txtTitle.Text = _applicationType.Title;
            txtFees.Text = _applicationType.Fees.ToString();
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

        private void txtTitle_Validating(object sender, CancelEventArgs e)
        {
            if (!ValidateRequireField((TextBox)sender, "This field is required"))
                return;
        }

        private void txtFees_Validating(object sender, CancelEventArgs e)
        {
            if (!ValidateRequireField((TextBox)sender, "This field is required"))
                return;
        }
    }
}
