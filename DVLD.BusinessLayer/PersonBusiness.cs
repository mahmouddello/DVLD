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
            return PersonData.GetAllPeople();
        }

        public static Person Find(int personID)
        {
            DataRow row = PersonData.GetById(personID);

            if (row == null)
                return null;

            return new Person
            (
                id: (int)row["PersonID"],
                nationalNo: (string)row["NationalNo"],
                firstName: (string)row["FirstName"],
                secondName: (string)row["SecondName"],
                thirdName: row["ThirdName"] != DBNull.Value ? (string)row["ThirdName"] : string.Empty,
                lastName: (string)row["LastName"],
                dateOfBirth: (DateTime)row["DateOfBirth"],
                gender: (enGender)(byte)row["Gender"],
                address: (string)row["Address"],
                phone: (string)row["Phone"],
                email: row["Email"] != DBNull.Value ? (string)row["Email"] : string.Empty,
                imagePath: row["ImagePath"] != DBNull.Value ? (string)row["ImagePath"] : string.Empty,
                nationality: new Country
                (
                    countryId: (int)row["CountryID"], 
                    countryName: (string)row["Nationality"]
                )
            );
        }

        public static Person Find(string nationalNo)
        {
            DataRow row = PersonData.GetByNationalNo(nationalNo);

            if (row == null)
                return null;

            return new Person
            (
                id: (int)row["PersonID"],
                nationalNo: (string)row["NationalNo"],
                firstName: (string)row["FirstName"],
                secondName: (string)row["SecondName"],
                thirdName: row["ThirdName"] != DBNull.Value ? (string)row["ThirdName"] : string.Empty,
                lastName: (string)row["LastName"],
                dateOfBirth: (DateTime)row["DateOfBirth"],
                gender: (enGender)(byte)row["Gender"],
                address: (string)row["Address"],
                phone: (string)row["Phone"],
                email: row["Email"] != DBNull.Value ? (string)row["Email"] : string.Empty,
                imagePath: row["ImagePath"] != DBNull.Value ? (string)row["ImagePath"] : string.Empty,
                nationality: new Country
                (
                    countryId: (int)row["CountryID"], 
                    countryName: (string)row["Nationality"]
                )
            );
        }

        public static bool Delete(int personID)
        {
            return PersonData.DeleteById(personID);
        }

        private static bool Add(Person person)
        {
            // Add new
            person.ID = PersonData.InsertNew(
                person.NationalNo, person.FirstName, person.SecondName,
                person.ThirdName, person.LastName, person.DateOfBirth,
                (byte)person.Gender, person.Address, person.Phone,
                person.Email, person.Nationality.ID, person.ImagePath
            );

            return person.ID != -1;
        }

        private static bool Update(Person person)
        {
            return PersonData.UpdateById(person.ID, person.NationalNo, person.FirstName, person.SecondName,
                    person.ThirdName, person.LastName, person.DateOfBirth,
                    (byte)person.Gender, person.Address, person.Phone,
                    person.Email, person.Nationality.ID, person.ImagePath);
        }

        public static bool Save(Person person)
        {
            if (person.ID == -1)
                return Add(person);

            return Update(person);
        }

        public static bool Exists(string nationalNo)
        {
            return PersonData.ExistsByNationalNo(nationalNo);
        }

        public static bool Exists(int personID)
        {
            return PersonData.ExistsById(personID);
        }
    }
}
