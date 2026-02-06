using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD.BusinessLayer;
using DVLD.EntityLayer;
using DVLD.PresentationLayer.GlobalClasses;

namespace DVLD.PresentationLayer.People
{
    public partial class ctrlPersonCardWithFilter : UserControl
    {
        private enum enFilterMode
        {
            PersonID = 0,
            NationalNo = 1
        }
        private enFilterMode filterMode;

        // Define a custom event handler delegate with parameters
        public event Action<int> OnPersonSelected;

        // Create a protected method to raise the event with a parameter
        protected virtual void PersonSelected(int PersonID)
        {
            Action<int> handler = OnPersonSelected;
            if (handler != null)
            {
                handler(PersonID); // Raise the event with the parameter
            }
        }

        private bool _ShowAddPerson = true;
        public bool ShowAddPerson
        {
            get
            {
                return _ShowAddPerson;
            }
            set
            {
                _ShowAddPerson = value;
                btnAddNewPerson.Visible = _ShowAddPerson;
            }
        }

        private bool _FilterEnabled = true;
        public bool FilterEnabled
        {
            get
            {
                return _FilterEnabled;
            }
            set
            {
                _FilterEnabled = value;
                gbFilters.Enabled = _FilterEnabled;
            }
        }

        private int _PersonID = -1;
        public int PersonID
        {
            get { return ctrlPersonCard1.PersonID; }
        }

        public Person SelectedPerson 
        {
            get { return ctrlPersonCard1.SelectedPerson; } 
        }

        public string QueryText
        {
            get => txtQuery.Text;
            set => txtQuery.Text = value;
        }

        public ctrlPersonCardWithFilter()
        {
            InitializeComponent();
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            filterMode = (enFilterMode)cbFilter.SelectedIndex;
            txtQuery.Clear();
            txtQuery.Focus();
        }

        private void txtQuery_KeyPress(object sender, KeyPressEventArgs e)
        {
            // ENTER key
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnSearch.PerformClick();
                e.Handled = true;
                return;
            }

            // Allow control keys (Backspace, Delete, etc.)
            if (char.IsControl(e.KeyChar))
                return;

            switch (filterMode)
            {
                case enFilterMode.PersonID:
                    if (!char.IsDigit(e.KeyChar))
                        Utility.HandleWrongKey(e);
                    break;

                case enFilterMode.NationalNo:
                    if (!char.IsLetterOrDigit(e.KeyChar))
                        Utility.HandleWrongKey(e);
                    break;
            }
        }

        private void txtQuery_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtQuery.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtQuery, "This field is required!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtQuery, string.Empty);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
                return;

            FindNow();
        }

        private void FindNow()
        {
            string query = txtQuery.Text.Trim();

            switch (filterMode)
            {
                case enFilterMode.PersonID:
                    ctrlPersonCard1.LoadPersonInfo(int.Parse(query));
                    break;
                case enFilterMode.NationalNo:
                    ctrlPersonCard1.LoadPersonInfo(query);
                    break;
                default:
                    break;
            }

            if (OnPersonSelected != null && FilterEnabled)
                OnPersonSelected(ctrlPersonCard1.PersonID);
        }

        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson form = new frmAddUpdatePerson();
            form.ShowDialog();
            
            form.DataBack += AddUpdatePersonForm_DataBack;
        }

        private void AddUpdatePersonForm_DataBack(object sender, int personID)
        {
            cbFilter.SelectedIndex = 0;
            txtQuery.Text = personID.ToString();
            ctrlPersonCard1.LoadPersonInfo(personID);
        }

        private void ctrlPersonCardWithFilter_Load(object sender, EventArgs e)
        {
            cbFilter.SelectedIndex = 0;
            filterMode = (enFilterMode)cbFilter.SelectedIndex;
            txtQuery.Focus();
        }

        private void ctrlPersonCard1_Load(object sender, EventArgs e)
        {

        }
    }
}
