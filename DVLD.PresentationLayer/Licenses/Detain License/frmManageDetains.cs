using DVLD.BusinessLayer;
using DVLD.EntityLayer;
using DVLD.PresentationLayer.GlobalClasses;
using DVLD.PresentationLayer.People;
using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DVLD.PresentationLayer.Licenses.Detain_License
{
    public partial class frmManageDetains : Form
    {
        private enum Filter
        { 
            None = 0,
            DetainId = 1,
            LicenseId = 2,
            IsReleased = 3,
            NationalNo = 4,
            FullName = 5,
            ReleaseApplicationId = 6
        }

        private enum ReleaseStatus
        {
            All = -1,
            No = 0,
            Yes = 1
        }
        
        private DataTable _detainsTable;
        private Filter _filter;
        private ReleaseStatus _status;

        public frmManageDetains()
        {
            InitializeComponent();
        }

        private void ConfigureDGV()
        {
            if (dgvDetains.Columns.Count == 0)
                return;

            dgvDetains.Columns[0].HeaderText = "Detain ID";
            dgvDetains.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

            dgvDetains.Columns[1].HeaderText = "License ID";
            dgvDetains.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

            dgvDetains.Columns[2].HeaderText = "Detain Date";
            dgvDetains.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgvDetains.Columns[3].HeaderText = "Fine Fees";
            dgvDetains.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

            dgvDetains.Columns[4].HeaderText = "N.No";
            dgvDetains.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

            dgvDetains.Columns[5].HeaderText = "Full Name";
            dgvDetains.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgvDetains.Columns[6].HeaderText = "Is Released";
            dgvDetains.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

            dgvDetains.Columns[7].HeaderText = "Release App.ID";
            dgvDetains.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void RefreshData()
        {
            LoadDetainsData();
            BindDetainsToGrid();
        }

        private void LoadDetainsData()
        {
            _detainsTable = DetainLicenseService.GetAll();
        }

        private void BindDetainsToGrid()
        {
            dgvDetains.DataSource = _detainsTable;
            lblRecordsCount.Text = $"Records: #{dgvDetains.Rows.Count}";
        }

        private void BindEnumsToComboBoxes()
        {
            cmbFilter.DataSource = Enum.GetValues(typeof(Filter));
            cmbFilter.DropDownStyle = ComboBoxStyle.DropDownList;

            cmbReleaseStatus.DataSource = Enum.GetValues(typeof(ReleaseStatus));
            cmbReleaseStatus.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void ClearAndFocusOnQueryTextBox()
        {
            txtQuery.Clear();
            txtQuery.Focus();
        }

        private string GetColumnName()
        {
            switch (_filter)
            {
                case Filter.DetainId:
                    return "DetainID";

                case Filter.LicenseId:
                    return "LicenseID";

                case Filter.IsReleased:
                    return "IsReleased";

                case Filter.NationalNo:
                    return "NationalNo";

                case Filter.FullName:
                    return "FullName";

                case Filter.ReleaseApplicationId:
                    return "ReleaseApplicationID";

                default:
                    return string.Empty;
            }
        }

        private void ResetDetainsList()
        {
            dgvDetains.DataSource = _detainsTable;
            lblRecordsCount.Text = $"Records: #{dgvDetains.Rows.Count}";
        }

        private void frmManageDetains_Load(object sender, EventArgs e)
        {
            BindEnumsToComboBoxes();
            LoadDetainsData();
            BindDetainsToGrid();
            ConfigureDGV();
        }

        private void btnDetain_Click(object sender, EventArgs e)
        {
            frmDetainLicense form = new frmDetainLicense();
            form.ShowDialog();

            // Refresh 
            RefreshData();
        }

        private void btnRelease_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicense form = new frmReleaseDetainedLicense();
            form.ShowDialog();

            // Refresh 
            RefreshData();
        }

        private void btnCloseForm_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            _filter = (Filter)cmbFilter.SelectedIndex;
            txtQuery.Visible = _filter != Filter.None && _filter != Filter.IsReleased;
            cmbReleaseStatus.Visible = _filter == Filter.IsReleased;

            switch(_filter)
            {
                case Filter.None:
                    ResetDetainsList();;
                    break;
                case Filter.IsReleased:
                    cmbReleaseStatus.SelectedIndex = 0;
                    break;
                default:
                    ResetDetainsList();
                    ClearAndFocusOnQueryTextBox();
                    break;
            }
        }

        private void cmbReleaseStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            _status = (ReleaseStatus)cmbReleaseStatus.SelectedIndex;

            if (_status == ReleaseStatus.All)
            {
                ResetDetainsList();
                return;
            }

            bool isReleased = _status == ReleaseStatus.Yes;

            DataView dv = new DataView(_detainsTable);
            dv.RowFilter = $"IsReleased = {isReleased}";

            dgvDetains.DataSource = dv;
            lblRecordsCount.Text = $"Records: #{dgvDetains.Rows.Count}";
        }

        private void txtQuery_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow control keys (backspace, etc.)
            if (char.IsControl(e.KeyChar))
                return;

            // Full Name -> letters only
            if (_filter == Filter.FullName)
            {
                if (!char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
                    Utility.HandleWrongKey(e);

                return;
            }

            // National No -> allow everything (letters + digits)
            if (_filter == Filter.NationalNo)
                return;

            // Rest filters -> Only Numbers
            if (!char.IsDigit(e.KeyChar))
                Utility.HandleWrongKey(e);
        }

        private void txtQuery_TextChanged(object sender, EventArgs e)
        {
            string query = txtQuery.Text.Trim();

            if (string.IsNullOrEmpty(query))
            {
                ResetDetainsList();
                return;
            }

            DataView dataView = new DataView(_detainsTable);

            if (_filter == Filter.FullName || _filter == Filter.NationalNo)
                dataView.RowFilter = $"{GetColumnName()} LIKE '%{query}%'";
            else
                dataView.RowFilter = $"{GetColumnName()} = {query}";

            dgvDetains.DataSource = dataView;
            lblRecordsCount.Text = $"Records: #{dgvDetains.Rows.Count}";
        }

        private License FetchLocalLicense()
        {
            int licenseId = (int)dgvDetains.CurrentRow.Cells[1].Value;
            return LicenseService.FindById(licenseId);
        }

        private void showPersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var localLicense = FetchLocalLicense();

            if (localLicense == null)
            {
                Utility.ShowErrorMessage("Local License Info wasn't found");
                return;
            }

            var form = new frmPersonDetails(localLicense.MainApplicationInfo.ApplicantPersonId);
            form.ShowDialog();
            
            // refresh
            RefreshData();
        }

        private void showLicenseInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var localLicense = FetchLocalLicense();

            if (localLicense == null)
            {
                Utility.ShowErrorMessage("Local License Info wasn't found");
                return;
            }

            frmShowLicenseInfo form = frmShowLicenseInfo.CreateByLicenseId(localLicense.Id);
            form.ShowDialog();

            // refresh
            RefreshData();
        }

        private void showLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var localLicense = FetchLocalLicense();

            if (localLicense == null)
            {
                Utility.ShowErrorMessage("Local License Info wasn't found");
                return;
            }

            frmShowLicenseHistory form = new frmShowLicenseHistory(localLicense.MainApplicationInfo.ApplicantPersonId);
            form.ShowDialog();
        }

        private void releaseDetainedLicenseToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var localLicense = FetchLocalLicense();

            if (localLicense == null)
            {
                Utility.ShowErrorMessage("Local License Info wasn't found");
                return;
            }

            frmReleaseDetainedLicense form = new frmReleaseDetainedLicense(localLicense);
            form.ShowDialog();
        }

        private void cmsOptions_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            bool isReleased = (bool)dgvDetains.CurrentRow.Cells[6].Value;

            if (isReleased)
                releaseDetainedLicenseToolStripMenuItem1.Enabled = false;
            else
                releaseDetainedLicenseToolStripMenuItem1.Enabled = true;
        }
    }
}
