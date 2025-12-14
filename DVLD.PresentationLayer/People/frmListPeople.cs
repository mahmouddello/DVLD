using System;
using System.Data;
using System.Windows.Forms;
using DVLD.BusinessLayer;

namespace DVLD.PresentationLayer.People
{
    public partial class frmListPeople : Form
    {
        DataTable _peopleDataTable;
        
        public frmListPeople()
        {
            InitializeComponent();
        }

        private void _LoadData()
        {
            _peopleDataTable = PersonBusiness.GetAllPeople();
        }

        private void _RefreshPeopleList()
        {
            dgvAllPeople.DataSource = _peopleDataTable;
            lblRecordsCount.Text = $"Records: #{dgvAllPeople.Rows.Count}";
        }

        private void frmListPeople_Load(object sender, EventArgs e)
        {
            _LoadData();
            _RefreshPeopleList();
        }

        private void btnCloseForm_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int currentRowPersonID = (int)dgvAllPeople.CurrentRow.Cells[0].Value;

            frmPersonDetails form = new frmPersonDetails(currentRowPersonID);
            form.ShowDialog();
        }
    }
}
