using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using DVLD.EntityLayer;

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
                Debug.WriteLine($"Error in LDLAData.Add: {ex.Message}");
            }

            return -1;
        }

        public static DataTable GetAllAsTable()
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand("SELECT * FROM LDLA_View", connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                        dt.Load(reader);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in LDLAData.GetAllAsTable: {ex.Message}");
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
                Debug.WriteLine($"Error in LDLAData.GetById: {ex.Message}");
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
                Debug.WriteLine($"Error in LDLAData.GetByMainApplicationId: {ex.Message}");
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
                Debug.WriteLine($"Error in LDLAData.UpdateLicenseClass: {ex.Message}");
                return false;
            }
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
                Debug.WriteLine($"Error in LDLAData.Delete: {ex.Message}");
                return false;
            }
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
                Debug.WriteLine($"Error in LDLAData.ExistsActiveApplicationForClass: {ex.Message}");
                return false;
            }
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
