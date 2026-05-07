using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using DVLD.DataAccessLayer;
using DVLD.EntityLayer;

namespace DVLD.BusinessLayer
{
    public class UserService
    {
        public User Info { get; private set; }

        public UserService(User user)
        {
            Info = user ?? throw new ArgumentNullException(nameof(user));
        }

        public static DataTable GetAllUsers() => UserData.GetAllAsTable();
        public static User FindById(int userId) => UserData.GetById(userId);
        public static User FindByUsername(string username) => UserData.GetByUsername(username);
        public static bool ExistsById(int userId) => UserData.ExistsById(userId);
        public static bool ExistsByUsername(string username) => UserData.ExistsByUsername(username);
        public static bool Delete(int userId) => UserData.Delete(userId);
        public static bool IsPersonLinkedToUser(int personId) => UserData.ExistsByPersonId(personId);

        private static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // Hash the password
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert the raw bytes into a human-readable hexadecimal 64 digit
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        private static bool VerifyPassword(string enteredPassword, string storedHash)
        {
            return storedHash == HashPassword(enteredPassword);
        }

        public static User Login(string username, string enteredPassword)
        {
            User user = FindByUsername(username);

            if (user == null) return null;
            if (!user.IsActive) return null;
            if (!VerifyPassword(enteredPassword, user.Password)) return null;

            return user;
        }
        
        public static bool ChangePassword(User user, string enteredPassword, string newPassword)
        {
            if (user == null) return false;
            if (!VerifyPassword(enteredPassword, user.Password)) return false;

            return UserData.UpdatePassword(user.Id, HashPassword(newPassword));
        }
        
        public bool Save()
        {
            if (string.IsNullOrWhiteSpace(Info.Username)) return false;
            if (string.IsNullOrWhiteSpace(Info.Password)) return false;
            if (Info.LinkedPerson == null) return false;
            if (Info.IsNew && ExistsByUsername(Info.Username)) return false; // new user, not unique username

            if (Info.IsNew)
            {
                Info.Password = HashPassword(Info.Password); // hash the password before saving the new user
                Info.Id = UserData.Add(Info);
                return !Info.IsNew;
            }

            return UserData.Update(Info);
        }

    }
}
