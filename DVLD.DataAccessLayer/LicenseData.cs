using System;
using System.Data;
using System.Data.SqlClient;
using DVLD.EntityLayer;
using DVLD.Infrastructure;

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
                Logger.Log(
                    $"Failed to add License. DriverID={license.DriverId}, ApplicationID={license.ApplicationId}, ClassID={license.LicenseClassID}",
                    System.Diagnostics.EventLogEntryType.Error,
                    ex,
                    nameof(Add));
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
                             FROM Licenses AS L
                             INNER JOIN LicenseClasses AS LC 
                                ON L.LicenseClass = LC.LicenseClassID
                             WHERE L.DriverID = @DriverID";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@DriverID", driverId);
                    connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                        if (reader.HasRows)
                            dt.Load(reader);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(
                    $"Failed to retrieve licenses list. DriverID={driverId}",
                    System.Diagnostics.EventLogEntryType.Error,
                    ex,
                    nameof(GetAllLicensesAsTable));
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
                Logger.Log(
                    $"Failed to retrieve License. LicenseID={licenseId}",
                    System.Diagnostics.EventLogEntryType.Error,
                    ex,
                    nameof(GetById));
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
                Logger.Log(
                    $"Failed to retrieve License by ApplicationID={applicationId}",
                    System.Diagnostics.EventLogEntryType.Error,
                    ex,
                    nameof(GetByMainApplicationId));
            }

            return null;
        }

        public static int GetActiveLicenseCount(int driverId)
        {
            string query = @"SELECT COUNT(*) 
                             FROM Licenses
                             WHERE DriverID = @DriverID
                             AND IsActive = 1";

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
                Logger.Log(
                    $"Failed to count active licenses. DriverID={driverId}",
                    System.Diagnostics.EventLogEntryType.Error,
                    ex,
                    nameof(GetActiveLicenseCount));
            }

            return -1;
        }

        public static bool UpdateLicenseStatus(int licenseId, bool status)
        {
            string query = @"UPDATE Licenses 
                             SET IsActive = @Status 
                             WHERE LicenseID = @Id";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", licenseId);
                    command.Parameters.AddWithValue("@Status", status ? 1 : 0);

                    connection.Open();

                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(
                    $"Failed to update license status. LicenseID={licenseId}, Status={status}",
                    System.Diagnostics.EventLogEntryType.Error,
                    ex,
                    nameof(UpdateLicenseStatus));
            }

            return false;
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

            var notesParam = new SqlParameter("@Notes", SqlDbType.NVarChar)
            {
                Value = string.IsNullOrWhiteSpace(license.Notes)
                    ? (object)DBNull.Value
                    : license.Notes
            };

            command.Parameters.Add(notesParam);

            command.Parameters.AddWithValue("@PaidFees", license.PaidFees);
            command.Parameters.AddWithValue("@IsActive", license.IsActive);
            command.Parameters.AddWithValue("@IssueReason", (int)license.IssueReason);
            command.Parameters.AddWithValue("@CreatedByUserID", license.CreatedByUserId);
        }
    }
}