using System;
using System.Data;
using DVLD.DataAccessLayer;
using DVLD.EntityLayer;

namespace DVLD.BusinessLayer
{
    public class DriverService
    {
        public Driver Info { get; private set; }

        public DriverService(Driver driver)
        {
            Info = driver ?? throw new ArgumentNullException(nameof(Driver));
        }

        // ── Static Lookups ────────────────────────────────
        public static DataTable GetAllAsTable() => DriverData.GetAllAsTable();

        public static Driver FindById(int driverId)
        {
            var driver = DriverData.GetById(driverId);

            if (driver != null)
                ResolveNavigationProperties(driver);

            return driver;
        }

        public static Driver FindByPersonId(int personId)
        {
            var driver = DriverData.GetByPersonID(personId);

            if (driver != null)
                ResolveNavigationProperties(driver);

            return driver;
        }

        private static void ResolveNavigationProperties(Driver driver)
        {
            driver.PersonInfo = PersonService.FindById(driver.PersonId);
            driver.CreatorUserInfo = UserService.FindById(driver.CreatedByUserId);
        }

        public static bool Delete(int driverId) => DriverData.DeleteById(driverId);

        // ── Instance Methods ──────────────────────────────

        public bool Save()
        {
            if (Info.PersonId == -1) return false;
            if (Info.CreatedByUserId == -1) return false;

            if (Info.IsNew)
            {
                Info.Id = DriverData.Add(Info);

                return !Info.IsNew;
            }

            return false; // driver information can't be updated
        }
    }
}
