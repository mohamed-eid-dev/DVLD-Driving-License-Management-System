using DVLD.Model.Countries;
using DVLD.Model.People;
using System;

namespace DVLD.Model
{
    public class Person
    {
        private enMode _Mode = enMode.AddNew;

        public enMode Mode
        {
            get { return _Mode; }
        }

        // Identity
        public int PersonID { get; set; }
        public string NationalNo { get; set; }

        // Name
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public string LastName { get; set; }

        public string FullName =>
            $"{FirstName} {SecondName} {ThirdName} {LastName}";

        // Personal Info
        public DateTime DateOfBirth { get; set; }
        public enGenderType Gender { get; set; }

        // Contact
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        // Country
        public Country CountryInfo { get; set; }

        // Image
        public string ImagePath { get; set; }

        public Person()
        {
            PersonID = -1;
            NationalNo = "";
            FirstName = "";
            SecondName = "";
            ThirdName = "";
            LastName = "";

            DateOfBirth = DateTime.Now.AddYears(-18);
            Gender = enGenderType.Male;

            Address = "";
            Phone = "";
            Email = "";

            CountryInfo = new Country();

            ImagePath = "";
        }

        public Person(Person person)
        {
            PersonID = person.PersonID;
            FirstName = person.FirstName;
            SecondName = person.SecondName;
            ThirdName = person.ThirdName;
            LastName = person.LastName;

            NationalNo = person.NationalNo;

            DateOfBirth = person.DateOfBirth;
            Gender = person.Gender;

            Address = person.Address;
            Phone = person.Phone;
            Email = person.Email;

            CountryInfo = new Country(person.CountryInfo);

            ImagePath = person.ImagePath;
        }

        public void PersonIsUpdate()
        {
            _Mode = enMode.Update;
        }
    }
}