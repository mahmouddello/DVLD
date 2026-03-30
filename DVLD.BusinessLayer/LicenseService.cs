using System;
using System.Data;
using DVLD.DataAccessLayer;
using DVLD.EntityLayer;

namespace DVLD.BusinessLayer
{
    public class LicenseService
    {
        public License Info { get; private set; }

        public LicenseService(License license)
        {
            Info = license ?? throw new ArgumentNullException(nameof(License));
        }

        // ── Static Lookups ────────────────────────────────

        public static DataTable GetAllLicensesAsTable(int driverId) => LicenseData.GetAllLicensesAsTable(driverId);

        public static License FindById(int licenseId)
        {
            if (licenseId <= 0)
                throw new Exception("Invalid license id!");

            var license = LicenseData.GetById(licenseId);

            if (license == null)
                return null;

            ResolveNavigationProperties(license);

            return license;
        }

        public static License FindByApplicationId(int applicationId)
        {
            if (applicationId <= 0)
                throw new Exception("Invalid application id!");

            var license = LicenseData.GetByMainApplicationId(applicationId);

            if (license == null)
                return null;

            ResolveNavigationProperties(license);

            return license;
        }

        public static int GetActiveLicenseForDriver(int driverId) => LicenseData.GetActiveLicenseCount(driverId);

        private static License CreateLicense(LDLA localDla, string notes, int userId)
        {
            return new License(
                applicationId: localDla.MainApplicationId,
                notes: notes,
                createdByUserId: userId,
                issueReason: enLicenseIssueReason.FirstTime,
                licenseClass: (enLicenseClass)localDla.LicenseClassId
            );
        }

        public static License IssueDLFirstTime(int ldlaId, string rawNotes, int createdByUserId)
        {
            var localDla = LDLAService.FindById(ldlaId);
            bool hasPassedAllTests = TestService.GetPassedTestCount(localDla.Id) == 3;

            if (localDla == null || !hasPassedAllTests)
                return null;

            var license = CreateLicense(localDla, rawNotes, createdByUserId);
            var licenseService = new LicenseService(license);

            if (!licenseService.Save())
                return null;

            ApplicationService applicationService = new ApplicationService(localDla.MainApplicationInfo);

            if (!applicationService.Complete())
            {
                // rollback
                //LicenseData.Delete(license.Id);
                return null;
            }

            return license;
        }

        // ── Instance Methods ──────────────────────────────
        public bool Save()
        {
            if (Info.IsNew)
                return IssueNewLicense();

            return false; // issued license can't be edited
        }

        private bool IssueNewLicense()
        {
            // 1. Find the Main Application Info
            Info.MainApplicationInfo = ApplicationService.FindById(Info.ApplicationId);

            if (Info.MainApplicationInfo == null)
                return false;

            // first time issues
            if (Info.IssueReason == enLicenseIssueReason.FirstTime &&
                Info.MainApplicationInfo.Status != enApplicationStatus.New)
                return false;

            Info.DriverInfo = DriverService.FindById(Info.DriverId);

            // 2. Create driver record if not exists
            Driver driver = DriverService.FindByPersonId(Info.MainApplicationInfo.ApplicantPersonId);

            if (driver == null)
            {
                driver = new Driver(personId: Info.MainApplicationInfo.ApplicantPersonId, createdByUserId: Info.CreatedByUserId);
                DriverService driverService = new DriverService(driver);

                if (!driverService.Save())
                    return false;

                driver = driverService.Info;
            }

            Info.DriverId = driver.Id;
            Info.DriverInfo = driver;

            // 3. Assing the rest properties
            LicenseClass licenseClass = LicenseClassService.FindById(Info.LicenseClassID);
            if (licenseClass == null)
                return false;

            Info.ExpirationDate = DateTime.Now.AddYears(licenseClass.DefaultValidityLength);
            Info.PaidFees = licenseClass.Fees;
            Info.IsActive = true;

            // 4. Save the license
            Info.Id = LicenseData.Add(Info);
            return !Info.IsNew; // means we added the Info
        }

