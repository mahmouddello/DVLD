using DVLD.BusinessLayer;
using DVLD.EntityLayer;
using DVLD.PresentationLayer.Applications.Controls;
using DVLD.PresentationLayer.GlobalClasses;
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
        private enum Mode { AddNew = 0, Update = 1, Locked = 2 }
        private Mode mode;

        int localDlaId, testTypeId, testApptId;
        TestAppointment testAppointment;

        private bool HasFailedTest
        {
            get
            {
                // If in AddNew mode, localDlaId and testTypeId are known
                if (mode == Mode.AddNew)
                {
                    return TestBusiness.HasFailedTest(localDlaId, testTypeId);
                }

                // If in Update mode, fetch localDlaId and testTypeId from the existing appointment
                else if (mode == Mode.Update && testApptId > 0)
                {
                    var appointment = TestAppointmentBusiness.Find(testApptId);
                    if (appointment == null) return false;

                    return TestBusiness.HasFailedTest(
                        appointment.LocalDrivingLicenseApplicationId,
                        appointment.TestTypeId
                    );
                }
                return false; // fallback
            }
        }

        public frmScheduleEditAppointment(int localDlaId, int testTypeId)
        {
            InitializeComponent();
            mode = Mode.AddNew;

            this.localDlaId = localDlaId;
            this.testTypeId = testTypeId;
        }

        public frmScheduleEditAppointment(int testApptId, bool isLocked)
        {
            InitializeComponent();
            this.testApptId = testApptId;

            if (isLocked)
                mode = Mode.Locked;
            else
                mode = Mode.Update;
        }

        private void frmScheduleEditAppointment_Load(object sender, EventArgs e)
        {
            if (mode == Mode.Update || mode == Mode.Locked)
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
                    lblTestType.Text = "Schedule Vision Test Appointment";
                    pictureBox1.Image = Properties.Resources.vision_test;
                    break;

                case 2:
                    lblTestType.Text = "Schedule Written Test Appointment";
                    pictureBox1.Image = Properties.Resources.written_test;
                    break;

                case 3:
                    lblTestType.Text = "Schedule Street Test Appointment";
                    pictureBox1.Image = Properties.Resources.driving_test;
                    break;
            }
        }

        private void PopulateForm(LocalDrivingLicenseApplication localDla, int testTypeId)
        {
            // Objects
            TestType testType = TestTypeBusiness.Find(testTypeId);
            ApplicationType applicationType = ApplicationTypeBusiness.Find(ApplicationType.enApplicationType.RetakeTest);

            // application data
            lblLocalDla.Text = localDla.Id.ToString();
            lblLicenseClass.Text = localDla.LicenseClassInfo.Name;
            lblPersonName.Text = localDla.MainApplicationInfo.ApplicantPersonInfo.FullName;
            lblTrialCount.Text = TestBusiness.GetTrialsCount(localDla.Id, testTypeId).ToString();
            lblFees.Text = TestTypeBusiness.Find(testTypeId).Fees.ToString();

            // group box data
            gbRetakeTest.Enabled = HasFailedTest;
            lblRetakeApplicationFees.Text = applicationType.Fees.ToString();
            lblTotalFees.Text = HasFailedTest ? (applicationType.Fees + testType.Fees).ToString() : testType.Fees.ToString();
            lblAdditional.Text = "";

            if (mode == Mode.Locked)
            {
                lblAdditional.Text = "Person already has sat for this test, you can't modify this appointment!";
                btnSave.Enabled = false;
                dtpAppointment.Enabled = false;
            }
        }

        private void LoadNewAppointmentDefaults()
        {
            LocalDrivingLicenseApplication localDla = LocalDrivingLicenseApplicationBusiness.Find(localDlaId);

            if (localDla == null)
            {
                Utility.ShowErrorMessage($"No Local DLA found with id: {localDlaId}!");
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

        private bool UpdateAppointmentDate()
        {
            TestAppointment testAppointment = TestAppointmentBusiness.Find(testApptId);

            if (testAppointment == null)
                return false;

            testAppointment.AppointmentDate = dtpAppointment.Value; // replace the value in case of date chang

            return TestAppointmentBusiness.Save(testAppointment);
        }

        private void SaveTestAppointment()
        {
            Application application = new Application();
            

            // if the mode is update, means we have an appointment, so we update only it's date (if changed)
            if (mode == Mode.Update)
            {
                testAppointment = TestAppointmentBusiness.Find(testApptId);

                if (!UpdateAppointmentDate())
                {
                    Utility.ShowErrorMessage($"Error finding the appointment wiht id: {testApptId}!");
                    return;
                }

                Utility.ShowSuccessMessage("Updated the appointment successfully!");
                return;
            }

            // From here, the mode is add new
            testAppointment = new TestAppointment();
            MapTestAppointmentData(testAppointment); // map data (new appointment)

            // if the person has failed this test before, we need to create application of type Retake test.
            if (HasFailedTest)
            {
                if (!CreateAndSaveRetakeTestApplication(application))
                {
                    Utility.ShowErrorMessage("Error saving retake test application. This form will be closed");
                    return;
                }

                // Saved the retake, now map to the test appointment
                testAppointment.RetakeTestApplicationId = application.Id;
            }

            if (TestAppointmentBusiness.Save(testAppointment))
            {
                Utility.ShowSuccessMessage($"Saved the new appointment successfully with id: {testAppointment.Id}");
                lblRetakeTestApplicationID.Text = testAppointment.RetakeTestApplicationId.ToString();
                mode = Mode.Update;
                return;
            }

            // Handle save failure
            if (HasFailedTest && application != null)
                ApplicationBusiness.Delete(application);

            Utility.ShowErrorMessage("Error booking the new appointment!");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
           SaveTestAppointment();
        }

        private void MapTestAppointmentData(TestAppointment testAppointment)
        {
            testAppointment.Id = -1; // default
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
                Utility.ShowErrorMessage($"No Appointment found with id: {testApptId}!");
                return;
            }

            var localDla = LocalDrivingLicenseApplicationBusiness.Find(appointment.LocalDrivingLicenseApplicationId);
            testTypeId = appointment.TestTypeId;

            if (localDla == null)
            {
                Utility.ShowErrorMessage($"No Local DLA found with id: {appointment.LocalDrivingLicenseApplicationId}!");
                return;
            }

            if (appointment.RetakeTestApplicationId != -1)
                lblRetakeTestApplicationID.Text = appointment.RetakeTestApplicationId.ToString();

            dtpAppointment.Value = appointment.AppointmentDate;

            if (DateTime.Now < appointment.AppointmentDate)
                dtpAppointment.MinDate = DateTime.Now;
            else
                dtpAppointment.MinDate = appointment.AppointmentDate;

            PopulateForm(localDla, appointment.TestTypeId);
        }

    }
}
