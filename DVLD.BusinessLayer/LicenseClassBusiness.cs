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
    public static class LicenseClassBusiness
    {
        public static DataTable GetAll()
        {
            return LicenseClassData.GetAllLicenseClasses();
        }

        public static LicenseClass Find(int licenseClassId)
        {
            DataRow row = LicenseClassData.GetById(licenseClassId);

            if (row == null)
                return null;

            LicenseClass licenseClass = new LicenseClass
            (
                id: (int)row["LicenseClassID"],
                name: (string)row["LicenseClassName"],
                description: (string)row["ClassDescription"],
                minimumAllowedAge: (int)row["MinimumAllowedAge"],
                defaultValidityLength: (int)row["DefaultValidityLength"],
                fees: (decimal)row["ClassFees"]
            );

            return licenseClass;
        }
    }
}
