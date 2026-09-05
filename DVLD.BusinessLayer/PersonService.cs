using DVLD.BusinessLayer;
using DVLD.DataAccessLayer;
using DVLD.Model;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace DVLD.Application
{
    public static class PersonService
    {

        public static Result<List<Person>> GetAll()
        {
            Result<List<Person>> result = new Result<List<Person>>();

            try
            {

                bool isFound = false;
                result.Data = PersonRepository.GetAll(ref isFound);

                if (result.Data == null)
                    result.Data = new List<Person>();

                result.SuccessResult("People retrieved successfully.");
                return result;
            }
            catch
            {
                result.FailureResult("Failed to retrieve people.");
                return result;
            }
        }

        public static Result<Person> Find(int personID)
        {
            Result<Person> result = new Result<Person>();

            if (personID <= 0)
            {
                result.FailureResult("Invalid person ID.");
                return result;
            }

            try
            {
                bool isFound = false;
                Person person = PersonRepository.GetById(personID,ref isFound);

                if (person == null)
                {
                    result.FailureResult(message: "Person not Found.");
                    return result;
                }
                person.PersonID = personID;
                result.Data = person;
                result.SuccessResult("Person found.");

                return result;
            }
            catch
            {
                result.FailureResult("Failed to retrieve person.");
                return result;
            }
        }

        public static Result<Person> Find(string nationalNo)
        {
            Result<Person> result = new Result<Person>();

            if (string.IsNullOrWhiteSpace(nationalNo))
            {
                result.FailureResult("Invalid national number.");
                return result;
            }

            try
            {
                bool isFound = false;
                Person person = PersonRepository.GetByNationalNo(nationalNo,ref isFound);

                if (person == null)
                {
                    result.FailureResult("Person not found.");
                    return result;
                }

                result.Data = person;
                result.SuccessResult("Person found.");

                return result;
            }
            catch
            {
                result.FailureResult("Failed to retrieve person.");
                return result;
            }
        }

    
        public static Result<Person> Save(Person person , string imageLocation)
        {
            if (person == null)
            {
                return new Result<Person>
                {
                    Success = false,
                    Message = "Invalid person data."
                };
            }

          Result<string> resultHandelImagePath =  Util._SaveImage(person.ImagePath,imageLocation);

            if (!resultHandelImagePath.Success)
            {
                return new Result<Person>
                {
                    Success = false,
                    Message = resultHandelImagePath.Message
                };
            }

            person.ImagePath = resultHandelImagePath.Data;

            switch (person.Mode)
            {
                case Model.People.enMode.AddNew:
                    return _Add(person);

                case Model.People.enMode.Update:
                    return _Update(person);

                default:
                    return new Result<Person>
                    {
                        Success = false,
                        Message = "Invalid mode."
                    };
            }
        }

        private static Result<Person> _Add(Person person)
        {
            Result<Person> result = new Result<Person>();

            if (string.IsNullOrWhiteSpace(person.NationalNo))
            {
                result.FailureResult("National number is required.");
                return result;
            }

            if (PersonRepository.Exists(person.NationalNo))
            {
                result.FailureResult("Person already exists.");
                return result;
            }


            try
            {
                int created = PersonRepository.Add(person);

                if (created == -1)
                {
                    result.FailureResult("Failed to add person.");
                    return result;
                }
                person.PersonIsUpdate();
                person.PersonID = created;
                result.Data = person;
               
                result.SuccessResult("Person added successfully.");
                return result;
            }
            catch
            {
                result.FailureResult("Failed to add person.");
                return result;
            }
        }

        private static Result<Person> _Update(Person person)
        {
            Result<Person> result = new Result<Person>();

            if (person.PersonID <= 0)
            {
                result.FailureResult("Invalid person ID.");
                return result;
            }

            if (!PersonRepository.Exists(person.PersonID))
            {
                result.FailureResult("Person not found.");
                return result;
            }

            try
            {
                bool updated = PersonRepository.Update(person);

                if (!updated)
                {
                    result.FailureResult("Failed to update person.");
                    return result;
                }

                result.Data = person;
                result.SuccessResult("Person updated successfully.");

                return result;
            }
            catch
            {
                result.FailureResult("Failed to update person.");
                return result;
            }
        }

        public static Result Delete(int personID)
        {
            Result result = new Result();

            if (personID <= 0)
            {
                result.FailureResult("Invalid person ID.");
                return result;
            }

            if (!PersonRepository.Exists(personID))
            {
                result.FailureResult("Person not found.");
                return result;
            }

            try
            {
                bool deleted = PersonRepository.Delete(personID);

                if (!deleted)
                {
                    result.FailureResult("Failed to delete person.");
                    return result;
                }

                result.SuccessResult("Person deleted successfully.");
                return result;
            }
            catch
            {
                result.FailureResult("Failed to delete person.");
                return result;
            }
        }

        public static bool Exists(string nationalNo)
        {
            if (string.IsNullOrWhiteSpace(nationalNo))
                return false;

            return PersonRepository.Exists(nationalNo);
        }

        public static bool Exists(int personID)
        {
            if (personID <= 0)
                return false;

            return PersonRepository.Exists(personID);
        }

    }
}
