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
        private int _personId;
        private int _ldlaId;

        private enum Mode { AddNew = 0, Update = 1 }
        private Mode mode;
        
        private Application _application;
        private LDLA _ldlaApplication;

        public frmAddEditLocalDLA()
        {
            InitializeComponent();
            mode = Mode.AddNew;
        }

        public frmAddEditLocalDLA(int ldlaId)
        {
            InitializeComponent();

            _ldlaId = ldlaId;
            mode = Mode.Update;
        }

        private void LocalDrivingLicenseApplication_Load(object sender, EventArgs e)
        {
            ApplyPreSettings();

            if (mode == Mode.Update)
                LoadApplicationInfo();
        }

        private void LoadLicenseClassesToComboBox()
        {
            cbLicenseClass.Items.Add("None");
            DataTable dt = LicenseClassService.GetAll();

            foreach(DataRow row in dt.Rows)
                cbLicenseClass.Items.Add(row["ClassName"]);
        }

        private void ApplyPreSettings()
        {
            LoadLicenseClassesToComboBox();
            cbLicenseClass.SelectedIndex = 0; // Default, None
            lblApplicationFees.Text = ApplicationTypeService.FindByType(enApplicationType.NewLocalDrivingLicense)?
                .Fees.ToString();

            if (mode == Mode.AddNew)
            {
                lblTitle.Text = "New Local Driving License Application";
                lblApplicationDate.Text = DateTime.Now.ToShortDateString();
                lblCreatedBy.Text = Globals.CurrentUser.Username;

                _application = new Application();
                _ldlaApplication = new LDLA();
                return;
            }
        }

        private void LoadApplicationInfo()
        {
            // buttons
            btnNext.Enabled = true;
            btnSave.Enabled = true;

            _ldlaApplication = LDLAService.FindById(_ldlaId);

            if (_ldlaApplication == null)
            {
                Utility.ShowErrorMessage("Local License wasn't found, this form will be closed!");
                this.Close();
                return;
            }

            _application = _ldlaApplication.MainApplicationInfo;

            ctrlPersonCardWithFilter1.ShowAddPerson = false;
            ctrlPersonCardWithFilter1.FilterEnabled = false;
            ctrlPersonCardWithFilter1.QueryText = $"{_application.ApplicantPersonId}";
            ctrlPersonCardWithFilter1.ctrlPersonCard1.LoadPersonInfo(_personId);

            cbLicenseClass.SelectedIndex = _ldlaApplication.LicenseClassId;
            lblApplicationId.Text = _application.Id.ToString();
            lblApplicationFees.Text = _application.PaidFees.ToString();
            lblCreatedBy.Text = _application.CreatorUserInfo.Username;
            lblApplicationDate.Text = _application.ApplicationDate.ToShortDateString();
        }

        private void ctrlPersonCardWithFilter1_OnPersonSelected(int obj)
        {
            _personId = obj;

            // enable next button and _application info form
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
                _application.Id = -1;
                _application.ApplicationDate = DateTime.Now;
                _application.LastStatusDate = DateTime.Now;
            }
            else
                _application.LastStatusDate = DateTime.Now; // Updating

            _application.ApplicantPersonId = _personId;
            _application.ApplicationTypeId = Convert.ToInt32(enApplicationType.NewLocalDrivingLicense);
            _application.Status = enApplicationStatus.New;
            _application.PaidFees = (decimal)ApplicationTypeService.FindByType(enApplicationType.NewLocalDrivingLicense)?.Fees;
            _application.CreatedByUserId = GlobalClasses.Globals.CurrentUser.Id;
        }

        private void MapLocalDrivingLicenseApplicationFields()
        {
            if (mode == Mode.AddNew)
                _ldlaApplication.Id = -1;

            _ldlaApplication.MainApplicationId = _application.Id;
            _ldlaApplication.LicenseClassId = cbLicenseClass.SelectedIndex;
        }

        private bool IsFormValid()
        {
            if (ValidateChildren())
                return true;

            Utility.ShowErrorMessage("Some fields are not valid!");
            return false;
        }

        private bool HasDuplicateApplication()
        {
            if (!ApplicationService.HasSameClassApplication(
                    _application.ApplicantPersonId,
                    cbLicenseClass.SelectedIndex))
                return false;

            MessageBox.Show(
                @"Choose another license class, the selected person already has an active or completed _application for the selected class.",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);

            return true;
        }

        private bool SatisfiesMinimumAllowedAge()
        {
            int selectedLicenseClassId = cbLicenseClass.SelectedIndex;

            if (ApplicationService.MeetsMinimumAgeRequirement(selectedLicenseClassId, _application.ApplicantPersonId))
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
            var appService = new ApplicationService(_application);

            if (!appService.Save())
            {
                Utility.ShowErrorMessage("Failed to save _application!");
                return false;
            }

            Utility.ShowSuccessMessage($"Saved the _application successfully with id {_application.Id}");
            _application = ApplicationService.FindById(_application.Id); // load the full object

            lblApplicationId.Text = _application.Id.ToString();
            lblCreatedBy.Text = _application.CreatorUserInfo.Username;
            return true;
        }

        private void SaveLocalApplication()
        {
            MapLocalDrivingLicenseApplicationFields();
            var ldlaService = new LDLAService(_ldlaApplication);

            if (ldlaService.Save())
                Utility.ShowSuccessMessage($"Saved the local driving license _application successfully!");
            else
                Utility.ShowErrorMessage("Failed to save local _application!");
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
