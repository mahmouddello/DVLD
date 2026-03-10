using System;
using System.ComponentModel;
using System.Windows.Forms;
using DVLD.BusinessLayer;
using DVLD.EntityLayer;
using DVLD.PresentationLayer.GlobalClasses;

namespace DVLD.PresentationLayer.Tests.TestTypes
{
    public partial class frmUpdateTestType : Form
    {
        private int _testTypeId;
        private TestType _testType;

        public frmUpdateTestType(int testTypeId)
        {
            InitializeComponent();
            _testTypeId= testTypeId;
        }

        private void frmUpdateTestType_Load(object sender, EventArgs e)
        {
            InitializeForm();
        }

        private void InitializeForm()
        {
            _testType = TestTypeService.FindById(_testTypeId);

            if (_testType == null)
            {
                Utility.ShowErrorMessage($"A test type with id {_testTypeId} wasn't found, the form will get closed");
                this.Close();
                return;
            }

            LoadTestTypeInfo();
        }

        private void LoadTestTypeInfo()
        {
            lblID.Text = _testType.Id.ToString();
            txtTitle.Text = _testType.Title;
            txtDescription.Text = _testType.Description;
            txtFees.Text = _testType.Fees.ToString();
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

            _testType.Title = txtTitle.Text.Trim();
            _testType.Description = txtDescription.Text.Trim();
            _testType.Fees = Convert.ToDecimal(txtFees.Text.Trim());

            var testTypeService = new TestTypeService(_testType);

            if (testTypeService.Save())
                Utility.ShowSuccessMessage("Updated the test type successfully!");
            else
                Utility.ShowErrorMessage("Failed to update the test type data!");
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
            this.Close();
        }
    }
}
