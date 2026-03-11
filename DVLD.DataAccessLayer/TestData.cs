using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
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

                    return result != null ? Convert.ToInt32(result) : -1;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in TestData.Add: {ex.Message}");
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
                Debug.WriteLine($"Error in TestData.GetAllAsTable: {ex.Message}");
                return new DataTable();
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
                Debug.WriteLine($"Error in TestData.GetById: {ex.Message}");
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
                Debug.WriteLine($"Error in TestData.GetTestByAppointmentId: {ex.Message}");
            }

            return null;
        }

        public static int GetTestTrialsCount(int ldlaId, int testTypeId)
        {
            string query = @"SELECT 
                                COUNT(*)
                            FROM 
                                Tests
                            WHERE TestAppointmentID IN 
                            (
                                SELECT 
                                        TestAppointmentID
                                    FROM 
                                        TestAppointments
                                WHERE
                                    LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID
                                    AND
                                    TestTypeID = @TestTypeID
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

                    return result == null ? -1 : Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in TestData.GetTestTrainsCount: {ex.Message}");
            }

            return -1;
        }

        public static bool HasTestRecord(int ldlaId, int testTypeId, bool isPassed)
        {
            string query = @"SELECT 
	                            1
                            FROM 
	                            Tests
                            WHERE TestAppointmentID IN 
                            (
	                            SELECT 
			                            TestAppointmentID
		                            FROM 
			                            TestAppointments
	                            WHERE
		                            LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID
		                            AND
		                            TestTypeID = @TestTypeID
		                            AND
		                            IsLocked = 1
                            ) AND 
	                            TestResult = @TestResult;";
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

                    return Convert.ToBoolean(result);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in TestData.HasTestRecord: {ex.Message}");
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

            if (string.IsNullOrWhiteSpace(test.Notes))
                param.Value = DBNull.Value;
            else
                param.Value = test.Notes;
        }
    }
}
