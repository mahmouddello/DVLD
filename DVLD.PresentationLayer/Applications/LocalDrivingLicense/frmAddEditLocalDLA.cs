using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using DVLD.BusinessLayer;
using DVLD.EntityLayer;
using DVLD.PresentationLayer.GlobalClasses;
using Application = DVLD.EntityLayer.Application;

namespace DVLD.PresentationLayer.Applications
{
    public partial class frmAddEditLocalDLA : Form
    {
        private int personId;
        private int localDrivingLicenseId;
        private enum Mode { AddNew = 0, Update = 1 }
        private Mode mode;

        private enApplicationType applicationType = enApplicationType.NewLocalDrivingLicense;
        private Application application;
        private LDLA ldlaApplication;

        public frmAddEditLocalDLA()
        {
            InitializeComponent();
            mode = Mode.AddNew;
        }

        public frmAddEditLocalDLA(int ldlaId)
        {
            InitializeComponent();

            this.localDrivingLicenseId = ldlaId;
            mode = Mode.Update;
        }

        private void LoadLicenseClassesToComboBox()
        {
            cbLicenseClass.Items.Add("None");
            DataTable dt = LicenseClassBusiness.GetAll();

            foreach(DataRow row in dt.Rows)
                cbLicenseClass.Items.Add(row["ClassName"]);

            cbLicenseClass.SelectedIndex = 0;
        }

        private void ApplyPreSettings()
        {
            LoadLicenseClassesToComboBox();
            cbLicenseClass.SelectedIndex = 0; // Default, None

            if (mode == Mode.AddNew)
            {
                lblTitle.Text = "New Local Driving License Application";
                lblApplicationDate.Text = DateTime.Now.ToShortDateString();
                this.application = new Application();
                this.ldlaApplication = new LDLA();
                return;
            }
        }

        private void LoadApplicationInfo()
        {
            // buttons
            btnNext.Enabled = true;
            btnSave.Enabled = true;

            ldlaApplication = LDLAService.FindById(this.localDrivingLicenseId);

            if (ldlaApplication == null)
            {
                MessageBox.Show(
                    $"This form will be closed because there's no local application with ID = {localDrivingLicenseId}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                this.Close();
                return;
            }

            this.application = ldlaApplication.MainApplicationInfo;
            personId = this.application.ApplicantPersonId;

            ctrlPersonCardWithFilter1.ShowAddPerson = false;
            ctrlPersonCardWithFilter1.FilterEnabled = false;
            ctrlPersonCardWithFilter1.QueryText = $"{personId}";
            ctrlPersonCardWithFilter1.ctrlPersonCard1.LoadPersonInfo(personId);

            cbLicenseClass.SelectedIndex = ldlaApplication.LicenseClassId;
            lblApplicationId.Text = this.application.Id.ToString();
            lblApplicationFees.Text = this.application.PaidFees.ToString();
            lblCreatedBy.Text = this.application.CreatorUserInfo.Username;
            lblApplicationDate.Text = this.application.ApplicationDate.ToShortDateString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LocalDrivingLicenseApplication_Load(object sender, EventArgs e)
        {
            ApplyPreSettings();

            if (mode == Mode.Update)
                LoadApplicationInfo();
        }

        private void ctrlPersonCardWithFilter1_OnPersonSelected(int obj)
        {
            personId = obj;

            // enable next button and application info form
            btnNext.Enabled = true;
            tpApplicationInfo.Enabled = true;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tpApplicationInfo;
            btnSave.Enabled = true;
        }

        private void ctrlPersonCardWithFilter1_Load(object sender, EventArgs e)
        {

        }

        private void MapApplicationFields()
        {
            if (mode == Mode.AddNew)
            {
                this.application.Id = -1;
                this.application.ApplicationDate = DateTime.Now;
                this.application.LastStatusDate = DateTime.Now;
            }
            else
                this.application.LastStatusDate = DateTime.Now; // Updating

            this.application.ApplicantPersonId = personId;
            this.application.ApplicationTypeId = Convert.ToInt32(applicationType);
            this.application.Status = enApplicationStatus.New;
            this.application.PaidFees = (decimal)ApplicationTypeService.FindByType(applicationType)?.Fees;
            this.application.CreatedByUserId = GlobalClasses.Globals.CurrentUser.Id;
        }

        private void MapLocalDrivingLicenseApplicationFields()
        {
            if (mode == Mode.AddNew)
                ldlaApplication.Id = -1;

            ldlaApplication.MainApplicationId = this.application.Id;
            ldlaApplication.LicenseClassId = cbLicenseClass.SelectedIndex;
        }

        private bool IsFormValid()
        {
            if (ValidateChildren())
                return true;

            MessageBox.Show("Some fields are not valid!", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);

            return false;
        }

        private bool HasDuplicateApplication()
        {
            if (!ApplicationService.HasSameClassApplication(
                    this.application.ApplicantPersonId,
                    cbLicenseClass.SelectedIndex))
                return false;

            MessageBox.Show(
                @"Choose another license class, the selected person already has an active or completed application for the selected class.",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);

            return true;
        }

        private bool SatisfiesMinimumAllowedAge()
        {
            int selectedLicenseClassId = cbLicenseClass.SelectedIndex;

            if (ApplicationService.MeetsMinimumAgeRequirement(selectedLicenseClassId, this.application.ApplicantPersonId))
                return true;

            MessageBox.Show(
                "Choose another license class, the selected person doesn't satisfy the minimum age requirement!",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);

            return false;
        }

        private bool SaveApplication()
        {
            var appService = new ApplicationService(application);

            if (!appService.Save())
            {
                Utility.ShowErrorMessage("Failed to save application!");
                return false;
            }

            Utility.ShowSuccessMessage($"Saved the application successfully with id {application.Id}");
            application = ApplicationService.FindById(application.Id); // load the full object

            lblApplicationId.Text = application.Id.ToString();
            lblCreatedBy.Text = application.CreatorUserInfo.Username;
            return true;
        }

        private void SaveLocalApplication()
        {
            MapLocalDrivingLicenseApplicationFields();
            var ldlaService = new LDLAService(ldlaApplication);

            if (ldlaService.Save())
                Utility.ShowSuccessMessage($"Saved the local driving license application successfully!");
            else
                Utility.ShowErrorMessage("Failed to save local application!");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsFormValid())
                return;

            MapApplicationFields();

            if (HasDuplicateApplication())
                return;

            if (!SatisfiesMinimumAllowedAge())
                return;

            if (!SaveApplication())
                return;

            SaveLocalApplication();
        }

        private void cbLicenseClass_Validating(object sender, CancelEventArgs e)
        {
            if (cbLicenseClass.SelectedIndex == 0) // None
            {
                e.Cancel = true;
                errProvider.SetError(cbLicenseClass, "This field is required");
            }
            else
            {
                e.Cancel = false;
                errProvider.SetError(cbLicenseClass, string.Empty);
            }
        }
    }
}
