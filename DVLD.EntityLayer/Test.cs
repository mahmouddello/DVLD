using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.EntityLayer
{
    public enum TestResult { Failed, Passed }

    public class Test
    {

        public int Id { get; set; }
        public int TestAppointmentId { get; set; }
        public TestResult Result { get; set; }
        public string Notes { get; set; }
        public int CreatedByUserId { get; set; }

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
