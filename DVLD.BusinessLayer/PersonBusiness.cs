using System;
using System.Data;
using DVLD.DataAccessLayer;
using DVLD.EntityLayer;

namespace DVLD.BusinessLayer
{
    public class PersonBusiness
    {
        public static DataTable GetPeople()
        {
            return PersonDataAccess.GetAllPeople();
        }

        public static Person Find(int personID)
        {
            return PersonDataAccess.GetPersonByID(personID);
        }
    }
}
