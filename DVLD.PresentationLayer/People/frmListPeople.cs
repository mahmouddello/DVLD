using System;
using System.Data;
using System.Windows.Forms;
using DVLD.BusinessLayer;
using DVLD.PresentationLayer.GlobalClasses;

namespace DVLD.PresentationLayer.People
{
    public partial class frmListPeople : Form
    {
        private enum FilterMode : byte
        {
            None, PersonID, NationalNo, FirstName,
            SecondName, ThirdName, LastName,
            Gender, Nationality, Phone, Email
        }

        private enum GenderFilter
        {
            All = -1,
            Male = 0,
            Female = 1
        }

        private DataTable _peopleDataTable;
        private FilterMode _filter;
        private GenderFilter _genderFilter = GenderFilter.All;

        public frmListPeople()
        {
            InitializeComponent();
        }

        private void frmListPeople_Load(object sender, EventArgs e)
        {
            InitializeSettings();
        }

        private void InitializeSettings()
        {
            LoadData();
            RefreshPeopleList();
            ApplyDGVSettings();
            BindGenderComboBox();
            cbFilterBy.SelectedIndex = 0;
        }

        private void LoadData()
        {
            _peopleDataTable = PersonService.GetAllAsTable();
        }

        private void RefreshPeopleList()
        {
            dgvAllPeople.DataSource = _peopleDataTable;
            lblRecordsCount.Text = $"Records: #{dgvAllPeople.Rows.Count}";
        }

        private void ReloadAndRefresh()
        {
            LoadData();
            RefreshPeopleList();
        }

        private void SetColumn(int index, string header,
            DataGridViewAutoSizeColumnMode mode = DataGridViewAutoSizeColumnMode.AllCells)
        {
            dgvAllPeople.Columns[index].HeaderText = header.Trim();
            dgvAllPeople.Columns[index].AutoSizeMode = mode;
        }

        private void ApplyDGVSettings()
        {
            if (dgvAllPeople.Columns.Count < 1) return;

            SetColumn(0, "Person ID");
            SetColumn(1, "National No");
            SetColumn(2, "First Name");
            SetColumn(3, "Second Name");
            SetColumn(4, "Third Name");
            SetColumn(5, "Last Name");
            SetColumn(6, "Date Of Birth");
            SetColumn(7, "Gender");
            SetColumn(8, "Nationality");
            SetColumn(9, "Phone");
            SetColumn(10, "Email");
        }

        private void BindGenderComboBox()
        {
            cbGender.DataSource = Enum.GetValues(typeof(GenderFilter));
        }

        private void ApplyQueryFilter()
        {
            string filterValue = txtFilterQuery.Text.Trim();
            DataView dv = new DataView(_peopleDataTable);

            try
            {
                if (_filter == FilterMode.PersonID)
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
                    dv.RowFilter = $"{_filter} LIKE '%{filterValue}%'";

                dgvAllPeople.DataSource = dv;
                lblRecordsCount.Text = $"Records: #{dgvAllPeople.Rows.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Filter error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                RefreshPeopleList();
            }
        }

        private void ApplyGenderFilter()
        {
            if (_genderFilter == GenderFilter.All)
            {
                RefreshPeopleList();
                return;
            }

            DataView dv = new DataView(_peopleDataTable);
            dv.RowFilter = $"Gender = '{_genderFilter}'";
            dgvAllPeople.DataSource = dv;
            lblRecordsCount.Text = $"Records: #{dgvAllPeople.Rows.Count}";
        }

        // ── Event Handlers ──────────────────────────────────────

        private void btnCloseForm_Click(object sender, EventArgs e) => this.Close();

        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson();
            frm.ShowDialog();
            ReloadAndRefresh();
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            _filter = (FilterMode)cbFilterBy.SelectedIndex;
            txtFilterQuery.Visible = _filter != FilterMode.Gender && _filter != FilterMode.None;
            cbGender.Visible = _filter == FilterMode.Gender;

            switch (_filter)
            {
                case FilterMode.None:
                    RefreshPeopleList();
                    break;
                case FilterMode.Gender:
                    cbGender.SelectedItem = GenderFilter.All;
                    break;
                default:
                    RefreshPeopleList();
                    txtFilterQuery.Clear();
                    break;
            }
        }

        private void txtFilterQuery_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFilterQuery.Text))
                RefreshPeopleList();
            else
                ApplyQueryFilter();
        }

        private void cbGender_SelectedIndexChanged(object sender, EventArgs e)
        {
            _genderFilter = (GenderFilter)cbGender.SelectedItem;
            ApplyGenderFilter();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int personId = (int)dgvAllPeople.CurrentRow.Cells[0].Value;
            new frmPersonDetails(personId).ShowDialog();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int personId = (int)dgvAllPeople.CurrentRow.Cells[0].Value;
            new frmAddUpdatePerson(personId).ShowDialog();
            ReloadAndRefresh();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int personId = (int)dgvAllPeople.CurrentRow.Cells[0].Value;

            if (MessageBox.Show(
                "Are you sure you want to delete this person? This action can't be undone!",
                $"Delete Person ID = {personId}",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning) == DialogResult.OK)
            {
                if (!PersonService.Delete(personId))
                {
                    MessageBox.Show("Deleted Successfully!");
                    ReloadAndRefresh();
                }
                else
                    Utility.ShowErrorMessage("Delete failed due to a referential integrity error!");
            }
        }

        private void txtFilterQuery_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar)) return;

            switch (_filter)
            {
                case FilterMode.PersonID:
                case FilterMode.Phone:
                    if (!char.IsDigit(e.KeyChar))
                        Utility.HandleWrongKey(e);
                    break;

                case FilterMode.FirstName:
                case FilterMode.SecondName:
                case FilterMode.ThirdName:
                case FilterMode.LastName:
                case FilterMode.Nationality:
                    if (!char.IsLetter(e.KeyChar))
                        Utility.HandleWrongKey(e);
                    break;
            }
        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e) =>
            Utility.ShowWarningMessage("This feature will be implemented in the future", "Stub");

        private void sendSMSToolStripMenuItem_Click(object sender, EventArgs e) =>
            Utility.ShowWarningMessage("This feature will be implemented in the future", "Stub");

    }
}
