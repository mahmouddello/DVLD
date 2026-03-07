using DVLD.EntityLayer;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace DVLD.DataAccessLayer
{
    public static class TestTypeData
    {
        public static DataTable GetAllAsTable()
        {
            DataTable dt = new DataTable();
            string query = @"SELECT * FROM TestTypes";

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
                Debug.WriteLine($"Error in TestTypeData.GetAllAsTable: {ex.Message}");
                return new DataTable();
            }

            return dt;
        }

        public static TestType GetById(int testTypeId)
        {
            string query = @"SELECT * FROM TestTypes WHERE TestTypeID = @TestTypeID";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TestTypeID", testTypeId);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                        if (reader.Read())
                            return MapToEntity(reader);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in TestTypeData.GetById: {ex.Message}");
            }

            return null;
        }

        public static bool Update(TestType testType)
        {
            string query = @"UPDATE TestTypes SET
                                 TestTypeTitle       = @Title,
                                 TestTypeDescription = @Description,
                                 TestTypeFees        = @Fees
                             WHERE
                                 TestTypeID = @TestTypeID";
            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    AddSharedParameters(command, testType);
                    command.Parameters.AddWithValue("@TestTypeID", testType.Id);
                    connection.Open();

                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in TestTypeData.Update: {ex.Message}");
                return false;
            }
        }

        private static TestType MapToEntity(SqlDataReader reader)
        {
            return new TestType(
               type: (enTestType)reader["TestTypeID"],
               title: (string)reader["TestTypeTitle"],
               description: (string)reader["TestTypeDescription"],
               fees: (decimal)reader["TestTypeFees"]
           );
        }

        private static void AddSharedParameters(SqlCommand command, TestType testType)
        {
            command.Parameters.AddWithValue("@Title", testType.Title);
            command.Parameters.AddWithValue("@Description", testType.Description);
            command.Parameters.AddWithValue("@Fees", testType.Fees);
        }
    }
}
