using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.EntityLayer
{
    public enum enLicenseIssueReason
    {
        None = 0,
        FirstTime = 1,
        Renew = 2,
        ReplacementForDamaged = 3,
        ReplacementForLost = 4
    }

    public class clsLicense
    {
        public int Id { get; set; }
        public int ApplicationId { get; private set; }
        public int DriverId { get; private set; }
        public int LicenseClassID { get; private set; }
        public DateTime IssueDate { get; private set; }
        public DateTime ExpirationDate { get; private set; }
        public string Notes { get; private set; }
        public decimal PaidFees { get; private set; }
        public bool IsActive { get; private set; }
        public enLicenseIssueReason IssueReason { get; private set; }
        public int CreatedByUserId { get; private set; }

        public clsLicense()
        {
            this.Id = 0;
            this.ApplicationId = 0;
            this.DriverId = 0;
            this.LicenseClassID = 0;
            this.IssueDate = DateTime.Now;
            this.ExpirationDate = DateTime.Now;
            this.Notes = string.Empty;
            this.PaidFees = decimal.Zero;
            this.IsActive = false;
            this.IssueReason = enLicenseIssueReason.None;
            this.CreatedByUserId = 0;
        }

        public clsLicense
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
            this.Id = id;
            this.ApplicationId = applicationId;
            this.DriverId = driverId;
            this.LicenseClassID = licenseClassID;
            this.IssueDate = issueDate;
            this.ExpirationDate = expirationDate;
            this.Notes = notes;
            this.PaidFees = paidFees;
            this.IsActive = isActive;
            this.IssueReason = issueReason;
            this.CreatedByUserId = createdByUserId;
        }
    }
}
