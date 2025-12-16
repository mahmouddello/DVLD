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
            _peopleDataTable = PersonBusiness.GetPeople();
        }

        private void _RefreshPeopleList()
        {
            dgvAllPeople.DataSource = _peopleDataTable;
            lblRecordsCount.Text = $"Records: #{dgvAllPeople.Rows.Count}";
        }

        private void frmListPeople_Load(object sender, EventArgs e)
        {
            cbFilterBy.SelectedIndex = 0;
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

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Hide both controls first and clear the textbox
            txtFilterQuery.Visible = false;
            cbGender.Visible = false;
            txtFilterQuery.Clear();

            string selectedFilter = cbFilterBy.SelectedItem.ToString();

            switch (selectedFilter)
            {
                case "None":
                    _RefreshPeopleList();
                    break;

                case "Gender":
                    cbGender.Visible = true;
                    cbGender.SelectedIndex = 0;
                    break;
                
                // All other fields are text
                default:
                    txtFilterQuery.Visible = true;
                    break;
            }
        }

        private void txtFilterQuery_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFilterQuery.Text.Trim()))
                _RefreshPeopleList();
            else
                _ApplyFilter();
        }

        private void _ApplyFilter()
        {
            string selectedFilter = cbFilterBy.SelectedItem.ToString();
            string filterValue = txtFilterQuery.Text.Trim();

            // If filter value is empty, show all records
            if (string.IsNullOrEmpty(filterValue))
            {
                _RefreshPeopleList();
                return;
            }

            DataView dv = new DataView(_peopleDataTable);

            try
            {
                if (selectedFilter == "PersonID")
                {
                    if (!int.TryParse(filterValue, out int numericValue))
                    {
                        dgvAllPeople.DataSource = _peopleDataTable.Clone(); // Empty Table
                        lblRecordsCount.Text = $"Records: #{dgvAllPeople.Rows.Count}";
                    }

                    // Parse succsessfull, numeric value is entered
                    dv.RowFilter = $"{selectedFilter} = {numericValue}";
                }
                else
                    // For string columns: use LIKE with quotes
                    dv.RowFilter = $"{selectedFilter} LIKE '{filterValue}%'";

                dgvAllPeople.DataSource = dv;
            }
            catch (Exception ex) 
            {
                MessageBox.Show($"Filter error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _RefreshPeopleList();
            }

            lblRecordsCount.Text = $"Records: #{dgvAllPeople.Rows.Count}";
        }

        private void cbGender_SelectedIndexChanged(object sender, EventArgs e)
        {
            _ApplyGenderFilter();
        }

        private void _ApplyGenderFilter()
        {
            if (cbGender.SelectedIndex == -1 || cbGender.SelectedItem == null)
            {
                _RefreshPeopleList();
                return;
            }

            string genderValue = cbGender.SelectedItem.ToString();

            // If "All" or similar option is selected
            if (genderValue == "All")
            {
                _RefreshPeopleList();
                return;
            }

            DataView dv = new DataView(_peopleDataTable);

            try
            {
                dv.RowFilter = $"Gender = '{genderValue}'";

                dgvAllPeople.DataSource = dv;
                lblRecordsCount.Text = $"Records: #{dv.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Filter error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _RefreshPeopleList();
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int currentRowPersonID = (int)dgvAllPeople.CurrentRow.Cells[0].Value;
            frmAddUpdatePerson frm = new frmAddUpdatePerson(currentRowPersonID);
            frm.ShowDialog();

            _LoadData(); 
            _RefreshPeopleList();
        }

        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson(-1);
            frm.ShowDialog();

            _LoadData();
            _RefreshPeopleList();
        }
    }
}
