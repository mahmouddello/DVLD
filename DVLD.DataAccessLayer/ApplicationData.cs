using System;
using System.Data.SqlClient;
using System.Diagnostics;
using DVLD.EntityLayer;
using DVLD.Infrastructure;

namespace DVLD.DataAccessLayer
{
    public class ApplicationData
    {
        public static int Add(Application application)
        {
            string query = @"
                            INSERT INTO Applications
                            (
                                ApplicantPersonID,
                                ApplicationDate,
                                ApplicationTypeID,
                                ApplicationStatus,
                                LastStatusDate,
                                PaidFees,
                                CreatedByUserID
                            )
                            VALUES
                            (
                                @ApplicantPersonID,
                                @ApplicationDate,
                                @ApplicationTypeID,
                                @ApplicationStatus,
                                @LastStatusDate,
                                @PaidFees,
                                @CreatedByUserID
                            );

                            SELECT SCOPE_IDENTITY();";
            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    AddSharedParameters(command, application);
                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                        return Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Error occurd when adding a new person record", EventLogEntryType.Error, ex, nameof(Add));
            }

            return -1;
        }

        public static Application GetById(int applicationId)
        {
            string query = @"SELECT * FROM Applications WHERE ApplicationID = @ApplicationID";

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ApplicationID", applicationId);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                    if (reader.Read())
                        return MapToEntity(reader);
            }

            return null;
        }

        public static bool ExistsById(int applicationId)
        {
            string query = @"SELECT 1 FROM Applications WHERE ApplicationID = @ApplicationID";

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ApplicationID", applicationId);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                    return reader.HasRows;
            }
        }

        public static bool Update(Application application)
        {
            string query = @"UPDATE Applications SET
                            ApplicantPersonID = @ApplicantPersonId,
                            ApplicationDate   = @ApplicationDate,
                            ApplicationTypeID = @ApplicationTypeId,
                            ApplicationStatus = @Status,
                            LastStatusDate    = @LastStatusDate,
                            PaidFees          = @PaidFees,
                            CreatedByUserID   = @CreatedByUserId
                            WHERE ApplicationID = @Id;";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", application.Id);
                    AddSharedParameters(command, application);
                    connection.Open();

                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(
                $"Failed to update Application. ApplicationID={application.Id}",
                EventLogEntryType.Error,
                ex,
                nameof(Update));

                return false;
            }
        }

        public static bool UpdateStatus(int applicationId, enApplicationStatus status)
        {
            string query = @"UPDATE Applications SET
                            ApplicationStatus = @Status,
                            LastStatusDate    = @LastStatusDate
                            WHERE ApplicationID = @Id;";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", applicationId);
                    command.Parameters.AddWithValue("@Status", (int)status);
                    command.Parameters.AddWithValue("@LastStatusDate", DateTime.Now);
                    connection.Open();

                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(
                $"Failed to update Application status. ApplicationID={applicationId}, NewStatus={(int)status}",
                EventLogEntryType.Error,
                ex,
                nameof(UpdateStatus));
                return false;
            }
        }

        public static bool Delete(int id)
        {
            string query = "DELETE FROM Applications WHERE ApplicationID = @Id";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    connection.Open();

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        Logger.Log(
                            $"Delete operation affected 0 rows. ApplicationID={id} may not exist.",
                            EventLogEntryType.Warning,
                            null,
                            nameof(Delete));

                        return false;
                    }


                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(
                $"Failed to delete Application. ApplicationID={id}",
                EventLogEntryType.Error,
                ex,
                nameof(Delete));
                return false;
            }
        }

        public static bool ExistsSameClassApplication(int applicantPersonId, int licenseClassId)
        {
            string query = @"SELECT TOP 1 1
                                FROM Applications AS A
                                INNER JOIN LocalDrivingLicenseApplications AS LA
                                    ON A.ApplicationID = LA.ApplicationID
                                WHERE
                                    A.ApplicantPersonID = @ApplicantPersonID
                                    AND LA.LicenseClassID = @LicenseClassID
                                    AND A.ApplicationStatus IN (1, 3);";


            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@ApplicantPersonID", applicantPersonId);
                command.Parameters.AddWithValue("@LicenseClassID", licenseClassId);

                return command.ExecuteScalar() != null;
            }
        }

        private static Application MapToEntity(SqlDataReader reader)
        {
            return new Application(
                id: Convert.ToInt32(reader["ApplicationID"]),
                applicantPersonId: Convert.ToInt32(reader["ApplicantPersonID"]),
                applicationDate: Convert.ToDateTime(reader["ApplicationDate"]),
                applicationTypeId: Convert.ToInt32(reader["ApplicationTypeID"]),
                status: (enApplicationStatus)Convert.ToInt32(reader["ApplicationStatus"]),
                lastStatusDate: Convert.ToDateTime(reader["LastStatusDate"]),
                paidFees: Convert.ToDecimal(reader["PaidFees"]),
                createdByUserId: Convert.ToInt32(reader["CreatedByUserID"])
            );
        }

        private static void AddSharedParameters(SqlCommand command, Application application)
        {
            command.Parameters.AddWithValue("@ApplicantPersonID", application.ApplicantPersonId);
            command.Parameters.AddWithValue("@ApplicationDate", application.ApplicationDate);
            command.Parameters.AddWithValue("@ApplicationTypeID", application.ApplicationTypeId);
            command.Parameters.AddWithValue("@ApplicationStatus", (int)application.Status);
            command.Parameters.AddWithValue("@LastStatusDate", application.LastStatusDate);
            command.Parameters.AddWithValue("@PaidFees", application.PaidFees);
            command.Parameters.AddWithValue("@CreatedByUserID", application.CreatedByUserId);
        }
    }
}
