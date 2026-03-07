using System;
using System.Data;
using System.Windows.Forms;
using DVLD.BusinessLayer;
using DVLD.EntityLayer;
using DVLD.PresentationLayer.Applications.ApplicationTypes;

namespace DVLD.PresentationLayer.ApplicationTypes
{
    public partial class frmApplicationTypes : Form
    {
        private DataTable applicationTypesTable;

        private void LoadRecordsFromDB()
        {
            applicationTypesTable = ApplicationTypeService.GetAllApplicationTypes();
        }

        private void RefreshApplicationTypesList()
        {
            dgvApplicationTypes.DataSource = applicationTypesTable;
            lblRecordsCount.Text = $"Records: #{dgvApplicationTypes.Rows.Count}";
        }

        private void ReloadAndRefresh()
        {
            LoadRecordsFromDB();
            RefreshApplicationTypesList();
        }

        private void ApplyDGVSettings()
        {
            if (dgvApplicationTypes.Columns.Count > 0)
            {
                dgvApplicationTypes.Columns[0].HeaderText = "ID";
                dgvApplicationTypes.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

                dgvApplicationTypes.Columns[1].HeaderText = "Title";
                dgvApplicationTypes.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                dgvApplicationTypes.Columns[2].HeaderText = "Fees";
                dgvApplicationTypes.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            }
        }

        public frmApplicationTypes()
        {
            InitializeComponent();
        }

        private void frmApplicationTypes_Load(object sender, EventArgs e)
        {
            LoadRecordsFromDB();
            RefreshApplicationTypesList();

            ApplyDGVSettings();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int appTypeId = (int)dgvApplicationTypes.CurrentRow.Cells[0].Value;

            frmUpdateApplicationType frm = new frmUpdateApplicationType((enApplicationType)appTypeId);
            frm.ShowDialog();

            ReloadAndRefresh(); // reload data and refresh the data grid view
        }

        private void btnCloseForm_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
