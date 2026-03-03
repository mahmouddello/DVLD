using System;

namespace DVLD.EntityLayer
{
    public enum Gender : byte
    {
        Male = 0,
        Female = 1,
        Unknown = 2
    }

    public class Person
    {
        public int Id { get; set; } = -1;
        public string NationalNo { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string SecondName { get; set; } = string.Empty;
        public string ThirdName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; } = DateTime.MinValue;
        public Gender Gender { get; set; } = Gender.Unknown;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Country Nationality { get; set; }
        public string ImagePath { get; set; } = string.Empty;

        public bool IsNew => Id == -1;

        public string FullName =>
            $"{FirstName} {SecondName} {ThirdName} {LastName}"
            .Replace("  ", " ")
            .Trim();

        public Person() { }

        public Person(
            int id,
            string nationalNo,
            string firstName,
            string secondName,
            string thirdName,
            string lastName,
            DateTime dateOfBirth,
            Gender gender,
            string address,
            string phone,
            string email,
            Country nationality,
            string imagePath)
        {
            Id = id;
            NationalNo = nationalNo?.Trim() ?? string.Empty;
            FirstName = firstName?.Trim() ?? string.Empty;
            SecondName = secondName?.Trim() ?? string.Empty;
            ThirdName = thirdName?.Trim() ?? string.Empty;
            LastName = lastName?.Trim() ?? string.Empty;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            Address = address?.Trim() ?? string.Empty;
            Phone = phone?.Trim() ?? string.Empty;
            Email = email?.Trim() ?? string.Empty;
            Nationality = nationality;
            ImagePath = imagePath?.Trim() ?? string.Empty;
        }
    }
}