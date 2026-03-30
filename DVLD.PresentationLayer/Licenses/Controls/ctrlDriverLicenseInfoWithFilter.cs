using System;
using System.ComponentModel;
using System.Windows.Forms;
using DVLD.PresentationLayer.GlobalClasses;
using License = DVLD.EntityLayer.License;

namespace DVLD.PresentationLayer.Licenses.Controls
{
    public partial class ctrlDriverLicenseInfoWithFilter : UserControl
    {
        // delegate
        public event Action<int> OnLicenseSelected;
        protected virtual void LicenseSelected(int licenseId)
        {
            // If event has subscribers, invoke (send back) the license id
            OnLicenseSelected?.Invoke(licenseId);
        }

        private bool _filterEnabled = false;
        private string _filterText;

        // public: exposed properties
        public bool FilterEnabled
        {
            get { return _filterEnabled; }
            set
            {
                _filterEnabled = value;
                gbFilter.Enabled = _filterEnabled;
            }
        }
        public string FilterText 
        {
            get { return txtLicenseId.Text.Trim(); }
            set { txtLicenseId.Text = value; }
        }

        public int LicenseId => ctrlDriverLicenseInfo1.LicenseId;

        public License SelectedLicense => ctrlDriverLicenseInfo1.SelectedLicense;

        public ctrlDriverLicenseInfoWithFilter()
        {
            InitializeComponent();
        }

        private void txtLicenseId_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnSearch.PerformClick();
                e.Handled = true;
                return;
            }

            if (char.IsControl(e.KeyChar))
                return;

            if (!char.IsDigit(e.KeyChar))
                Utility.HandleWrongKey(e);
        }

        private void txtLicenseId_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLicenseId.Text.Trim()))
                errorProvider1.SetError(txtLicenseId, "This field is required");
            else
                errorProvider1.SetError(txtLicenseId, string.Empty);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!ValidateChildren())
                Utility.ShowErrorMessage("Some fields are not valid");

            _filterText = txtLicenseId.Text.Trim();

            if (int.TryParse(_filterText, out int licenseId))
                LoadLicenseInfo(licenseId);
            else
                Utility.ShowErrorMessage("Couldn't convert the filter text into a number");
        }

        private void LoadLicenseInfo(int licenseId)
        {
            ctrlDriverLicenseInfo1.LoadLicenseByLicenseId(licenseId);

            if (!FilterEnabled)
                return;

            // Only fire event if valid
            if (SelectedLicense != null)
            {
                OnLicenseSelected?.Invoke(licenseId);
            }
            else
            {
                OnLicenseSelected?.Invoke(0); // or 0
            }
        }
    }
}
