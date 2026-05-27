using System;
using System.Configuration;

namespace DVLD.DataAccessLayer
{
    internal class DataAccessSettings
    {
        private static string _connectionString;
        static string GetConnectionStringByName(string name)
        {
            // Look for the name in the connectionStrings section.
            ConnectionStringSettings settings =
                ConfigurationManager.ConnectionStrings[name];

            // If found, return the connection string (otherwise return null)
            return settings?.ConnectionString;
        }

        public static string ConnectionString
        {
            get
            {
                if (_connectionString == null)
                    _connectionString = GetConnectionStringByName("DVLD");

                return _connectionString;
            }
        }
    }
}
