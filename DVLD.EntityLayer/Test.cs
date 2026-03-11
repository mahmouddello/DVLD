using System;

namespace DVLD.EntityLayer
{
    public enum TestResult { Failed, Passed }

    public class Test
    {
        public int Id { get; set; } = -1;
        public int TestAppointmentId { get; set; } = -1;
        public TestResult Result { get; set; } = TestResult.Failed;
        public string Notes { get; set; } = string.Empty;
        public int CreatedByUserId { get; set; } = -1;

        public bool IsNew => Id == -1;

        // Navigation
        public User CreatorUserInfo { get; set; } = null;

        public Test()
        {

        }

        public Test(int id, int testAppointmentId, TestResult testResult, string notes, int createdByUserId)
        {
            Id = id;
            TestAppointmentId = testAppointmentId;
            Result = testResult;
            Notes = notes;
            CreatedByUserId = createdByUserId;
        }
    }
}
