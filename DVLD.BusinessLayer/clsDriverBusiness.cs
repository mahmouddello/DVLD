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
    public static class clsDriverBusiness
    {
        public static DataTable GetAllDrivers()
        {
            var drivers = clsDriverData.GetAllDrivers();

            // Simple business rule
            if (drivers.Rows.Count == 0)
                throw new Exception("No drivers found in the system.");

            return drivers;
        }

        public static clsDriver Find(int driverId)
        {
            if (driverId <= 0)
                throw new Exception("Invalid Driver Id");

            clsDriver driver = clsDriverData.GetById(driverId);

            if (driver != null)
                driver.PersonInfo = PersonBusiness.Find(driver.PersonId);

            return driver;
        }

        public static clsDriver FindByPersonId(int personId)
        {
            if (personId <= 0)
                throw new Exception("Person id is invalid!");

            clsDriver driver = clsDriverData.GetByPersonID(personId);

            if (driver != null)
                driver.PersonInfo = PersonBusiness.Find(driver.PersonId);

            return driver;
        }

        public static bool Delete(int driverId)
        {
            return clsDriverData.DeleteById(driverId);
        }

        public static bool Save(clsDriver driver)
        {
            if (driver.Id == -1)
                return Add(driver);

            return false;
        }

        private static bool Add(clsDriver driver)
        {
            driver.Id = clsDriverData.InsertNew(driver.PersonId, driver.CreatedByUserId, driver.CreatedAt);

            return driver.Id != -1;
        }
    }
}
