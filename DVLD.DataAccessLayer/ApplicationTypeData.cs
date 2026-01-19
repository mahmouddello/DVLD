using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD.EntityLayer;

namespace DVLD.DataAccessLayer
{
    public static class ApplicationTypeData
    {
        public static DataTable GetAllApplicationTypes()
        {
            DataTable dt = new DataTable();
            string query = "SELECT * FROM ApplicationTypes";

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    dt.Load(reader);
                }
            }

            return dt;
        }

        public static DataRow GetById(int applicationTypeId)
        {
            DataTable dt = new DataTable();
            string query = @"SELECT * FROM ApplicationTypes WHERE ApplicationTypeID = @ApplicationTypeID";

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ApplicationTypeID", applicationTypeId);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    dt.Load(reader);
                }
            }

            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        public static bool UpdateById(int id, string title, decimal fees)
        {
            string query = @"UPDATE ApplicationTypes
                             SET
                                 ApplicationTypeTitle = @ApplicationTypeTitle,
                                 ApplicationFees = @ApplicationFees
                             WHERE
                                 ApplicationTypeID = @ApplicationTypeID";

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ApplicationTypeID", id);
                command.Parameters.AddWithValue("@ApplicationTypeTitle", title);
                command.Parameters.AddWithValue("@ApplicationFees", fees);

                connection.Open();

                return command.ExecuteNonQuery() > 0;
            }
        }
    }
}
