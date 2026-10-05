using DVLD.Application;
using DVLD.Model;
using System;
using System.Windows.Forms;

namespace ProjectDVLD.People.Controls
{
    public partial class ctrlPersonCardWithFilter : UserControl
    {

        #region Failds && Event

        public event Action<int> OnPersonSelected;
        protected virtual void PersonSelected(int personID)
        {
            OnPersonSelected?.Invoke(personID);
        }

        private bool _FilterEnable = true;
        public bool FilterEnabled { 
            get { return _FilterEnable; } 
            set { _FilterEnable = value;
                gbFilters.Enabled = _FilterEnable;
            } }

        private bool _ShowAddPerson = false;
        public bool ShowAddPerson
        {
            get { return _ShowAddPerson; }
            set { _ShowAddPerson = value;
                btnAddNewPerson.Visible = _ShowAddPerson; }
        }

        public int PersonID { get { return ctrlPersonInfo.PersonID; } }

        public Person SelectedPersonInfo { get { return ctrlPersonInfo.Person; }  }

        #endregion

        #region Constructor
        public ctrlPersonCardWithFilter()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods

        #region Initial Data To Form

        public void LoadPersonInfo(int personID)
        {
            cbFilterBy.SelectedIndex = 1;
            txtFilterValue.Text = personID.ToString();
            FindNow();
        }

        private void FindNow()
        {
            switch (cbFilterBy.SelectedIndex) {

                case 0:
                    ctrlPersonInfo.LoadPersonData(txtFilterValue.Text);
                    break;

                case 1:
                    ctrlPersonInfo.LoadPersonData(int.Parse( txtFilterValue.Text));
                    break;

                default:
                    break;            
            }

            if (OnPersonSelected != null && FilterEnabled)
                PersonSelected(ctrlPersonInfo.PersonID);

        }

        private void ctrlPersonCardWithFilter_Load(object sender, EventArgs e)
        {
            cbFilterBy.SelectedIndex = 0;
            txtFilterValue.Focus();
        }



        #endregion

        #region Input Validation 
        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (cbFilterBy.SelectedIndex)
            {
                case 0:
                    if (!InputValidator.IsAllowedChar(e.KeyChar, InputValidator.enCharType.LettersAndDigits))
                    {
                        e.Handled = true;
                        epErrorHandle.SetError(txtFilterValue, "Please Enter Only Letters and Digits");
                    }
                    else
                    {
                        epErrorHandle.SetError(txtFilterValue, "");
                    }
                    break;

                case 1:
                    if (!InputValidator.IsAllowedChar(e.KeyChar, InputValidator.enCharType.Digits))
                    {
                        e.Handled = true;
                        epErrorHandle.SetError(txtFilterValue, "Please Enter Only Digits");
                    }
                    else
                    {
                        epErrorHandle.SetError(txtFilterValue, "");
                    }
                    break;

                default:
                    break;
            }

        }

        private void cbFilterBy_TextChanged(object sender, EventArgs e)
        {
            txtFilterValue.Text = "";
            if (cbFilterBy.SelectedIndex != -1)
            {
                epErrorHandle.SetError(cbFilterBy, "");
                txtFilterValue.Enabled = true;
            }
            else
            {
                epErrorHandle.SetError(cbFilterBy, "Please Selected Filter BY ");
                txtFilterValue.Enabled = false;
            }
        }

        #endregion

        #region Buttons Find & Update

          private void btnFind_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFilterValue.Text))
            {
                epErrorHandle.SetError(txtFilterValue,"Please Enter Values");
                return;
            }

            FindNow();

        }

        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson addUpdatePerson = new frmAddUpdatePerson();
            addUpdatePerson.BackData += HandlePersonAdded;
            addUpdatePerson.ShowDialog();
        }

        private void HandlePersonAdded(object sender,int personId )
        {
                
            if (personId != -1)
            {
                ctrlPersonInfo.LoadPersonData(personId);
                cbFilterBy.SelectedIndex = 1;
                txtFilterValue.Text = personId.ToString();
            }
            else
            {
                MessageBox.Show("No Relative Person!");
            }

        }

        #endregion

        #endregion

    }
}
