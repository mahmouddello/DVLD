using System;
using System.Data;
using System.Data.SqlClient;
using DVLD.Infrastructure;
using DVLD.EntityLayer;

namespace DVLD.DataAccessLayer
{
    public class TestData
    {
        public static int Add(Test test)
        {
            string query = @"INSERT INTO Tests VALUES 
                            (@TestAppointmentID, @TestResult, @Notes, @CreatedByUserID);
                            SELECT SCOPE_IDENTITY();";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    AddSharedParameters(command, test);
                    connection.Open();

                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        return Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(
                    $"Failed to add Test. TestAppointmentID={test.TestAppointmentId}, Result={test.Result}, CreatedByUserID={test.CreatedByUserId}",
                    System.Diagnostics.EventLogEntryType.Error,
                    ex,
                    nameof(Add));
            }

            return -1;
        }

        public static DataTable GetAllAsTable()
        {
            DataTable dt = new DataTable();
            string query = @"SELECT * FROM Tests";

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
                Logger.Log(
                    "Failed to retrieve Tests list.",
                    System.Diagnostics.EventLogEntryType.Error,
                    ex,
                    nameof(GetAllAsTable));
            }

            return dt;
        }

        public static Test GetById(int testId)
        {
            string query = "SELECT * FROM Tests WHERE TestID = @TestID";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TestID", testId);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                        if (reader.Read())
                            return MapToEntity(reader);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(
                    $"Failed to retrieve Test. TestID={testId}",
                    System.Diagnostics.EventLogEntryType.Error,
                    ex,
                    nameof(GetById));
            }

            return null;
        }

        public static Test GetTestByAppointmentId(int appointmentId)
        {
            string query = @"SELECT * FROM Tests WHERE TestAppointmentID = @TestAppointmentID";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TestAppointmentID", appointmentId);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                        if (reader.Read())
                            return MapToEntity(reader);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(
                    $"Failed to retrieve Test by AppointmentID={appointmentId}",
                    System.Diagnostics.EventLogEntryType.Error,
                    ex,
                    nameof(GetTestByAppointmentId));
            }

            return null;
        }

        public static int GetTestTrialsCount(int ldlaId, int testTypeId)
        {
            string query = @"SELECT COUNT(*)
                            FROM Tests
                            WHERE TestAppointmentID IN 
                            (
                                SELECT TestAppointmentID
                                FROM TestAppointments
                                WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID
                                  AND TestTypeID = @TestTypeID
                            )";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", ldlaId);
                    command.Parameters.AddWithValue("@TestTypeID", testTypeId);

                    connection.Open();

                    object result = command.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : -1;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(
                    $"Failed to count test trials. LDLAID={ldlaId}, TestTypeID={testTypeId}",
                    System.Diagnostics.EventLogEntryType.Error,
                    ex,
                    nameof(GetTestTrialsCount));
            }

            return -1;
        }

        public static int GetPassedTestCount(int ldlaId)
        {
            string query = @"SELECT COUNT(*) AS PassedTests
                            FROM Tests t
                            INNER JOIN TestAppointments ta 
                                ON t.TestAppointmentID = ta.TestAppointmentID
                            WHERE ta.LocalDrivingLicenseApplicationID = @LdlaId
                              AND t.TestResult = 1";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LdlaId", ldlaId);
                    connection.Open();

                    object result = command.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(
                    $"Failed to count passed tests. LDLAID={ldlaId}",
                    System.Diagnostics.EventLogEntryType.Error,
                    ex,
                    nameof(GetPassedTestCount));
            }

            return 0;
        }

        public static bool HasTestRecord(int ldlaId, int testTypeId, bool isPassed)
        {
            string query = @"SELECT 1
                            FROM Tests
                            WHERE TestAppointmentID IN 
                            (
                                SELECT TestAppointmentID
                                FROM TestAppointments
                                WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID
                                  AND TestTypeID = @TestTypeID
                                  AND IsLocked = 1
                            )
                            AND TestResult = @TestResult;";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", ldlaId);
                    command.Parameters.AddWithValue("@TestTypeID", testTypeId);
                    command.Parameters.AddWithValue("@TestResult", isPassed ? 1 : 0);

                    connection.Open();

                    object result = command.ExecuteScalar();
                    return result != null;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(
                    $"Failed to check test record. LDLAID={ldlaId}, TestTypeID={testTypeId}, IsPassed={isPassed}",
                    System.Diagnostics.EventLogEntryType.Error,
                    ex,
                    nameof(HasTestRecord));
            }

            return false;
        }

        private static Test MapToEntity(SqlDataReader reader)
        {
            return new Test(
                id: (int)reader["TestID"],
                testAppointmentId: (int)reader["TestAppointmentID"],
                testResult: (TestResult)Convert.ToInt32(reader["TestResult"]),
                notes: reader["Notes"] == DBNull.Value ? "" : (string)reader["Notes"],
                createdByUserId: (int)reader["CreatedByUserID"]
            );
        }

        private static void AddSharedParameters(SqlCommand command, Test test)
        {
            command.Parameters.AddWithValue("@TestAppointmentID", test.TestAppointmentId);
            command.Parameters.AddWithValue("@TestResult", (int)test.Result);
            command.Parameters.AddWithValue("@CreatedByUserID", test.CreatedByUserId);

            var param = command.Parameters.Add("@Notes", SqlDbType.NVarChar);
            param.Value = string.IsNullOrWhiteSpace(test.Notes)
                ? (object)DBNull.Value
                : test.Notes;
        }
    }
}