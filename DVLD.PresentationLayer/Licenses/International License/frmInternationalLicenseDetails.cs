using System;
using System.Windows.Forms;

namespace DVLD.PresentationLayer.Licenses.International_License
{
    public partial class frmInternationalLicenseDetails : Form
    {
        private int _internationalLicenseId;

        public frmInternationalLicenseDetails(int internationalLicenseId)
        {
            InitializeComponent();
            _internationalLicenseId = internationalLicenseId;
        }

        private void frmInternationalLicenseDetails_Load(object sender, EventArgs e)
        {
            ctrlInternationalLicenseInfo1.LoadLicense(_internationalLicenseId);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
