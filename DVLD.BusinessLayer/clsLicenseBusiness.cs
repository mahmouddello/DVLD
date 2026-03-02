using DVLD.DataAccessLayer;
using DVLD.EntityLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD.BusinessLayer
{
    public static class clsLicenseBusiness
    {
        public static DataTable GetDriverLicenseForDriver(int driverId)
        {
            var licenses = clsLicenseData.GetAllLicenses(driverId);

            if (licenses.Rows.Count == 0)
                throw new Exception("There are no license records in the system.");

            return licenses;
        }

        public static int GetActiveLicenseForDriver(int driverId)
        {
            return clsLicenseData.GetActiveLicenseCountByDriverId(driverId);
        }

        public static clsLicense FindByLicenseId(int licenseId)
        {
            if (licenseId <= 0)
                throw new Exception("Invalid license id!");
            
            return clsLicenseData.GetById(licenseId);
        }

        public static clsLicense FindByApplicationId(int applicationId)
        {
            if (applicationId <= 0)
                throw new Exception("Invalid application id!");

            return clsLicenseData.GetByApplicationId(applicationId);
        }

        public static bool Save(clsLicense license)
        {
            if (license.Id <= 0)
                return Add(license);

            return false;
        }

        private static bool Add(clsLicense license)
        {
            license.Id = clsLicenseData.InsertNew(license);
            return license.Id > 0;
        }
    }
}
