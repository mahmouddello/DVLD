using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public LicenseClass(int id, string name, string description, int minimumAllowedAge, int defaultValidityLength, decimal fees)
        {
            Id = id;
            Name = name;
            Description = description;
            MinimumAllowedAge = minimumAllowedAge;
            DefaultValidityLength = defaultValidityLength;
            Fees = fees;
        }
    }
}
