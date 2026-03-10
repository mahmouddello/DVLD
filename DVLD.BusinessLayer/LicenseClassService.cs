using System;
using System.Data;
using DVLD.DataAccessLayer;
using DVLD.EntityLayer;

namespace DVLD.BusinessLayer
{
    public static class LicenseClassService
    {
        public static DataTable GetAll() => LicenseClassData.GetAllAsTable();

        public static LicenseClass FindById(int licenseClassId)
        {
            if (licenseClassId < 1)
                throw new ArgumentOutOfRangeException(nameof(licenseClassId), "License class ID must be greater than 0.");

            return LicenseClassData.GetById(licenseClassId);
        }
    }
}
