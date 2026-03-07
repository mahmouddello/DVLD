using DVLD.BusinessLayer;
using DVLD.EntityLayer;
using DVLD.PresentationLayer.GlobalClasses;
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
        private ApplicationType _applicationType;
        private enApplicationType _appType;

        public frmUpdateApplicationType(enApplicationType appType)
        {
            InitializeComponent();
            _appType = appType;
        }

        private void frmUpdateApplicationType_Load(object sender, EventArgs e)
        {
            LoadApplicationInfo();
        }

        private void LoadApplicationInfo()
        {
            _applicationType = ApplicationTypeService.FindByType(_appType);

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
            lblID.Text = ((int)_applicationType.Type).ToString();
            txtTitle.Text = _applicationType.Title;
            txtFees.Text = _applicationType.Fees.ToString();
        }

        private bool ValidateRequiredField(TextBox textBox, string message)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text.Trim()))
            {
                errProvider.SetError(textBox, message);
                return false;
            }

            errProvider.SetError(textBox, string.Empty);
            return true;
        }

        private void txtTitle_Validating(object sender, CancelEventArgs e)
        {
            if (!ValidateRequiredField((TextBox)sender, "This field is required"))
                return;
        }

        private void txtFees_Validating(object sender, CancelEventArgs e)
        {
            if (!ValidateRequiredField((TextBox)sender, "This field is required"))
                return;

            if (!decimal.TryParse(txtFees.Text.Trim(), out _))
                errProvider.SetError(txtFees, "Please enter a valid decimal number.");
            else
                errProvider.SetError(txtFees, string.Empty);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateChildren())
                return;

            if (!decimal.TryParse(txtFees.Text.Trim(), out decimal applicationFees))
                return;

            _applicationType.Title = txtTitle.Text.Trim();
            _applicationType.Fees = applicationFees;

            var appTypeService = new ApplicationTypeService(_applicationType);

            if (!appTypeService.Save())
            {
                Utility.ShowErrorMessage("Failed to update the application type");
                return;
            }

            Utility.ShowSuccessMessage("Updated successfully");
            FillApplicationInfo();
        }
    }
}
