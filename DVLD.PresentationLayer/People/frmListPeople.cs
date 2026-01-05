using System;
using System.Data;
using System.Windows.Forms;
using DVLD.BusinessLayer;
using DVLD.PresentationLayer.Globals;

namespace DVLD.PresentationLayer.People
{
    public partial class frmListPeople : Form
    {
        private enum enFilter : byte
        {
            None,
            PersonID,
            NationalNo,
            FirstName,
            SecondName,
            ThirdName,
            LastName,
            Gender,
            Nationality,
            Phone,
            Email
        }

        private enum enGender
        {
            All = -1,
            Male = 0, 
            Female = 1
        }

        DataTable _peopleDataTable;

        private enFilter filter;
        private enGender genderType;

        private bool _isInitializing;

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

        private void ApplyViewSettings()
        {

            dgvAllPeople.Columns[0].HeaderText = "Person ID";
            dgvAllPeople.Columns[0].Width = 150;

            dgvAllPeople.Columns[1].HeaderText = "N.No";
            dgvAllPeople.Columns[1].Width = 80;

            dgvAllPeople.Columns[2].HeaderText = "First Name";
            dgvAllPeople.Columns[2].Width = 160;

            dgvAllPeople.Columns[3].HeaderText = "Second Name";
            dgvAllPeople.Columns[3].Width = 180;

            dgvAllPeople.Columns[4].HeaderText = "Third Name";
            dgvAllPeople.Columns[4].Width = 160;

            dgvAllPeople.Columns[5].HeaderText = "Last Name";
            dgvAllPeople.Columns[5].Width = 160;

            dgvAllPeople.Columns[6].HeaderText = "Date Of Birth";
            dgvAllPeople.Columns[6].Width = 150;

            dgvAllPeople.Columns[7].HeaderText = "Gender";
            dgvAllPeople.Columns[7].Width = 100;

            dgvAllPeople.Columns[8].HeaderText = "Nationality";
            dgvAllPeople.Columns[8].Width = 120;

            dgvAllPeople.Columns[9].HeaderText = "Phone";
            dgvAllPeople.Columns[9].Width = 120;

            dgvAllPeople.Columns[10].HeaderText = "Email";
            dgvAllPeople.Columns[10].Width = 200;
        }


        private void BindGenderComboBox()
        {
            _isInitializing = true;

            cbGender.DataSource = Enum.GetValues(typeof(enGender));

            _isInitializing = false;
        }

        private void frmListPeople_Load(object sender, EventArgs e)
        {
            _LoadData();
            _RefreshPeopleList();
            ApplyViewSettings();

            BindGenderComboBox();
            cbFilterBy.SelectedIndex = 0;
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
            filter = (enFilter)cbFilterBy.SelectedIndex;
            txtFilterQuery.Visible = (filter != enFilter.Gender) && (filter != enFilter.None);
            cbGender.Visible = filter == enFilter.Gender;

            switch (filter)
            {
                case enFilter.None:
                    _RefreshPeopleList();
                    break;

                case enFilter.Gender:
                    cbGender.SelectedItem = enGender.All;
                    break;
                
                // All other fields are text
                default:
                    _RefreshPeopleList();
                    txtFilterQuery.Clear();
                    break;
            }
        }

        private void txtFilterQuery_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFilterQuery.Text.Trim()))
                _RefreshPeopleList();
            else
                _ApplyQueryFilter();
        }

        private void _ApplyQueryFilter()
        {
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
                if (filter == enFilter.PersonID)
                {
                    if (!int.TryParse(filterValue, out int numericValue))
                    {
                        dgvAllPeople.DataSource = _peopleDataTable.Clone();
                        lblRecordsCount.Text = "Records: #0";
                        return;
                    }

                    dv.RowFilter = $"PersonID = {numericValue}";
                }
                else
                    // For string columns: use LIKE with quotes
                    dv.RowFilter = $"{filter.ToString()} LIKE '%{filterValue}%'";

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
            if (_isInitializing)
                return;

            genderType = (enGender)cbGender.SelectedItem;
            _ApplyGenderFilter();
        }

        private void _ApplyGenderFilter()
        {
            // If "All" or similar option is selected
            if (genderType == enGender.All)
            {
                _RefreshPeopleList();
                return;
            }

            DataView dv = new DataView(_peopleDataTable);

            switch(genderType)
            {
                case enGender.Male:
                    dv.RowFilter = "Gender = 'Male'";
                    break;
                case enGender.Female:
                    dv.RowFilter = "Gender = 'Female'";
                    break;
                default:
                    break;
            }
            dgvAllPeople.DataSource = dv;
            lblRecordsCount.Text = $"Records: #{dgvAllPeople.Rows.Count}";
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
            frmAddUpdatePerson frm = new frmAddUpdatePerson();
            frm.ShowDialog();

            _LoadData();
            _RefreshPeopleList();
        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show
            (
                "This feature will be implemented in the future",
                "Stub",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
        }

        private void sendSMSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show
            (
                "This feature will be implemented in the future",
                "Stub",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int currentRowPersonID = (int)dgvAllPeople.CurrentRow.Cells[0].Value;

            if (MessageBox.Show
            (
                "Are you sure you want to delete this person? This action can't be undone!",
                $"Delete Person ID = {currentRowPersonID}",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning
            ) == DialogResult.OK)
            {
                if (PersonBusiness.Delete(currentRowPersonID))
                {
                    MessageBox.Show("Deleted Successfully!");
                    _LoadData();
                    _RefreshPeopleList();
                }
                else
                    MessageBox.Show(
                        "Delete operation failed because of referential integrity error!",
                        "Operation Failed",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
            }
        }

        private void txtFilterQuery_KeyPress(object sender, KeyPressEventArgs e)
        {
            // The pressed key is : space, delete, backspace, ...etc. skips the checks.
            if (char.IsControl(e.KeyChar)) return;

            switch (filter)
            {
                // Numeric Fields: Only numbers allowed
                case enFilter.PersonID:
                case enFilter.Phone:
                    if (!char.IsDigit(e.KeyChar)) { UtilityHelper.PlayBeepSound(); e.Handled = true; }
                    break;

                // String Only Fields : Only letters allowed
                case enFilter.FirstName:
                case enFilter.SecondName:
                case enFilter.ThirdName:
                case enFilter.LastName:
                case enFilter.Nationality:
                    if (!char.IsLetter(e.KeyChar)) { UtilityHelper.PlayBeepSound(); e.Handled = true; }
                    break;

                // String + Numeric Mix Fields (National No, Email): Don't do checks;
                default:
                    break;
            }
        }
    }
}
