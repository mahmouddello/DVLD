using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD.DataAccessLayer;
using DVLD.EntityLayer;

namespace DVLD.BusinessLayer
{
    public static class TestBusiness
    {
        public static DataTable GetAll()
        {
            return TestData.GetAllTests();
        }

        public static Test Find(int testId)
        {
            return TestData.GetById(testId);
        }

        public static bool HasPassedTest(int ldlaId, int testTypeId)
        {
            return TestData.HasTestPassedRecord(ldlaId, testTypeId);
        }

        public static bool HasFailedTest(int ldlaId, int testTypeId)
        {
            return TestData.HasTestFailedRecord(ldlaId, testTypeId);
        }

        public static int GetTrialsCount(int ldlaId, int testTypeId)
        {
            return TestData.GetTestTrialsCount(ldlaId, testTypeId);
        }

        public static bool Save(Test test)
        {
            if (test.Id == -1)
                return Add(test);

            return false;
        }

        private static bool Add(Test test)
        {
            test.Id = TestData.InsertNew(test);

            return test.Id != -1;
        }

        public static int GetAssociatedTestId(int testAppointmentId)
        {
            return TestData.GetTestIdByAppointmentId(testAppointmentId);
        }
    }
}
