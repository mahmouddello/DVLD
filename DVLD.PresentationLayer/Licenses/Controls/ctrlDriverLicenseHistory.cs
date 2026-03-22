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

namespace DVLD.PresentationLayer.Licenses.Controls
{
    public partial class ctrlDriverLicenseHistory : UserControl
    {
        private Driver _driver;
        private DataTable localLicensesTable;
        private DataTable internationalLicensesTable;

        public ctrlDriverLicenseHistory()
        {
            InitializeComponent();
        }

        private DataTable GetLocalLicenses(int driverId)
        {
           return LicenseService.GetAllLicensesAsTable(driverId);
        }

        private DataTable GetInternationalLicense(int driverId)
        {
            return InternationalLicenseService.GetAllAsTable(driverId);
        }

        private void ApplyLocalDGVSettings()
        {
            if (dgvLocalLicenses.Rows.Count == 0)
                return;

            dgvLocalLicenses.Columns[0].HeaderText = "License ID";
            dgvLocalLicenses.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

            dgvLocalLicenses.Columns[1].HeaderText = "Application ID";
            dgvLocalLicenses.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

            dgvLocalLicenses.Columns[2].HeaderText = "Class";
            dgvLocalLicenses.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgvLocalLicenses.Columns[3].HeaderText = "Issue Date";
            dgvLocalLicenses.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

            dgvLocalLicenses.Columns[4].HeaderText = "Expiration Date";
            dgvLocalLicenses.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

            dgvLocalLicenses.Columns[5].HeaderText = "Is Active";
            dgvLocalLicenses.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
        }

        private void ApplyInternationalDGVSettings()
        {
            dgvIntLicenses.Columns[0].HeaderText = "Inter. License ID";
            dgvIntLicenses.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

            dgvIntLicenses.Columns[1].HeaderText = "Application ID";
            dgvIntLicenses.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

            dgvIntLicenses.Columns[2].HeaderText = "Driver ID";
            dgvIntLicenses.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgvIntLicenses.Columns[3].HeaderText = "L.License ID";
            dgvIntLicenses.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

            dgvIntLicenses.Columns[4].HeaderText = "Issue Date";
            dgvIntLicenses.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

            dgvIntLicenses.Columns[5].HeaderText = "Expiration Date";
            dgvIntLicenses.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

            dgvIntLicenses.Columns[6].HeaderText = "Is Active";
            dgvIntLicenses.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
        }

        private void LoadLocalLicenses()
        {
            localLicensesTable = GetLocalLicenses(_driver.Id);
            dgvLocalLicenses.DataSource = localLicensesTable;
            ApplyLocalDGVSettings();
        }

        private void LoadInternationalLicenses()
        {
            internationalLicensesTable = GetInternationalLicense(_driver.Id);
            dgvIntLicenses.DataSource = internationalLicensesTable;
            ApplyInternationalDGVSettings();
        }

        public void LoadDataByPersonId(int personId)
        {
            _driver = DriverService.FindByPersonId(personId);

            if (_driver == null)
            {
                Utility.ShowErrorMessage("Driver wasn't found!");
                return;
            }

            LoadLocalLicenses();
            LoadInternationalLicenses();
        }

        private void showLicenseInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int licenseId = Convert.ToInt32(dgvIntLicenses.CurrentRow.Cells[0].Value);
            frmShowLicenseInfo.CreateByLicenseId(licenseId).ShowDialog();
        }
    }
}
