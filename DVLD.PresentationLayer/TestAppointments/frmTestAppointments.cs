using System;
using System.Data;
using System.Windows.Forms;
using DVLD.BusinessLayer;
using DVLD.EntityLayer;
using DVLD.PresentationLayer.GlobalClasses;
using DVLD.PresentationLayer.Tests.TestAppointments;

namespace DVLD.PresentationLayer.Tests
{
    public partial class frmTestAppointments : Form
    {
        private int _testTypeId;
        private int _ldlaId;

        private DataTable appointmentsTable;

        public frmTestAppointments(int ldlaId, int testTypeId)
        {
            InitializeComponent();
            _testTypeId = testTypeId;
            _ldlaId = ldlaId;
        }        

        private void GetDataFromDB()
        {
            appointmentsTable = TestAppointmentService.GetAllAsTable(_ldlaId, _testTypeId);
        }

        private void RefreshAppointmentsList()
        {
            dgvAppointments.DataSource = appointmentsTable;
            lblRecords.Text = $"Records: #{dgvAppointments.Rows.Count}";
        }

        private void ReloadDataAndRefreshAppointments()
        {
            GetDataFromDB();
            RefreshAppointmentsList();
        }

        private void ApplyDGVSettings()
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

        private void SetUIByTestType()
        {
            switch (_testTypeId)
            {
                case (int)enTestType.VisionTest:
                    lblTestType.Text = "Schedule Vision Test Appointment";
                    pictureBox1.Image = Properties.Resources.vision_test;
                    break;

                case (int)enTestType.WrittenTest:
                    lblTestType.Text = "Schedule Written Test Appointment";
                    pictureBox1.Image = Properties.Resources.written_test;
                    break;

                case (int)enTestType.PracticalTest:
                    lblTestType.Text = "Schedule Street Test Appointment";
                    pictureBox1.Image = Properties.Resources.driving_test;
                    break;
            }
        }

        private void InitializeForm()
        {
            // Fill the form info by loading it using local driving license application id
            ctrlApplicationDetails1.LoadApplicationInfo(_ldlaId);

            // Get Data from DB, Set in the Datagridview
            GetDataFromDB();
            RefreshAppointmentsList();

            // View custom settings
            ApplyDGVSettings();

            // Conifgures the image, title of the form upon the test (Vision, Written, Street)
            SetUIByTestType();
        }

        private void frmScheduleTest_Load(object sender, EventArgs e)
        {
            InitializeForm();
        }

        private void btnBookAppointment_Click(object sender, EventArgs e)
        {
            if (!TestAppointmentService.CanSchedule(_ldlaId, _testTypeId))
            {
                Utility.ShowErrorMessage("Person already has an active appointment, or completed this test");
                return;
            }

            frmScheduleEditAppointment frm = new frmScheduleEditAppointment(_ldlaId, _testTypeId);
            frm.ShowDialog();

            // Reload in case of new appointment is booked
            ReloadDataAndRefreshAppointments();
        }

        private void btnCloseForm_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int appointmentId = (int)dgvAppointments.CurrentRow.Cells[0].Value;
            bool isLocked = (bool)dgvAppointments.CurrentRow.Cells["IsLocked"].Value;

            frmScheduleEditAppointment frm = new frmScheduleEditAppointment(appointmentId, isLocked);
            frm.ShowDialog();

            // Reload in case of edit occurd on existing appointment
            ReloadDataAndRefreshAppointments();
        }

        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int appointmentId= (int)dgvAppointments.CurrentRow.Cells[0].Value;
            bool isLocked = (bool)dgvAppointments.CurrentRow.Cells["IsLocked"].Value;

            frmTakeTest form = new frmTakeTest(appointmentId, isLocked);
            form.ShowDialog();

            // Reload to ensure displaying test result, lock state of the test
            ReloadDataAndRefreshAppointments();
        }

    }
}
