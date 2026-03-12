using System;

namespace DVLD.EntityLayer
{
    public class TestAppointment
    {
        public int Id { get; set; } = -1;
        public int TestTypeId { get; set; } = -1;
        public int LdlaId { get; set; } = -1;
        public DateTime AppointmentDate { get; set; } = DateTime.MinValue;
        public decimal PaidFees { get; set; } = decimal.Zero;
        public int CreatedByUserId { get; set; } = -1;
        public bool IsLocked { get; set; } = false;
        public int RetakeTestApplicationId { get; set; } = -1;

        // Computed helpers
        public bool IsNew => Id == -1;
        public bool HasRetakeApplication => RetakeTestApplicationId != -1;

        // Navigation properties — loaded by BLL when needed
        public TestType TestTypeInfo { get; set; } = null;
        public LDLA LdlaInfo { get; set; } = null;
        public User CreatedByUserInfo { get; set; } = null;

        public TestAppointment() { }

        public TestAppointment(
            int id,
            int testTypeId,
            int _ldlaId,
            DateTime appointmentDate,
            decimal paidFees,
            int createdByUserId,
            bool isLocked,
            int retakeTestApplicationId)
        {
            Id = id;
            TestTypeId = testTypeId;
            LdlaId = _ldlaId;
            AppointmentDate = appointmentDate;
            PaidFees = paidFees;
            CreatedByUserId = createdByUserId;
            IsLocked = isLocked;
            RetakeTestApplicationId = retakeTestApplicationId;
        }
    }
}
