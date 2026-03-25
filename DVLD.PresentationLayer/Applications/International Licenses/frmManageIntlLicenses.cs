using DVLD.EntityLayer;
using DVLD.PresentationLayer.GlobalClasses;
using DVLD.PresentationLayer.Licenses;
using DVLD.PresentationLayer.Licenses.International_License;
using DVLD.PresentationLayer.Licenses.International_Licenses;
using DVLD.PresentationLayer.People;
using System;
using System.Data;
using System.Windows.Forms;
using ILS = DVLD.BusinessLayer.InternationalLicenseService;

namespace DVLD.PresentationLayer.Applications.International_Licenses
{
    public partial class frmManageIntlLicenses : Form
    {
        private enum GeneralFilter
        {
            None = 0,
            InternationalLicenseID = 1,
            ApplicationID = 2,
            DriverID = 3,
            LocalLicenseID = 4,
            IsActive = 5,
        }

        private enum ActivityFilter
        {
            No = 0,
            Yes = 1,
            All = 2,
        }

        private DataTable _intlLicensesTable;
        private GeneralFilter _generalFilter;
        private ActivityFilter _activityFilter;

        public frmManageIntlLicenses()
        {
            InitializeComponent();
        }

        private void SetDGVSettings()
        {
            if (dgvInternationalLicenses.Rows.Count > 0)
            {
                dgvInternationalLicenses.Columns[0].HeaderText = "Int. License ID";
                dgvInternationalLicenses.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                dgvInternationalLicenses.Columns[1].HeaderText = "Application ID";
                dgvInternationalLicenses.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                dgvInternationalLicenses.Columns[2].HeaderText = "Driver ID";
                dgvInternationalLicenses.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

                dgvInternationalLicenses.Columns[3].HeaderText = "L. License ID";
                dgvInternationalLicenses.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                dgvInternationalLicenses.Columns[4].HeaderText = "Issue Date";
                dgvInternationalLicenses.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

                dgvInternationalLicenses.Columns[5].HeaderText = "Expiration Date";
                dgvInternationalLicenses.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

                dgvInternationalLicenses.Columns[6].HeaderText = "Is Active";
                dgvInternationalLicenses.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        private void ResetDGVData()
        {
            dgvInternationalLicenses.DataSource = _intlLicensesTable;
            lblRecordsCount.Text = $"Records: #{dgvInternationalLicenses.Rows.Count}";
        }

        private void InitializeForm()
        {
            _intlLicensesTable = ILS.GetAllAsTable();

            if (_intlLicensesTable == null)
            {
                Utility.ShowWarningMessage("System didn't find any international license", "Not Found");
                this.Close();
                return;
            }

            dgvInternationalLicenses.DataSource = _intlLicensesTable;
            lblRecordsCount.Text = $"Records: #{dgvInternationalLicenses.Rows.Count}";
            SetDGVSettings();
        }

        private void InitializeComboBoxes()
        {
            cmbGeneralFilter.DataSource = Enum.GetValues(typeof(GeneralFilter));
            cmbActivityFilter.DataSource = Enum.GetValues(typeof(ActivityFilter));

            cmbGeneralFilter.SelectedIndex = 0; // Default, None
            cmbActivityFilter.SelectedIndex = 0; // Default, All
        }

        private void frmManageIntlLicenses_Load(object sender, EventArgs e)
        {
            InitializeComboBoxes();
            InitializeForm();
        }

        private void btnCloseForm_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ApplyFilters()
        {
            if (_intlLicensesTable == null)
                return;

            if (string.IsNullOrWhiteSpace(txtQuery.Text.Trim()))
            {
                _intlLicensesTable.DefaultView.RowFilter = string.Empty; // clear filter
                ResetDGVData();
                return;
            }

            string filter = string.Empty;

            if (_generalFilter == GeneralFilter.IsActive)
                filter = BuildActivityFilter();
            else
            {
                switch (_generalFilter)
                {
                    case GeneralFilter.InternationalLicenseID:
                        filter = BuildNumericFilter("InternationalLicenseID");
                        break;
                    case GeneralFilter.ApplicationID:
                        filter = BuildNumericFilter("ApplicationID");
                        break;
                    case GeneralFilter.DriverID:
                        filter = BuildNumericFilter("DriverID");
                        break;
                    case GeneralFilter.LocalLicenseID:
                        filter = BuildNumericFilter("IssuedUsingLocalLicenseID");
                        break;
                }
            }

            DataView dv = _intlLicensesTable.DefaultView;
            dv.RowFilter = filter;

            dgvInternationalLicenses.DataSource = dv;
            lblRecordsCount.Text = $"Records: #{dv.Count}";
        }

        private string BuildActivityFilter()
        {
            switch (_activityFilter)
            {
                case ActivityFilter.Yes:
                    return "IsActive = true";

                case ActivityFilter.No:
                    return "IsActive = false";

                default:
                    return string.Empty;
            }
        }

        private string BuildNumericFilter(string columnName)
        {
            if (int.TryParse(txtQuery.Text.Trim(), out int value))
                return $"{columnName} = {value}";

            return string.Empty;
        }

        private void cmbGeneralFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            _generalFilter = (GeneralFilter)cmbGeneralFilter.SelectedIndex;
            txtQuery.Visible = _generalFilter != GeneralFilter.IsActive && _generalFilter != GeneralFilter.None;
            cmbActivityFilter.Visible = _generalFilter == GeneralFilter.IsActive;

            txtQuery.Clear();
            ApplyFilters();
        }

        private void cmbActivityFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            _activityFilter = (ActivityFilter)cmbActivityFilter.SelectedIndex;
            ApplyFilters();
        }

        private void txtQuery_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow control characters (backspace, delete, etc.)
            if (char.IsControl(e.KeyChar))
                return;

            // All fields expect numeric input
            if (!char.IsDigit(e.KeyChar))
                Utility.HandleWrongKey(e);
        }

        private void txtQuery_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void btnAddNewLicense_Click(object sender, EventArgs e)
        {
            var form = new frmInternationalLicenseApplication();
            form.ShowDialog();

            // Reload form
            frmManageIntlLicenses_Load(null, null);
        }

        private InternationalLicense FetchIntlLicense()
        {
            int intelLicenseId = (int)dgvInternationalLicenses.CurrentRow.Cells[0].Value;
            return ILS.FindById(intelLicenseId);
        }

        private void showPersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var internationalLicense = FetchIntlLicense();

            if (internationalLicense == null)
            {
                Utility.ShowErrorMessage("International License Info wasn't found");
                return;
            }

            var form = new frmPersonDetails(internationalLicense.ApplicationInfo.ApplicantPersonId);
            form.ShowDialog();
            
            frmManageIntlLicenses_Load(null, null);
        }

        private void showLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var internationalLicense = FetchIntlLicense();

            if (internationalLicense == null)
            {
                Utility.ShowErrorMessage("International License Info wasn't found");
                return;
            }

            var form = new frmShowLicenseHistory(internationalLicense.ApplicationInfo.ApplicantPersonId);
            form.ShowDialog();

            frmManageIntlLicenses_Load(null, null);
        }

        private void showLicenseInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int intelLicenseId = (int)dgvInternationalLicenses.CurrentRow.Cells[0].Value;

            frmInternationalLicenseDetails form = new frmInternationalLicenseDetails(intelLicenseId);
            form.ShowDialog();

            frmManageIntlLicenses_Load(null, null);
        }
    }
}
