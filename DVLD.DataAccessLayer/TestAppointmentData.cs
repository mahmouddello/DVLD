using System;
using System.Data;
using System.Data.SqlClient;
using DVLD.EntityLayer;
using DVLD.Infrastructure;

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
                Logger.Log(
                    $"Failed to add TestAppointment. LDLA_ID={testAppointment.LdlaId}, TestTypeID={testAppointment.TestTypeId}, Date={testAppointment.AppointmentDate}",
                    System.Diagnostics.EventLogEntryType.Error,
                    ex,
                    nameof(Add));
            }

            return -1;
        }

        public static DataTable GetAllAsTable(int ldlaId, int testTypeId)
        {
            DataTable dt = new DataTable();

            string query = @"SELECT
                                TestAppointmentID,
                                AppointmentDate,
                                PaidFees,
                                IsLocked
                             FROM TestAppointments
                             WHERE LocalDrivingLicenseApplicationID = @ldlaId
                             AND TestTypeID = @TestTypeID";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ldlaId", ldlaId);
                    command.Parameters.AddWithValue("@TestTypeID", testTypeId);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                        dt.Load(reader);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(
                    $"Failed to retrieve TestAppointments. LDLA_ID={ldlaId}, TestTypeID={testTypeId}",
                    System.Diagnostics.EventLogEntryType.Error,
                    ex,
                    nameof(GetAllAsTable));
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
                Logger.Log(
                    $"Failed to retrieve TestAppointment. TestAppointmentID={testAppointmentId}",
                    System.Diagnostics.EventLogEntryType.Error,
                    ex,
                    nameof(GetById));
            }

            return null;
        }

        public static bool ExistsPendingAppointment(int ldlaId, int testTypeId)
        {
            string query = @"SELECT 1
                             FROM TestAppointments
                             WHERE LocalDrivingLicenseApplicationID = @LDLAID
                             AND TestTypeID = @TestTypeID
                             AND IsLocked = 0";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LDLAID", ldlaId);
                    command.Parameters.AddWithValue("@TestTypeID", testTypeId);

                    connection.Open();

                    return command.ExecuteScalar() != null;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(
                    $"Failed to check pending TestAppointment. LDLA_ID={ldlaId}, TestTypeID={testTypeId}",
                    System.Diagnostics.EventLogEntryType.Error,
                    ex,
                    nameof(ExistsPendingAppointment));
            }

            return false;
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
                Logger.Log(
                    $"Failed to update TestAppointment date. TestAppointmentID={testAppointmentId}, NewDate={newAppointmentDate}",
                    System.Diagnostics.EventLogEntryType.Error,
                    ex,
                    nameof(UpdateAppointmentDate));
            }

            return false;
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
                Logger.Log(
                    $"Failed to update TestAppointment lock status. TestAppointmentID={testAppointmentId}, IsLocked={isLocked}",
                    System.Diagnostics.EventLogEntryType.Error,
                    ex,
                    nameof(UpdateLockStatus));
            }

            return false;
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