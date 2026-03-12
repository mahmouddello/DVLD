using System;
using System.Data;
using DVLD.DataAccessLayer;
using DVLD.EntityLayer;

namespace DVLD.BusinessLayer
{
    public class TestService
    {
        public Test Info { get; private set; }

        public TestService(Test test)
        {
            Info = test ?? throw new ArgumentNullException(nameof(test));
        }

        // ── Static Lookups ────────────────────────────────

        public static DataTable GetAllTestsAsTable() => TestData.GetAllAsTable();

        public static Test FindById(int testId)
        {
            if (testId <= 0) 
                return null;

            var test = TestData.GetById(testId);

            if (test == null) 
                return null;

            ResolveNavigationProperties(test);
            return test;
        }

        public static Test FindByAppointmentId(int testAppId)
        {
            if (testAppId <= 0) 
                return null;

            var test = TestData.GetTestByAppointmentId(testAppId);

            if (test == null)
                return null;

            ResolveNavigationProperties(test);
            return test;
        }

        public static int GetTrialsCount(int _ldlaId, int testTypeId) => TestData.GetTestTrialsCount(_ldlaId, testTypeId);

        public static bool HasPassedTest(int _ldlaId, int testTypeId) => TestData.HasTestRecord(_ldlaId, testTypeId, true);

        public static bool HasFailedTest(int _ldlaId, int testTypeId) => TestData.HasTestRecord(_ldlaId, testTypeId, false);

        private static void ResolveNavigationProperties(Test test)
        {
            test.CreatorUserInfo = UserService.FindById(test.CreatedByUserId);
        }

        // ── Instance Methods ──────────────────────────────

        public bool Save()
        {
            if (Info.TestAppointmentId <= 0) return false;
            if (Info.CreatedByUserId <= 0) return false;

            if (Info.IsNew)
            {
                Info.Id = TestData.Add(Info);
                return !Info.IsNew;
            }

            return false; // Test doesn't get updated after getting recorded
        }
    }
}
