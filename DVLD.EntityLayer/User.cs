namespace DVLD.EntityLayer
{
    public class User
    {
        public int Id { get; set; } = -1;
        public int PersonId { get; set; } = -1;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool IsActive { get; set; } = false;
        public Person LinkedPerson { get; set; } = null;

        public User() { }

        public User(int userId, int personId, string username, string password, bool isActive)
        {
            this.Id = userId;
            this.PersonId = personId;
            this.Username = username;
            this.Password = password;
            this.IsActive = isActive;
        }
    }
}
