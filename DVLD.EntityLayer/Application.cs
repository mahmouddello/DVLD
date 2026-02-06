using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.EntityLayer
{
    public class Application
    {
        public enum ApplicationStatus : int { None = 0, New = 1, Cancelled = 2, Completed = 3 }

        public int Id { get; set; } = -1;
        public int ApplicantPersonId { get; set; } = -1;
        public DateTime Date { get; set; } = DateTime.MinValue;
        public int ApplicationTypeId { get; set; } = -1;
        public ApplicationStatus Status { get; set; } = ApplicationStatus.None;
        public DateTime LastStatusDate { get; set; } = DateTime.MinValue;
        public decimal PaidFees { get; set; } = decimal.Zero;
        public int CreatedByUserId { get; set; } = -1;

        // Composition
        public Person ApplicantPersonInfo { get; set; } = null;
        public ApplicationType ApplicationTypeInfo { get; set; } = null;
        public User CreatorUserInfo { get; set; } = null;

        public Application()
        {

        }

        public Application(int id, int applicantPersonId, DateTime date, int applicationTypeId, ApplicationStatus applicationstatus, DateTime lastApplicationStatusDate, decimal paidFees, int createdByUserId)
        {
            Id = id;
            ApplicantPersonId = applicantPersonId;
            Date = date;
            ApplicationTypeId = applicationTypeId;
            Status = applicationstatus;
            LastStatusDate = lastApplicationStatusDate;
            PaidFees = paidFees;
            CreatedByUserId = createdByUserId;
        }
    }
}
