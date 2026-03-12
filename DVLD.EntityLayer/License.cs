using System;

namespace DVLD.EntityLayer
{
    public enum enLicenseIssueReason
    {
        FirstTime = 1,
        Renew = 2,
        ReplacementForDamaged = 3,
        ReplacementForLost = 4
    }

    public class License
    {
        public int Id { get; set; } = -1;
        public int ApplicationId { get; private set; } = -1;
        public int DriverId { get; private set; } = -1;
        public int LicenseClassID { get; private set; } = -1;
        public DateTime IssueDate { get; private set; } = DateTime.Now;
        public DateTime ExpirationDate { get; private set; } = DateTime.Now;
        public string Notes { get; private set; } = string.Empty;
        public decimal PaidFees { get; private set; } = decimal.Zero;
        public bool IsActive { get; private set; } = false;
        public enLicenseIssueReason IssueReason { get; private set; } = enLicenseIssueReason.FirstTime;
        public int CreatedByUserId { get; private set; } = -1;

        public bool IsNew => Id == -1;

        // Navigation
        public Application MainApplicationInfo { get; set; } = null;
        public Driver DriverInfo { get; set; } = null;
        public LicenseClass LicenseClassInfo { get; set; } = null;
        public User CreatorUserInfo { get; set; } = null;

        public License()
        {

        }

        public License
        (
            int id,
            int applicationId,
            int driverId,
            int licenseClassID,
            DateTime issueDate,
            DateTime expirationDate,
            string notes,
            decimal paidFees,
            bool isActive,
            enLicenseIssueReason issueReason,
            int createdByUserId
        )
        {
            Id = id;
            ApplicationId = applicationId;
            DriverId = driverId;
            LicenseClassID = licenseClassID;
            IssueDate = issueDate;
            ExpirationDate = expirationDate;
            Notes = notes?.Trim() ?? string.Empty;
            PaidFees = paidFees;
            IsActive = isActive;
            IssueReason = issueReason;
            CreatedByUserId = createdByUserId;
        }
    }
}
