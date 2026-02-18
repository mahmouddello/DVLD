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
        public static DataTable GetAll(int ldlaId, int testTypeId)
        {
            return TestAppointmentData.GetAllTestAppointments(ldlaId, testTypeId);
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

        public static bool Save(TestAppointment testAppointment)
        {
            if (testAppointment.Id == -1)
                return Add(testAppointment);

            return Update(testAppointment);
        }

        private static bool Update(TestAppointment testAppointment)
        {
            return TestAppointmentData.UpdateById(testAppointment.Id, testAppointment.AppointmentDate);
        }

        private static bool Add(TestAppointment testAppointment)
        {
            testAppointment.Id = TestAppointmentData.InsertNew(testAppointment);

            return testAppointment.Id != -1;
        }
    }
}
