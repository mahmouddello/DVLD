using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using DVLD.EntityLayer;

namespace DVLD.DataAccessLayer
{
    public class UserData
    {
        public static DataTable GetAllAsTable()
        {
            DataTable dt = new DataTable();
            string query = @"SELECT * FROM UsersDetails_View";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                        dt.Load(reader);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in UserData.GetAllAsTable: {ex.Message}");
                return new DataTable();
            }

            return dt;
        }

        public static int Add(User user)
        {
            string query = @"INSERT INTO Users (PersonID, UserName, Password, IsActive)
                            VALUES (@PersonID, @Username, @Password, @IsActive);
                            SELECT SCOPE_IDENTITY();";
            try
            {

                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    AddSharedParameters(command, user);
                    connection.Open();

                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        return Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in UserData.Add: {ex.Message}");
            }

            return -1;
        }

        public static User GetById(int userId)
        {
            string query = @"SELECT * FROM Users WHERE UserID = @UserID";

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserID", userId);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                    if (reader.Read())
                        return MapToEntity(reader);
            }

            return null;
        }

        public static User GetByUsername(string username)
        {
            string query = @"SELECT * FROM Users WHERE UserName = @Username";

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Username", username);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                    if (reader.Read())
                        return MapToEntity(reader);
            }

            return null;
        }

        public static bool ExistsById(int userId)
        {
            string query = @"SELECT 1 FROM Users WHERE UserID = @UserID";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userId);
                    connection.Open();

                    return command.ExecuteScalar() != null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in UserData.ExistsById: {ex.Message}");
                return false;
            }
        }

        public static bool ExistsByUsername(string username)
        {
            string query = @"SELECT 1 FROM Users WHERE UserName = @Username";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    connection.Open();

                    return command.ExecuteScalar() != null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in UserData.ExistsByUsername: {ex.Message}");
                return false;
            }
        }

        public static bool ExistsByPersonId(int personId)
        {
            string query = @"SELECT 1 FROM Users WHERE PersonID = @PersonID";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", personId);
                    connection.Open();

                    return command.ExecuteScalar() != null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in UserData.ExistsByPersonId: {ex.Message}");
                return false;
            }
        }

        public static bool Update(User user)
        {
            string query = @"UPDATE Users SET
                                 PersonID = @PersonID,
                                 UserName = @Username,
                                 Password = @Password,
                                 IsActive = @IsActive
                             WHERE 
                                 UserID = @UserID";
            try
            {

                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    AddSharedParameters(command, user);
                    command.Parameters.AddWithValue("@UserID", user.Id);
                    connection.Open();

                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in UserData.Update: {ex.Message}");
                return false;
            }
        }

        public static bool UpdatePassword(int userId, string newPassword)
        {
            string query = @"UPDATE Users SET Password = @Password WHERE UserID = @UserID";

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("@UserID", userId);
                command.Parameters.AddWithValue("@Password", newPassword);

                return command.ExecuteNonQuery() > 0;
            }
        }

        public static bool Delete(int userId)
        {
            try
            {
                string query = "DELETE FROM Users WHERE UserID = @UserID";

                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userId);
                    connection.Open();

                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch (SqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        private static User MapToEntity(SqlDataReader reader)
        {
            return new User(
                id: (int)reader["UserID"],
                username: (string)reader["Username"],
                password: (string)reader["Password"],
                isActive: (bool)reader["IsActive"],
                person: PersonData.GetById((int)reader["PersonID"])
            );
        }

        private static void AddSharedParameters(SqlCommand command, User user)
        {
            command.Parameters.AddWithValue("@PersonID", user.PersonId);
            command.Parameters.AddWithValue("@Username", user.Username);
            command.Parameters.AddWithValue("@Password", user.Password);
            command.Parameters.AddWithValue("@IsActive", user.IsActive);
        }

    }
}
