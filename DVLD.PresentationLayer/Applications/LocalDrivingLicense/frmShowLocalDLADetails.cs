using System.Windows.Forms;

namespace DVLD.PresentationLayer.Applications.LocalDrivingLicense
{
    public partial class frmShowLocalDLADetails : Form
    {
        public frmShowLocalDLADetails(int _ldlaId)
        {
            InitializeComponent();
            ctrlApplicationDetails1.LoadApplicationInfo(_ldlaId);
        }
    }
}
