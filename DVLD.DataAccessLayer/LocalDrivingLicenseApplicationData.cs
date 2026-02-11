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
    }
}
