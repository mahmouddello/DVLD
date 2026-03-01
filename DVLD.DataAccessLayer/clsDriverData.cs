using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD.EntityLayer;

namespace DVLD.DataAccessLayer
{
    public static class clsDriverData
    {
        public static DataTable GetAllDrivers()
        {
            string query = @"SELECT * FROM DriversView";
            DataTable dataTable = new DataTable();

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

            return dataTable;
        }

        public static clsDriver GetById(int driverId)
        {
            string query = @"SELECT * FROM Drivers WHERE DriverID = @DriverID";

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@DriverID", driverId);
                connection.Open();

                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    if (dataReader.HasRows)
                        return MapReaderToObject(dataReader);
                }
            }

            return null;
        }

        public static clsDriver GetByPersonID(int personID)
        {
            string query = "SELECT * FROM Drivers WHERE PersonID = @PersonID";
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@PersonID", personID);
                connection.Open();
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    if (dataReader.HasRows)
                        return MapReaderToObject(dataReader);
                }
            }
            return null;
        }

        public static int InsertNew(int personId, int createdByUserId, DateTime createdDate)
        {
            string query = @"INSERT INTO Drivers (PersonID, CreatedByUserID, CreatedDate)
                                 VALUES (@PersonID, @CreatedByUserID, @CreatedDate);
                              SELECT SCOPE_IDENTITY();";
            int driverId = -1;

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@PersonID", personId);
                command.Parameters.AddWithValue("@CreatedByUserID", createdByUserId);
                command.Parameters.AddWithValue("@CreatedDate", createdDate);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null)
                        driverId = Convert.ToInt32(result);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }

                return driverId;
            }
        }

        public static bool DeleteById(int driverId)
        {
            try
            {
                string query = "DELETE FROM Drivers WHERE DriverID = @DriverID";

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
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        private static clsDriver MapReaderToObject(SqlDataReader reader)
        {
            return new clsDriver(
                id: Convert.ToInt32(reader["DriverID"]),
                personId: Convert.ToInt32(reader["PersonID"]),
                createdByUserId: Convert.ToInt32(reader["CreatedByUserID"]),
                createdAt: Convert.ToDateTime(reader["CreatedDate"])
            );
        }
    }
}
