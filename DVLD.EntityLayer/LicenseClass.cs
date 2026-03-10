using System;

namespace DVLD.EntityLayer
{
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