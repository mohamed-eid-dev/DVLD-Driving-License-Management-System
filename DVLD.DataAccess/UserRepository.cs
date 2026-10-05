using DVLD.Model;
using DVLD.Model.Users;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.DataAccess
{
    public static class UserRepository
    {
        
        private static User MapUser(SqlDataReader reader)
        {
            User user = new User();
            user.UserId = Convert.ToInt32( reader["UserID"]);
            user.PersonId = Convert.ToInt32( reader["PersonID"]);
            user.UserName =  reader["UserName"].ToString();
            user.UserMode = Model.People.enMode.Update;
            user.IsActive = Convert.ToBoolean(reader["IsActive"]);
            return user;
        }
       
        public static int Add(User user ,string Password)
        {

            using (var connection = new SqlConnection(DBConnection.ConnectionString))
            using (var command = new SqlCommand("Users.sp_CreateUser", connection)) { 

                command.Parameters.Add("@UserName", System.Data.SqlDbType.NVarChar, 20).Value = user.UserName;
                command.Parameters.Add("@HashPassword", System.Data.SqlDbType.NVarChar, 100).Value = Password;
                command.Parameters.Add("@PersonID", System.Data.SqlDbType.Int).Value = user.PersonId;


                SqlParameter outputParameter =  command.Parameters.Add("@NewUserID", SqlDbType.Int);

                outputParameter.Direction = ParameterDirection.Output;

                command.CommandType = System.Data.CommandType.StoredProcedure;
               
                connection.Open();
                command.ExecuteNonQuery();

                if (outputParameter.Value == DBNull.Value)
                    return -1;

                return  Convert.ToInt32(outputParameter.Value);
               
            }
        }

        
        public static List<User> GetAll(ref bool isFound)
        {
            isFound = false;
            string query = @"select UserID,PersonID,UserName,IsActive
                                from Users
                                order by Users.UserID , Users.UserName";
            List<User> users = new List<User>();

            using (var connection = new SqlConnection(DBConnection.ConnectionString)) 
                using(var command = new SqlCommand(query, connection))
            {

               connection.Open();
              var reader =  command.ExecuteReader();

                while (reader.Read())
                {
                    users.Add(MapUser(reader));
                }
                isFound = users.Count > 0; 
                return users;
            }
        }

        /// ///////////////////////////////////////////////////////////
        public static bool Update(User user)
        {
            using (var connection = new SqlConnection(DBConnection.ConnectionString))
            using (var command = new SqlCommand("Users.sp_UpdateUser", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@UserID", SqlDbType.Int).Value = user.UserId;
                command.Parameters.Add("@UserName", SqlDbType.NVarChar, 20).Value = user.UserName;
                command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = user.IsActive;

                SqlParameter returnParameter = command.Parameters.Add("@ReturnValue", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.Output;

                connection.Open();
                command.ExecuteNonQuery();

                int rowsAffected =
                    Convert.ToInt32(returnParameter.Value);

                return rowsAffected > 0;
            }
        }

        public static bool Delete(int userID)
        {

            return false;
        }

        public static User Find(int userID)
        {
            return null;
        }

        public static bool ChangePassword(string oldPassword,string newPassword)
        {

            return false;
        }

    }
}
