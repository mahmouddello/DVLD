using DVLD.DataAccessLayer;
using DVLD.EntityLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.BusinessLayer
{
    public static class TestTypeBusiness
    {
        public static DataTable GetAll()
        {
            return TestTypeData.GetAllTestTypes();
        }

        public static TestType Find(int testTypeId)
        {
            DataRow row = TestTypeData.GetById(testTypeId);

            if (row == null)
                return null;

            return new TestType(
                id: (int)row["TestTypeID"],
                title: (string)row["TestTypeTitle"],
                description: (string)row["TestTypeDescription"],
                fees: (decimal)row["TestTypeFees"]);
        }

        public static bool Save(TestType testType)
        {
            return TestTypeData.UpdateById(testType.Id, testType.Title, testType.Description, testType.Fees);
        }
    }
}