        public bool Deactivate()
        {
            if (Info == null) 
                return false;

            Info.IsActive = false;

            return LicenseData.UpdateLicenseStatus(Info.Id, false);
        }

        public License Renew(string notes, int createdByUserId)
        {
            // Business Logic Checks
            if (!Info.IsExpired)
                throw new ConstraintException("Renew Failed: License is not expired yet");

            if (!Info.IsValid)
                throw new ArgumentOutOfRangeException("License Information isn't valid to process");


            // Renew Proceess

            // We fetch application type and license incase their fees did change 
            var applicationType = ApplicationTypeService.FindByType(enApplicationType.RenewDrivingLicense);
            var licenseClass = LicenseClassService.FindById(Info.LicenseClassID);

            // 1. Create an application of type renew, and save it
            Application application = new Application(
                id: -1,
                applicantPersonId: Info.DriverInfo.PersonId,
                applicationTypeId: (int)enApplicationType.RenewDrivingLicense,
                createdByUserId: createdByUserId,
                applicationDate: DateTime.Now,
                status: enApplicationStatus.Completed,
                lastStatusDate: DateTime.Now,
                paidFees: applicationType.Fees
            );

            ApplicationService applicationService = new ApplicationService(application);

            if (!applicationService.Save())
                throw new NoNullAllowedException("Failed to create an application to renew the license");

            // 2. Create a license object, save it
            License newLicense = new License(
                applicationId: application.Id,
                notes: string.IsNullOrWhiteSpace(notes) ? null : notes,
                createdByUserId: createdByUserId,
                issueReason: enLicenseIssueReason.Renew,
                licenseClass: (enLicenseClass)Info.LicenseClassID
            );

            LicenseService newLicenseService = new LicenseService(newLicense);
            if (!newLicenseService.Save())
            {
                ApplicationService.Delete(application.Id); // Delete the application to avoid orphaned records
                throw new NoNullAllowedException("Failed to renew the license");
            }

            // 3. If license renewed, deactivate the old license

            Deactivate(); // deactivates the old license
            return newLicense;
        }

        public License Replace(enLicenseIssueReason replacementReason, int createdByUserId)
        {
            // Business Logic Checks
            if (Info.IsExpired)
                throw new ConstraintException("Replacement Failed: License is expired, you must apply for renewal");

            if (!Info.IsActive)
                throw new ConstraintException("Liceense can't be replaced becuase it's Inactive");

            if (!Info.IsValid)
                throw new ConstraintException("License Information isn't valid to process");

            bool reasonNotAcceptable = replacementReason != enLicenseIssueReason.ReplacementForDamaged
                                    && replacementReason != enLicenseIssueReason.ReplacementForLost;

            if (reasonNotAcceptable)
                throw new ConstraintException("Replacement reason information is invalid");


            // We fetch application type and license incase their fees did change 
            ApplicationType applicationType;
             if (replacementReason == enLicenseIssueReason.ReplacementForDamaged)
                applicationType = ApplicationTypeService.FindByType(enApplicationType.ReplaceDamagedDrivingLicense);
            else
                applicationType = ApplicationTypeService.FindByType(enApplicationType.ReplaceLostDrivingLicense);
            LicenseClass licenseClass = LicenseClassService.FindById(Info.LicenseClassID);

            // Replace Process

            // 1. Create a new application, set the type accroding to the replacement reason
            Application application = new Application(
                id: -1,
                applicantPersonId: Info.DriverInfo.PersonId,
                applicationTypeId: (int)replacementReason,
                createdByUserId: createdByUserId,
                applicationDate: DateTime.Now,
                status: enApplicationStatus.Completed,
                lastStatusDate: DateTime.Now,
                paidFees: applicationType.Fees
            );

            ApplicationService applicationService = new ApplicationService(application);
            if (!applicationService.Save())
                throw new NoNullAllowedException("Failed to create an application to replace the license");

            // 2. Create a license record, save it

            License newLicense = new License(
                applicationId: application.Id,
                notes: Info.Notes,
                createdByUserId: createdByUserId,
                issueReason: replacementReason,
                licenseClass: (enLicenseClass)Info.LicenseClassID
            );

            LicenseService newLicenseService = new LicenseService(newLicense);
            if (!newLicenseService.Save())
            {
                ApplicationService.Delete(application.Id); // Delete the application to avoid orphaned records
                throw new NoNullAllowedException("Failed to renew the license");
            }

            // Application and License Created, Assign fields
            newLicense.ApplicationId = application.Id;
            newLicense.LicenseClassID = Info.LicenseClassID;
            newLicense.DriverId = Info.DriverId;
            ResolveNavigationProperties(newLicense);

            Deactivate(); // deactivates the old license
            return newLicense;
        }

