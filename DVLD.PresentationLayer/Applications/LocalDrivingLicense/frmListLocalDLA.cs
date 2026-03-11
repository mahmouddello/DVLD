using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using DVLD.BusinessLayer;
using DVLD.EntityLayer;
using DVLD.PresentationLayer.GlobalClasses;
using DVLD.PresentationLayer.Licenses;
using DVLD.PresentationLayer.Tests;

namespace DVLD.PresentationLayer.Applications.LocalDrivingLicense
{
    public partial class frmListLocalDLA : Form
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
            ldlaTable = LDLAService.GetAllAsTable();
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

        private void ApplyDGVSettings()
        {
            if (dgvLDLA.Columns.Count > 0)
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
        }

        private void InitializeApplicationsView()
        {
            LoadFromDB();
            RefreshApplicationsList();

            ApplyDGVSettings();
        }

        private void InitializeFilters()
        {
            cbFilter.DataSource = Enum.GetValues(typeof(Filter));
            cbStatus.DataSource = Enum.GetValues(typeof(StatusFilter));
        }

        public frmListLocalDLA()
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
            frmAddEditLocalDLA frm = new frmAddEditLocalDLA();
            frm.ShowDialog();

            ReloadAndRefresh(); // In Case of changes
        }

        private void cancelApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvLDLA.CurrentRow == null) 
                return;

            int ldlaId = (int)dgvLDLA.CurrentRow.Cells[0].Value;
            LDLA ldla = LDLAService.FindById(ldlaId);

            var result = MessageBox.Show(
                "Are you sure you want to cancel this application?",
                "Cancel Application",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes && ldla.MainApplicationInfo != null)
            {
                var service = new ApplicationService(ldla.MainApplicationInfo);

                if (service.Cancel())
                    Utility.ShowSuccessMessage("Cancelled the application succesfully!");
                else
                    Utility.ShowErrorMessage("User cancelled operation or Error occurd!");
            }

            cbFilter.SelectedIndex = 0;
            ReloadAndRefresh();
        }

        private void deleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvLDLA.CurrentRow == null)
                return;

            int ldlaId = (int)dgvLDLA.CurrentRow.Cells[0].Value;
            LDLA ldla = LDLAService.FindById(ldlaId);

            var result = MessageBox.Show(
                "Are you sure you want to delete this application?",
                 "Delete Application",
                 MessageBoxButtons.YesNo,
                 MessageBoxIcon.Error
            );

            if (result == DialogResult.Yes && ldla.MainApplicationInfo != null)
            {
                if (ApplicationService.Delete(ldla.MainApplicationId))
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

            int ldlaId = (int)dgvLDLA.CurrentRow.Cells[0].Value;
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

                scheduleVisionTestToolStripMenuItem.Enabled = passedTests == 0;
                scheduleWrittenTestToolStripMenuItem.Enabled = TestService.HasPassedTest(ldlaId, 1) && !TestService.HasPassedTest(ldlaId, 2);
                scheduleStreetTestToolStripMenuItem.Enabled = TestService.HasPassedTest(ldlaId, 2) && !TestService.HasPassedTest(ldlaId, 3);
            }
        }

        // Helper method to disable multiple items at once
        private void SetEnabled(bool enabled, params ToolStripItem[] items)
        {
            foreach (var item in items) item.Enabled = enabled;
        }

        private void visionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ldlaId = (int)dgvLDLA.CurrentRow.Cells[0].Value;
            frmTestAppointments frm = new frmTestAppointments(ldlaId, 1);

            frm.ShowDialog();
        }

        private void scheduleVisionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ldlaId = (int)dgvLDLA.CurrentRow.Cells[0].Value;
            frmTestAppointments frm = new frmTestAppointments(ldlaId, 2);

            frm.ShowDialog();
        }

        private void scheduleVisionTestToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int ldlaId = (int)dgvLDLA.CurrentRow.Cells[0].Value;
            frmTestAppointments frm = new frmTestAppointments(ldlaId, 3);

            frm.ShowDialog();
        }

        private void issueDrivingLicenseFirstTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ldlaId = (int)dgvLDLA.CurrentRow.Cells[0].Value;
            frmIssueDrivingLicense form = new frmIssueDrivingLicense(ldlaId);

            form.ShowDialog();
            RefreshApplicationsList();
        }

        private void showLicenseInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ldlaId = (int)dgvLDLA.CurrentRow.Cells[0].Value;
            frmShowLicenseInfo.CreateByLdlaId(ldlaId).ShowDialog();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ldlaId = (int)dgvLDLA.CurrentRow.Cells[0].Value;
            var ldla = LDLAService.FindById(ldlaId);

            var form = new frmShowLicenseHistory(ldla.MainApplicationInfo.ApplicantPersonId);
            form.ShowDialog();
        }

        private void showApplicationDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ldlaId = (int)dgvLDLA.CurrentRow.Cells[0].Value;

            frmShowLocalDLADetails frm = new frmShowLocalDLADetails(ldlaId);
            frm.ShowDialog();
        }

        private void editApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ldlaId = (int)dgvLDLA.CurrentRow.Cells[0].Value;

            frmAddEditLocalDLA frm = new frmAddEditLocalDLA(ldlaId);
            frm.ShowDialog();

            ReloadAndRefresh();
        }
    }
}
