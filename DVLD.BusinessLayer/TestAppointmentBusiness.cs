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
    public static class TestAppointmentBusiness
    {
        public static DataTable GetAll()
        {
            return TestAppointmentData.GetAllTestAppointments();
        }

        public static TestAppointment Find(int testAppointmentId)
        {
            return TestAppointmentData.GetById(testAppointmentId);
        }

        public static bool CanScheduleTest(int ldlaId, int testTypeId)
        {
            if (TestAppointmentData.ExistsActiveAppointmentByTestType(ldlaId, testTypeId))
                return false;

            if (TestBusiness.HasPassedTest(ldlaId, testTypeId))
                return false;

            return true;
        }
    }
}
