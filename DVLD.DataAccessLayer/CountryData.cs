using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DVLD.EntityLayer;
using DVLD.Infrastructure;

namespace DVLD.DataAccessLayer
{
    public class CountryData
    {
        public static List<Country> GetAllCountries()
        {
            string query = @"SELECT * FROM Countries";
            List<Country> countries = new List<Country>();

            try
            {
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
            catch (Exception ex)
            {
                Logger.Log(
                    "Failed to retrieve Countries list.",
                    System.Diagnostics.EventLogEntryType.Error,
                    ex,
                    nameof(GetAllCountries));

                return new List<Country>();
            }
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

                Logger.Log(
                    $"Country not found. CountryID={countryId}",
                    System.Diagnostics.EventLogEntryType.Warning,
                    null,
                    nameof(GetById));

                return null;
            }
            catch (Exception ex)
            {
                Logger.Log(
                    $"Failed to retrieve Country. CountryID={countryId}",
                    System.Diagnostics.EventLogEntryType.Error,
                    ex,
                    nameof(GetById));

                return null;
            }
        }

        public static Country GetByName(string countryName)
        {
            string query = @"SELECT * FROM Countries WHERE CountryName = @CountryName";

            try
            {
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

                Logger.Log(
                    $"Country not found. CountryName={countryName}",
                    System.Diagnostics.EventLogEntryType.Warning,
                    null,
                    nameof(GetByName));

                return null;
            }
            catch (Exception ex)
            {
                Logger.Log(
                    $"Failed to retrieve Country. CountryName={countryName}",
                    System.Diagnostics.EventLogEntryType.Error,
                    ex,
                    nameof(GetByName));

                return null;
            }
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

                    bool exists = command.ExecuteScalar() != null;

                    if (!exists)
                    {
                        Logger.Log(
                            $"Country does not exist. CountryID={id}",
                            System.Diagnostics.EventLogEntryType.Warning,
                            null,
                            nameof(ExistsById));
                    }

                    return exists;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(
                    $"Failed to check Country existence. CountryID={id}",
                    System.Diagnostics.EventLogEntryType.Error,
                    ex,
                    nameof(ExistsById));

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

                    bool exists = command.ExecuteScalar() != null;

                    if (!exists)
                    {
                        Logger.Log(
                            $"Country does not exist. CountryName={name}",
                            System.Diagnostics.EventLogEntryType.Warning,
                            null,
                            nameof(ExistsByName));
                    }

                    return exists;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(
                    $"Failed to check Country existence. CountryName={name}",
                    System.Diagnostics.EventLogEntryType.Error,
                    ex,
                    nameof(ExistsByName));

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