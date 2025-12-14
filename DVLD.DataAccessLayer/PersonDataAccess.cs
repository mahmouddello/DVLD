using DVLD.EntityLayer;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLD.DataAccessLayer
{
    public class PersonDataAccess
    {
        public static DataTable GetAllPeople()
        {
            DataTable dt = new DataTable();
            string query = @"SELECT * FROM vw_PeopleDetails";

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    dt.Load(reader);
                }
            }

            return dt;
        }

        public static Person GetPersonByID(int PersonID)
        {
            string query = @"SELECT * FROM vw_PersonDetails WHERE PersonID = @PersonID";

            Person person = new Person();
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@PersonID", PersonID);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                        return null;

                    person.ID = (int)reader["PersonID"];
                    person.NationalNo = (string)reader["NationalNo"];
                    person.FirstName = (string)reader["FirstName"];
                    person.SecondName = (string)reader["SecondName"];
                    person.ThirdName = reader["ThirdName"] != DBNull.Value ? (string)reader["ThirdName"] : string.Empty;
                    person.LastName = (string)reader["LastName"];
                    person.DateOfBirth = (DateTime)reader["DateOfBirth"];
                    person.Gender = (string)reader["Gender"];
                    person.Address = (string)reader["Address"];
                    person.Phone = (string)reader["Phone"];
                    person.Email = reader["Email"] != DBNull.Value ? (string)reader["Email"] : string.Empty;
                    person.Nationality = (string)reader["Nationality"];
                    person.ImagePath = reader["ImagePath"] != DBNull.Value ? (string)reader["ImagePath"] : string.Empty;
                }
            }

            return person;
        }
    }
}
