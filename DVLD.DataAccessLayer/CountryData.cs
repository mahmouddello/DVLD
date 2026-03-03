using DVLD.EntityLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace DVLD.DataAccessLayer
{
    public class CountryData
    {
        public static List<Country> GetAllCountries()
        {
            string query = @"SELECT * FROM Countries";
            List<Country> countries = new List<Country>();

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        countries.Add(MapToEntity(reader));
                }
            }

            return countries;
        }

        public static Country GetById(int countryId)
        {
            string query = "SELECT * FROM Countries WHERE CountryID = @CountryID";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CountryID", countryId);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                            return MapToEntity(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in CountryData.GetById: {ex.Message}");
            }

            return null;
        }

        public static Country GetByName(string countryName)
        {
            string query = @"SELECT * FROM Countries WHERE CountryName = @CountryName";

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CountryName", countryName);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                        return MapToEntity(reader);
                }
            }

            return null;
        }

        public static bool ExistsById(int id)
        {
            string query = "SELECT 1 FROM Countries WHERE CountryID = @Id";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    connection.Open();
                    return command.ExecuteScalar() != null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in CountryData.ExistsById: {ex.Message}");
                return false;
            }
        }

        public static bool ExistsByName(string name)
        {
            string query = "SELECT 1 FROM Countries WHERE CountryName = @Name";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    connection.Open();
                    return command.ExecuteScalar() != null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in CountryData.ExistsByName: {ex.Message}");
                return false;
            }
        }

        private static Country MapToEntity(SqlDataReader reader)
        {
            return new Country(
                id: (int)reader["CountryID"],
                name: (string)reader["CountryName"]
            );
        }
    }
}