using System;
using System.Data;
using DVLD.DataAccessLayer;
using DVLD.EntityLayer;

namespace DVLD.BusinessLayer
{
    public class TestTypeService
    {
        public TestType Info { get; private set; }

        public TestTypeService(TestType testType)
        {
            Info = testType ?? throw new ArgumentNullException(nameof(testType));
        }

        public static DataTable GetAllTestTypes() => TestTypeData.GetAllAsTable();

        public static TestType FindById(int testTypeId)
        {
            if (testTypeId <= 0 || testTypeId > 3)
                return null;

            return TestTypeData.GetById(testTypeId);
        }

        public bool Save()
        {
            if (string.IsNullOrWhiteSpace(Info.Title)) return false;
            if (string.IsNullOrWhiteSpace(Info.Description)) return false;
            if (Info.Fees < 0) return false;

            return TestTypeData.Update(Info);
        }
    }
}