using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.PresentationLayer.Applications.LocalDrivingLicense
{
    public partial class frmLocalDrivingLicenseApplicationDetails : Form
    {
        public frmLocalDrivingLicenseApplicationDetails(int ldlaId)
        {
            InitializeComponent();
            ctrlApplicationDetails1.LoadApplicationInfo(ldlaId);
        }
    }
}
