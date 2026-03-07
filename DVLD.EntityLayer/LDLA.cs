using System;

namespace DVLD.EntityLayer
{
    public class LDLA
    {
        public int Id { get; set; } = -1;
        public int MainApplicationId { get; set; } = -1;
        public int LicenseClassId { get; set; } = -1;

        public bool IsNew => Id == -1;

        // Composition
        public Application MainApplicationInfo { get; set; } = null;
        public LicenseClass LicenseClassInfo { get; set; } = null;

        public LDLA()
        {

        }

        public LDLA(int id, int mainApplicationId, int licenseClassId)
        {
            Id = id;
            MainApplicationId = mainApplicationId;
            LicenseClassId = licenseClassId;
        }
    }
}
