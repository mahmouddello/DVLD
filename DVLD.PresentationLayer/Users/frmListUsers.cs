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

        private enum Filter
        {
            None,
            UserID,
            PersonID,
            FullName,
            UserName,
            IsActive
        }
        private static Filter filter;

        private enum ActiveStatus
        {
            All,
            Yes,
            No
        }
        private static ActiveStatus activeStatusFilter;

        public frmListUsers()
        {
            InitializeComponent();
        }

        private void LoadUsersFromDB()
        {
            dtAllUsers = UserBusiness.GetUsers();
        }

        private void RefreshUsersList()
        {
            dtUsersList = dtAllUsers;
            dgvUsers.DataSource = dtUsersList;
            lblRecordsCount.Text = $"Records: #{dgvUsers.Rows.Count}";
        }

        private void ApplyViewSettings()
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

        private void frmListUsers_Load(object sender, EventArgs e)
        {
            LoadUsersFromDB();
            RefreshUsersList();
            ApplyViewSettings();
            cbFilterBy.SelectedIndex = 0; // default, None
        }

        private void btnCloseForm_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            filter = (Filter)cbFilterBy.SelectedIndex;
            txtFilter.Visible = filter != Filter.None && filter != Filter.IsActive;
            cbIsActive.Visible = filter == Filter.IsActive;

            switch(filter)
            {
                case Filter.None:
                    RefreshUsersList();
                    break;
                case Filter.IsActive:
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
            activeStatusFilter = (ActiveStatus)cbIsActive.SelectedIndex;
            ApplyActivityFilter();
        }

        private void ApplyActivityFilter()
        {
            if (cbIsActive.SelectedIndex == -1 || cbIsActive.SelectedItem == null)
            {
                RefreshUsersList();
                return;
            }

            if (activeStatusFilter == ActiveStatus.All)
            {
                RefreshUsersList();
                return;
            }

            DataView dv = new DataView(dtAllUsers);

            switch (activeStatusFilter)
            {
                case ActiveStatus.Yes:
                    dv.RowFilter = "IsActive = 'true'";
                    break;
                case ActiveStatus.No:
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

            switch (filter)
            {
                // Only Numeric
                case Filter.UserID:
                case Filter.PersonID:
                    if (!char.IsDigit(e.KeyChar))
                        Utility.HandleWrongKey(e);
                    break;
                // Only Letters
                case Filter.FullName:
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
                if (UserBusiness.Delete(rowUserID))
                {
                    MessageBox.Show("Deleted Successfully!");
                    LoadUsersFromDB();
                    RefreshUsersList();
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

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int rowUserId = (int)dgvUsers.CurrentRow.Cells[0].Value;

            frmUserDetails form = new frmUserDetails(rowUserId);
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

        private void ApplyQueryFilter()
        {
            DataView dv = new DataView(dtAllUsers);
            string query = txtFilter.Text.Trim();

            switch (filter)
            {
                case Filter.PersonID:
                case Filter.UserID:
                    if (!int.TryParse(query, out int id))
                    {
                        dgvUsers.DataSource = dtAllUsers.Clone();
                        lblRecordsCount.Text = "Records: #0";
                        return;
                    }
                    dv.RowFilter = $"{filter.ToString()} = {id}";
                    break;
                default:
                    query = MakeQuerySafe(query);
                    dv.RowFilter = $"{filter.ToString()} LIKE '%{query}%'";
                    break;
            }

            dgvUsers.DataSource = dv;
            lblRecordsCount.Text = $"Records: #{dgvUsers.Rows.Count}";
        }
    }
}
