using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DVLD.DataAccessLayer
{
    public class CountryDataAccess
    {
        public static DataTable GetAllCountries()
        {
            DataTable dt = new DataTable();
            string query = @"SELECT * FROM Countries";

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

        public static DataRow GetCountryByID(int countryID)
        {
            DataTable dt = new DataTable();
            string query = @"SELECT * FROM Countries WHERE CountryID = @CountryID";

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CountryID", countryID);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    dt.Load(reader);
                }
            }

            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }
    }
}