using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD.DataAccessLayer;
using DVLD.EntityLayer;

namespace DVLD.BusinessLayer
{
    public static class ApplicationTypeBusiness
    {
        public static DataTable GetAll()
        {
            return ApplicationTypeData.GetAllApplicationTypes();
        }

        public static ApplicationType Find(ApplicationType.enApplicationType applicationTypeId)
        {
            DataRow row = ApplicationTypeData.GetById((int)applicationTypeId);

            if (row == null) 
                return null;

            return new ApplicationType(
                applicationTypeId: (ApplicationType.enApplicationType)row["ApplicationTypeID"],
                title: (string)row["ApplicationTypeTitle"],
                fees: (decimal)row["ApplicationFees"]);
        }

        public static bool Save(ApplicationType applicationType)
        {
            return ApplicationTypeData.UpdateById((int)applicationType.Id, applicationType.Title, applicationType.Fees);
        }
    }
}
