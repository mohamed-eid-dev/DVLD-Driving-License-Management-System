using DVLD.Model;
using DVLD.Model.Countries;
using DVLD.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DVLD.Application
{
    public static class CountryService
    {


        public static Result<Country> Find(int id)
        {
            Result<Country> result = new Result<Country>();

            if (id <= 0)
            {
                result.FailureResult("Invalid Country ID");
                return result;
            }

            try
            {
                result.Data = CountriesRepository.GetByID(id);

                if (result.Data == null)
                {
                    result.FailureResult("Country not found");
                    return result;
                }

                result.SuccessResult("Country found");
                return result;

            }
            catch (Exception ex)
            {
                result.SuccessResult("Failed to retrieve Country. " + ex.Message);
                return result;
            }
        }

        public static Result<Country> Find(string countryName)
        {
            Result<Country> result = new Result<Country>();

            if (string.IsNullOrWhiteSpace(countryName))
            {
                result.FailureResult("Invalid Country Name");
                return result;
            }

            try
            {
                result.Data = CountriesRepository.GetByName(countryName);

                if (result.Data == null)
                {
                    result.FailureResult("Country not found");
                    return result;
                }

                result.SuccessResult("Country found");
                return result;
            }
            catch (Exception ex)
            {
                result.FailureResult("Failed to retrieve country. " + ex.Message);
                return result;
            }
        }

        public static Result<List<Country>> GetAll()
        {
            Result<List<Country>> result = new Result<List<Country>>();

            try
            {
                result.Data = CountriesRepository.GetAll();

                if (result.Data == null || result.Data.Count == 0)
                {
                    result.FailureResult("No countries found");
                    return result;
                }

                result.SuccessResult("Countries retrieved successfully");
                return result;
            }
            catch (Exception ex)
            {
                result.FailureResult("Failed to retrieve countries. " + ex.Message);
                return result;
            }
        }


    }
}
