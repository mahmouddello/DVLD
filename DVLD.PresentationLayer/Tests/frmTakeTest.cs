using DVLD.BusinessLayer;
using DVLD.EntityLayer;
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

namespace DVLD.PresentationLayer.Tests
{
    public partial class frmTakeTest : Form
    {
        private int testAppointmentId;
        private TestAppointment testAppointment;
        private Test test;

        private enum enMode { AddNew = 0 , Locked = 1}
        private enMode mode;

        public frmTakeTest(int testAppointmentId, bool isLocked)
        {
            InitializeComponent();

            if (isLocked)
                mode = enMode.Locked;
            else
                mode = enMode.AddNew;

            this.testAppointmentId = testAppointmentId;
        }

        private void SetDefaultSettings()
        {
            switch (testAppointment.TestTypeId)
            {
                case 1:
                    gbTest.Text = "Vision Test";
                    pictureBox1.Image = Properties.Resources.vision_test;
                    break;

                case 2:
                    gbTest.Text = "Written Test";
                    pictureBox1.Image = Properties.Resources.written_test;
                    break;

                case 3:
                    gbTest.Text = "Street Test";
                    pictureBox1.Image = Properties.Resources.driving_test;
                    break;
            }

            if (mode == enMode.Locked)
            {
                lblAdditional.Text = "You can't change the result!";
                gbResult.Enabled = false;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmTakeTest_Load(object sender, EventArgs e)
        {
            testAppointment = TestAppointmentBusiness.Find(testAppointmentId);

            if (testAppointment == null)
            {
                Utility.ShowErrorMessage($"Test Appointment with id: {testAppointmentId} wasn't found!");
                this.Close();
                return;
            }

            FillTestInfo();
        }

        private bool ValidateRadioSelection(GroupBox groupBox)
        {
            return groupBox.Controls
                           .OfType<RadioButton>()
                           .Any(r => r.Checked);
        }

        private void LoadAppointmentSharedData(TestAppointment testAppointment)
        {
            int ldlaId = testAppointment.LocalDrivingLicenseApplicationId;
            var localDla = LocalDrivingLicenseApplicationBusiness.Find(ldlaId);

            if (localDla == null)
            {
                Utility.ShowErrorMessage($"Local Application with id: {ldlaId} not found!");
                this.Close();
                return;
            }

            SetDefaultSettings();

            lblTestType.Text = "Scheduled Test";
            lblLocalDla.Text = localDla.Id.ToString();
            lblLicenseClass.Text = localDla.LicenseClassInfo.Name;
            lblPersonName.Text = localDla.MainApplicationInfo.ApplicantPersonInfo.FullName;
            lblTrialCount.Text = TestBusiness
                .GetTrialsCount(localDla.Id, testAppointment.TestTypeId)
                .ToString();

            lblDate.Text = testAppointment.AppointmentDate.ToShortDateString();
            lblFees.Text = TestTypeBusiness
                .Find(testAppointment.TestTypeId)
                .Fees.ToString();
        }

        private void FillTestInfo()
        {
            switch (mode)
            {
                case enMode.Locked:
                    LoadLockedMode();
                    break;

                case enMode.AddNew:
                    LoadNewMode();
                    break;
                default:
                    break;
            }
        }

        private void LoadNewMode()
        {
            lblTestID.Text = "???";
            LoadAppointmentSharedData(testAppointment);
        }

        private void LoadLockedMode()
        {
            int associatedTestId = TestAppointmentBusiness.GetAssociatedTestId(testAppointmentId);
            Test test = TestBusiness.Find(associatedTestId);

            if (test == null)
            {
                Utility.ShowErrorMessage($"No associated test found for the appointment with id: {testAppointmentId}!");
                this.Close();
                return;
            }

            txtNotes.Text = test.Notes;
            lblTestID.Text = associatedTestId.ToString();
            lblAdditional.Text = "You can't change the result!";

            gbResult.Enabled = false;
            rbPass.Checked = Convert.ToBoolean(test.Result);
            rbFail.Checked = !rbPass.Checked;

            LoadAppointmentSharedData(testAppointment);
        }

        private bool SaveTestRecord()
        {
            test = CreateAndMapTestObject();

            return TestBusiness.Save(test);
        }

        private Test CreateAndMapTestObject()
        {
            int ldlaId = testAppointment.LocalDrivingLicenseApplicationId;
            var localDla = LocalDrivingLicenseApplicationBusiness.Find(ldlaId);

            string notes = txtNotes.Text.Trim();

            return new Test(
                id: -1,
                testAppointmentId: testAppointmentId,
                testResult: rbPass.Checked ? TestResult.Passed : TestResult.Failed,
                notes: notes,
                createdByUserId: GlobalClasses.Globals.CurrentUser.Id
            );
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateRadioSelection(gbResult))
            {
                Utility.ShowErrorMessage("You need to select one option!");
                return;
            }

            if (mode != enMode.AddNew)
            {
                Utility.ShowErrorMessage("Saving mode is locked, you can't edit the test!");
                this.Close();
                return;
            }
                

            if (!SaveTestRecord())
            {
                Utility.ShowErrorMessage("Failed to save the test record!");
                this.Close();
                return;
            }

            lblTestID.Text = test.Id.ToString();
            Utility.ShowSuccessMessage($"Saved the test record with id: {test.Id}");
            TestAppointmentBusiness.UpdateAppointmentLockStatus(testAppointmentId, true);

            mode = enMode.Locked; // switch mode after adding
        }

    }
}
