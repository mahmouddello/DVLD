using DVLD.EntityLayer;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Reflection.Emit;

namespace DVLD.DataAccessLayer
{
    public class InternationalLicenseData
    {
        public static DataTable GetAllAsTable()
        {
            string query = @"SELECT * FROM InternationalLicenseApplications_View";
            DataTable table = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                        table.Load(reader);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in InternationalLicenseData.GetAllAsTable: {ex.Message}");
            }

            return table;
        }

        public static InternationalLicense GetById(int id)
        {
            string query = @"SELECT * FROM InternationalLicenses WHERE InternationalLicenseID = @Id";

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
                Debug.WriteLine($"Error in InternationalLicenseData.GetById: {ex.Message}");
            }

            return null;
        }

        public static InternationalLicense GetByLicenseId(int licenseId)
        {
            string query = @"SELECT * FROM InternationalLicenses WHERE IssuedUsingLocalLicenseID = @Id";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", licenseId);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                        if (reader.Read())
                            return MapToEntity(reader);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in InternationalLicenseData.GetById: {ex.Message}");
            }

            return null;
        }

        public static InternationalLicense GetByApplicationId(int applicationId)
        {
            string query = @"SELECT * FROM InternationalLicenses WHERE ApplicationID = @Id";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", applicationId);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                        if (reader.Read())
                            return MapToEntity(reader);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in InternationalLicenseData.GetById: {ex.Message}");
            }

            return null;
        }

        public static bool ExistsById(int internationaLicenseId)
        {
            string query = @"SELECT 1 FROM InternationalLicenses WHERE InternationalLicenseID = @LicenseId";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LicenseId", internationaLicenseId);
                    connection.Open();

                    return command.ExecuteScalar() != null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in InternationalLicenseData.ExsistByLocalLicenseId: {ex.Message}");
            }

            return false;
        }

        public static bool ExistsByLocalLicenseId(int licenseId)
        {
            string query = @"SELECT 1 FROM InternationalLicenses WHERE IssuedUsingLocalLicenseID = @LicenseId";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LicenseId", licenseId);
                    connection.Open();
                    
                    return command.ExecuteScalar() != null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in InternationalLicenseData.ExsistByLocalLicenseId: {ex.Message}");
            }

            return false;
        }

        public static bool ExistsByApplicationId(int applicationId)
        {
            string query = @"SELECT 1 FROM InternationalLicenses WHERE ApplicationID = @LicenseId";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LicenseId", applicationId);
                    connection.Open();

                    return command.ExecuteScalar() != null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in InternationalLicenseData.ExsistByLocalLicenseId: {ex.Message}");
            }

            return false;
        }

        private static InternationalLicense MapToEntity(SqlDataReader reader)
        {
            return new InternationalLicense(
                internationalLicenseId: Convert.ToInt32(reader["InternationalLicenseID"]),
                applicationId: Convert.ToInt32(reader["ApplicationID"]),
                driverId: Convert.ToInt32(reader["DriverID"]),
                localLicenseId: Convert.ToInt32(reader["IssuedUsingLocalLicenseID"]),
                issueDate: Convert.ToDateTime(reader["IssueDate"]),
                expirationDate: Convert.ToDateTime(reader["ExpirationDate"]),
                isActive: Convert.ToBoolean(reader["IsActive"]),
                createdByUserId: Convert.ToInt32(reader["CreatedByUserID"])
            );
        }
    }
}
