using System;
using System.Windows.Forms;
using DVLD.BusinessLayer;

namespace DVLD.PresentationLayer.People
{
    public partial class frmListPeople : Form
    {
        public frmListPeople()
        {
            InitializeComponent();
        }

        private void _RefreshPeopleList()
        {
            dgvAllPeople.DataSource = PersonBusiness.GetAllPeople();
        }

        private void frmListPeople_Load(object sender, EventArgs e)
        {
            _RefreshPeopleList();
            lblRecordsCount.Text = $"Records: #{dgvAllPeople.Rows.Count}";
        }

        private void btnCloseForm_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
