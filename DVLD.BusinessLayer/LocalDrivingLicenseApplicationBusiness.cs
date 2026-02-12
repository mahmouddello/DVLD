using DVLD.DataAccessLayer;
using DVLD.EntityLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.BusinessLayer
{
    public static class LocalDrivingLicenseApplicationBusiness
    {
        public static DataTable GetAll()
        {
            return LocalDrivingLicenseApplicationData.GetAllApplications();
        }

        public static LocalDrivingLicenseApplication Find(int ldlaId)
        {
            DataRow row = LocalDrivingLicenseApplicationData.GetById(ldlaId);

            if (row == null)
                return null;

            LocalDrivingLicenseApplication ldla = new LocalDrivingLicenseApplication
            (
                id: (int)row["LocalDrivingLicenseApplicationID"],
                mainApplicationId: (int)row["ApplicationID"],
                licenseClassId: (int)row["LicenseClassID"]
            );

            ldla.MainApplicationInfo = ApplicationBusiness.Find(ldla.MainApplicationId);
            ldla.LicenseClassInfo = LicenseClassBusiness.Find(ldla.LicenseClassId);

            return ldla;
        }

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

        public static bool DeleteLocalApplicationByMainId(int mainApplicationId)
        {
            return LocalDrivingLicenseApplicationData.DeleteByMainApplicationId(mainApplicationId);
        }
    }
}
