using System;
using System.Data;
using System.Data.SqlClient;
using DVLD.EntityLayer;
using DVLD.Infrastructure;

namespace DVLD.DataAccessLayer
{
    public class DriverData
    {
        public static int Add(Driver driver)
        {
            string query = @"INSERT INTO Drivers (PersonID, CreatedByUserID, CreatedDate)
                             VALUES (@PersonID, @CreatedByUserID, @CreatedDate);
                             SELECT SCOPE_IDENTITY();";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", driver.PersonId);
                    command.Parameters.AddWithValue("@CreatedByUserID", driver.CreatedByUserId);
                    command.Parameters.AddWithValue("@CreatedDate", driver.CreatedAt);

                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result != null)
                        return Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(
                    $"Failed to add Driver. PersonID={driver.PersonId}",
                    System.Diagnostics.EventLogEntryType.Error,
                    ex,
                    nameof(Add));
            }

            return -1;
        }

        public static DataTable GetAllAsTable()
        {
            string query = @"SELECT * FROM DriversView";
            DataTable dataTable = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                            dataTable.Load(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(
                    "Failed to retrieve Drivers list.",
                    System.Diagnostics.EventLogEntryType.Error,
                    ex,
                    nameof(GetAllAsTable));
            }

            return dataTable;
        }

        public static Driver GetById(int driverId)
        {
            string query = @"SELECT * FROM Drivers WHERE DriverID = @DriverID";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DriverID", driverId);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                            return MapToEntity(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(
                    $"Failed to retrieve Driver. DriverID={driverId}",
                    System.Diagnostics.EventLogEntryType.Error,
                    ex,
                    nameof(GetById));
            }

            return null;
        }

        public static Driver GetByPersonID(int personID)
        {
            string query = "SELECT * FROM Drivers WHERE PersonID = @PersonID";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", personID);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                            return MapToEntity(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(
                    $"Failed to retrieve Driver by PersonID={personID}",
                    System.Diagnostics.EventLogEntryType.Error,
                    ex,
                    nameof(GetByPersonID));
            }

            return null;
        }

        public static bool DeleteById(int driverId)
        {
            string query = "DELETE FROM Drivers WHERE DriverID = @DriverID";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DriverID", driverId);
                    connection.Open();

                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch (SqlException ex)
            {
                Logger.Log(
                    $"Failed to delete Driver. DriverID={driverId}",
                    System.Diagnostics.EventLogEntryType.Error,
                    ex,
                    nameof(DeleteById));
            }

            return false;
        }

        private static Driver MapToEntity(SqlDataReader reader)
        {
            return new Driver(
                id: Convert.ToInt32(reader["DriverID"]),
                personId: Convert.ToInt32(reader["PersonID"]),
                createdByUserId: Convert.ToInt32(reader["CreatedByUserID"]),
                createdAt: Convert.ToDateTime(reader["CreatedDate"])
            );
        }
    }
}