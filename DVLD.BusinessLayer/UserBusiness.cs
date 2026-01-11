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
    public static class UserBusiness
    {
        public static DataTable GetUsers()
        {
            return UserData.GetAll();
        }

        public static User Find(int userId)
        {
            DataRow row = UserData.GetById(userId);

            if (row == null)
                return null;

            User user = new User(
                userId: (int)row["UserID"],
                personId: (int)row["PersonID"],
                username: (string)row["Username"],
                password: (string)row["Password"],
                isActive: (bool)row["IsActive"]
            );

            user.LinkedPerson = PersonBusiness.Find((int)row["PersonID"]);

            return user;
        }

        public static User Find(string username)
        {
            DataRow row = UserData.GetByUsername(username);

            if (row == null)
                return null;

            User user = new User(
                userId: (int)row["UserID"],
                personId: (int)row["PersonID"],
                username: (string)row["Username"],
                password: (string)row["Password"],
                isActive: (bool)row["IsActive"]
            );

            user.LinkedPerson = PersonBusiness.Find((int)row["PersonID"]);

            return user;
        }

        private static bool Add(User user)
        {
            user.UserId = UserData.InsertNew(user.PersonId, user.Username, user.Password, user.IsActive);

            return user != null;
        }

        private static bool Update(User user)
        {
            return UserData.UpdateById(user.UserId, user.PersonId, user.Username, user.Password, user.IsActive);
        }

        public static bool Delete(int userId)
        {
            return UserData.DeleteById(userId);
        }

        public static bool Save(User user)
        {
            if (user.UserId == -1)
                return Add(user);

            return Update(user);
        }

        public static bool Exists(int userId)
        {
            return UserData.ExistsById(userId);
        }

        public static bool Exists(string username)
        {
            return UserData.ExistsByUsername(username);
        }

        public static bool IsPersonLinkedToUser(int personId)
        {
            return UserData.ExistsByPersonId(personId);
        }

        public static User Login(string username, string password)
        {
            User user = Find(username);

            return user == null || !user.IsActive || user.Password != password ? null: user;
        }

        public static bool ChangePassword(int userId, string currentPassword, string newPassword)
        {
            User user = Find(userId);

            if (user == null)
                return false;

            if (user.Password != currentPassword)
                return false;

            return UserData.UpdatePassword(userId, newPassword);
        }
    }
}
