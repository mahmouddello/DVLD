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

        public static ApplicationType Find(int applicationId)
        {
            DataRow row = ApplicationTypeData.GetById(applicationId);

            if (row == null) 
                return null;

            return new ApplicationType(
                id: (int)row["ApplicationTypeID"],
                title: (string)row["ApplicationTypeTitle"],
                fees: (decimal)row["ApplicationFees"]);
        }

        public static bool Save(ApplicationType application)
        {
            return ApplicationTypeData.UpdateById(application.Id, application.Title, application.Fees);
        }
    }
}
