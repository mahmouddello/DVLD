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

        private void frmUserDetails_Load(object sender, EventArgs e)
        {
            if (ctrlUserLoginInfo1.SelectedUser != null) 
                ctrlPersonCard1.LoadPersonInfo(ctrlUserLoginInfo1.SelectedUser.PersonId);
        }
    }
}
