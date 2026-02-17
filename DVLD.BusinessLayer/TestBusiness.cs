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
    }
}
