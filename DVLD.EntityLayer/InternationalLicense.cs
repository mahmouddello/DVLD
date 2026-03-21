using System;

namespace DVLD.EntityLayer
{
    public class InternationalLicense
    {
        public int Id { get; set; } = -1;
        public int ApplicationId { get; set; } = -1;
        public int DriverId { get; set; } = -1;
        public int LocalLicenseId { get; set; } = -1;
        public DateTime IssueDate { get; set; } = DateTime.Now;
        public DateTime ExpirationDate { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = false;
        public int CreatedByUserId { get; set; } = -1;


        // Navigation (Most Important)
        public Application ApplicationInfo { get; set; } = null;
        public License LocalLicense { get; set; } = null;

        // Helpers
        public bool IsExpired => DateTime.Now > ExpirationDate;

        public InternationalLicense()
        {

        }

        public InternationalLicense(
            int internationalLicenseId,
            int applicationId, 
            int driverId,
            int localLicenseId,
            DateTime issueDate,
            DateTime expirationDate,
            bool isActive,
            int createdByUserId
        )
        {
            Id = internationalLicenseId;
            ApplicationId = applicationId;
            DriverId = driverId;
            LocalLicenseId = localLicenseId;
            IssueDate = issueDate;
            ExpirationDate = expirationDate;
            IsActive = isActive;
            CreatedByUserId = createdByUserId;
        }
    }
}
