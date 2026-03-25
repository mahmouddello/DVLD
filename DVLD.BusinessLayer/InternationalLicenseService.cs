using System;
using System.Data;
using DVLD.DataAccessLayer;
using DVLD.EntityLayer;

namespace DVLD.BusinessLayer
{
    public class InternationalLicenseService
    {
        public InternationalLicense Info { get; private set; }

        public InternationalLicenseService(InternationalLicense info)
        {
            Info = info ?? throw new ArgumentNullException(nameof(info));
        }

        // ── Static Methods ────────────────────────────────
        public static DataTable GetAllAsTable() => InternationalLicenseData.GetAllAsTable();

        public static DataTable GetAllAsTableForDriver(int driverId) => InternationalLicenseData.GetAllAsTableForDriver(driverId); 

        public static InternationalLicense FindById(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException("Id value should be greater than zero");

            InternationalLicense internationalLicense = InternationalLicenseData.GetById(id);

            if (internationalLicense == null)
                return null;

            ResolveNavigationProperties(internationalLicense);
            return internationalLicense;
        }

        public static InternationalLicense FindByLicenseId(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException("Id value should be greater than zero");

            InternationalLicense internationalLicense = InternationalLicenseData.GetByLicenseId(id);

            if (internationalLicense == null)
                return null;

            ResolveNavigationProperties(internationalLicense);
            return internationalLicense;
        }

        // BLL - simpler and accurate check
        public static bool ExistsActiveByLocalLicenseId(int licenseId) 
            => InternationalLicenseData.ExistsActiveNonExpiredByLocalLicenseId(licenseId);

        public static InternationalLicense FindByApplicationId(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException("Id value should be greater than zero");

            InternationalLicense internationalLicense = InternationalLicenseData.GetByApplicationId(id);

            if (internationalLicense == null)
                return null;

            ResolveNavigationProperties(internationalLicense);
            return internationalLicense;
        }

        public static bool ExistsById(int id) => InternationalLicenseData.ExistsById(id);

        public static bool ExistsByLocalLicenseId(int id) => InternationalLicenseData.ExistsByLocalLicenseId(id);

        public static bool ExistsByApplicationId(int id) => InternationalLicenseData.ExistsByApplicationId(id);

        public static InternationalLicense IssueInternationalLicense(License localLicense, int createdByUserId, int validityLength)
        {
            // 1. Business logic checks
            if (localLicense == null)
                throw new ArgumentNullException(nameof(localLicense));

            if (createdByUserId <= 0)
                throw new ArgumentOutOfRangeException(nameof(createdByUserId));

            if (validityLength < 1 || validityLength > 3)
                throw new ArgumentOutOfRangeException(nameof(validityLength), "Validity must be between 1 and 3 years");

            if ((enLicenseClass)localLicense.LicenseClassID != enLicenseClass.C3_Ordinary)
                throw new ConstraintException("Only ordinary driving licenses are accepted");

            if (!localLicense.IsActive)
                throw new ConstraintException("The local license is not active");

            if (localLicense.IsExpired)
                throw new ConstraintException("The local license is expired");

            if (ExistsActiveByLocalLicenseId(localLicense.Id))
                throw new ConstraintException("An active international license already exists for this local license");

            // 2. Create a new international license application

            // international license fees
            decimal fees = ApplicationTypeService.FindByType(enApplicationType.NewInternationalLicense).Fees;
            Application application = new Application(
                id: -1,
                applicantPersonId: localLicense.DriverInfo.PersonId,
                applicationTypeId: (int)enApplicationType.NewInternationalLicense,
                createdByUserId: createdByUserId,
                applicationDate: DateTime.Now,
                status: enApplicationStatus.Completed,
                lastStatusDate: DateTime.Now,
                paidFees: fees
            );  

            var applicationService = new ApplicationService(application);
            if (!applicationService.Save())
                return null;

            // 3. Create and add the new license record
            int mainApplicationId = applicationService.Info.Id;
            InternationalLicense intlLicense = CreateIntlLicenseObject(localLicense, mainApplicationId, createdByUserId, validityLength);

            var service = new InternationalLicenseService(intlLicense);
            if (!service.Save())
                return null;

            return service.Info;
        }

        private static InternationalLicense CreateIntlLicenseObject(License localLicense, int mainApplicationId, int createdByUserId, int validityLength)
        {
            return new InternationalLicense(
                internationalLicenseId: -1,
                applicationId: mainApplicationId,
                driverId: localLicense.DriverId,
                localLicenseId: localLicense.Id,
                issueDate: DateTime.Now,
                expirationDate: DateTime.Now.AddYears(validityLength),
                isActive: true,
                createdByUserId: createdByUserId
            );
        }

        // ── Instance Methods ────────────────────────────────
        public bool Save()
        {
            if (!Info.IsValid)
                return false;

            if (Info.IsNew)
            {
                Info.Id = InternationalLicenseData.Add(Info);
                return !Info.IsNew;
            }

            return false; // no update method currently
        }


        // ── Helpers ────────────────────────────────
        private static void ResolveNavigationProperties(InternationalLicense internationalLicense)
        {
            internationalLicense.ApplicationInfo = ApplicationService.FindById(internationalLicense.ApplicationId);
            internationalLicense.LocalLicense = LicenseService.FindById(internationalLicense.LocalLicenseId);
        }

    }
}
