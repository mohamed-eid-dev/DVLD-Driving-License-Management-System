
using DVLD.Model.Countries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.DataAccess
{

    public static class CountriesRepository
    {

        // Private Method
        private static Country MapCountry(SqlDataReader reader)
        {
            return new Country
            {
                CountryID = Convert.ToInt32(reader["CountryID"]),
                CountryName = reader["CountryName"] as string
            };
        }

        // Public Method
        public static List<Country> GetAll()
        {
       
            List<Country> countries = new List<Country>();
   
                using (SqlConnection connection = new SqlConnection(DBConnection.ConnectionString))
                {
                    string query = @"SELECT CountryID, CountryName FROM Countries";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                    connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                countries.Add( MapCountry(reader));
                            }

                        }
                    }

                }
                    return countries;

        }

        public static  Country GetByID(int CountryID)
        {
        
            Country country = null;
   
                using (SqlConnection connection = new SqlConnection(DBConnection.ConnectionString))
                {
                    string query = @"SELECT CountryID, CountryName FROM Countries 
                                           Where CountryID = @CountryID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@CountryID", SqlDbType.Int).Value = CountryID;

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.Read())
                            {
                                country = MapCountry(reader);
                            }

                        }
                    }

                }
                return country;

        }

        public static Country GetByName(string CountryName)
        {
            Country country = null;

   
                using (SqlConnection connection = new SqlConnection(DBConnection.ConnectionString))
                {
                    string query = @"SELECT CountryID, CountryName FROM Countries 
                                           Where CountryName = @CountryName";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@CountryName", SqlDbType.NVarChar).Value = CountryName;

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.Read())
                            {
                                country = MapCountry(reader);
                            }

                        }
                    }

                }
                return country;
        }


    }
}
