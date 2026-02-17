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
    public static class TestAppointmentData
    {
        public static DataTable GetAllTestAppointments()
        {
            DataTable dt = new DataTable();
            string query = "SELECT * FROM TestAppointments";

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

        private static TestAppointment Map(SqlDataReader reader)
        {
            if (!reader.Read())
                return null;

            return new TestAppointment(
                id: (int)reader["TestAppointmentID"],
                testTypeId: (int)reader["TestTypeID"],
                localDrivingApplicationLicenseId: (int)reader["LocalDrivingLicenseApplicationID"],
                appointmentDate: (DateTime)reader["AppointmentDate"],
                paidFees: (decimal)reader["PaidFees"],
                createdByUserId: (int)reader["CreatedByUserID"],
                isLocked: (bool)reader["IsLocked"],
                retakeTestApplicationId: (int)reader["RetakeTestApplicationID"]
            );
        }
    }
}
