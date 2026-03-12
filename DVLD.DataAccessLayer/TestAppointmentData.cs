using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using DVLD.EntityLayer;

namespace DVLD.DataAccessLayer
{
    public class TestAppointmentData
    {
        public static int Add(TestAppointment testAppointment)
        {
            string query = @"INSERT INTO TestAppointments
                             (TestTypeID, LocalDrivingLicenseApplicationID, AppointmentDate,
                              PaidFees, CreatedByUserID, IsLocked, RetakeTestApplicationID)
                             VALUES
                             (@TestTypeID, @LocalDrivingLicenseApplicationID, @AppointmentDate,
                              @PaidFees, @CreatedByUserID, @IsLocked, @RetakeTestApplicationID);
                             SELECT SCOPE_IDENTITY();";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    AddSharedParameters(command, testAppointment);
                    connection.Open();

                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        return Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in TestAppointmentData.Add: {ex.Message}");
            }

            return -1;
        }
        
        public static DataTable GetAllAsTable(int _ldlaId, int testTypeId)
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
                                LocalDrivingLicenseApplicationID = @_ldlaId
                                AND TestTypeID = @TestTypeID";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@_ldlaId", _ldlaId);
                    command.Parameters.AddWithValue("@TestTypeID", testTypeId);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                        dt.Load(reader);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in TestAppointmentData.GetAllAsTable: {ex.Message}");
                return new DataTable();
            }

            return dt;
        }

        public static TestAppointment GetById(int testAppointmentId)
        {
            string query = "SELECT * FROM TestAppointments WHERE TestAppointmentID = @TestAppointmentID";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TestAppointmentID", testAppointmentId);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                        if (reader.Read())
                            return MapToEntity(reader);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in TestAppointmentData.GetById: {ex.Message}");
            }

            return null;
        }

        public static bool ExistsPendingAppointment(int _ldlaId, int testTypeId)
        {
            string query = @"SELECT 1
                            FROM TestAppointments
                            WHERE
                                LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID
                                AND TestTypeID = @TestTypeID
                                AND IsLocked = 0";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", _ldlaId);
                    command.Parameters.AddWithValue("@TestTypeID", testTypeId);
                    connection.Open();

                    return command.ExecuteScalar() != null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in TestAppointmentData.ExistsActiveAppointment: {ex.Message}");
                return false;
            }
        }

        public static bool UpdateAppointmentDate(int testAppointmentId, DateTime newAppointmentDate)
        {
            string query = @"UPDATE TestAppointments
                             SET AppointmentDate = @AppointmentDate
                             WHERE TestAppointmentID = @TestAppointmentID";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TestAppointmentID", testAppointmentId);
                    command.Parameters.AddWithValue("@AppointmentDate", newAppointmentDate);
                    connection.Open();

                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in TestAppointmentData.Update: {ex.Message}");
                return false;
            }
        }

        public static bool UpdateLockStatus(int testAppointmentId, bool isLocked)
        {
            string query = @"UPDATE TestAppointments
                             SET IsLocked = @IsLocked
                             WHERE TestAppointmentID = @TestAppointmentID";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TestAppointmentID", testAppointmentId);
                    command.Parameters.AddWithValue("@IsLocked", isLocked);
                    connection.Open();

                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in TestAppointmentData.UpdateLockStatus: {ex.Message}");
                return false;
            }
        }

        private static TestAppointment MapToEntity(SqlDataReader reader)
        {
            return new TestAppointment(
                id: Convert.ToInt32(reader["TestAppointmentID"]),
                testTypeId: Convert.ToInt32(reader["TestTypeID"]),
                _ldlaId: Convert.ToInt32(reader["LocalDrivingLicenseApplicationID"]),
                appointmentDate: Convert.ToDateTime(reader["AppointmentDate"]),
                paidFees: Convert.ToDecimal(reader["PaidFees"]),
                createdByUserId: Convert.ToInt32(reader["CreatedByUserID"]),
                isLocked: Convert.ToBoolean(reader["IsLocked"]),
                retakeTestApplicationId: reader["RetakeTestApplicationID"] != DBNull.Value
                    ? Convert.ToInt32(reader["RetakeTestApplicationID"])
                    : -1
            );
        }

        private static void AddSharedParameters(SqlCommand command, TestAppointment testAppointment)
        {
            command.Parameters.AddWithValue("@TestTypeID", testAppointment.TestTypeId);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", testAppointment.LdlaId);
            command.Parameters.AddWithValue("@AppointmentDate", testAppointment.AppointmentDate);
            command.Parameters.AddWithValue("@PaidFees", testAppointment.PaidFees);
            command.Parameters.AddWithValue("@CreatedByUserID", testAppointment.CreatedByUserId);
            command.Parameters.AddWithValue("@IsLocked", testAppointment.IsLocked);

            var retakeParam = command.Parameters.Add("@RetakeTestApplicationID", SqlDbType.Int);
            retakeParam.Value = testAppointment.HasRetakeApplication
                ? (object)testAppointment.RetakeTestApplicationId
                : DBNull.Value;
        }
    }
}
