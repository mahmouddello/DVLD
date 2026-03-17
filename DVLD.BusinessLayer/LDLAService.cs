using System;
using System.Data;
using DVLD.DataAccessLayer;
using DVLD.EntityLayer;

namespace DVLD.BusinessLayer
{
    public class LDLAService
    {
        public LDLA Info { get; private set; }

        public LDLAService(LDLA ldla)
        {
            Info = ldla ?? throw new ArgumentNullException(nameof(ldla));
        }

        // ── Static Lookups ────────────────────────────────
        public static DataTable GetAllAsTable() => LDLAData.GetAllAsTable();

        public static LDLA FindById(int id)
        {
            LDLA ldla = LDLAData.GetById(id);
            if (ldla == null) return null;

            ResolveNavigationProperties(ldla);
            return ldla;
        }

        public static LDLA FindByMainApplicationId(int mainAppId)
        {
            LDLA ldla = LDLAData.GetByApplicationId(mainAppId);
            if (ldla == null) return null;

            ResolveNavigationProperties(ldla);
            return ldla;
        }

        public static bool Delete(int id) => LDLAData.Delete(id);

        private static void ResolveNavigationProperties(LDLA ldla)
        {
            ldla.MainApplicationInfo = ApplicationService.FindById(ldla.MainApplicationId);
            ldla.LicenseClassInfo = LicenseClassService.FindById(ldla.LicenseClassId);
        }

        // ── Instance Methods ──────────────────────────────
        public bool Save()
        {
            if (Info.MainApplicationId == -1) return false;
            if (Info.LicenseClassId == -1) return false;

            if (Info.IsNew)
            {
                Info.Id = LDLAData.Add(Info.MainApplicationId, Info.LicenseClassId);
                return !Info.IsNew;
            }

            return LDLAData.UpdateLicenseClass(Info.Id, Info.LicenseClassId);
        }

    }
}
