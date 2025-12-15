using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DVLD.EntityLayer;

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

        public static Country GetCountryByID(int countryID)
        {
            string query = @"SELECT * FROM Countries WHERE CountryID = @CountryID";
            Country country;

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CountryID", countryID);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                        return null;

                    country = new Country();
                    country.ID = (int)reader["CountryID"];
                    country.Name = (string)reader["Name"];
                }
            }

            return country;
        }
    }
}