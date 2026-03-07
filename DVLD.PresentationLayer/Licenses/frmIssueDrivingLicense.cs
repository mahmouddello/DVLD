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
        private int ldlaId;
        public frmIssueDrivingLicense(int ldlaId)
        {
            InitializeComponent();
            this.ldlaId = ldlaId;
            this.ctrlApplicationDetails1.LoadApplicationInfo(ldlaId);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private clsDriver GetOrCreateDriverRecord(int personId)
        {
            clsDriver driver = clsDriverBusiness.FindByPersonId(personId);
            if (driver != null)
                return driver;

            // no driver record, create a new one
            driver = new clsDriver
            {
                Id = -1,
                PersonId = personId,
                CreatedByUserId = GlobalClasses.Globals.CurrentUser.Id,
                CreatedAt = DateTime.Now
            };

            if (!clsDriverBusiness.Save(driver))
                return null;

            return driver;
        }
        private void btnIssue_Click(object sender, EventArgs e)
        {
            LDLA ldla = LDLAService.FindById(ldlaId);
            clsDriver driver = GetOrCreateDriverRecord(ldla.MainApplicationInfo.ApplicantPersonId);

            if (driver == null)
            {
                Utility.ShowErrorMessage("Driver record failed found or to add! Aborting...");
                this.Close();
                return;
            }

            // license issue procedure
            string rawNotes = txtNotes.Text.Trim();
            DateTime expirationDate = DateTime.Now.AddYears(ldla.LicenseClassInfo.DefaultValidityLength);

            clsLicense license = new clsLicense(
                id: -1,
                applicationId: ldla.MainApplicationId,
                driverId: driver.Id,
                licenseClassID: ldla.LicenseClassId,
                issueDate: DateTime.Now,
                expirationDate: expirationDate,
                notes: string.IsNullOrWhiteSpace(rawNotes) ? null : rawNotes,
                paidFees: ldla.LicenseClassInfo.Fees,
                isActive: true,
                issueReason: enLicenseIssueReason.FirstTime,
                createdByUserId: GlobalClasses.Globals.CurrentUser.Id
            );

            if (clsLicenseBusiness.Save(license))
            {
                Utility.ShowSuccessMessage($"Issued the new license successfully with id: {license.Id}");

                var appService = new ApplicationService(ldla.MainApplicationInfo);

                if (!(ldla.MainApplicationInfo != null && appService.Complete()))
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
                Utility.ShowErrorMessage($"Failed to issue the new license for driver with id: {driver.Id}");
                this.Close();
            }
        }
    }
}
