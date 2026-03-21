using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using DVLD.EntityLayer;
using License = DVLD.EntityLayer.License;


namespace DVLD.DataAccessLayer
{
    public class LicenseData
    {
        public static int Add(License license)
        {
            string query = @"INSERT INTO [Licenses]
                        ([ApplicationID], [DriverID], [LicenseClass], [IssueDate], 
                         [ExpirationDate], [Notes], [PaidFees], [IsActive], 
                         [IssueReason], [CreatedByUserID])
                        VALUES
                        (@ApplicationID, @DriverID, @LicenseClass, @IssueDate, 
                         @ExpirationDate, @Notes, @PaidFees, @IsActive, 
                         @IssueReason, @CreatedByUserID);
                        SELECT SCOPE_IDENTITY();";
            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    AddSharedParameters(command, license);
                    connection.Open();

                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        return Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in LicenseData.Add: {ex.Message}");
            }

            return -1;
        }

        public static DataTable GetAllLicensesAsTable(int driverId)
        {
            DataTable dt = new DataTable();
            string query = @"SELECT 
	                            L.LicenseID,
	                            L.ApplicationID,
	                            LC.ClassName,
	                            L.IssueDate,
	                            L.ExpirationDate,
	                            L.IsActive
                            FROM 
	                            Licenses AS L
                            INNER JOIN 
	                            LicenseClasses AS LC ON L.LicenseClass = LC.LicenseClassID
                            WHERE L.DriverID = @DriverID";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@DriverID", driverId);
                    connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                        dt.Load(reader);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in LicenseData.GetAllLicensesAsTable: {ex.Message}");
            }

            return dt;
        }

        public static License GetById(int licenseId)
        {
            string query = @"SELECT * FROM Licenses WHERE LicenseID = @LicenseID";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LicenseID", licenseId);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                        if (reader.Read())
                            return MapToEntity(reader);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in LicenseData.GetById: {ex.Message}");
            }

            return null;
        }

        public static License GetByMainApplicationId(int applicationId)
        {
            string query = @"SELECT * FROM Licenses WHERE ApplicationID = @ApplicationID";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationID", applicationId);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                        if (reader.Read())
                            return MapToEntity(reader);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in LicenseData.GetByMainApplicationID: {ex.Message}");
            }

            return null;
        }

        public static int GetActiveLicenseCount(int driverId)
        {
            string query = @"SELECT 
		                        COUNT (IsActive)
	                        FROM
		                        Licenses
	                        WHERE 
		                        DriverID = @DriverID
		                        AND
		                        IsActive = 1";
            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@DriverID", driverId);
                    connection.Open();

                    object result = cmd.ExecuteScalar();

                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in LicenseData.GetActiveLicenseCount: {ex.Message}");
            }

            return -1;
        }

        private static License MapToEntity(SqlDataReader reader)
        {
            var notesIndex = reader.GetOrdinal("Notes");

            return new License(
                id: reader.GetInt32(reader.GetOrdinal("LicenseID")),
                applicationId: reader.GetInt32(reader.GetOrdinal("ApplicationID")),
                driverId: reader.GetInt32(reader.GetOrdinal("DriverID")),
                licenseClassID: reader.GetInt32(reader.GetOrdinal("LicenseClass")),
                issueDate: reader.GetDateTime(reader.GetOrdinal("IssueDate")),
                expirationDate: reader.GetDateTime(reader.GetOrdinal("ExpirationDate")),
                notes: reader.IsDBNull(notesIndex) ? null : reader.GetString(notesIndex),
                paidFees: reader.GetDecimal(reader.GetOrdinal("PaidFees")),
                isActive: reader.GetBoolean(reader.GetOrdinal("IsActive")),
                issueReason: (enLicenseIssueReason)Convert.ToInt32(reader["IssueReason"]),
                createdByUserId: reader.GetInt32(reader.GetOrdinal("CreatedByUserID"))
            );
        }

        private static void AddSharedParameters(SqlCommand command, License license)
        {
            command.Parameters.AddWithValue("@ApplicationID", license.ApplicationId);
            command.Parameters.AddWithValue("@DriverID", license.DriverId);
            command.Parameters.AddWithValue("@LicenseClass", license.LicenseClassID);
            command.Parameters.AddWithValue("@IssueDate", license.IssueDate);
            command.Parameters.AddWithValue("@ExpirationDate", license.ExpirationDate);
            command.Parameters.AddWithValue("@Notes", (object)license.Notes ?? DBNull.Value);
            command.Parameters.AddWithValue("@PaidFees", license.PaidFees);
            command.Parameters.AddWithValue("@IsActive", license.IsActive);
            command.Parameters.AddWithValue("@IssueReason", (int)license.IssueReason);
            command.Parameters.AddWithValue("@CreatedByUserID", license.CreatedByUserId);
        }

    }
}
