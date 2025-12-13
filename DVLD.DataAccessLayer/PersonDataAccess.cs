using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLD.DataAccessLayer
{
    public class PersonDataAccess
    {
        public static DataTable GetAllPeople()
        {
            DataTable dt = new DataTable();
            string query = @"SELECT 
                                p.PersonID,
                                p.NationalNo,
                                p.FirstName,
                                p.SecondName,
                                p.ThirdName,
                                p.LastName,
                                p.DateOfBirth,
                                Gender =
                                    CASE 
                                        WHEN p.Gendor = 0 THEN 'Male'
                                        WHEN p.Gendor = 1 THEN 'Female'
                                        ELSE 'Unknown'
                                    END,
                                c.CountryName AS Nationality,
                                p.Phone,
                                p.Email
                            FROM People p
                            INNER JOIN Countries c
                                ON c.CountryID = p.NationalityCountryID;";

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
    }
}
