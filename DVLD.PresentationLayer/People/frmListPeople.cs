using System;
using System.Data;
using System.Windows.Forms;
using DVLD.BusinessLayer;
using DVLD.PresentationLayer.Globals;

namespace DVLD.PresentationLayer.People
{
    public partial class frmListPeople : Form
    {
        private enum enFilterTypes : byte
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

            enFilterTypes filterType = (enFilterTypes)cbFilterBy.SelectedIndex;

            switch (filterType)
            {
                case enFilterTypes.None:
                    _RefreshPeopleList();
                    break;

                case enFilterTypes.Gender:
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
            enFilterTypes filterType = (enFilterTypes)cbFilterBy.SelectedIndex;
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
                if (filterType == enFilterTypes.PersonID)
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
                    dv.RowFilter = $"{filterType.ToString()} LIKE '%{filterValue}%'";

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

            enFilterTypes filterType = (enFilterTypes)cbFilterBy.SelectedIndex;

            switch (filterType)
            {
                // Numeric Fields: Only numbers allowed
                case enFilterTypes.PersonID:
                case enFilterTypes.Phone:
                    if (!char.IsDigit(e.KeyChar)) { UtilityHelper.PlayBeepSound(); e.Handled = true; }
                    break;

                // String Only Fields : Only letters allowed
                case enFilterTypes.FirstName:
                case enFilterTypes.SecondName:
                case enFilterTypes.ThirdName:
                case enFilterTypes.LastName:
                case enFilterTypes.Nationality:
                    if (!char.IsLetter(e.KeyChar)) { UtilityHelper.PlayBeepSound(); e.Handled = true; }
                    break;

                // String + Numeric Mix Fields (National No, Email): Don't do checks;
                default:
                    break;
            }
        }
    }
}
