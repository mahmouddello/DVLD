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
            DataRow row = PersonDataAccess.GetPersonByID(personID);

            if (row == null)
                return null;

            return new Person
            {
                ID = (int)row["PersonID"],
                FirstName = (string)row["FirstName"],
                SecondName = (string)row["SecondName"],
                ThirdName = row["ThirdName"] != DBNull.Value ? (string)row["ThirdName"] : string.Empty,
                LastName = (string)row["LastName"],
                DateOfBirth = (DateTime)row["DateOfBirth"],
                Gender = (enGender)(byte)row["Gender"],
                Address = (string)row["Address"],
                Phone = (string)row["Phone"],
                Email = row["Email"] != DBNull.Value ? (string)row["Email"] : string.Empty,
                ImagePath = row["ImagePath"] != DBNull.Value ? (string)row["ImagePath"] : string.Empty,
                Nationality = new Country{
                    ID = (int)row["CountryID"],
                    Name = (string)row["Nationality"]
                }
            };
        }
    }
}
