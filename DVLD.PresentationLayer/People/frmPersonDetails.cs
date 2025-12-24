using System;
using System.Windows.Forms;

namespace DVLD.PresentationLayer.People
{
    public partial class frmPersonDetails : Form
    {
        public frmPersonDetails(int personID)
        {
            InitializeComponent();
            ctrlPersonInformation1.LoadPersonInfo(personID);
        }

        public frmPersonDetails(string nationalNo)
        {
            InitializeComponent();
            ctrlPersonInformation1.LoadPersonInfo(nationalNo);
        }

        private void btnCloseForm_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
