using System;
using System.Data;
using DVLD.DataAccessLayer;
using DVLD.EntityLayer;

namespace DVLD.BusinessLayer
{
    public class PersonBusiness
    {
        public static DataTable GetAllPeople()
        {
            return PersonDataAccess.GetAllPeople();
        }

        public static Person GetPersonByID(int id)
        {
            return PersonDataAccess.GetPersonByID(id);
        }
    }
}
