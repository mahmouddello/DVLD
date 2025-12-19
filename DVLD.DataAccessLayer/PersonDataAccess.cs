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

        public static DataRow GetPersonByID(int PersonID)
        {
            DataTable dt = new DataTable();
            string query = @"SELECT * FROM vw_PersonDetails WHERE PersonID = @PersonID";

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@PersonID", PersonID);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    dt.Load(reader);
                }
            }

            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        public static int InsertNewPerson(string nationalNo, string firstName,
            string secondName, string thirdName, string lastName, DateTime dateOfBirth,
            byte gender, string address, string phone, string email,
            int countryID, string imagePath)
        {
            string query = @"INSERT INTO People (NationalNo, FirstName, SecondName, 
                     ThirdName, LastName, DateOfBirth, Gender, Address, Phone, 
                     Email, NationalityCountryID, ImagePath)
                     VALUES (@NationalNo, @FirstName, @SecondName, @ThirdName,
                     @LastName, @DateOfBirth, @Gender, @Address, @Phone, 
                     @Email, @CountryID, @ImagePath);
                     SELECT SCOPE_IDENTITY();";

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                // AddWithValue handles null automatically
                command.Parameters.AddWithValue("@NationalNo", nationalNo);
                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@SecondName", secondName);
                command.Parameters.AddWithValue("@ThirdName", DbValue(thirdName));
                command.Parameters.AddWithValue("@LastName", lastName);
                command.Parameters.AddWithValue("@DateOfBirth", dateOfBirth);
                command.Parameters.AddWithValue("@Gender", gender);
                command.Parameters.AddWithValue("@Address", address);
                command.Parameters.AddWithValue("@Phone", phone);
                command.Parameters.AddWithValue("@Email", DbValue(email));
                command.Parameters.AddWithValue("@CountryID", countryID);
                command.Parameters.AddWithValue("@ImagePath", DbValue(imagePath));

                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                    return Convert.ToInt32(result);
                else
                    return -1;
            }
        }

        public static bool UpdatePerson(int personID, string nationalNo, string firstName,
            string secondName, string thirdName, string lastName, DateTime dateOfBirth,
            byte gender, string address, string phone, string email,
            int countryID, string imagePath)
        {
            string query = @"UPDATE People SET
                            NationalNo = @NationalNo,
                            FirstName = @FirstName,
                            SecondName = @SecondName,
                            ThirdName = @ThirdName,
                            LastName = @LastName,
                            DateOfBirth = @DateOfBirth,
                            Gender = @Gender,
                            Address = @Address, 
                            Phone = @Phone, 
                            Email = @Email, 
                            NationalityCountryID = @CountryID,
                            ImagePath = @ImagePath
                            WHERE PersonID = @PersonID;";
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@PersonID", personID);
                command.Parameters.AddWithValue("@NationalNo", nationalNo);
                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@SecondName", secondName);
                command.Parameters.AddWithValue("@ThirdName", DbValue(thirdName));
                command.Parameters.AddWithValue("@LastName", lastName);
                command.Parameters.AddWithValue("@DateOfBirth", dateOfBirth);
                command.Parameters.AddWithValue("@Gender", gender);
                command.Parameters.AddWithValue("@Address", address);
                command.Parameters.AddWithValue("@Phone", phone);
                command.Parameters.AddWithValue("@Email", DbValue(email));
                command.Parameters.AddWithValue("@CountryID", countryID);
                command.Parameters.AddWithValue("@ImagePath", DbValue(imagePath));

                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }

            return rowsAffected > 0; 
        }

        public static bool DeletePersonByID(int personID)
        {
            string query = @"DELETE FROM People WHERE PersonID = @PersonID";
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@PersonID", personID);
                connection.Open();

                try
                {
                    rowsAffected = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex}");
                    return false;
                }
            }

            return rowsAffected > 0;
        }

        private static object DbValue(string value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? (object)DBNull.Value
                : value;
        }
    }
}
