using DVLD.DataAccessLayer;
using DVLD.EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.BusinessLayer
{
    public static class clsLicenseBusiness
    {
        public static List<clsLicense> GetAllLicenses()
        {
            var licenses = clsLicenseData.GetAllLicenses();

            if (licenses.Count == 0)
                throw new Exception("There are no license records in the system.");

            return licenses;
        }

        public static int GetActiveLicenseForDriver(int driverId)
        {
            return clsLicenseData.GetActiveLicenseCountByDriverId(driverId);
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
