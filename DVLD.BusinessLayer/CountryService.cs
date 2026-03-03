using System;
using System.Collections.Generic;
using DVLD.DataAccessLayer;
using DVLD.EntityLayer;

namespace DVLD.BusinessLayer
{
    public class CountryService
    {
        public static List<Country> GetAllCountries() => CountryData.GetAllCountries();

        public static Country GetById(int id) => CountryData.GetById(id);

        public static Country GetByName(string name) => CountryData.GetByName(name);

        public static bool ExistsById(int id) => CountryData.ExistsById(id);

        public static bool ExistsByName(string name) => CountryData.ExistsByName(name);
    }
}
