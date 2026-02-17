using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.EntityLayer
{
    public class TestAppointment
    {
        public int Id { get; set; }
        public int TestTypeId { get; set; }
        public int LocalDrivingLicenseApplicationId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public decimal PaidFees { get; set; }
        public int CreatedByUserId { get; set; }
        public bool IsLocked { get; set; }
        public int? RetakeTestApplicationId { get; set; }

        public TestAppointment()
        {

        }

        public TestAppointment(int id, int testTypeId, int localDrivingApplicationLicenseId, DateTime appointmentDate, decimal paidFees, int createdByUserId, bool isLocked, int? retakeTestApplicationId)
        {
            this.Id = id;
            this.TestTypeId = testTypeId;
            this.LocalDrivingLicenseApplicationId = localDrivingApplicationLicenseId;
            this.AppointmentDate = appointmentDate;
            this.PaidFees = paidFees;
            this.CreatedByUserId = createdByUserId;
            this.IsLocked = isLocked;
            this.RetakeTestApplicationId = retakeTestApplicationId;
        }
    }
}
