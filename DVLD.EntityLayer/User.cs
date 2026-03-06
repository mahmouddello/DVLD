using System;

namespace DVLD.EntityLayer
{
    public class User
    {
        public int Id { get; set; } = -1;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool IsActive { get; set; } = false;
        public Person LinkedPerson { get; set; }
        public int PersonId => LinkedPerson?.Id ?? -1;
        public bool IsNew => Id == -1;

        public User() { }

        public User(int id, string username, string password, bool isActive, Person person)
        {
            Id = id;
            Username = username?.Trim() ?? string.Empty;
            Password = password ?? string.Empty;
            IsActive = isActive;
            LinkedPerson = person;
        }
    }
}