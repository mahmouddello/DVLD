using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.EntityLayer
{
    public class ApplicationType
    {
        public enum enApplicationType
        {
            None = 0,
            NewLocalDrivingLicense = 1,
            RenewDrivingLicense = 2,
            ReplaceLostDrivingLicense = 3,
            ReplaceDamagedDrivingLicense = 4,
            ReleaseDetainedLicense = 5,
            NewInternationalLicense = 6,
            RetakeTest = 7
        }

        public enApplicationType Id { get; } = enApplicationType.None;
        public string Title { get; set; } = string.Empty;
        public decimal Fees { get; set; } = 0;

        public ApplicationType(enApplicationType applicationTypeId, string title, decimal fees)
        {
            this.Id = applicationTypeId;
            this.Title = title;
            this.Fees = fees;
        }
    }
}
