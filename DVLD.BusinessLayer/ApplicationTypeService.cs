using System;
using System.Data;
using DVLD.DataAccessLayer;
using DVLD.EntityLayer;

namespace DVLD.BusinessLayer
{
    public class ApplicationTypeService
    {
        public ApplicationType Info { get; private set; }

        public ApplicationTypeService(ApplicationType applicationType)
        {
            Info = applicationType ?? throw new ArgumentNullException(nameof(applicationType));
        }

        public static DataTable GetAllApplicationTypes() => ApplicationTypeData.GetAllAsTable();
        public static ApplicationType FindByType(enApplicationType applicationType) => ApplicationTypeData.GetByType(applicationType);

        public bool Save()
        {
            if (string.IsNullOrWhiteSpace(Info.Title)) return false;
            if (Info.Fees < 0) return false;

            return ApplicationTypeData.Update(Info);
        }
    }
}