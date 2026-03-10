using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using DVLD.EntityLayer;

namespace DVLD.DataAccessLayer
{
    // No Add, Delete, Update for this class
    public class LicenseClassData
    {
        public static DataTable GetAllAsTable()
        {
            DataTable dt = new DataTable();
            string query = @"SELECT * FROM LicenseClasses";

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
                Debug.WriteLine($"Error in LicenseClassData.GetAllAsTable: {ex.Message}");
                return new DataTable();
            }

            return dt;
        }

        public static LicenseClass GetById(int licenseClassId)
        {
            string query = @"SELECT * FROM LicenseClasses WHERE LicenseClassID = @LicenseClassID";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LicenseClassID", licenseClassId);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                        if (reader.Read())
                            return MapToEntity(reader);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in LicenseClassData.GetById: {ex.Message}");
            }

            return null;
        }

        private static LicenseClass MapToEntity(SqlDataReader reader)
        {
            return new LicenseClass(
                id: (int)reader["LicenseClassID"],
                name: (string)reader["ClassName"],
                description: (string)reader["ClassDescription"],
                minimumAllowedAge: Convert.ToInt32(reader["MinimumAllowedAge"]),
                defaultValidityLength: Convert.ToInt32(reader["DefaultValidityLength"]),
                fees: Convert.ToDecimal(reader["ClassFees"])
            );
        }
    }
}
