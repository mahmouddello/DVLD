using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.DataAccessLayer
{
    public static class LocalDrivingLicenseApplicationData
    {
        public static DataTable GetAllApplications()
        {
            string query = @"SELECT * FROM LDLA_View";
            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    if (dataReader.HasRows)
                        dataTable.Load(dataReader);
                }
            }

            return dataTable;
        }

        public static DataRow GetById(int ldlaId)
        {
            string query = @"SELECT * FROM LocalDrivingLicenseApplications WHERE LocalDrivingLicenseApplicationID = @ID";
            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@ID", ldlaId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                        dataTable.Load(reader);
                }
            }

            return dataTable.Rows.Count > 0 ? dataTable.Rows[0] : null;
        }

        public static int GetPassedTestCountById(int ldlaId)
        {
            string query = @"SELECT
                                COUNT(CASE WHEN t.TestResult = 1 THEN 1 END) AS PassedTests
                            FROM LocalDrivingLicenseApplications AS ldla

                            LEFT JOIN TestAppointments AS ta
                                ON ta.LocalDrivingLicenseApplicationID = ldla.LocalDrivingLicenseApplicationID

                            LEFT JOIN Tests AS t
                                ON t.TestAppointmentID = ta.TestAppointmentID

                            WHERE ldla.LocalDrivingLicenseApplicationID = @ldlaId

                            GROUP BY ldla.LocalDrivingLicenseApplicationID;";

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@ldlaId", ldlaId);


                object result = command.ExecuteScalar();

                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        public static bool DeleteByMainApplicationId(int mainApplicationId)
        {
            string query = @"DELETE FROM LocalDrivingLicenseApplications WHERE ApplicationID = @ApplicationID";

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ApplicationID", mainApplicationId);
                connection.Open();

                return command.ExecuteNonQuery() > 0;
            }
        }

        public static int InsertNew(int mainApplicationId, int licenseClassId)
        {
            string query = @"INSERT INTO 
                                LocalDrivingLicenseApplications (ApplicationID, LicenseClassID)
                                VALUES (@ApplicationID, @LicenseClassID);
                             SELECT SCOPE_IDENTITY();";

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ApplicationID", mainApplicationId);
                command.Parameters.AddWithValue("@LicenseClassID", licenseClassId);

                connection.Open();
                object result = command.ExecuteScalar();

                return result == null ? -1 : Convert.ToInt32(result);
            }
        }

        public static bool UpdateLicenseClass(int ldlaId, int licenseClassId)
        {
            string query = @"UPDATE LocalDrivingLicenseApplications
                     SET LicenseClassID = @LicenseClassID
                     WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", ldlaId);
                command.Parameters.AddWithValue("@LicenseClassID", licenseClassId);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }
    }
}
