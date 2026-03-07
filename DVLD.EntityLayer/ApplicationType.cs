using System;

namespace DVLD.EntityLayer
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

    public class ApplicationType
    {
        public enApplicationType Type { get; set; } = enApplicationType.None;
        public string Title { get; set; } = string.Empty;
        public decimal Fees { get; set; } = 0;

        public ApplicationType(enApplicationType type, string title, decimal fees)
        {
            Type = type;
            Title = title;
            Fees = fees;
        }
    }
}