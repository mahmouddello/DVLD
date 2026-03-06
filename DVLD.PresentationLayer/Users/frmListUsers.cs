using DVLD.BusinessLayer;
using DVLD.PresentationLayer.GlobalClasses;
using System;
using System.Data;
using System.Windows.Forms;

namespace DVLD.PresentationLayer.Users
{
    public partial class frmListUsers : Form
    {

        private DataTable dtAllUsers, dtUsersList;

        private enum FilterMode
        {
            None,
            UserID,
            PersonID,
            FullName,
            UserName,
            IsActive
        }
        private FilterMode _filterMode;

        private enum ApplicationActivityFilter
        {
            All,
            Active,
            Inactive
        }

        private static ApplicationActivityFilter _activityFilter;

        public frmListUsers()
        {
            InitializeComponent();
        }

        private void LoadUsersFromDB()
        {
            dtAllUsers = UserService.GetAllUsers();
        }

        private void RefreshUsersList()
        {
            dtUsersList = dtAllUsers;
            dgvUsers.DataSource = dtUsersList;
            lblRecordsCount.Text = $"Records: #{dgvUsers.Rows.Count}";
        }

        private void ApplyDGVSettings()
        {
            if (dgvUsers.Columns.Count > 0)
            {
                dgvUsers.Columns[0].HeaderText = "User ID";
                dgvUsers.Columns[0].Width = 120;

                dgvUsers.Columns[1].HeaderText = "Person ID";
                dgvUsers.Columns[1].Width = 150;

                dgvUsers.Columns[2].HeaderText = "Username";
                dgvUsers.Columns[2].Width = 180;

                dgvUsers.Columns[3].HeaderText = "Is Active";
                dgvUsers.Columns[3].Width = 120;

                dgvUsers.Columns[4].HeaderText = "Full Name";
                dgvUsers.Columns[4].Width = 445;
            }     
        }

        private void BindComboBoxes()
        {
            cbFilterBy.DataSource = Enum.GetValues(typeof(FilterMode));
            cbIsActive.DataSource = Enum.GetValues(typeof(ApplicationActivityFilter));
        }

        private void frmListUsers_Load(object sender, EventArgs e)
        {
            LoadUsersFromDB();
            RefreshUsersList();
           
            ApplyDGVSettings();
            BindComboBoxes();

            cbFilterBy.SelectedItem = FilterMode.None;
        }

        private void btnCloseForm_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            _filterMode = (FilterMode)cbFilterBy.SelectedIndex;
            txtFilter.Visible = _filterMode != FilterMode.None && _filterMode != FilterMode.IsActive;
            cbIsActive.Visible = _filterMode == FilterMode.IsActive;

            switch(_filterMode)
            {
                case FilterMode.None:
                    RefreshUsersList();
                    break;
                case FilterMode.IsActive:
                    cbIsActive.SelectedIndex = 0;
                    ApplyActivityFilter();
                    break;
                default:
                    RefreshUsersList();
                    txtFilter.Clear();
                    break;
            }
        }

        private void cbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            _activityFilter = (ApplicationActivityFilter)cbIsActive.SelectedItem;
            ApplyActivityFilter();
        }

        private void ApplyActivityFilter()
        {
            if (cbIsActive.SelectedItem == null)
            {
                RefreshUsersList();
                return;
            }

            if (_activityFilter == ApplicationActivityFilter.All)
            {
                RefreshUsersList();
                return;
            }

            DataView dv = new DataView(dtAllUsers);

            switch (_activityFilter)
            {
                case ApplicationActivityFilter.Active:
                    dv.RowFilter = "IsActive = 'true'";
                    break;
                case ApplicationActivityFilter.Inactive:
                    dv.RowFilter = "IsActive = 'false'";
                    break;
                default:
                    break;
            }

            dgvUsers.DataSource = dv;
            lblRecordsCount.Text = $"Records: #{dgvUsers.Rows.Count}";
        }

        private string MakeQuerySafe(string query)
        {
            return query
                    .Replace("'", "''")
                    .Replace("[", "[[]")
                    .Replace("]", "[]]");
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            // The pressed key is : space, delete, backspace, ...etc. skips the checks.
            if (char.IsControl(e.KeyChar)) 
                return;

            switch (_filterMode)
            {
                // Only Numeric
                case FilterMode.UserID:
                case FilterMode.PersonID:
                    if (!char.IsDigit(e.KeyChar))
                        Utility.HandleWrongKey(e);
                    break;
                // Only Letters
                case FilterMode.FullName:
                    if (!char.IsLetter(e.KeyChar) && e.KeyChar != ' ')
                        Utility.HandleWrongKey(e);
                    break;
                // Allow Numeric + Letters (Username)
                default:
                    break;
            }
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string query = txtFilter.Text.Trim();

            if (string.IsNullOrEmpty(query))
            {
                RefreshUsersList();
                return;
            }
            
            ApplyQueryFilter();
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
            int rowUserID = (int)dgvUsers.CurrentRow.Cells[0].Value;

            if (MessageBox.Show
            (
                "Are you sure you want to delete this person? This action can't be undone!",
                $"Delete Person ID = {rowUserID}",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning
            ) == DialogResult.OK)
            {
                if (UserService.Delete(rowUserID))
                {
                    Utility.ShowSuccessMessage("Deleted Successfully!");
                    LoadUsersFromDB();
                    RefreshUsersList();
                }
                else
                    Utility.ShowWarningMessage(
                        "Delete operation failed because of referential integrity error!",
                        "Operation Failed"
                    );
            }
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int rowUserId = (int)dgvUsers.CurrentRow.Cells[0].Value;

            var form = frmUserDetails.CreateById(rowUserId);
            form.ShowDialog();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int rowUserId = (int)dgvUsers.CurrentRow.Cells[0].Value;

            frmAddUpdateUser frm = new frmAddUpdateUser(rowUserId);
            frm.ShowDialog();

            LoadUsersFromDB();
            RefreshUsersList(); // refresh incase changes applied.
        }

        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            frmAddUpdateUser frm = new frmAddUpdateUser();
            frm.ShowDialog();

            // Refresh users in case of change
            LoadUsersFromDB();
            RefreshUsersList();
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int rowUserId = (int)dgvUsers.CurrentRow.Cells[0].Value;

            var form = frmChangeUserPassword.CreateById(rowUserId);
            form.ShowDialog();
        }

        private void ApplyQueryFilter()
        {
            DataView dv = new DataView(dtAllUsers);
            string query = txtFilter.Text.Trim();

            switch (_filterMode)
            {
                case FilterMode.PersonID:
                case FilterMode.UserID:
                    if (!int.TryParse(query, out int id))
                    {
                        dgvUsers.DataSource = dtAllUsers.Clone();
                        lblRecordsCount.Text = "Records: #0";
                        return;
                    }
                    dv.RowFilter = $"{_filterMode.ToString()} = {id}";
                    break;
                default:
                    query = MakeQuerySafe(query);
                    dv.RowFilter = $"{_filterMode.ToString()} LIKE '%{query}%'";
                    break;
            }

            dgvUsers.DataSource = dv;
            lblRecordsCount.Text = $"Records: #{dgvUsers.Rows.Count}";
        }
    }
}
