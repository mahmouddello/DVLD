using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.DataAccessLayer
{
    public static class UserData
    {
        public static DataTable GetAll()
        {
            DataTable dt = new DataTable();
            string query = @"SELECT 
                                U.UserID,
                                U.PersonID,
                                U.UserName,
                                U.IsActive,
                                CONCAT(
                                    P.FirstName, ' ',
                                    P.SecondName, ' ',
                                    ISNULL(P.ThirdName + ' ', ''),
                                    P.LastName
                                ) AS FullName
                            FROM Users U
                            INNER JOIN People P ON U.PersonID = P.PersonID;";

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

        public static DataRow GetById(int userId)
        {
            DataTable dt = new DataTable();
            string query = @"SELECT * FROM Users WHERE UserID = @UserID";

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserID", userId);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    dt.Load(reader);
                }
            }

            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        public static DataRow GetByUsername(string username)
        {
            DataTable dt = new DataTable();
            string query = @"SELECT * FROM Users WHERE UserName = @Username";

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Username", username);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    dt.Load(reader);
                }
            }

            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        public static bool ExistsById(int userId)
        {
            string query = @"SELECT 1 FROM Users WHERE UserID = @UserID";

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserID", userId);
                connection.Open();

                return command.ExecuteScalar() != null;
            }
        }

        public static bool ExistsByUsername(string username)
        {
            string query = @"SELECT 1 FROM Users WHERE UserName = @Username";

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Username", username);
                connection.Open();

                return command.ExecuteScalar() != null;
            }
        }

        public static bool DeleteById(int userId)
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

        public static int InsertNew(int personId, string username, string password, bool isActive)
        {
            try
            {
                string query = @"INSERT INTO Users (PersonID, UserName, Password, IsActive)
                         VALUES (@PersonID, @Username, @Password, @IsActive);
                         SELECT SCOPE_IDENTITY();";

                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", personId);
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);
                    command.Parameters.AddWithValue("@IsActive", isActive);

                    connection.Open();
                    object result = command.ExecuteScalar();

                    return result == null ? -1 : Convert.ToInt32(result);
                }
            }
            catch
            {
                return -1;
            }
        }

        public static bool UpdateById(int userId, int personId, string username, string password, bool isActive)
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
                    command.Parameters.AddWithValue("@UserID", userId);
                    command.Parameters.AddWithValue("@PersonID", personId);
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);
                    command.Parameters.AddWithValue("@IsActive", isActive);

                    connection.Open();

                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        public static bool UpdatePassword(int userId, string password)
        {
            string query = @"UPDATE Users SET Password = @Password WHERE UserID = @UserID";

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("@UserID", userId);
                command.Parameters.AddWithValue("@Password", password);

                return command.ExecuteNonQuery() > 0;
            }
        }
    }
}
