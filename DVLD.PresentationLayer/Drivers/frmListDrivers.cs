using DVLD.BusinessLayer;
using DVLD.PresentationLayer.GlobalClasses;
using DVLD.PresentationLayer.Licenses;
using DVLD.PresentationLayer.People;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.PresentationLayer.Drivers
{
    public partial class frmListDrivers : Form
    {
        private DataTable drivers;

        private enum enFilter
        {
            None = 0,
            DriverID = 1,
            PersonID = 2,
            NationalNo = 3,
            FullName = 4,
        }
        private enFilter filter;

        public frmListDrivers()
        {
            InitializeComponent();
        }

        private string GetDBColumnName(enFilter filter)
        {
            switch (filter)
            {
                case enFilter.DriverID:
                    return "DriverID";

                case enFilter.PersonID:
                    return "PersonID";

                case enFilter.NationalNo:
                    return "NationalNo";

                case enFilter.FullName:
                    return "FullName";

                default:
                    return null;
            }
        }

        private void GetDataFromDB()
        {
            drivers = DriverService.GetAllAsTable();
        }

        private void RefreshDGVData()
        {
            dgvDrivers.DataSource = drivers;
            lblRecordsCount.Text = $"Records: #{dgvDrivers.Rows.Count}";
        }

        private void ReloadAndRefresh()
        {
            GetDataFromDB();
            RefreshDGVData();
        }

        private void ApplyDGVSettings()
        {
            dgvDrivers.DataSource = drivers;

            if (dgvDrivers.Rows.Count > 0)
            {
                dgvDrivers.Columns[0].HeaderText = "Driver ID";
                dgvDrivers.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

                dgvDrivers.Columns[1].HeaderText = "Person ID";
                dgvDrivers.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                dgvDrivers.Columns[2].HeaderText = "National No";
                dgvDrivers.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

                dgvDrivers.Columns[3].HeaderText = "Full Name";
                dgvDrivers.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

                dgvDrivers.Columns[4].HeaderText = "Created Date";
                dgvDrivers.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

                dgvDrivers.Columns[5].HeaderText = "Active Licenses";
                dgvDrivers.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            }
        }

        private void frmListDrivers_Load(object sender, EventArgs e)
        {
            GetDataFromDB();
            ApplyDGVSettings();

            // bind combobox to the enum
            cbFilter.DataSource = Enum.GetValues(typeof(enFilter));
        }

        private void btnCloseForm_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilter.SelectedItem == null)
                return;

            filter = (enFilter)cbFilter.SelectedItem;
            txtFilter.Visible = filter != enFilter.None;

            if (filter == enFilter.None)
            {
                RefreshDGVData();
                return;
            }

            // When filter changed, clear the textbox and refresh the list
            RefreshDGVData();
            txtFilter.Clear();
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            // The pressed key is : space, delete, backspace, ...etc. skips the checks.
            if (char.IsControl(e.KeyChar))
                return;

            switch (filter)
            {
                // Only Numeric
                case enFilter.DriverID:
                case enFilter.PersonID:
                    if (!char.IsDigit(e.KeyChar))
                        Utility.HandleWrongKey(e);
                    break;

                // Numeric + String
                default:
                    break;
            }
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string query = txtFilter.Text.Trim();

            if (string.IsNullOrEmpty(query))
            {
                RefreshDGVData();
                return;
            }

            bool isIdFilter = filter == enFilter.DriverID || filter == enFilter.PersonID;

            if (isIdFilter)
            {
                FilterById(query);
                return;
            }

            FilterByString(query);
        }

        private void FilterByString(string query)
        {
            DataView dv = new DataView(drivers)
            {
                RowFilter = $"{GetDBColumnName(filter)} LIKE '%{query}%'"
            };

            dgvDrivers.DataSource = dv;
            lblRecordsCount.Text = $"Records: #{dgvDrivers.Rows.Count}";
        }

        private void FilterById(string query)
        {
            if (!int.TryParse(query, out int id))
            {
                dgvDrivers.DataSource = drivers.Clone();
                lblRecordsCount.Text = "Records: #0";
                return;
            }

            DataView dv = new DataView(drivers)
            {
                RowFilter = $"{GetDBColumnName(filter)} = {id}"
            };

            dgvDrivers.DataSource = dv;
            lblRecordsCount.Text = $"Records: #{dgvDrivers.Rows.Count}";
        }

        private void showPersonInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int personId = (int)dgvDrivers.CurrentRow.Cells["PersonID"].Value;

            frmPersonDetails frm = new frmPersonDetails(personId);
            frm.ShowDialog();

            ReloadAndRefresh(); // In case of changes
        }

        private void issueInternationalLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utility.ShowWarningMessage(@"This feature isn't implemented yet!", "Warning");
        }

        private void shownPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int personId = (int)dgvDrivers.CurrentRow.Cells["PersonID"].Value;
            
            frmShowLicenseHistory frm = new frmShowLicenseHistory(personId); 
            frm.ShowDialog();

            ReloadAndRefresh(); // In case of changes
        }
    }
}
