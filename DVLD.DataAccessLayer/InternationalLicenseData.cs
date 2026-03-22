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
        public static int Add(InternationalLicense entity)
        {
            string query = @"INSERT INTO [dbo].[InternationalLicenses]
                        ([ApplicationID]
                        ,[DriverID]
                        ,[IssuedUsingLocalLicenseID]
                        ,[IssueDate]
                        ,[ExpirationDate]
                        ,[IsActive]
                        ,[CreatedByUserID])
                     VALUES
                        (@ApplicationID
                        ,@DriverID
                        ,@IssuedUsingLocalLicenseID
                        ,@IssueDate
                        ,@ExpirationDate
                        ,@IsActive
                        ,@CreatedByUserID);
                     SELECT SCOPE_IDENTITY();";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationID", entity.ApplicationId);
                    command.Parameters.AddWithValue("@DriverID", entity.DriverId);
                    command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", entity.LocalLicenseId);
                    command.Parameters.AddWithValue("@IssueDate", entity.IssueDate);
                    command.Parameters.AddWithValue("@ExpirationDate", entity.ExpirationDate);
                    command.Parameters.AddWithValue("@IsActive", entity.IsActive);
                    command.Parameters.AddWithValue("@CreatedByUserID", entity.CreatedByUserId);

                    connection.Open();

                    object result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int newId))
                        return newId;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in InternationalLicenseData.Add: {ex.Message}");
            }

            return -1;
        }

        public static DataTable GetAllAsTable(int driverId)
        {
            string query = @"SELECT * FROM InternationalLicenseApplications_View WHERE DriverID = @Id";
            DataTable table = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", driverId);
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

        public static bool ExistsActiveNonExpiredByLocalLicenseId(int licenseId)
        {
            string query = @"SELECT 1 FROM InternationalLicenses 
                     WHERE IssuedUsingLocalLicenseID = @LicenseId 
                     AND IsActive = 1
                     AND ExpirationDate > GETDATE()";

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
                Debug.WriteLine($"Error in InternationalLicenseData.ExistsActiveByLocalLicenseId: {ex.Message}");
            }

            return false;
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
