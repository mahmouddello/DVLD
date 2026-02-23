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
    public static class TestData
    {
        public static DataTable GetAllTests()
        {
            DataTable dt = new DataTable();
            string query = "SELECT * FROM Tests";

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                    if (reader.HasRows)
                        dt.Load(reader);
            }

            return dt;
        }

        public static Test GetById(int testId)
        {
            string query = "SELECT * FROM Tests WHERE TestID = @TestID";

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@TestID", testId);

                using (SqlDataReader reader = command.ExecuteReader())
                    return Map(reader);
            }
        }

        public static int InsertNew(Test test)
        {
            const string query = @"INSERT INTO Tests VALUES (@TestAppointmentID, @TestResult, @Notes, @CreatedByUserID);
                                   SELECT SCOPE_IDENTITY();";
            int newTestId = -1;

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TestAppointmentID", test.TestAppointmentId);
                command.Parameters.AddWithValue("@TestResult", (int)test.Result);
                command.Parameters.AddWithValue("@CreatedByUserID", test.CreatedByUserId);
                var param = command.Parameters.Add("@Notes", SqlDbType.NVarChar);

                if (string.IsNullOrWhiteSpace(test.Notes))
                    param.Value = DBNull.Value;
                else
                    param.Value = test.Notes;

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != DBNull.Value)
                        newTestId = Convert.ToInt32(result);
                }
                catch
                {
                    // handle exception
                }
            }

            return newTestId;
        }

        private static Test Map(SqlDataReader reader)
        {
            if (!reader.Read())
                return null;

            return new Test(
                id: (int)reader["TestID"],
                testAppointmentId: (int)reader["TestAppointmentID"],
                testResult: (TestResult)Convert.ToInt32(reader["TestResult"]),
                notes: reader["Notes"] == DBNull.Value ? "" : (string)reader["Notes"],
                createdByUserId: (int)reader["CreatedByUserID"]
            );
        }

        public static bool HasTestPassedRecord(int ldlaId, int testTypeId)
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
	                            TestResult = 1;";

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", ldlaId);
                command.Parameters.AddWithValue("@TestTypeID", testTypeId);

                object result = command.ExecuteScalar();
                return Convert.ToBoolean(result);
            }
        }
        
        public static bool HasTestFailedRecord(int ldlaId, int testTypeId)
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
                            ) AND 
	                            TestResult = 0;";

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", ldlaId);
                command.Parameters.AddWithValue("@TestTypeID", testTypeId);

                object result = command.ExecuteScalar();
                return Convert.ToBoolean(result);
            }
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

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", ldlaId);
                command.Parameters.AddWithValue("@TestTypeID", testTypeId);

                object result = command.ExecuteScalar();

                return result == null ? -1 : Convert.ToInt32(result);
            }
        }
    }
}
