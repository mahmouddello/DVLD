using DVLD.BusinessLayer;
using DVLD.EntityLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.PresentationLayer.Users
{
    public partial class ctrlUserLoginInfo : UserControl
    {
        private User user;
        private int userID = -1;

        public int UserID { get { return userID; } }
        public User SelectedUser { get { return user; } }


        public ctrlUserLoginInfo()
        {
            InitializeComponent();
        }

        public void LoadUserInfo(int userId)
        {
            user = UserBusiness.Find(userId);

            if (user == null)
            {
                ResetUserInfo();
                MessageBox.Show("No User with UserID = " + userID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            FillUserInfo();
        }

        public void LoadUserInfo(User userParam)
        {
            if (userParam == null)
            {
                ResetUserInfo();
                MessageBox.Show("No User with UserID = " + userParam.Id.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            user = userParam;
            FillUserInfo();
        }

        private void ResetUserInfo()
        {
            userID = -1; // not found
            lblUserID.Text = "???";
            lblUsername.Text = "???";
            lblIsActive.Text = "???";
        }

        private void FillUserInfo()
        {
            userID = user.Id;
            lblUserID.Text = userID.ToString();
            lblUsername.Text = user.Username;
            lblIsActive.Text = user.IsActive ? "Yes" : "No";
        }
    }
}
