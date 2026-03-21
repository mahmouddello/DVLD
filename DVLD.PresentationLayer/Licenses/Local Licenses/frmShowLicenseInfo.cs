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
        private int _ldlaId;
        private int licenseId;

        private enum OpenMode { ByLdlaId, ByLicenseId }
        private OpenMode openMode;

        private frmShowLicenseInfo()
        {
            InitializeComponent();
        }

        public static frmShowLicenseInfo CreateByLdlaId(int _ldlaId)
        {
            var form = new frmShowLicenseInfo();
            form._ldlaId = _ldlaId;
            form.openMode = OpenMode.ByLdlaId;
            return form;
        }

        public static frmShowLicenseInfo CreateByLicenseId(int licenseId)
        {
            var form = new frmShowLicenseInfo();
            form.licenseId = licenseId;
            form.openMode = OpenMode.ByLicenseId;
            return form;
        }

        private void frmShowLicenseInfo_Load(object sender, EventArgs e)
        {
            if (openMode == OpenMode.ByLdlaId)
                ctrlDriverLicenseInfo1.LoadLicenseByLocalAppId(_ldlaId);
            else
                ctrlDriverLicenseInfo1.LoadLicenseByLicenseId(licenseId);
        }
    }
}
