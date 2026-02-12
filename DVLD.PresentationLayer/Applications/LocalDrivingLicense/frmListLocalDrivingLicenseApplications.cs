using DVLD.BusinessLayer;
using DVLD.EntityLayer;
using DVLD.PresentationLayer.GlobalClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.PresentationLayer.Applications.LocalDrivingLicense
{
    public partial class frmListLocalDrivingLicenseApplications : Form
    {
        private enum Filter
        {
            None,
            LdlaId,
            ClassName,
            NationalNo,
            FullName,
            Status
        }

        private enum StatusFilter
        {
            All,
            New,
            Cancelled,
            Completed
        }

        private string GetColumnName(Filter filter)
        {
            switch (filter)
            {
                case Filter.LdlaId:
                    return "LocalDrivingLicenseApplicationID";

                case Filter.ClassName:
                    return "ClassName";

                case Filter.NationalNo:
                    return "NationalNo";

                case Filter.FullName:
                    return "FullName";

                case Filter.Status:
                    return "Status";

                default:
                    return null;
            }
        }

        private string GetStatusFilterValue(StatusFilter statusFilter)
        {
            switch (statusFilter)
            {
                case StatusFilter.New:
                    return "New";

                case StatusFilter.Cancelled:
                    return "Cancelled";

                case StatusFilter.Completed:
                    return "Completed";

                default:
                    return null;
            }
        }


        private Filter filter;
        private StatusFilter statusFilter;
        private DataTable ldlaTable;

        private void LoadFromDB()
        {
            ldlaTable = LocalDrivingLicenseApplicationBusiness.GetAll();
        }

        private void RefreshApplicationsList()
        {
            dgvLDLA.DataSource = ldlaTable;
            lblRecordsCount.Text = $"Records: #{dgvLDLA.Rows.Count}";
        }

        private void ReloadAndRefresh()
        {
            LoadFromDB();
            RefreshApplicationsList();
        }

        private void ApplyViewSettings()
        {

            dgvLDLA.Columns[0].HeaderText = "LDLA ID";
            dgvLDLA.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

            dgvLDLA.Columns[1].HeaderText = "Class Name";
            dgvLDLA.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgvLDLA.Columns[2].HeaderText = "N.No";
            dgvLDLA.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

            dgvLDLA.Columns[3].HeaderText = "Full Name";
            dgvLDLA.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgvLDLA.Columns[4].HeaderText = "Application Date";
            dgvLDLA.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

            dgvLDLA.Columns[5].HeaderText = "Passed Tests";
            dgvLDLA.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

            dgvLDLA.Columns[6].HeaderText = "Status";
            dgvLDLA.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
        }

        private void InitializeApplicationsView()
        {
            ReloadAndRefresh();   // Load + bind
            ApplyViewSettings();  // UI formatting
        }

        private void InitializeFilters()
        {
            cbFilter.DataSource = Enum.GetValues(typeof(Filter));
            cbStatus.DataSource = Enum.GetValues(typeof(StatusFilter));
        }

        public frmListLocalDrivingLicenseApplications()
        {
            InitializeComponent();
        }

        private void frmListLocalDrivingLicenseApplications_Load(object sender, EventArgs e)
        {
            InitializeApplicationsView();
            InitializeFilters();
        }

        private void btnCloseForm_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilter.SelectedItem == null)
                return;

            filter = (Filter)cbFilter.SelectedItem;
            txtQuery.Visible = filter != Filter.Status && filter != Filter.None;
            cbStatus.Visible = filter == Filter.Status;

            switch (filter)
            {
                case Filter.None:
                    RefreshApplicationsList();
                    break;

                case Filter.Status:

                    cbStatus.SelectedItem = StatusFilter.All;
                    break;

                default:
                    RefreshApplicationsList();
                    txtQuery.Clear();
                    break;
            }
        }

        private void txtQuery_TextChanged(object sender, EventArgs e)
        {
            string query = txtQuery.Text.Trim();

            if (string.IsNullOrEmpty(query))
            {
                RefreshApplicationsList();
                return;
            }

            DataView dv = new DataView(ldlaTable);

            if (filter == Filter.LdlaId)
            {
                if (!int.TryParse(query, out int ldlaId))
                {
                    dgvLDLA.DataSource = ldlaTable.Clone();
                    lblRecordsCount.Text = "Records: #0";
                    return;
                }

                dv.RowFilter = $"{GetColumnName(filter)} = {ldlaId}";
            }
            else
                dv.RowFilter = $"{GetColumnName(filter)} LIKE '%{query}%'";

            dgvLDLA.DataSource = dv;
            lblRecordsCount.Text = $"Records: #{dgvLDLA.Rows.Count}";
        }

        private void txtQuery_KeyPress(object sender, KeyPressEventArgs e)
        {
            // The pressed key is : space, delete, backspace, ...etc. skips the checks.
            if (char.IsControl(e.KeyChar))
                return;

            switch (filter)
            {
                // only numeric
                case Filter.LdlaId:
                    if (!char.IsDigit(e.KeyChar))
                        Utility.HandleWrongKey(e);
                    break;

                // string + numeric
                default:
                    break;
            }
        }

        private void cbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            statusFilter = (StatusFilter)cbStatus.SelectedItem;

            if (statusFilter == StatusFilter.All)
            {
                RefreshApplicationsList();
                return;
            }

            DataView dv = new DataView(ldlaTable);
            dv.RowFilter = $"Status = '{GetStatusFilterValue(statusFilter)}'";

            dgvLDLA.DataSource = dv;
            lblRecordsCount.Text = $"Records: #{dgvLDLA.Rows.Count}";
        }

        private void btnNewLDLA_Click(object sender, EventArgs e)
        {
            frmLocalDrivingLicenseApplication frm = new frmLocalDrivingLicenseApplication();
            frm.ShowDialog();

            ReloadAndRefresh(); // In Case of changes
        }

        private void cancelApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvLDLA.CurrentRow == null) return;

            int ldlaId = (int)dgvLDLA.CurrentRow.Cells[0].Value;
            LocalDrivingLicenseApplication ldla = LocalDrivingLicenseApplicationBusiness.Find(ldlaId);

            var result = MessageBox.Show("Are you sure you want to cancel this application?",
                                         "Cancel Application",
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Warning);

            if (result == DialogResult.Yes && ldla.MainApplicationInfo != null)
            {
                if (ApplicationBusiness.Cancel(ldla.MainApplicationInfo))
                    MessageBox.Show("Cancelled the application succesfully!");
                else
                    MessageBox.Show("User cancelled operation or Error occurd!");
            }

            cbFilter.SelectedIndex = 0;
            ReloadAndRefresh();
        }

        private void deleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvLDLA.CurrentRow == null)
                return;

            int ldlaId = (int)dgvLDLA.CurrentRow.Cells[0].Value;
            LocalDrivingLicenseApplication ldla = LocalDrivingLicenseApplicationBusiness.Find(ldlaId);

            var result = MessageBox.Show("Are you sure you want to delete this application?",
                                         "Delete Application",
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Error);

            if (result == DialogResult.Yes && ldla.MainApplicationInfo != null)
            {
                if (ApplicationBusiness.Delete(ldla.MainApplicationInfo))
                    MessageBox.Show("Deleted the application succesfully!");
                else
                    MessageBox.Show("User cancelled operation or Error occurd!");
            }

            cbFilter.SelectedIndex = 0;
            ReloadAndRefresh();
        }

        private void cmsLDLA_Opening(object sender, CancelEventArgs e)
        {
            // Don't show the menu if there's no row selected
            if (dgvLDLA.CurrentRow == null)
            {
                e.Cancel = true;
                return;
            }

            // Reset everything to a clean state
            foreach (ToolStripItem item in cmsLDLA.Items)
            {
                item.Enabled = true;
            }

            string status = dgvLDLA.CurrentRow.Cells["Status"].Value?.ToString();
            int passedTests = Convert.ToInt32(dgvLDLA.CurrentRow.Cells["PassedTests"].Value);

            // Logical override
            if (status == "Cancelled")
            {
                SetEnabled(false, editApplicationToolStripMenuItem, deleteApplicationToolStripMenuItem,
                           cancelApplicationToolStripMenuItem, scheduleTestToolStripMenuItem,
                           issueDrivingLicenseFirstTimeToolStripMenuItem, showLicenseInfoToolStripMenuItem);
            }
            else if (status == "Completed")
            {
                SetEnabled(false, editApplicationToolStripMenuItem, deleteApplicationToolStripMenuItem,
                           cancelApplicationToolStripMenuItem, scheduleTestToolStripMenuItem,
                           issueDrivingLicenseFirstTimeToolStripMenuItem);
            }
            else // New Application logic
            {
                scheduleTestToolStripMenuItem.Enabled = (passedTests < 3);
                issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = (passedTests == 3);
                showLicenseInfoToolStripMenuItem.Enabled = false; // Usually not available for new apps
            }
        }

        // Helper method to disable multiple items at once
        private void SetEnabled(bool enabled, params ToolStripItem[] items)
        {
            foreach (var item in items) item.Enabled = enabled;
        }

        private void visionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show
            (
                "This feature will be implemented in the future",
                "Stub",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
        }

        private void scheduleVisionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show
             (
                 "This feature will be implemented in the future",
                 "Stub",
                 MessageBoxButtons.OK,
                 MessageBoxIcon.Warning
             );
        }

        private void scheduleVisionTestToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show
             (
                 "This feature will be implemented in the future",
                 "Stub",
                 MessageBoxButtons.OK,
                 MessageBoxIcon.Warning
             );
        }

        private void issueDrivingLicenseFirstTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show
             (
                 "This feature will be implemented in the future",
                 "Stub",
                 MessageBoxButtons.OK,
                 MessageBoxIcon.Warning
             );
        }

        private void showLicenseInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show
             (
                 "This feature will be implemented in the future",
                 "Stub",
                 MessageBoxButtons.OK,
                 MessageBoxIcon.Warning
             );
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show
             (
                 "This feature will be implemented in the future",
                 "Stub",
                 MessageBoxButtons.OK,
                 MessageBoxIcon.Warning
             );
        }
    }
}
