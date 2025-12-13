using System;

namespace DVLD.DataAccessLayer
{
    internal class DataAccessSettings
    {
        private static string _connectionString;
        public static string ConnectionString
        {
            get
            {
                if (_connectionString == null)
                    _connectionString = Environment.GetEnvironmentVariable("connection");

                return _connectionString;
            }
        }
    }
}
