using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.EntityLayer
{
    public class LocalDrivingLicenseApplication
    {
        public int Id { get; set; } = -1;

        // Foreign Keys
        public int MainApplicationId { get; set; } = -1;
        public int LicenseClassId { get; set; } = -1;

        // Composition
        public Application MainApplicationInfo { get; set; } = null;
        public LicenseClass LicenseClassInfo { get; set; } = null;

        public LocalDrivingLicenseApplication()
        {

        }

        public LocalDrivingLicenseApplication(int id, int mainApplicationId, int licenseClassId)
        {
            Id = id;
            MainApplicationId = mainApplicationId;
            LicenseClassId = licenseClassId;
        }
    }
}
