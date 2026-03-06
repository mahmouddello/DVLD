using System.Windows.Forms;
using DVLD.BusinessLayer;
using DVLD.EntityLayer;
using DVLD.PresentationLayer.GlobalClasses;

namespace DVLD.PresentationLayer.Users
{
    public partial class ctrlUserLoginInfo : UserControl
    {
        private User _user;
        public User SelectedUser => _user;

        public ctrlUserLoginInfo()
        {
            InitializeComponent();
        }

        public void LoadUserInfo(int userId)
        {
            _user = UserService.FindById(userId);

            if (_user == null)
            {
                ResetUserInfo();
                Utility.ShowErrorMessage($"No User with UserID = {userId}");
                return;
            }

            FillUserInfo();
        }

        public void LoadUserInfo(User user)
        {
            if (user == null)
            {
                ResetUserInfo();
                Utility.ShowErrorMessage($"User isn't found!");
                return;
            }

            _user = user;
            FillUserInfo();
        }

        private void ResetUserInfo()
        {
            lblUserID.Text = "???";
            lblUsername.Text = "???";
            lblIsActive.Text = "???";
        }

        private void FillUserInfo()
        {
            lblUserID.Text = _user.Id.ToString();
            lblUsername.Text = _user.Username;
            lblIsActive.Text = _user.IsActive ? "Active" : "Inactive";
        }
    }
}
