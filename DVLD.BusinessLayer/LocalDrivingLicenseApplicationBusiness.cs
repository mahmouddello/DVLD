using DVLD.DataAccessLayer;
using DVLD.EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.BusinessLayer
{
    public static class LocalDrivingLicenseApplicationBusiness
    {
        private static bool Add(LocalDrivingLicenseApplication localDrivingLicenseApplication)
        {
            localDrivingLicenseApplication.Id = LocalDrivingLicenseApplicationData.InsertNew
            (
                localDrivingLicenseApplication.MainApplicationId,
                localDrivingLicenseApplication.LicenseClassId
            );

            if (localDrivingLicenseApplication.Id != -1)
            {
                localDrivingLicenseApplication.MainApplicationInfo = ApplicationBusiness.Find(localDrivingLicenseApplication.MainApplicationId);
                localDrivingLicenseApplication.LicenseClassInfo = LicenseClassBusiness.Find(localDrivingLicenseApplication.LicenseClassId);
                return true;
            }

            return false;
        }

        public static bool Save(LocalDrivingLicenseApplication localDrivingLicenseApplication)
        {
            if (localDrivingLicenseApplication.Id == -1)
                return Add(localDrivingLicenseApplication);

            return false;
        }
    }
}
