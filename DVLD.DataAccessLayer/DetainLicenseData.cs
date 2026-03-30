using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using DVLD.EntityLayer;

namespace DVLD.DataAccessLayer
{
    public class DetainLicenseData
    {
        public static int Add(DetainLicense detainLicense)
        {
            string query = @"INSERT INTO DetainedLicenses VALUES 
                            (@licenseId, @detainDate, @fineFees, @CreatedByUserId,
                            @IsReleased, @ReleaseDate, @ReleasedByUserId, @ReleaseApplicationId);
                            SELECT SCOPE_IDENTITY();";
            try
            {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    AddSharedParams(cmd, detainLicense);

                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        return Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error is DetainLicenseData.GetAllAsTable: {ex.Message}");
            }

            return -1;
        }

        public static DataTable GetAllAsTable()
        {
            string query = "SELECT * FROM DetainedRecords_View";
            DataTable table = new DataTable();

            try
            {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                        table.Load(reader);
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"Error is DetainLicenseData.GetAllAsTable: {ex.Message}");
            }

            return table;
        }

        public static DetainLicense GetById(int id)
        {
            string query = "SELECT * FROM DetainedLicenses WHERE DetainID = @Id";

            try
            {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                        if (reader.Read())
                            return MapToEntity(reader);
                }
            }

            catch (Exception ex)
            {
                Debug.WriteLine($"Error is DetainLicenseData.GetById: {ex.Message}");
            }

            return null;
        }

        public static DetainLicense GetByLicenseId(int id)
        {
            string query = "SELECT * FROM DetainedLicenses WHERE LicenseID = @Id AND IsReleased = 0";

            try
            {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                        if (reader.Read())
                            return MapToEntity(reader);
                }
            }

            catch (Exception ex)
            {
                Debug.WriteLine($"Error is DetainLicenseData.GetByLicenseId: {ex.Message}");
            }

            return null;
        }

        public static bool ExistsById(int id)
        {
            string query = "SELECT * FROM DetainedLicenses WHERE DetainID = @Id";

            try
            {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    conn.Open();

                    return cmd.ExecuteScalar() != null;
                }
            }

            catch (Exception ex)
            {
                Debug.WriteLine($"Error is DetainLicenseData.ExistsById: {ex.Message}");
            }

            return false;
        }

        // This method can indicate if a license is detained and not released
        public static bool ExistsByLicenseId(int id)
        {
            string query = @"SELECT * FROM DetainedLicenses WHERE LicenseID = @Id AND IsReleased = 0";

            try
            {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    conn.Open();

                    return cmd.ExecuteScalar() != null;
                }
            }

            catch (Exception ex)
            {
                Debug.WriteLine($"Error is DetainLicenseData.ExistsByLicenseId: {ex.Message}");
            }

            return false;
        }

        public static bool UpdateReleaseInfo(DetainLicense record)
        {
            string query = @"UPDATE DetainedLicenses SET
                            IsReleased = 1,
                            ReleaseDate = @ReleasedDate,
                            ReleasedByUserID = @ReleasedByUserID,
                            ReleaseApplicationID = @ReleaseApplicationID
                           WHERE DetainID = @DetainID";
            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DetainID", record.Id);
                    command.Parameters.AddWithValue("@ReleasedDate", record.ReleasedDate);
                    command.Parameters.AddWithValue("@ReleasedByUserID", record.ReleasedByUserId);
                    command.Parameters.AddWithValue("@ReleaseApplicationID", record.ReleaseApplicationId);

                    connection.Open();

                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error is DetainLicenseData.UpdateReleaseInfo: {ex.Message}");
            }

            return false;
        }

        // ---- Helpers ----
        private static T GetValue<T>(SqlDataReader reader, string column, T defaultValue = default)
        {
            return reader[column] == DBNull.Value ? defaultValue : (T)Convert.ChangeType(reader[column], typeof(T));
        }

        private static DetainLicense MapToEntity(SqlDataReader reader)
        {
            return new DetainLicense(
                id: GetValue<int>(reader, "DetainID"),
                licenseId: GetValue<int>(reader, "LicenseID"),
                detainDate: GetValue<DateTime>(reader, "DetainDate"),
                fineFees: GetValue<decimal>(reader, "FineFees"),
                createdByUserId: GetValue<int>(reader, "CreatedByUserID"),
                isReleased: GetValue<bool>(reader, "IsReleased"),
                releasedDate: GetValue<DateTime>(reader, "ReleaseDate"),      // returns DateTime.MinValue if null
                releasedByUserId: GetValue<int>(reader, "ReleasedByUserID", -1),   // returns -1 if null
                releaseApplicationId: GetValue<int>(reader, "ReleaseApplicationID", -1)
            );
        }

        private static void AddSharedParams(SqlCommand cmd, DetainLicense detainLicense)
        {
            cmd.Parameters.AddWithValue("@licenseId", detainLicense.LicenseId);
            cmd.Parameters.AddWithValue("@detainDate", detainLicense.DetainDate);
            cmd.Parameters.AddWithValue("@fineFees", detainLicense.FineFees);
            cmd.Parameters.AddWithValue("@createdByUserId", detainLicense.CreatedByUserId);
            cmd.Parameters.AddWithValue("@IsReleased", detainLicense.IsReleased);
            cmd.Parameters.AddWithValue("@ReleaseDate", detainLicense.ReleasedDate == DateTime.MinValue ? (object)DBNull.Value : detainLicense.ReleasedDate);
            cmd.Parameters.AddWithValue("@ReleasedByUserId", detainLicense.ReleasedByUserId == -1 ? (object)DBNull.Value : detainLicense.ReleasedByUserId);
            cmd.Parameters.AddWithValue("@ReleaseApplicationId", detainLicense.ReleaseApplicationId == -1 ? (object)DBNull.Value : detainLicense.ReleaseApplicationId);
        }


    }
}
