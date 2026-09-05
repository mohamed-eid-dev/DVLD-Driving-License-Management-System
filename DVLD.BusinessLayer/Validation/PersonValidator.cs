using DVLD.Model;
using DVLD.Model.Countries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DVLD.Application.InputValidator;

namespace DVLD.Application
{
    public static class PersonValidator
    {
        public static Result Validate(Person person)
        {
            Result result = new Result();

            if (person == null)
            {
                result.FailureResult("Person data is required.");
                return result;
            }

            if (string.IsNullOrWhiteSpace(person.FirstName))
            {
                result.FailureResult("First name is required.");
                return result;
            }

            if (string.IsNullOrWhiteSpace(person.SecondName))
            {
                result.FailureResult("Second name is required.");
                return result;
            }

            // ThirdName اختياري، لذلك لا نتحقق من أنه غير فارغ

            if (string.IsNullOrWhiteSpace(person.LastName))
            {
                result.FailureResult("Last name is required.");
                return result;
            }

            if (string.IsNullOrWhiteSpace(person.NationalNo))
            {
                result.FailureResult("National number is required.");
                return result;
            }

            if (person.DateOfBirth == default(DateTime))
            {
                result.FailureResult("Date of birth is required.");
                return result;
            }

            if (string.IsNullOrWhiteSpace(person.Address))
            {
                result.FailureResult("Address is required.");
                return result;
            }

            if (string.IsNullOrWhiteSpace(person.Phone))
            {
                result.FailureResult("Phone number is required.");
                return result;
            }

            if (person.CountryInfo == null ||
                person.CountryInfo.CountryID <= 0)
            {
                result.FailureResult("Please select a country.");
                return result;
            }

            if (person.Gender != enGenderType.Male &&
                person.Gender != enGenderType.Female)
            {
                result.FailureResult("Please select a gender.");
                return result;
            }

            // Email اختياري، لذلك نفحصه فقط لو المستخدم أدخله
            if (!string.IsNullOrWhiteSpace(person.Email))
            {
                if (!InputValidator.ValidateStringByType(
                        person.Email,
                        InputValidator.enStringType.Email))
                {
                    result.FailureResult("Email is not valid.");
                    return result;
                }
            }

            result.SuccessResult("");
            return result;
        }
    }
}
