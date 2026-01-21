using System;
using System.Collections.Generic;
using System.Data;
using DVLD.DataAccessLayer;
using DVLD.EntityLayer;

namespace DVLD.BusinessLayer
{
    public class CountryBusiness
    {
        public static DataTable GetCountries()
        {
            return CountryData.GetAllCountries();
        }

        public static Country Find(int countryID)
        {
            DataRow row = CountryData.GetById(countryID);

            if (row == null)
                return null;

            return new Country(countryId: (int)row["CountryID"], countryName: (string)row["CountryName"]);
        }

        public static Country Find(string countryName)
        {
            DataRow row = CountryData.GetByName(countryName);

            if (row == null)
                return null;

            return new Country(countryId: (int)row["CountryID"], countryName: (string)row["CountryName"]);
        }

    }
}
