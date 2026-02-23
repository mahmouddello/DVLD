using DVLD.EntityLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.DataAccessLayer
{
    public static class TestAppointmentData
    {
        public static DataTable GetAllTestAppointments(int ldlaId, int testTypeId)
        {
            DataTable dt = new DataTable();
            string query = @"SELECT
	                            TestAppointmentID,
	                            AppointmentDate,
	                            PaidFees,
	                            IsLocked
                            FROM
	                            TestAppointments
                            WHERE 
                                 LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID
                                 AND
                                 TestTypeID = @TestTypeID";

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", ldlaId);
                command.Parameters.AddWithValue("@TestTypeID", testTypeId);

                using (SqlDataReader reader = command.ExecuteReader())
                    if (reader.HasRows)
                        dt.Load(reader);
            }

            return dt;
        }

        public static TestAppointment GetById(int testAppointmentId)
        {
            string query = "SELECT * FROM TestAppointments WHERE TestAppointmentID = @TestAppointmentID";

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@TestAppointmentID", testAppointmentId);

                using (SqlDataReader reader = command.ExecuteReader())
                    return Map(reader);
            }
        }

        public static bool ExistsActiveAppointmentByTestType(int ldlaId, int testTypeId)
        {
            string query = @"SELECT 1
                            FROM TestAppointments
                            WHERE
                                LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID
                                AND TestTypeID = @TestTypeID
                                AND IsLocked = 0;";

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", ldlaId);
                command.Parameters.AddWithValue("@TestTypeID", testTypeId);

                return command.ExecuteScalar() != null;
            }
        }

        public static int InsertNew(TestAppointment testAppointment)
        {
            string query = @"INSERT INTO TestAppointments 
                             VALUES (@TestTypeID, @LocalDrivingLicenseApplicationID, @AppointmentDate,
                                     @PaidFees, @CreatedByUserID, @IsLocked, @RetakeTestApplicationID);
                             SELECT SCOPE_IDENTITY()";

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("@TestTypeID", testAppointment.TestTypeId);
                command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", testAppointment.LocalDrivingLicenseApplicationId);
                command.Parameters.AddWithValue("@AppointmentDate", testAppointment.AppointmentDate);
                command.Parameters.AddWithValue("@PaidFees", testAppointment.PaidFees);
                command.Parameters.AddWithValue("@CreatedByUserID", testAppointment.CreatedByUserId);
                command.Parameters.AddWithValue("@IsLocked", testAppointment.IsLocked);
                var param = command.Parameters.Add("@RetakeTestApplicationID", SqlDbType.Int);

                if (testAppointment.RetakeTestApplicationId == -1)
                    param.Value = DBNull.Value;
                else
                    param.Value = testAppointment.RetakeTestApplicationId;

                object result = command.ExecuteScalar();

                return result == null ? -1 : Convert.ToInt32(result);
            }
        }

        public static bool UpdateById(int testAppointmentId, DateTime newAppointmentDate)
        {
            string query = @"UPDATE TestAppointments SET AppointmentDate = @AppointmentDate
                             WHERE TestAppointmentID = @TestAppointmentID";

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("@TestAppointmentID", testAppointmentId);
                command.Parameters.AddWithValue("@AppointmentDate", newAppointmentDate);

                return command.ExecuteNonQuery() > 0;
            }
        }

        public static bool UpdateLockStatus(int testAppointmentId, bool isLocked)
        {
            string query = @"UPDATE TestAppointments SET IsLocked = @IsLocked
                             WHERE TestAppointmentID = @TestAppointmentID";

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("@TestAppointmentID", testAppointmentId);
                command.Parameters.AddWithValue("@IsLocked", isLocked);

                return command.ExecuteNonQuery() > 0;
            }
        }

        private static TestAppointment Map(SqlDataReader reader)
        {
            if (!reader.Read())
                return null;

            return new TestAppointment(
                id: Convert.ToInt32(reader["TestAppointmentID"]),
                testTypeId: Convert.ToInt32(reader["TestTypeID"]),
                localDrivingApplicationLicenseId: Convert.ToInt32(reader["LocalDrivingLicenseApplicationID"]),
                appointmentDate: Convert.ToDateTime(reader["AppointmentDate"]),
                paidFees: Convert.ToDecimal(reader["PaidFees"]),
                createdByUserId: Convert.ToInt32(reader["CreatedByUserID"]),
                isLocked: Convert.ToBoolean(reader["IsLocked"]),
                retakeTestApplicationId: reader["RetakeTestApplicationID"] != DBNull.Value ? Convert.ToInt32(reader["RetakeTestApplicationID"]) : -1
            );
        }

        public static int GetTestIdByAppointmentId(int appointmentId)
        {
            const string query = @"SELECT
	                                    TOP 1 TestID 
                                   FROM 
	                                    Tests 
                                   WHERE 
	                                    TestAppointmentID = @TestAppointmentID
                                   ORDER BY TestID DESC;";
            int testId = -1;
            
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TestAppointmentID", appointmentId);
                try
                {
                    connection.Open();
                    var returnValue = command.ExecuteScalar();

                    if (returnValue != null)
                        testId = Convert.ToInt32(returnValue);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }

                return testId;
            }
        }
    }
}
