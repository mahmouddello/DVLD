using DVLD.BusinessLayer;
using DVLD.EntityLayer;
using DVLD.PresentationLayer.GlobalClasses;
using System;
using System.Windows.Forms;
using Application = DVLD.EntityLayer.Application;

namespace DVLD.PresentationLayer.Tests.TestAppointments
{
    public partial class frmScheduleEditAppointment : Form
    {
        private enum Mode { AddNew, Update, Locked }
        private Mode _mode;

        private int _localDlaId;
        private int _testTypeId;
        private int _testAppointmentId;

        // cached objects — set once in Load, read everywhere
        private LDLA _localDla;
        private TestAppointment _testAppointment;
        private TestType _testType;
        private ApplicationType _retakeType;
        private bool _hasFailedTest;

        public frmScheduleEditAppointment(int localDlaId, int testTypeId)
        {
            InitializeComponent();
            _mode = Mode.AddNew;
            _localDlaId = localDlaId;
            _testTypeId = testTypeId;
        }

        public frmScheduleEditAppointment(int testAppointmentId, bool isLocked)
        {
            InitializeComponent();
            _testAppointmentId = testAppointmentId;
            _mode = isLocked ? Mode.Locked : Mode.Update;
        }

        private void frmScheduleEditAppointment_Load(object sender, EventArgs e)
        {
            if (_mode == Mode.AddNew)
            {
                _localDla = LDLAService.FindById(_localDlaId);
                if (_localDla == null)
                {
                    Utility.ShowErrorMessage($"No Local DLA found with id: {_localDlaId}!");
                    return;
                }

                _testType = TestTypeService.FindById(_testTypeId);
                _hasFailedTest = TestService.HasFailedTest(_localDlaId, _testTypeId);

                dtpAppointment.Value = DateTime.Today;
                dtpAppointment.MinDate = DateTime.Today;
            }
            else
            {
                _testAppointment = TestAppointmentService.FindById(_testAppointmentId);
                if (_testAppointment == null)
                {
                    Utility.ShowErrorMessage($"Failed to find appointment with id: {_testAppointmentId}");
                    return;
                }

                // loaded navigation properties inside the object
                _localDla = _testAppointment.LdlaInfo;
                _testType = _testAppointment.TestTypeInfo;

                _hasFailedTest = TestService.HasFailedTest(_testAppointment.LdlaId, _testAppointment.TestTypeId);
                dtpAppointment.Value = _testAppointment.AppointmentDate;
                dtpAppointment.MinDate = DateTime.Now < _testAppointment.AppointmentDate
                    ? DateTime.Now
                    : _testAppointment.AppointmentDate;

                if (_mode == Mode.Locked)
                {
                    lblAdditional.Text = "Person already has sat for this test, you can't modify this appointment!";
                    btnSave.Enabled = false;
                    dtpAppointment.Enabled = false;
                }
            }

            _retakeType = ApplicationTypeService.FindByType(enApplicationType.RetakeTest); // load once incase of usage
            PopulateForm();
            SetUIByTestType();
        }

        private void SetUIByTestType()
        {
            switch (_testType.Id)
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

        private void PopulateForm()
        {
            lblLocalDla.Text = _localDla.Id.ToString();
            lblLicenseClass.Text = _localDla.LicenseClassInfo.Name;
            lblPersonName.Text = _localDla.MainApplicationInfo.ApplicantPersonInfo.FullName;
            lblTrialCount.Text = TestService.GetTrialsCount(_localDla.Id, _testType.Id).ToString();
            lblFees.Text = _testType.Fees.ToString();

            // groupbox
            gbRetakeTest.Enabled = _hasFailedTest;
            lblRetakeApplicationFees.Text = _retakeType.Fees.ToString();
            lblTotalFees.Text = _hasFailedTest ?
                (_retakeType.Fees + _testType.Fees).ToString() :
                _testType.Fees.ToString();
            lblAdditional.Text = string.Empty;

            if (_testAppointment != null && _testAppointment.HasRetakeApplication)
                lblRetakeTestApplicationID.Text = _testAppointment.RetakeTestApplicationId.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveTestAppointment();
        }

        private void SaveTestAppointment()
        {
            if (_mode == Mode.Update)
                UpdateExistingAppointment();
            else
                CreateNewAppointment();
        }

        private void UpdateExistingAppointment()
        {
            _testAppointment.AppointmentDate = dtpAppointment.Value;

            var service = new TestAppointmentService(_testAppointment);
            if (!service.Save())
            {
                Utility.ShowErrorMessage("Error updating the appointment date!");
                return;
            }

            Utility.ShowSuccessMessage("Updated the appointment successfully!");
        }

        private void CreateNewAppointment()
        {
            Application retakeApplication = null;

            if (_hasFailedTest)
            {
                retakeApplication = BuildRetakeApplication();
                var appService = new ApplicationService(retakeApplication);

                if (!appService.Save())
                {
                    Utility.ShowErrorMessage("Error saving retake test application. This form will be closed.");
                    this.Close();
                    return;
                }
            }


            var testAppointment = BuildNewTestAppointment(retakeApplication?.Id ?? -1);
            var testAppointmentService = new TestAppointmentService(testAppointment);

            if (!testAppointmentService.Save())
            {
                // Delete the retake test application incase of test appointment failed to save
                if (retakeApplication != null)
                    ApplicationService.Delete(retakeApplication.Id);

                Utility.ShowErrorMessage("Error booking the new appointment!");
                return;
            }

            // Success
            _testAppointment = testAppointment;
            _mode = Mode.Update;
            lblRetakeTestApplicationID.Text = testAppointment.HasRetakeApplication
                ? testAppointment.RetakeTestApplicationId.ToString()
                : string.Empty;

            Utility.ShowSuccessMessage($"Saved the new appointment successfully with id: {testAppointment.Id}");
        }

        private Application BuildRetakeApplication()
        {
            ApplicationType retakeType = ApplicationTypeService.FindByType(enApplicationType.RetakeTest);

            return new Application
            {
                ApplicantPersonId = _localDla.MainApplicationInfo.ApplicantPersonId,
                ApplicationDate = DateTime.Now,
                ApplicationTypeId = retakeType.Id,
                Status = enApplicationStatus.Completed,
                LastStatusDate = DateTime.Now,
                PaidFees = retakeType.Fees,
                CreatedByUserId = Globals.CurrentUser.Id
            };
        }

        private TestAppointment BuildNewTestAppointment(int retakeApplicationId)
        {
            return new TestAppointment
            {
                TestTypeId = _testType.Id,
                LdlaId = _localDlaId,
                AppointmentDate = dtpAppointment.Value,
                PaidFees = _testType.Fees,
                CreatedByUserId = Globals.CurrentUser.Id,
                IsLocked = false,
                RetakeTestApplicationId = retakeApplicationId
            };
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
