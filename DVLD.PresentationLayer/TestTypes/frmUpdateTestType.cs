using DVLD.BusinessLayer;
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

namespace DVLD.PresentationLayer.Tests.TestTypes
{
    public partial class frmUpdateTestType : Form
    {
        private int testTypeId;
        private TestType testType;

        public frmUpdateTestType(int testTypeId)
        {
            InitializeComponent();
            this.testTypeId = testTypeId;
        }

        private void frmUpdateTestType_Load(object sender, EventArgs e)
        {
            this.testType = TestTypeBusiness.Find(this.testTypeId);

            if (this.testType == null)
            {
                MessageBox.Show($"A test type with id {this.testTypeId} wasn't found, the form will be closed!");
                return;
            }

            LoadTestTypeInfo();
        }

        private void LoadTestTypeInfo()
        {
            lblID.Text = this.testType.Id.ToString();
            txtTitle.Text = this.testType.Title;
            txtDescription.Text = this.testType.Description;
            txtFees.Text = this.testType.Fees.ToString();
        }

        private bool ValidateRequireField(TextBox textBox, string message = "This field is required")
        { 
            if (textBox == null)
                return false;

            string text = textBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(text))
            {
                errProvider.SetError(textBox, message);
                return false;
            }
            else
                errProvider.SetError(textBox, string.Empty);

            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateChildren())
                return;

            testType.Title = txtTitle.Text.Trim();
            testType.Description = txtDescription.Text.Trim();
            testType.Fees = Convert.ToDecimal(txtFees.Text.Trim());

            if (TestTypeBusiness.Save(testType))
                MessageBox.Show("Updated the test type successfully!");
            else
                MessageBox.Show("Failed to update the test type data!");
        }

        private void txtTitle_Validating(object sender, CancelEventArgs e)
        {
            if (!ValidateRequireField((TextBox)sender))
                return;
        }

        private void txtDescription_Validating(object sender, CancelEventArgs e)
        {
            if (!ValidateRequireField((TextBox)sender))
                return;
        }

        private void txtFees_Validating(object sender, CancelEventArgs e)
        {
            if (!ValidateRequireField((TextBox)sender))
                return;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
