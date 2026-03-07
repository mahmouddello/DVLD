using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using DVLD.EntityLayer;

namespace DVLD.DataAccessLayer
{
    public class ApplicationTypeData
    {
        public static DataTable GetAllAsTable()
        {
            DataTable dt = new DataTable();
            string query = @"SELECT * FROM ApplicationTypes";

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
                Debug.WriteLine($"Error in ApplicationTypeData.GetAllAsTable: {ex.Message}");
                return new DataTable();
            }

            return dt;
        }

        public static ApplicationType GetByType(enApplicationType appType)
        {
            string query = @"SELECT * FROM ApplicationTypes WHERE ApplicationTypeID = @ApplicationTypeID";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationTypeID", (int)appType);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                        if (reader.Read())
                            return MapToEntity(reader);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in ApplicationTypeData.GetByType: {ex.Message}");
            }

            return null;
        }

        public static bool Update(ApplicationType appType)
        {
            string query = @"UPDATE ApplicationTypes SET
                                 ApplicationTypeTitle = @Title,
                                 ApplicationFees = @Fees
                             WHERE
                                 ApplicationTypeID = @ApplicationTypeID";
            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationTypeID", (int)appType.Type);
                    AddSharedParameters(command, appType);
                    connection.Open();

                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in ApplicationTypeData.Update: {ex.Message}");
                return false;
            }
        }

        private static void AddSharedParameters(SqlCommand command, ApplicationType appType)
        {
            command.Parameters.AddWithValue("@Title", appType.Title);
            command.Parameters.AddWithValue("@Fees", appType.Fees);
        }

        private static ApplicationType MapToEntity(SqlDataReader reader)
        {
            return new ApplicationType(
                type: (enApplicationType)reader["ApplicationTypeID"],
                title: (string)reader["ApplicationTypeTitle"],
                fees: (decimal)reader["ApplicationFees"]
            );
        }
    }
}