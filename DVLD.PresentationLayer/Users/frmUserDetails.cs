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
    public partial class frmUserDetails : Form
    {
        private int _userId;
        private User _user;

        private frmUserDetails()
        {
            InitializeComponent();
        }

        public static frmUserDetails CreateById(int userId)
        {
            var form = new frmUserDetails();
            form._userId = userId;
            return form;
        }

        public static frmUserDetails CreateByUser(User user)
        {
            var form = new frmUserDetails();
            form._user = user;
            return form;
        }

        private void frmUserDetails_Load(object sender, EventArgs e)
        {
            if (_user != null)
                ctrlUserLoginInfo1.LoadUserInfo(_user);
            else
                ctrlUserLoginInfo1.LoadUserInfo(_userId);

            ctrlPersonCard1.LoadPersonInfo(ctrlUserLoginInfo1.SelectedUser.PersonId);
        }
    }
}
