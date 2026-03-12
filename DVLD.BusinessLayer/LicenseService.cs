using System;
using System.Data;
using System.Linq;
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

        // ── Instance Methods ──────────────────────────────
        public bool Save()
        {
            if (Info.IsNew)
            {
                Info.Id = LicenseData.Add(Info);
                
                return !Info.IsNew; // means we added the Info
            }

            return false; // issued license can't be edited
        }
    }
}
