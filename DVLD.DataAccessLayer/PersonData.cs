using System;
using System.Data;
using System.Data.SqlClient;
using DVLD.EntityLayer;
using DVLD.Infrastructure;

namespace DVLD.DataAccessLayer
{
    public class PersonData
    {
        public static DataTable GetAllAsTable()
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand("SELECT * FROM PeopleDetails_View", connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                        dt.Load(reader);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(
                    "Failed to retrieve People list from PeopleDetails_View.",
                    System.Diagnostics.EventLogEntryType.Error,
                    ex,
                    nameof(GetAllAsTable));
            }

            return dt;
        }

        public static int Add(Person person)
        {
            string query = @"INSERT INTO People (NationalNo, FirstName, SecondName, 
                            ThirdName, LastName, DateOfBirth, Gender, Address, Phone, 
                            Email, NationalityCountryID, ImagePath)
                            VALUES (@NationalNo, @FirstName, @SecondName, @ThirdName,
                            @LastName, @DateOfBirth, @Gender, @Address, @Phone, 
                            @Email, @CountryID, @ImagePath);
                            SELECT SCOPE_IDENTITY();";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    AddParameters(command, person);
                    connection.Open();

                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        return Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(
                    $"Failed to add Person. NationalNo={person.NationalNo}, Name={person.FirstName} {person.LastName}",
                    System.Diagnostics.EventLogEntryType.Error,
                    ex,
                    nameof(Add));
            }

            return -1;
        }

        public static Person GetById(int id)
        {
            string query = "SELECT * FROM PersonDetails_View WHERE PersonID = @Id";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                        if (reader.Read())
                            return MapToEntity(reader);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(
                    $"Failed to retrieve Person. PersonID={id}",
                    System.Diagnostics.EventLogEntryType.Error,
                    ex,
                    nameof(GetById));
            }

            return null;
        }

        public static Person GetByNationalNo(string nationalNo)
        {
            string query = "SELECT * FROM PersonDetails_View WHERE NationalNo = @NationalNo";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NationalNo", nationalNo);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                        if (reader.Read())
                            return MapToEntity(reader);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(
                    $"Failed to retrieve Person. NationalNo={nationalNo}",
                    System.Diagnostics.EventLogEntryType.Error,
                    ex,
                    nameof(GetByNationalNo));
            }

            return null;
        }

        public static bool ExistsById(int id)
        {
            string query = "SELECT 1 FROM People WHERE PersonID = @Id";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    connection.Open();

                    return command.ExecuteScalar() != null;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(
                    $"Failed to check Person existence. PersonID={id}",
                    System.Diagnostics.EventLogEntryType.Error,
                    ex,
                    nameof(ExistsById));
            }

            return false;
        }

        public static bool ExistsByNationalNo(string nationalNo)
        {
            string query = "SELECT 1 FROM People WHERE NationalNo = @NationalNo";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NationalNo", nationalNo);
                    connection.Open();

                    return command.ExecuteScalar() != null;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(
                    $"Failed to check Person existence. NationalNo={nationalNo}",
                    System.Diagnostics.EventLogEntryType.Error,
                    ex,
                    nameof(ExistsByNationalNo));
            }

            return false;
        }

        public static bool Update(Person person)
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

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", person.Id);
                    AddParameters(command, person);
                    connection.Open();

                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(
                    $"Failed to update Person. PersonID={person.Id}, NationalNo={person.NationalNo}",
                    System.Diagnostics.EventLogEntryType.Error,
                    ex,
                    nameof(Update));
            }

            return false;
        }

        public static bool Delete(int id)
        {
            string query = "DELETE FROM People WHERE PersonID = @Id";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    connection.Open();

                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(
                    $"Failed to delete Person. PersonID={id}",
                    System.Diagnostics.EventLogEntryType.Error,
                    ex,
                    nameof(Delete));
            }

            return false;
        }

        private static void AddParameters(SqlCommand command, Person person)
        {
            command.Parameters.AddWithValue("@NationalNo", person.NationalNo);
            command.Parameters.AddWithValue("@FirstName", person.FirstName);
            command.Parameters.AddWithValue("@SecondName", person.SecondName);
            command.Parameters.AddWithValue("@ThirdName", GetValueOrDBNull(person.ThirdName));
            command.Parameters.AddWithValue("@LastName", person.LastName);
            command.Parameters.AddWithValue("@DateOfBirth", person.DateOfBirth);
            command.Parameters.AddWithValue("@Gender", (byte)person.Gender);
            command.Parameters.AddWithValue("@Address", person.Address);
            command.Parameters.AddWithValue("@Phone", person.Phone);
            command.Parameters.AddWithValue("@Email", GetValueOrDBNull(person.Email));
            command.Parameters.AddWithValue("@CountryID", person.Nationality.Id);
            command.Parameters.AddWithValue("@ImagePath", GetValueOrDBNull(person.ImagePath));
        }

        private static Person MapToEntity(SqlDataReader reader)
        {
            return new Person(
                id: (int)reader["PersonID"],
                nationalNo: (string)reader["NationalNo"],
                firstName: (string)reader["FirstName"],
                secondName: (string)reader["SecondName"],
                thirdName: reader["ThirdName"] as string ?? string.Empty,
                lastName: (string)reader["LastName"],
                dateOfBirth: (DateTime)reader["DateOfBirth"],
                gender: (Gender)(byte)reader["Gender"],
                address: (string)reader["Address"],
                phone: (string)reader["Phone"],
                email: reader["Email"] as string ?? string.Empty,
                nationality: new Country(
                    id: (int)reader["CountryID"],
                    name: (string)reader["Nationality"]
                ),
                imagePath: reader["ImagePath"] as string ?? string.Empty
            );
        }

        private static object GetValueOrDBNull(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? (object)DBNull.Value : value;
        }
    }
}