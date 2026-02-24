using DVLD.BusinessLayer;
using DVLD.EntityLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.PresentationLayer.Tests.TestTypes
{
    public partial class frmListTestTypes : Form
    {
        private DataTable testTypes;

        public frmListTestTypes()
        {
            InitializeComponent();
        }

        private void ApplyViewSettings()
        {
            dgvTestTypes.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;

            if (dgvTestTypes.Rows.Count > 0)
            {
                dgvTestTypes.Columns[0].HeaderText = "ID";
                dgvTestTypes.Columns[0].Width = 120;

                dgvTestTypes.Columns[1].HeaderText = "Title";
                dgvTestTypes.Columns[1].Width = 300;

                dgvTestTypes.Columns[2].HeaderText = "Description";
                dgvTestTypes.Columns[2].Width = 800;
                dgvTestTypes.Columns[2].DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                dgvTestTypes.Columns[3].HeaderText = "Fees";
                dgvTestTypes.Columns[3].Width = 120;
            }
        }

        private void LoadRecordsFromDB()
        {
            testTypes = TestTypeBusiness.GetAll();
        }

        private void RefreshTestTypesList()
        {
            dgvTestTypes.DataSource = testTypes;
            lblRecordsCount.Text = $"Records: #{dgvTestTypes.Rows.Count}";
        }

        private void ReloadAndRefresh()
        {
            LoadRecordsFromDB();
            RefreshTestTypesList();
        }

        private void btnCloseForm_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void frmListTestTypes_Load(object sender, EventArgs e)
        {
            LoadRecordsFromDB();

            RefreshTestTypesList();
            ApplyViewSettings();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int testTypeId = (int)dgvTestTypes.CurrentRow.Cells[0].Value;

            frmUpdateTestType frm = new frmUpdateTestType(testTypeId);
            frm.ShowDialog();

            ReloadAndRefresh(); // In case of update occurd.
        }
    }
}
