using DVLD.EntityLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using static DVLD.EntityLayer.Application;

namespace DVLD.DataAccessLayer
{
    public static class ApplicationData
    {

        public static DataRow GetById(int id)
        {
            DataTable dt = new DataTable();
            string query = @"SELECT * FROM Applications WHERE ApplicationID = @ApplicationID";

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ApplicationID", id);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    if (reader.HasRows)
                        dt.Load(reader);
                }
            }

            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        public static int InsertNew
        (
            int applicantPersonId,
            DateTime applicationDate,
            int applicationTypeId,
            int applicationstatus,
            DateTime lastStatusDate,
            decimal paidFees,
            int createdByUserId
        )
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

                            SELECT SCOPE_IDENTITY();
                        ";
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ApplicantPersonID", applicantPersonId);
                command.Parameters.AddWithValue("@ApplicationDate", applicationDate);
                command.Parameters.AddWithValue("@ApplicationTypeID", applicationTypeId);
                command.Parameters.AddWithValue("@ApplicationStatus", applicationstatus);
                command.Parameters.AddWithValue("@LastStatusDate", lastStatusDate);
                command.Parameters.AddWithValue("@PaidFees", paidFees);
                command.Parameters.AddWithValue("@CreatedByUserID", createdByUserId);

                connection.Open();
                object result = command.ExecuteScalar();

                return result == null ? -1 : Convert.ToInt32(result);
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

        public static bool UpdateApplication(int applicationId, int createdByUserId, DateTime lastStatusDate)
        {
            string query = @"UPDATE Applications SET 
                                LastStatusDate = @Date,
                                CreatedByUserID = @UserId
                            WHERE ApplicationID = @ApplicationID";

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@ApplicationID", applicationId);
                command.Parameters.AddWithValue("@UserId", createdByUserId);
                command.Parameters.AddWithValue("@Date", lastStatusDate);

                return command.ExecuteNonQuery() > 0;
            }
        }

        public static bool UpdateApplicationStatus(int applicationId, Application.ApplicationStatus status)
        {
            string query = @"UPDATE Applications SET ApplicationStatus = @Status WHERE ApplicationID = @ApplicationID";

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@ApplicationID", applicationId);
                command.Parameters.AddWithValue("@Status", (int)status);

                return command.ExecuteNonQuery() > 0;
            }
        }

        public static bool DeleteById(int applicationId)
        {
            string query = @"DELETE FROM Applications WHERE ApplicationID = @ApplicationID";

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@ApplicationID", applicationId);

                return command.ExecuteNonQuery() > 0;
            }
        }
    }
}
