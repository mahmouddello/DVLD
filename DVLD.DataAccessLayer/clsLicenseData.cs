using DVLD.EntityLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.DataAccessLayer
{
    public static class clsLicenseData
    {
        public static DataTable GetAllLicenses(int driverId)
        {
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
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                connection.Open();
                cmd.Parameters.AddWithValue(@"DriverID", driverId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                    dt.Load(reader);
            }

            return dt;
        }

        public static int InsertNew(clsLicense license)
        {
            int newLicenseID = -1;

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
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

                using (SqlCommand command = new SqlCommand(query, connection))
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

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                            newLicenseID = insertedID;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }

            return newLicenseID;
        }

        public static clsLicense GetById(int licenseId)
        {
            string query = @"SELECT * FROM Licenses WHERE LicenseID = @LicenseID";

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@LicenseID", licenseId);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                        return MapReaderToObject(reader);
                }
            }

            return null;
        }

        public static clsLicense GetByApplicationId(int applicationId)
        {
            string query = @"SELECT * FROM Licenses WHERE ApplicationID = @ApplicationID";

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ApplicationID", applicationId);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                        return MapReaderToObject(reader);
                }
            }

            return null;
        }

        public static int GetActiveLicenseCountByDriverId(int driverId)
        {
            string query = @"SELECT 
		                        COUNT (IsActive)
	                        FROM
		                        Licenses
	                        WHERE 
		                        DriverID = @DriverID
		                        AND
		                        IsActive = 1";

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@DriverID", driverId);
                connection.Open();

                object result = cmd.ExecuteScalar();

                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        private static clsLicense MapReaderToObject(SqlDataReader reader)
        {
            var notesIndex = reader.GetOrdinal("Notes");

            return new clsLicense(
                id: reader.GetInt32(reader.GetOrdinal("LicenseID")),
                applicationId: reader.GetInt32(reader.GetOrdinal("ApplicationID")),
                driverId: reader.GetInt32(reader.GetOrdinal("DriverID")),
                licenseClassID: reader.GetInt32(reader.GetOrdinal("LicenseClass")),
                issueDate: reader.GetDateTime(reader.GetOrdinal("IssueDate")),
                expirationDate: reader.GetDateTime(reader.GetOrdinal("ExpirationDate")),
                notes: reader.IsDBNull(notesIndex) ? null : reader.GetString(notesIndex),
                paidFees: reader.GetDecimal(reader.GetOrdinal("PaidFees")),
                isActive: reader.GetBoolean(reader.GetOrdinal("IsActive")),
                issueReason: (enLicenseIssueReason)reader.GetInt32(reader.GetOrdinal("IssueReason")),
                createdByUserId: reader.GetInt32(reader.GetOrdinal("CreatedByUserID"))
            );
        }
    }
}
