using DVLD.BusinessLayer;
using DVLD.EntityLayer;
using DVLD.PresentationLayer.Applications.ApplicationTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.PresentationLayer.ApplicationTypes
{
    public partial class frmApplicationTypes : Form
    {
        private DataTable applicationTypes;

        private void LoadRecordsFromDB()
        {
            applicationTypes = ApplicationTypeBusiness.GetAll();
        }

        private void RefreshApplicationTypesList()
        {
            dgvApplicationTypes.DataSource = applicationTypes;
            lblRecordsCount.Text = $"Records: #{dgvApplicationTypes.Rows.Count}";
        }

        private void ReloadAndRefresh()
        {
            LoadRecordsFromDB();
            RefreshApplicationTypesList();
        }

        private void ApplyViewSettings()
        {
            dgvApplicationTypes.Columns[0].HeaderText = "ID";
            dgvApplicationTypes.Columns[0].Width = 200;

            dgvApplicationTypes.Columns[1].HeaderText = "Title";
            dgvApplicationTypes.Columns[1].Width = 520;

            dgvApplicationTypes.Columns[2].HeaderText = "Fees";
            dgvApplicationTypes.Columns[2].Width = 200;
        }

        public frmApplicationTypes()
        {
            InitializeComponent();
        }

        private void btnCloseForm_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmApplicationTypes_Load(object sender, EventArgs e)
        {
            LoadRecordsFromDB(); // loads application types

            RefreshApplicationTypesList();
            ApplyViewSettings();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int applicationId = (int)dgvApplicationTypes.CurrentRow.Cells[0].Value;

            frmUpdateApplicationType frm = new frmUpdateApplicationType((ApplicationType.enApplicationType)applicationId);
            frm.ShowDialog();

            ReloadAndRefresh(); // reload data and refresh the data grid view
        }
    }
}
