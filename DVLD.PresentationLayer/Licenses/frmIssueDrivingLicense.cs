using System;
using System.Windows.Forms;
using DVLD.BusinessLayer;
using DVLD.EntityLayer;
using DVLD.PresentationLayer.GlobalClasses;
using Application = DVLD.EntityLayer.Application;

namespace DVLD.PresentationLayer.Licenses
{
    public partial class frmIssueDrivingLicense : Form
    {
        private int _ldlaId;
        private LDLA _ldla;
        private clsDriver _driver;

        public frmIssueDrivingLicense(int ldlaId)
        {
            InitializeComponent();
            this._ldlaId = ldlaId;
        }

        private void frmIssueDrivingLicense_Load(object sender, EventArgs e)
        {
            this.ctrlApplicationDetails1.LoadApplicationInfo(_ldlaId);

        }

        private License CreateAndMapNewLicense()
        {
            string rawNotes = txtNotes.Text.Trim();
            DateTime expirationDate = DateTime.Now.AddYears(_ldla.LicenseClassInfo.DefaultValidityLength);

            return new License(
                id: -1,
                applicationId: _ldla.MainApplicationId,
                driverId: _driver.Id,
                licenseClassID: _ldla.LicenseClassId,
                issueDate: DateTime.Now,
                expirationDate: expirationDate,
                notes: string.IsNullOrWhiteSpace(rawNotes) ? null : rawNotes,
                paidFees: _ldla.LicenseClassInfo.Fees,
                isActive: true,
                issueReason: enLicenseIssueReason.FirstTime,
                createdByUserId: GlobalClasses.Globals.CurrentUser.Id
            );
        }

        private clsDriver GetOrCreateDriverRecord(int personId)
        {
            _driver = clsDriverBusiness.FindByPersonId(personId);
            if (_driver != null)
                return _driver;

            // no _driver record, create a new one
            _driver = new clsDriver
            {
                Id = -1,
                PersonId = personId,
                CreatedByUserId = GlobalClasses.Globals.CurrentUser.Id,
                CreatedAt = DateTime.Now
            };

            if (!clsDriverBusiness.Save(_driver))
                return null;

            return _driver;
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            _ldla = LDLAService.FindById(_ldlaId);
            clsDriver _driver = GetOrCreateDriverRecord(_ldla.MainApplicationInfo.ApplicantPersonId);

            if (_driver == null)
            {
                Utility.ShowErrorMessage("Driver record failed found or to add! Aborting...");
                this.Close();
                return;
            }

            // license issue procedure
            License license = CreateAndMapNewLicense();
            var licenseService = new LicenseService(license);

            if (licenseService.Save())
            {
                Utility.ShowSuccessMessage($"Issued the new license successfully with id: {license.Id}");
                var appService = new ApplicationService(_ldla.MainApplicationInfo);

                if (!(_ldla.MainApplicationInfo != null && appService.Complete()))
                {
                    Utility.ShowErrorMessage("Failed to update the status of the application!");
                    this.Close();
                    return;
                }
                else
                {
                    Utility.ShowSuccessMessage("Updated the application status as completed!");
                    this.Close();
                }
            }
            else
            {
                Utility.ShowErrorMessage($"Failed to issue the new license for driver with id: {_driver.Id}");
                this.Close();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
