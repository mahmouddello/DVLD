using System;
using System.Data.SqlClient;
using System.Diagnostics;
using DVLD.EntityLayer;

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
                                enApplicationStatus,
                                LastStatusDate,
                                PaidFees,
                                CreatedByUserID
                            )
                            VALUES
                            (
                                @ApplicantPersonID,
                                @ApplicationDate,
                                @ApplicationTypeID,
                                @enApplicationStatus,
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
                Debug.WriteLine($"Error in ApplicationData.Add: {ex.Message}");
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
                            enApplicationStatus = @Status,
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
                Debug.WriteLine($"Error in ApplicationData.Update: {ex.Message}");
                return false;
            }
        }

        public static bool UpdateStatus(int applicationId, enApplicationStatus status)
        {
            string query = @"UPDATE Applications SET
                            enApplicationStatus = @Status,
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
                Debug.WriteLine($"Error in ApplicationData.UpdateStatus: {ex.Message}");
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

                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in ApplicationData.Delete: {ex.Message}");
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
                                    AND A.enApplicationStatus IN (1, 3);";


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
                status: (enApplicationStatus)Convert.ToInt32(reader["enApplicationStatus"]),
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
            command.Parameters.AddWithValue("@enApplicationStatus", (int)application.Status);
            command.Parameters.AddWithValue("@LastStatusDate", application.LastStatusDate);
            command.Parameters.AddWithValue("@PaidFees", application.PaidFees);
            command.Parameters.AddWithValue("@CreatedByUserID", application.CreatedByUserId);
        }
    }
}
