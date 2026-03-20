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
            
            return LicenseData.GetById(licenseId);
        }

        public static License FindByApplicationId(int applicationId)
        {
            if (applicationId <= 0)
                throw new Exception("Invalid application id!");

            return LicenseData.GetByMainApplicationId(applicationId);
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
            {
                // 1. Find the Main Application Info
                Info.MainApplicationInfo = ApplicationService.FindById(Info.ApplicationId);

                if (Info.MainApplicationInfo == null)
                    return false;

                if (Info.MainApplicationInfo.Status != enApplicationStatus.New)
                    return false;

                Info.DriverInfo = DriverService.FindById(Info.DriverId);

                // 2. Create driver record if not exists
                if (Info.DriverInfo == null)
                {
                    Driver driver = new Driver(personId: Info.MainApplicationInfo.ApplicantPersonId, createdByUserId: Info.CreatedByUserId);
                    var driverService = new DriverService(driver);

                    if (!driverService.Save())
                        return false;

                    Info.DriverId = driver.Id;
                    Info.DriverInfo = driverService.Info;
                }

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
            
            return false; // issued license can't be edited
        }
    }
}
