using System;
using System.Data;
using DVLD.DataAccessLayer;
using DVLD.EntityLayer;

namespace DVLD.BusinessLayer
{
    public class ApplicationTypeService
    {
        public ApplicationType Info { get; private set; }

        public ApplicationTypeService(ApplicationType appType)
        {
            Info = appType ?? throw new ArgumentNullException(nameof(appType));
        }

        public static DataTable GetAllApplicationTypes() => ApplicationTypeData.GetAllAsTable();
        public static ApplicationType FindById(int appTypeId) => ApplicationTypeData.GetById(appTypeId);
        public static ApplicationType FindByType(enApplicationType applicationType) => ApplicationTypeData.GetByType(applicationType);


        public bool Save()
        {
            if (string.IsNullOrWhiteSpace(Info.Title)) return false;
            if (Info.Fees < 0) return false;

            return ApplicationTypeData.Update(Info);
        }
    }
}