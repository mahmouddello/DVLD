using System;
using System.Data;
using DVLD.DataAccessLayer;
using DVLD.EntityLayer;

namespace DVLD.BusinessLayer
{
    public class TestAppointmentService
    {
        public TestAppointment Info { get; private set; }

        public TestAppointmentService(TestAppointment testAppointment)
        {
            Info = testAppointment ?? throw new ArgumentNullException(nameof(testAppointment));
        }

        // ── Static Lookups ────────────────────────────────
        public static DataTable GetAllAsTable(int ldlaId, int testTypeId) => TestAppointmentData.GetAllAsTable(ldlaId, testTypeId);

        public static TestAppointment FindById(int testAppointmentId)
        {
            var testAppointment = TestAppointmentData.GetById(testAppointmentId);

            if (testAppointment == null) return null;

            ResolveNavigationProperties(testAppointment);
            return testAppointment;
        }

        private static void ResolveNavigationProperties(TestAppointment testAppointment)
        {
            testAppointment.TestTypeInfo = TestTypeService.FindById(testAppointment.TestTypeId);
            testAppointment.LdlaInfo = LDLAService.FindById(testAppointment.LdlaId);
            testAppointment.CreatedByUserInfo = UserService.FindById(testAppointment.CreatedByUserId);
        }

        public static bool CanSchedule(int ldlaId, int testTypeId)
        {
            if (TestAppointmentData.ExistsPendingAppointment(ldlaId, testTypeId))
                return false;

            if (TestService.HasPassedTest(ldlaId, testTypeId))
                return false;

            return true;
        }

        // ── Instance Methods ──────────────────────────────
        public bool Save()
        {
            if (Info.TestTypeId == -1) return false;
            if (Info.LdlaId == -1) return false;
            if (Info.AppointmentDate == DateTime.MinValue) return false;
            if (Info.AppointmentDate < DateTime.Today) return false;
            if (Info.CreatedByUserId == -1) return false;

            if (Info.IsNew)
            {
                Info.Id = TestAppointmentData.Add(Info);
                return !Info.IsNew;
            }

            return TestAppointmentData.UpdateAppointmentDate(Info.Id, Info.AppointmentDate);
        }

        public bool Lock()
        {
            if (Info.IsLocked) return false;

            bool updated = TestAppointmentData.UpdateLockStatus(Info.Id, true);

            if (updated)
                Info.IsLocked = true;

            return updated;
        }

        public bool Unlock()
        {
            if (!Info.IsLocked) return false;

            bool updated = TestAppointmentData.UpdateLockStatus(Info.Id, false);

            if (updated)
                Info.IsLocked = false;

            return updated;
        }
    }
}
