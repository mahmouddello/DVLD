using DVLD.PresentationLayer.GlobalClasses;
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
    public partial class frmShowLicenseHistory : Form
    {

        private int personId;

        public frmShowLicenseHistory(int personId)
        {
            InitializeComponent();
            this.personId = personId;
        }

        private void ApplyDefaultSettings()
        {
            ctrlPersonCardWithFilter1.FilterEnabled = false;
            ctrlPersonCardWithFilter1.ShowAddPerson = false;
        }

        private void LoadPersonCardData()
        {
            ctrlPersonCardWithFilter1.ctrlPersonCard1.LoadPersonInfo(this.personId);
            ctrlPersonCardWithFilter1.QueryText = this.personId.ToString();
        }

        private void frmShowLicenseHistory_Load(object sender, EventArgs e)
        {
            ApplyDefaultSettings();
            LoadPersonCardData();

            ctrlDriverLicenseHistory1.LoadDataByPersonId(personId);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}