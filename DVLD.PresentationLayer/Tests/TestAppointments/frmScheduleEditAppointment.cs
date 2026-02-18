using DVLD.BusinessLayer;
using DVLD.EntityLayer;
using DVLD.PresentationLayer.Applications.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DVLD.EntityLayer.ApplicationType;
using Application = DVLD.EntityLayer.Application;

namespace DVLD.PresentationLayer.Tests.TestAppointments
{
    public partial class frmScheduleEditAppointment : Form
    {
        private enum Mode { AddNew = 0, Update = 1 }
        private Mode mode;

        int localDlaId, testTypeId, testApptId;
        private bool hasFailedTest;

        public frmScheduleEditAppointment(int localDlaId, int testTypeId)
        {
            InitializeComponent();
            mode = Mode.AddNew;

            this.localDlaId = localDlaId;
            this.testTypeId = testTypeId;
        }

        public frmScheduleEditAppointment(int testApptId)
        {
            InitializeComponent();
            mode = Mode.Update;

            this.testApptId = testApptId;
        }

        private void frmScheduleEditAppointment_Load(object sender, EventArgs e)
        {
            if (mode == Mode.Update)
                LoadExistingAppointment();
            else
                LoadNewAppointmentDefaults();

            SetDefaultSettings();
        }

        private void SetDefaultSettings()
        {
            switch (this.testTypeId)
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

        private void PopulateForm(LocalDrivingLicenseApplication localDla, int testTypeId)
        {
            TestType testType = TestTypeBusiness.Find(testTypeId);
            ApplicationType applicationType = ApplicationTypeBusiness.Find(ApplicationType.enApplicationType.RetakeTest);
            hasFailedTest = TestBusiness.HasFailedTest(localDla.Id, testTypeId);

            lblLocalDla.Text = localDla.Id.ToString();
            lblLicenseClass.Text = localDla.LicenseClassInfo.Name;
            lblPersonName.Text = localDla.MainApplicationInfo.ApplicantPersonInfo.FullName;
            lblTrialCount.Text = TestBusiness.GetTrialsCount(localDla.Id, testTypeId).ToString();
            lblFees.Text = TestTypeBusiness.Find(testTypeId).Fees.ToString();

            gbRetakeTest.Enabled = hasFailedTest;
            lblRetakeApplicationFees.Text = applicationType.Fees.ToString();
            lblTotalFees.Text = hasFailedTest ? (applicationType.Fees + testType.Fees).ToString() : testType.Fees.ToString();
        }

        private void LoadNewAppointmentDefaults()
        {
            var localDla = LocalDrivingLicenseApplicationBusiness.Find(localDlaId);

            if (localDla == null)
            {
                ShowErrorAndClose($"No Local DLA found with id: {localDlaId}!");
                return;
            }

            dtpAppointment.Value = DateTime.Now;
            dtpAppointment.MinDate = DateTime.Now;
            PopulateForm(localDla, testTypeId);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private bool CreateAndSaveRetakeTestApplication(Application application)
        {
            LocalDrivingLicenseApplication localDla = LocalDrivingLicenseApplicationBusiness.Find(localDlaId);
            ApplicationType applicationType = ApplicationTypeBusiness.Find(ApplicationType.enApplicationType.RetakeTest);

            // If failed a test, create an application of type retake test, save it and get back it's id.
            application.Id = -1;
            application.ApplicantPersonId = localDla.MainApplicationInfo.ApplicantPersonId;
            application.Date = DateTime.Now;
            application.ApplicationTypeId = (int)applicationType.Id;
            application.Status = Application.ApplicationStatus.Completed;
            application.LastStatusDate = DateTime.Now;
            application.PaidFees = applicationType.Fees;
            application.CreatedByUserId = GlobalClasses.Globals.CurrentUser.Id;

            return ApplicationBusiness.Save(application);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            TestAppointment testAppointment = new TestAppointment();

            // if the mode is update, means we have an appointment, so we update only it's date (if changed)
            if (mode == Mode.Update)
            {
                testAppointment = TestAppointmentBusiness.Find(testApptId);
                testAppointment.AppointmentDate = dtpAppointment.Value; // replace the value in case of date change

                if (testAppointment != null && TestAppointmentBusiness.Save(testAppointment))
                {
                    ShowSuccessMessage("Appointment Date updated sucessfully!");
                    return;
                }
            }
            else
            {
                if (hasFailedTest)
                {
                    Application application = new Application();

                    if (!CreateAndSaveRetakeTestApplication(application))
                        ShowErrorAndClose("Errorr saving retake test app. run in DEBUG Mode!");

                    // Saved the retake, now map a new testAppointment
                    MapTestAppointmentData(testAppointment);
                    testAppointment.RetakeTestApplicationId = application.Id;
                }
                else
                {
                    // if no failed test, then just book a normal first time appointment
                    MapTestAppointmentData(testAppointment);
                }

                if (TestAppointmentBusiness.Save(testAppointment))
                {
                    ShowSuccessMessage("Saved the new appoitment successfully!");
                    lblRetakeTestApplicationID.Text = testAppointment.RetakeTestApplicationId.ToString();
                }
                else
                    ShowErrorAndClose("Error booking the new appointment");
            }
        }

        private void MapTestAppointmentData(TestAppointment testAppointment)
        {
            testAppointment.Id = -1;
            testAppointment.TestTypeId = testTypeId;
            testAppointment.LocalDrivingLicenseApplicationId = localDlaId;
            testAppointment.AppointmentDate = DateTime.Now;
            testAppointment.PaidFees = TestTypeBusiness.Find(testTypeId).Fees;
            testAppointment.CreatedByUserId = GlobalClasses.Globals.CurrentUser.Id;
            testAppointment.IsLocked = false;
            testAppointment.RetakeTestApplicationId = -1;
        }

        private void LoadExistingAppointment()
        {
            var appointment = TestAppointmentBusiness.Find(testApptId);

            if (appointment == null)
            {
                ShowErrorAndClose($"No Appointment found with id: {testApptId}!");
                return;
            }

            var localDla = LocalDrivingLicenseApplicationBusiness.Find(appointment.LocalDrivingLicenseApplicationId);
            testTypeId = appointment.TestTypeId;

            if (localDla == null)
            {
                ShowErrorAndClose($"No Local DLA found with id: {appointment.LocalDrivingLicenseApplicationId}!");
                return;
            }

            if (appointment.RetakeTestApplicationId != null)
                lblRetakeTestApplicationID.Text = appointment.RetakeTestApplicationId.ToString();

            dtpAppointment.Value = appointment.AppointmentDate;

            if (DateTime.Now < appointment.AppointmentDate)
                dtpAppointment.MinDate = DateTime.Now;
            else
                dtpAppointment.MinDate = appointment.AppointmentDate;

            PopulateForm(localDla, appointment.TestTypeId);
        }

        private void ShowErrorAndClose(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            this.Close();
        }

        private void ShowSuccessMessage(string message)
        {
            MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

    }
}
