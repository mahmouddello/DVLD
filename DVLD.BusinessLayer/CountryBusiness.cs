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
            return CountryDataAccess.GetAllCountries();
        }

        public static Country Find(int countryID)
        {
            return CountryDataAccess.GetCountryByID(countryID);
        }
    }
}
