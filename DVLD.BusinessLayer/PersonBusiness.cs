using System;
using System.Data;
using DVLD.DataAccessLayer;

namespace DVLD.BusinessLayer
{
    public class PersonBusiness
    {
        public static DataTable GetAllPeople()
        {
            return PersonDataAccess.GetAllPeople();
        }
    }
}
