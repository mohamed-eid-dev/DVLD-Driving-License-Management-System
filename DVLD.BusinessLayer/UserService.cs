using DVLD.DataAccess;
using DVLD.Model;
using DVLD.Model.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.BusinessLayer
{
    public static class UserService
    {

        public static Result<List<User>> GetAll()
        {
            Result<List<User>> result = new Result<List<User>>();
            bool isFound = false;
            try
            {
                result.Data = UserRepository.GetAll(ref isFound);

                if (!isFound)
                {
                    result.Data = new List<User>();
                    result.SuccessResult("No users found.");
                    return result;
                }

                result.SuccessResult("Users retrieved successfully.");
                return result;
            }
            catch
            {
                result.FailureResult("Failed to retrieve Users.");
                return result;
            }


        }

        /// ///////////////////////////////////////////////////////////




    }
}
