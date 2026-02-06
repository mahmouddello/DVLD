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
