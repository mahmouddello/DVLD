using System;

namespace DVLD.EntityLayer
{
    public enum enApplicationStatus : byte
    {
        None = 0,
        New = 1,
        Cancelled = 2,
        Completed = 3
    }

    public class Application
    {
        public int Id { get; set; } = -1;
        public int ApplicantPersonId { get; set; } = -1;
        public int ApplicationTypeId { get; set; } = -1;
        public int CreatedByUserId { get; set; } = -1;
        public DateTime ApplicationDate { get; set; } = DateTime.MinValue;
        public enApplicationStatus Status { get; set; } = enApplicationStatus.None;
        public DateTime LastStatusDate { get; set; } = DateTime.MinValue;
        public decimal PaidFees { get; set; } = decimal.Zero;

        // navigation properties — optional, loaded by BLL when needed
        public Person ApplicantPersonInfo { get; set; }
        public ApplicationType ApplicationTypeInfo { get; set; }
        public User CreatorUserInfo { get; set; }

        // useful domain properties
        public bool IsNew => Id == -1;
        public bool IsCompleted => Status == enApplicationStatus.Completed;
        public bool IsCancelled => Status == enApplicationStatus.Cancelled;

        public Application() { }

        public Application(
            int id,
            int applicantPersonId,
            int applicationTypeId,
            int createdByUserId,
            DateTime applicationDate,
            enApplicationStatus status,
            DateTime lastStatusDate,
            decimal paidFees)
        {
            Id = id;
            ApplicantPersonId = applicantPersonId;
            ApplicationTypeId = applicationTypeId;
            CreatedByUserId = createdByUserId;
            ApplicationDate = applicationDate;
            Status = status;
            LastStatusDate = lastStatusDate;
            PaidFees = paidFees;
        }
    }
}
