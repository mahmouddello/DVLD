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

        // ── Static Lookups ────────────────────────────────
        public static DataTable GetAllAsTable() => InternationalLicenseData.GetAllAsTable(); 

        public static InternationalLicense GetById(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException("Id value should be greater than zero");

            InternationalLicense internationalLicense = InternationalLicenseData.GetById(id);

            if (internationalLicense == null)
                return null;

            ResolveNavigationProperties(internationalLicense);
            return internationalLicense;
        }

        public static InternationalLicense GetByLicenseId(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException("Id value should be greater than zero");

            InternationalLicense internationalLicense = InternationalLicenseData.GetByLicenseId(id);

            if (internationalLicense == null)
                return null;

            ResolveNavigationProperties(internationalLicense);
            return internationalLicense;
        }

        public static InternationalLicense GetByApplicationId(int id)
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

        // ── Instance Methods ────────────────────────────────



        // ── Helpers ────────────────────────────────
        private static void ResolveNavigationProperties(InternationalLicense internationalLicense)
        {
            internationalLicense.ApplicationInfo = ApplicationService.FindById(internationalLicense.ApplicationId);
            internationalLicense.LocalLicense = LicenseService.FindById(internationalLicense.LocalLicenseId);
        }
    }
}
