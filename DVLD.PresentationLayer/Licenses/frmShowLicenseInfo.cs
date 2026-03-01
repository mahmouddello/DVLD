using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.PresentationLayer.Licenses
{
    public partial class frmShowLicenseInfo : Form
    {
        private int ldlaId;
        public frmShowLicenseInfo(int ldlaId)
        {
            InitializeComponent();
            this.ldlaId = ldlaId;
        }

        private void frmShowLicenseInfo_Load(object sender, EventArgs e)
        {
            ctrlDriverLicenseInfo1.LoadLicense(ldlaId);
        }
    }
}
