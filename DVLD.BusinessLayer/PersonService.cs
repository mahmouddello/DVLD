using DVLD.DataAccessLayer;
using DVLD.EntityLayer;
using System;
using System.Data;

namespace DVLD.BusinessLayer
{
    public class PersonService
    {
        public Person Info { get; private set; }

        public PersonService(Person person)
        {
            Info = person;
        }

        public static DataTable GetAllAsTable() => PersonData.GetAllAsTable();
        public static Person GetById(int id) => PersonData.GetById(id);
        public static Person GetByNationalNo(string nationalNo) => PersonData.GetByNationalNo(nationalNo);
        public static bool ExistsById(int id) => PersonData.ExistsById(id);
        public static bool ExistsByNationalNo(string nationalNo) => PersonData.ExistsByNationalNo(nationalNo);
        public static bool Delete(int id) => PersonData.Delete(id);

        public bool Save()
        {
            if (string.IsNullOrWhiteSpace(Info.FirstName)) return false;
            if (string.IsNullOrWhiteSpace(Info.SecondName)) return false;
            if (string.IsNullOrWhiteSpace(Info.LastName)) return false;
            if (string.IsNullOrWhiteSpace(Info.NationalNo)) return false;
            if (Info.DateOfBirth == DateTime.MinValue) return false;
            if (Info.DateOfBirth >= DateTime.Today) return false;
            if (Info.Nationality == null) return false;

            // on add only — national number must be unique
            if (Info.IsNew && ExistsByNationalNo(Info.NationalNo)) return false;

            if (Info.IsNew)
            {
                Info.Id = PersonData.Add(Info);
                return !Info.IsNew;
            }

            return PersonData.Update(Info);
        }
    }
}