        public bool Detain(decimal fineFees, int createdByUserId, ref int detainId)
        {
            // Business Logic Checks
            if (Info.IsDetained)
                throw new ConstraintException("Detain Failed: License is already detained");

            if (Info.IsExpired)
                throw new ConstraintException("Detain Failed: License is expired, you must apply for renewal");

            if (!Info.IsActive)
                throw new ConstraintException("Liceense can't be detained becuase it's Inactive");

            if (!Info.IsValid)
                throw new ConstraintException("License Information isn't valid to process");

            detainId = DetainLicenseService.DetainLicense(Info, fineFees, createdByUserId);

            if (detainId == -1)
                return false;

            Info.IsDetained = true;
            return true;
        }

        public bool Release(decimal applicationFees, int createdByUserId, ref DetainLicense detainRecord)
        {
            // Business Logic Checks
            if (!Info.IsValid)
                throw new ConstraintException("License Information isn't valid to process");

            if (!Info.IsDetained)
                throw new ConstraintException("Release Failed: License is not detained at the moment");

            if (!Info.IsActive)
                throw new ConstraintException("Release Failed: You can't release an inactive license");

            if (Info.IsExpired)
                throw new ConstraintException("Release Failed: License is expired, you must apply for renewal");

            // 1. Create an application of type release and save it
            Application application = new Application(
                id: -1,
                applicantPersonId: Info.DriverInfo.PersonId,
                applicationTypeId: (int)enApplicationType.ReleaseDetainedLicense,
                createdByUserId: createdByUserId,
                applicationDate: DateTime.Now,
                status: enApplicationStatus.Completed,
                lastStatusDate: DateTime.Now,
                paidFees: applicationFees
            );

            ApplicationService applicationService = new ApplicationService(application);
            if (!applicationService.Save())
                throw new NoNullAllowedException("Failed to create an application to release the license");

            // 2. Create a detain service with detain record, update it's info

            DetainLicense recordCopy = new DetainLicense(detainRecord); // copy to avoid manipulating object data if fails
            recordCopy.IsReleased = true;
            recordCopy.ReleasedDate = DateTime.Now;
            recordCopy.ReleasedByUserId = createdByUserId;
            recordCopy.ReleaseApplicationId = application.Id;

            DetainLicenseService detainLicenseService = new DetainLicenseService(recordCopy);
            if (!detainLicenseService.Save())
            {
                ApplicationService.Delete(application.Id); // delete to avoid orphaned records
                throw new NoNullAllowedException("Failed to release the license from detain");
            }

            detainRecord = recordCopy; // assign after success
            Info = FindById(Info.Id); // refresh license object status

            return !Info.IsDetained;
        }

        // ── Helpers ──────────────────────────────
        private static void ResolveNavigationProperties(License license)
        {
            license.MainApplicationInfo = ApplicationService.FindById(license.ApplicationId);
            license.DriverInfo = DriverService.FindById(license.DriverId);
            license.LicenseClassInfo = LicenseClassService.FindById(license.LicenseClassID);
            license.IsDetained = DetainLicenseService.ExistsByLicenseId(license.Id);
        }

    }
}
