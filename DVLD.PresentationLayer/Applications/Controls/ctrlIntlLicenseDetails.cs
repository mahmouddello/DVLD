using DVLD.BusinessLayer;
using DVLD.EntityLayer;
using DVLD.PresentationLayer.GlobalClasses;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DVLD.PresentationLayer.Applications.Controls
{
    public partial class ctrlIntlLicenseDetails : UserControl
    {
        public ctrlIntlLicenseDetails()
        {
            InitializeComponent();
        }

        public void ResetInfo()
        {
            lblInternationalLicenseApplicationId.Text = "???";
            lblId.Text = "???";
            lblLocalLicenseId.Text = "???";
            lblApplicationDate.Text = "???";
            lblIssueDate.Text = "???";
            lblFees.Text = "???";
            lblExpirationDate.Text = "???";
            lblCreatedBy.Text = "???";
        }

        public void FillInfo(int licenseId)
        {
            lblLocalLicenseId.Text = licenseId.ToString();
            lblApplicationDate.Text = DateTime.Now.ToShortDateString();
            lblIssueDate.Text = DateTime.Now.ToShortDateString();

            decimal fees = ApplicationTypeService.FindByType(enApplicationType.NewInternationalLicense).Fees;
            lblFees.Text = fees.ToString();

            DateTime expirationDate = DateTime.Now.AddYears(Properties.Settings.Default.IntrLicenseValidityLength);
            lblExpirationDate.Text = expirationDate.ToShortDateString();
            lblCreatedBy.Text = Globals.CurrentUser.Username;
        }
    }
}
