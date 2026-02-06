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
    public static class LicenseClassData
    {
        public static DataTable GetAllLicenseClasses()
        {
            DataTable dt = new DataTable();
            string query = @"SELECT * FROM LicenseClasses";

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

        public static DataRow GetById(int licenseClassId)
        {
            DataTable dt = new DataTable();
            string query = @"SELECT * FROM LicenseClasses WHERE LicenseClassID = @LicenseClassID";

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@LicenseClassID", licenseClassId);
                    connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {

                    if (reader.Read())
                        dt.Load(reader);
                }
            }

            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }
    }
}
