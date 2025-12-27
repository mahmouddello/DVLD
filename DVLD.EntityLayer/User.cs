namespace DVLD.EntityLayer
{
    public class User
    {
        public int UserId { get; set; } = -1;
        public int PersonId { get; set; } = -1;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool IsActive { get; set; } = false;
        public Person LinkedPerson { get; set; } = null;

        public User() { }

        public User(int userId, int personId, string username, string password, bool isActive)
        {
            UserId = userId;
            PersonId = personId;
            Username = username;
            Password = password;
            IsActive = isActive;
        }
    }
}
