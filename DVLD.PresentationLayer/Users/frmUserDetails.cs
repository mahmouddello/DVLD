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
        public frmUserDetails(int userId)
        {
            InitializeComponent();
            ctrlUserLoginInfo1.LoadUserInfo(userId);
        }

        public frmUserDetails(User user)
        {
            InitializeComponent();
            ctrlUserLoginInfo1.LoadUserInfo(user);
        }

        private void frmUserDetails_Load(object sender, EventArgs e)
        {
            ctrlPersonCard1.LoadPersonInfo(ctrlUserLoginInfo1.SelectedUser.PersonId);
        }
    }
}
