using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
       
        public static User Login(string username, string password)
        {
            User user = FindByUsername(username);

            if (user == null) return null;
            if (!user.IsActive) return null;
            if (user.Password != password) return null;

            return user;
        }
        
        public static bool ChangePassword(User user, string enteredCurrentPassword, string newPassword)
        {
            if (user == null) return false;
            if (user.Password != enteredCurrentPassword) return false;

            return UserData.UpdatePassword(user.Id, newPassword);
        }
        
        public bool Save()
        {
            if (string.IsNullOrWhiteSpace(Info.Username)) return false;
            if (string.IsNullOrWhiteSpace(Info.Password)) return false;
            if (Info.LinkedPerson == null) return false;
            if (Info.IsNew && ExistsByUsername(Info.Username)) return false; // new user, not unique username

            if (Info.IsNew)
            {
                Info.Id = UserData.Add(Info);
                return !Info.IsNew;
            }

            return UserData.Update(Info);
        }

    }
}
