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
        private enum FilterMode
        {
            PersonID = 0,
            NationalNo = 1
        }

        private FilterMode filterMode;

        public event Action<int> OnPersonSelected;

        protected virtual void PersonSelected(int personId)
        {
            OnPersonSelected?.Invoke(personId);
        }


        public bool ShowAddPerson
        {
            get => _showAddPerson;
            set
            {
                _showAddPerson = value;
                btnAddNewPerson.Visible = value;
            }
        }
        private bool _showAddPerson = true;

        public bool FilterEnabled
        {
            get => _filterEnabled;
            set
            {
                _filterEnabled = value;
                gbFilters.Enabled = value;
            }
        }
        private bool _filterEnabled = true;

        public int PersonID => ctrlPersonCard1.PersonID;
        public Person SelectedPerson => ctrlPersonCard1.SelectedPerson;

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
            filterMode = (FilterMode)cbFilter.SelectedIndex;
            txtQuery.Clear();
            txtQuery.Focus();
        }

        private void txtQuery_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnSearch.PerformClick();
                e.Handled = true;
                return;
            }

            if (char.IsControl(e.KeyChar))
                return;

            switch (filterMode)
            {
                case FilterMode.PersonID:
                    if (!char.IsDigit(e.KeyChar))
                        Utility.HandleWrongKey(e);
                    break;

                case FilterMode.NationalNo:
                    if (!char.IsLetterOrDigit(e.KeyChar))
                        Utility.HandleWrongKey(e);
                    break;
            }
        }

        private void txtQuery_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtQuery.Text.Trim()))
                errorProvider1.SetError(txtQuery, "This field is required!");
            else
                errorProvider1.SetError(txtQuery, string.Empty);
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
                case FilterMode.PersonID:
                    ctrlPersonCard1.LoadPersonInfo(int.Parse(query));
                    break;
                case FilterMode.NationalNo:
                    ctrlPersonCard1.LoadPersonInfo(query);
                    break;
            }

            if (FilterEnabled)
                PersonSelected(ctrlPersonCard1.PersonID);
        }

        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson form = new frmAddUpdatePerson();
            form.DataBack += AddUpdatePersonForm_DataBack;
            form.ShowDialog();
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
            filterMode = (FilterMode)cbFilter.SelectedIndex;
            txtQuery.Focus();
        }
    }
}
