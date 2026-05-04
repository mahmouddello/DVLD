using System;
using System.Data;
using System.Data.SqlClient;
using DVLD.EntityLayer;
using DVLD.Infrastructure;

namespace DVLD.DataAccessLayer
{
    public class LDLAData
    {
        public static int Add(int mainAppId, int licenseClassId)
        {
            string query = @"INSERT INTO LocalDrivingLicenseApplications 
                            (ApplicationID, LicenseClassID)
                            VALUES (@ApplicationId, @LicenseClassId);
                            SELECT SCOPE_IDENTITY();";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationId", mainAppId);
                    command.Parameters.AddWithValue("@LicenseClassId", licenseClassId);

                    connection.Open();

                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        return Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(
                    $"Failed to add LDLA record. ApplicationID={mainAppId}, LicenseClassID={licenseClassId}",
                    System.Diagnostics.EventLogEntryType.Error,
                    ex,
                    nameof(Add));
            }

            return -1;
        }

        public static DataTable GetAllAsTable()
        {
            DataTable dt = new DataTable();
            string query = "SELECT * FROM LDLA_View ORDER BY LocalDrivingLicenseApplicationID DESC";

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
                    "Failed to retrieve LDLA list.",
                    System.Diagnostics.EventLogEntryType.Error,
                    ex,
                    nameof(GetAllAsTable));

                return new DataTable();
            }

            return dt;
        }

        public static LDLA GetById(int id)
        {
            string query = "SELECT * FROM LocalDrivingLicenseApplications WHERE LocalDrivingLicenseApplicationID = @Id";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                        if (reader.Read())
                            return MapToEntity(reader);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(
                    $"Failed to retrieve LDLA record. LDLA_ID={id}",
                    System.Diagnostics.EventLogEntryType.Error,
                    ex,
                    nameof(GetById));
            }

            return null;
        }

        public static LDLA GetByApplicationId(int mainAppId)
        {
            string query = "SELECT * FROM LocalDrivingLicenseApplications WHERE ApplicationID = @ApplicationID";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationID", mainAppId);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                        if (reader.Read())
                            return MapToEntity(reader);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(
                    $"Failed to retrieve LDLA by ApplicationID={mainAppId}",
                    System.Diagnostics.EventLogEntryType.Error,
                    ex,
                    nameof(GetByApplicationId));
            }

            return null;
        }

        public static bool UpdateLicenseClass(int id, int licenseClassId)
        {
            string query = @"UPDATE LocalDrivingLicenseApplications
                            SET LicenseClassID = @LicenseClassId
                            WHERE LocalDrivingLicenseApplicationID = @Id";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@LicenseClassId", licenseClassId);

                    connection.Open();

                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(
                    $"Failed to update LDLA LicenseClass. LDLA_ID={id}, LicenseClassID={licenseClassId}",
                    System.Diagnostics.EventLogEntryType.Error,
                    ex,
                    nameof(UpdateLicenseClass));
            }

            return false;
        }

        public static bool Delete(int id)
        {
            string query = "DELETE FROM LocalDrivingLicenseApplications WHERE LocalDrivingLicenseApplicationID = @Id";

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
                Logger.Log(
                    $"Failed to delete LDLA record. LDLA_ID={id}",
                    System.Diagnostics.EventLogEntryType.Error,
                    ex,
                    nameof(Delete));
            }

            return false;
        }

        public static bool ExistsActiveApplicationForClass(int personId, int licenseClassId)
        {
            string query = @"SELECT TOP 1 1
                            FROM Applications A
                            INNER JOIN LocalDrivingLicenseApplications LA
                                ON A.ApplicationID = LA.ApplicationID
                            WHERE A.ApplicantPersonID = @PersonId
                            AND LA.LicenseClassID = @LicenseClassId
                            AND A.ApplicationStatus IN (1, 3)";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonId", personId);
                    command.Parameters.AddWithValue("@LicenseClassId", licenseClassId);

                    connection.Open();
                    return command.ExecuteScalar() != null;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(
                    $"Failed to check active LDLA existence. PersonID={personId}, LicenseClassID={licenseClassId}",
                    System.Diagnostics.EventLogEntryType.Error,
                    ex,
                    nameof(ExistsActiveApplicationForClass));
            }

            return false;
        }

        private static LDLA MapToEntity(SqlDataReader reader)
        {
            return new LDLA(
                id: (int)reader["LocalDrivingLicenseApplicationID"],
                mainApplicationId: (int)reader["ApplicationID"],
                licenseClassId: (int)reader["LicenseClassID"]
            );
        }
    }
}