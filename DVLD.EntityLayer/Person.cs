using System;

namespace DVLD.EntityLayer
{

    public enum enGender : byte
    {
        Male = 0,
        Female = 1,
        Unknown = 2
    }

    public class Person
    {
        public int ID { get; set; } = -1;
        public string NationalNo { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string SecondName { get; set; } = string.Empty;
        public string ThirdName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; } = DateTime.MinValue;
        public enGender Gender { get; set; } = enGender.Unknown;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Country Nationality { get; set; }
        public string ImagePath { get; set; } = string.Empty;
        public string FullName
        {
            get
            {
                return $"{FirstName} {SecondName} {ThirdName} {LastName}";
            }
        }

        public Person()
        {

        }

        public Person (
            int id, 
            string nationalNo, 
            string firstName, 
            string secondName, 
            string thirdName, 
            string lastName,
            DateTime dateOfBirth,
            enGender gender,
            string address,
            string phone,
            string email,
            Country nationality,
            string imagePath
        )
        {
            this.ID = id;
            this.NationalNo = nationalNo;
            this.FirstName = firstName;
            this.SecondName = secondName;
            this.ThirdName = thirdName;
            this.LastName = lastName;
            this.DateOfBirth = dateOfBirth;
            this.Gender = gender;
            this.Address = address;
            this.Phone = phone;
            this.Email = email;
            this.Nationality = nationality;
            this.ImagePath = imagePath;
        }
    }
}
