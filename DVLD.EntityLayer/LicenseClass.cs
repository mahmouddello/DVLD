using System;

namespace DVLD.EntityLayer
{
    public enum enLicenseClass
    {
        C1_SmallMotorcycle = 1,
        C2_HeavyMotorcycle = 2,
        C3_Ordinary = 3,
        C4_Commercial = 4,
        C5_Agricultural = 5,
        C6_SmallAndMediumBus = 6,
        C7_TruckAndHeavyVehicle = 7,
    }

    public class LicenseClass
    {
        public int Id { get; set; } = -1;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int MinimumAllowedAge { get; set; } = -1;
        public int DefaultValidityLength { get; set; } = -1;
        public decimal Fees { get; set; } = decimal.Zero;

        public LicenseClass() { }

        public LicenseClass(
            int id,
            string name,
            string description,
            int minimumAllowedAge,
            int defaultValidityLength,
            decimal fees)
        {
            Id = id;
            Name = name?.Trim() ?? string.Empty;
            Description = description?.Trim() ?? string.Empty;
            MinimumAllowedAge = minimumAllowedAge;
            DefaultValidityLength = defaultValidityLength;
            Fees = fees;
        }
    }
}