using DVLD.BusinessLayer;
using DVLD.PresentationLayer.Tests.TestAppointments;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.PresentationLayer.Tests
{
    public partial class frmTestAppointments : Form
    {
        private int testTypeId;
        private int ldlaId;

        private DataTable appointmentsTable;

        public frmTestAppointments(int ldlaId, int testTypeId)
        {
            InitializeComponent();
            this.testTypeId = testTypeId;
            this.ldlaId = ldlaId;
        }

        private void ApplyViewSettings()
        {
            if (dgvAppointments.Columns.Count > 0)
            {
                dgvAppointments.Columns[0].HeaderText = "Appointment ID";
                dgvAppointments.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                dgvAppointments.Columns[1].HeaderText = "Appointment Date";
                dgvAppointments.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                dgvAppointments.Columns[2].HeaderText = "Paid Fees";
                dgvAppointments.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                dgvAppointments.Columns[3].HeaderText = "Is Locked";
                dgvAppointments.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        private void LoadAndRefresh()
        {
            appointmentsTable = TestAppointmentService.GetAllAsTable(this.ldlaId, this.testTypeId);
            dgvAppointments.DataSource = appointmentsTable;
            lblRecords.Text = $"Records: #{dgvAppointments.Rows.Count}";
        }

        private void SetDefaultSettings()
        {
            ctrlApplicationDetails1.LoadApplicationInfo(this.ldlaId);
            LoadAndRefresh();

            switch(this.testTypeId)
            {
                case 1:
                    lblTestType.Text = "Vision Test Appointments";
                    pictureBox1.Image = Properties.Resources.vision_test;
                    break;

                case 2:
                    lblTestType.Text = "Written Test Appointments";
                    pictureBox1.Image = Properties.Resources.written_test;
                    break;

                case 3:
                    lblTestType.Text = "Street Test Appointments";
                    pictureBox1.Image = Properties.Resources.driving_test;
                    break;
            }
        }

        private void frmScheduleTest_Load(object sender, EventArgs e)
        {
            SetDefaultSettings();
            ApplyViewSettings();
        }

        private void btnCloseForm_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!TestAppointmentService.CanSchedule(this.ldlaId, this.testTypeId))
            {
                MessageBox.Show
                (
                    "Person already has an active appointment, or completed this test.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
                
            else
            {
                frmScheduleEditAppointment frm = new frmScheduleEditAppointment(ldlaId, testTypeId);
                frm.ShowDialog();

                LoadAndRefresh();
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int appoitmentId = (int)dgvAppointments.CurrentRow.Cells[0].Value;
            bool isLocked = (bool)dgvAppointments.CurrentRow.Cells["IsLocked"].Value;

            frmScheduleEditAppointment frm = new frmScheduleEditAppointment(appoitmentId, isLocked);
            frm.ShowDialog();

            LoadAndRefresh(); // In case of changes
        }

        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int appoitmentId = (int)dgvAppointments.CurrentRow.Cells[0].Value;
            bool isLocked = (bool)dgvAppointments.CurrentRow.Cells["IsLocked"].Value;

            frmTakeTest form = new frmTakeTest(appoitmentId, isLocked);
            form.ShowDialog();

            LoadAndRefresh();
        }
    }
}
