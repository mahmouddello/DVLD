using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD.BusinessLayer;
using DVLD.EntityLayer;

namespace DVLD.PresentationLayer.Applications
{
    public partial class frmLocalDrivingLicenseApplication : Form
    {
        private int personId;
        private int localDrivingLicenseId;
        private enum Mode { AddNew = 0, Update = 1 }
        private Mode mode;
        private decimal paidFees;

        private ApplicationType.enApplicationType applicationType = ApplicationType.enApplicationType.NewLocalDrivingLicense;
        private EntityLayer.Application application;
        private EntityLayer.LocalDrivingLicenseApplication localDrivingLicenseApplication;

        public frmLocalDrivingLicenseApplication()
        {
            InitializeComponent();
            mode = Mode.AddNew;
        }

        public frmLocalDrivingLicenseApplication(int localDrivingLicenseId)
        {
            InitializeComponent();
            this.localDrivingLicenseId = localDrivingLicenseId;
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
            btnNext.Enabled = false;
            btnSave.Enabled = false;
            tpApplicationInfo.Enabled = false;

            lblTitle.Text = mode == Mode.AddNew ? "New Local Driving License Application" : "Update Local Driving License Application";
            lblApplicationDate.Text = DateTime.Now.ToShortDateString();

            paidFees = (decimal)(ApplicationTypeBusiness.Find(this.applicationType)?.Fees);
            lblApplicationFees.Text = paidFees.ToString();
            lblCreatedBy.Text = GlobalClasses.Globals.CurrentUser.Username;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LocalDrivingLicenseApplication_Load(object sender, EventArgs e)
        {
            ApplyPreSettings();
            LoadLicenseClassesToComboBox();
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
            application = new EntityLayer.Application();

            if (mode == Mode.AddNew)
            {
                this.application.Id = -1;
                this.application.ApplicantPersonId = personId;
                this.application.Date = DateTime.Now;
                this.application.ApplicationTypeId = Convert.ToInt32(applicationType);
                this.application.Status = EntityLayer.Application.ApplicationStatus.New;
                this.application.LastStatusDate = DateTime.Now;
                this.application.PaidFees = paidFees;
                this.application.CreatedByUserId = GlobalClasses.Globals.CurrentUser.Id;
            }
        }

        private void MapLocalDrivingLicenseApplicationFields()
        {
            localDrivingLicenseApplication = new EntityLayer.LocalDrivingLicenseApplication();

            if (mode == Mode.AddNew)
            {
                localDrivingLicenseApplication.Id = -1;
                localDrivingLicenseApplication.MainApplicationId = this.application.Id;
                localDrivingLicenseApplication.LicenseClassId = cbLicenseClass.SelectedIndex;
            }
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
            if (!ApplicationBusiness.HasSameClassApplication(
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

            if (ApplicationBusiness.MeetsMinimumAgeRequirement(selectedLicenseClassId, this.application.ApplicantPersonId))
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
            if (!ApplicationBusiness.Save(this.application))
            {
                MessageBox.Show("Failed to save application!");
                return false;
            }

            MessageBox.Show($"Saved the new application successfully with id {application.Id}");
            lblApplicationId.Text = application.Id.ToString();
            return true;
        }

        private void SaveLocalApplication()
        {
            MapLocalDrivingLicenseApplicationFields();

            if (LocalDrivingLicenseApplicationBusiness.Save(this.localDrivingLicenseApplication))
                MessageBox.Show($"Saved the new local driving license application successfully with id {localDrivingLicenseApplication.Id}");
            else
                MessageBox.Show("Failed to save local application!");
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
