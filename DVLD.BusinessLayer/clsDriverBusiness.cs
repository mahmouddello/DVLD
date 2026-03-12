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

            clsDriver _driver = clsDriverData.GetById(driverId);

            if (_driver != null)
                _driver.PersonInfo = PersonService.FindById(_driver.PersonId);

            return _driver;
        }

        public static clsDriver FindByPersonId(int personId)
        {
            if (personId <= 0)
                throw new Exception("Person id is invalid!");

            clsDriver _driver = clsDriverData.GetByPersonID(personId);

            if (_driver != null)
                _driver.PersonInfo = PersonService.FindById(_driver.PersonId);

            return _driver;
        }

        public static bool Delete(int driverId)
        {
            return clsDriverData.DeleteById(driverId);
        }

        public static bool Save(clsDriver _driver)
        {
            if (_driver.Id == -1)
                return Add(_driver);

            return false;
        }

        private static bool Add(clsDriver _driver)
        {
            _driver.Id = clsDriverData.InsertNew(_driver.PersonId, _driver.CreatedByUserId, _driver.CreatedAt);

            return _driver.Id != -1;
        }
    }
}
