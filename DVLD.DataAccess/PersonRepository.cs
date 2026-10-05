    using DVLD.DataAccess;
    using DVLD.Model;
    using DVLD.Model.Countries;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Runtime.Remoting.Messaging;
    using System.Text;
    using System.Threading.Tasks;
    namespace DVLD.DataAccessLayer
    {

    public static class PersonRepository 
    {
        public static Person MapPerson(SqlDataReader reader ,ref bool isFound)
            {
            Person person = new Person();

                isFound = true;
                person.PersonID = Convert.ToInt32(reader["PersonID"]);
                person.NationalNo = reader["NationalNo"] as string;
                person.FirstName = reader["FirstName"] as string;
                    person.SecondName = reader["SecondName"] as string;

                if (reader["ThirdName"] != DBNull.Value)
                    person.ThirdName = reader["ThirdName"] as string;
                else
                {
                    person.ThirdName = "";
                }
                    person.LastName = reader["LastName"] as string;
                    person.Gender = (enGenderType)Convert.ToInt16(reader["Gender"]);
                    person.DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]);
                    person.Phone = reader["Phone"] as string;
                if (reader["Email"]!= DBNull.Value)
                    person.Email = reader["Email"] as string;
                else
                {
                    person.Email = "";
                }    
                person.Address = reader["Address"] as string;

                    person.CountryInfo = new Country
                    {
                        CountryID = Convert.ToInt32(reader["NationalityCountryID"]),
                        CountryName = reader["CountryName"] as string
                    };
                if (reader["ImagePath"] != DBNull.Value)
                    person.ImagePath = reader["ImagePath"] as string;
                else
                {
                    person.ImagePath = "";
                }


                    person.PersonIsUpdate();
           
                return person;
            }
        public static int Add(Person person)
        {
            using (SqlConnection connection =
                   new SqlConnection(DBConnection.ConnectionString))
            using (SqlCommand command = new SqlCommand("Person.sp_CreatePerson", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@NationalNo", SqlDbType.NVarChar, 20).Value = person.NationalNo;
                command.Parameters.Add("@FirstName", SqlDbType.NVarChar, 20).Value = person.FirstName;
                command.Parameters.Add("@SecondName", SqlDbType.NVarChar, 50).Value = person.SecondName;
                command.Parameters.Add("@ThirdName", SqlDbType.NVarChar, 20).Value = person.ThirdName;
                command.Parameters.Add("@LastName", SqlDbType.NVarChar, 20).Value = person.LastName;
                command.Parameters.Add("@DateOfBirth", SqlDbType.Date).Value = person.DateOfBirth;
                command.Parameters.Add("@Gender", SqlDbType.TinyInt).Value = (short)person.Gender;
                command.Parameters.Add("@Address", SqlDbType.NVarChar, 500).Value = person.Address;
                command.Parameters.Add("@Phone", SqlDbType.NVarChar, 20).Value = person.Phone;
                command.Parameters.Add("@Email", SqlDbType.NVarChar, 50).Value =
                    (object)person.Email ?? DBNull.Value;
                command.Parameters.Add("@NationalityCountryID", SqlDbType.Int).Value =
                    person.CountryInfo.CountryID;
                
                command.Parameters.Add("@ImagePath", SqlDbType.NVarChar, 250).Value =
                    (object)person.ImagePath ?? DBNull.Value;

                SqlParameter outputParameter =
                    command.Parameters.Add("@NewPersonID", SqlDbType.Int);

                outputParameter.Direction = ParameterDirection.Output;

                connection.Open();
                command.ExecuteNonQuery();

                if (outputParameter.Value == DBNull.Value)
                    return -1;

                int newPersonID = Convert.ToInt32(outputParameter.Value);

                return newPersonID > 0 ? newPersonID : -1;

            }
        }
        public static bool Update(Person person)
        {

            using (SqlConnection connection =
                   new SqlConnection(DBConnection.ConnectionString))
            using (SqlCommand command = new SqlCommand("Person.sp_UpdatePerson", connection))
            {
                command.Parameters.Add("@PersonID", SqlDbType.Int).Value = person.PersonID;
                command.Parameters.Add("@NationalNo", SqlDbType.VarChar, 50).Value = person.NationalNo;
                command.Parameters.Add("@FirstName", SqlDbType.VarChar, 50).Value = person.FirstName;
                command.Parameters.Add("@SecondName", SqlDbType.VarChar, 50).Value = person.SecondName;
                command.Parameters.Add("@ThirdName", SqlDbType.VarChar, 50).Value = person.ThirdName;
                command.Parameters.Add("@LastName", SqlDbType.VarChar, 50).Value = person.LastName;
                command.Parameters.Add("@DateOfBirth", SqlDbType.Date).Value = person.DateOfBirth;
                command.Parameters.Add("@Gender", SqlDbType.Int).Value = (int)person.Gender;
                command.Parameters.Add("@Address", SqlDbType.VarChar, 250).Value = person.Address;
                command.Parameters.Add("@Phone", SqlDbType.VarChar, 30).Value = person.Phone;
                command.Parameters.Add("@Email", SqlDbType.VarChar, 100).Value =
                    (object)person.Email ?? DBNull.Value;
                command.Parameters.Add("@NationalityCountryID", SqlDbType.Int).Value =
                    person.CountryInfo.CountryID;
                command.Parameters.Add("@ImagePath", SqlDbType.VarChar, 250).Value =
                    (object)person.ImagePath ?? DBNull.Value;
                SqlParameter returnParameter =
                    command.Parameters.Add("@ReturnValue", SqlDbType.Int);

                command.CommandType = CommandType.StoredProcedure;
                returnParameter.Direction = ParameterDirection.ReturnValue;

                connection.Open();

                command.ExecuteNonQuery();

                int rowsAffected = Convert.ToInt32(returnParameter.Value);

                return rowsAffected > 0;
            }
        }
        public static bool Delete(int personID)
        {
            const string query = @"DELETE FROM People WHERE PersonID = @PersonID;";

            using (SqlConnection connection =
                   new SqlConnection(DBConnection.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@PersonID", SqlDbType.Int).Value = personID;

                connection.Open();

                return command.ExecuteNonQuery() > 0;
            }
        }
        public static Person GetById(int personID , ref bool isFound)
        {
                    isFound = false;

            const string query = @"
                                    SELECT p.*, c.CountryName
                                    FROM People p
                                    INNER JOIN Countries c
                                        ON c.CountryID = p.NationalityCountryID
                                    WHERE p.PersonID = @PersonID;";

            using (SqlConnection connection =
                   new SqlConnection(DBConnection.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@PersonID", SqlDbType.Int).Value = personID;

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                        return MapPerson(reader,ref isFound);

                    return null;
                }
            }
        }
        public static Person GetByNationalNo(string nationalNo, ref bool isFound )
        {
            isFound = false;

            const string query = @"
        SELECT p.*, c.CountryName
        FROM People p
        INNER JOIN Countries c
            ON c.CountryID = p.NationalityCountryID
        WHERE p.NationalNo = @NationalNo;";

            using (SqlConnection connection =
                   new SqlConnection(DBConnection.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@NationalNo", SqlDbType.VarChar, 50).Value = nationalNo;

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                        return MapPerson(reader,ref isFound);

                    return null;
                }
            }
        }
        public static List<Person> GetAll(ref bool isFound)
            {
                 isFound = false;

                List<Person> people = new List<Person>();

                const string query = @"
            SELECT p.*, c.CountryName
            FROM People p
            INNER JOIN Countries c
                ON c.CountryID = p.NationalityCountryID
            ORDER BY PersonID ASC, FirstName ASC";

                using (SqlConnection connection =
                       new SqlConnection(DBConnection.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            people.Add(MapPerson(reader,ref isFound));
                        }
                    }
                }

                return people;
            }
        public static bool Exists(int personID)
        {
            const string query =
                @"SELECT 1 FROM People WHERE PersonID = @PersonID;";

            using (SqlConnection connection =
                   new SqlConnection(DBConnection.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@PersonID", SqlDbType.Int).Value = personID;

                connection.Open();

                return command.ExecuteScalar() != null;
            }
        }
        public static bool Exists(string nationalNo)
        {
            const string query =
                @"SELECT 1 FROM People WHERE NationalNo = @NationalNo;";

            using (SqlConnection connection =
                   new SqlConnection(DBConnection.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@NationalNo", SqlDbType.VarChar, 50).Value = nationalNo;

                connection.Open();

                return command.ExecuteScalar() != null;
            }
        }

    }
}
