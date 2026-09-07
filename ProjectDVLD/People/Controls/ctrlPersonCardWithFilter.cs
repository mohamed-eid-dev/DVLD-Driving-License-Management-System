using DVLD.Application;
using DVLD.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectDVLD.People.Controls
{
    public partial class ctrlPersonCardWithFilter : UserControl
    {

        #region Failds
        private bool _FilterDisable = false;
        public bool FilterDisable { 
            get { return _FilterDisable; } 
            set { _FilterDisable = value; } }

        private bool _AddPersonDisable = false;
        public bool AddPersonDisable
        {
            get { return _AddPersonDisable; }
            set { _AddPersonDisable = value; }
        }

        public int personID = -1;

        #endregion

        #region Constructor
        public ctrlPersonCardWithFilter()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods

        #region Initial Data To Form

        public void LoadControl()
        {
            if (AddPersonDisable)
            {
                btnAddNewPerson.Enabled = false;
            }

            if (FilterDisable)
            {
                txtFilterValue.Enabled = false;
                btnFind.Enabled = false;
            }

            txtFilterValue.Enabled = false;


            ctrlPersonInfo.ResetPersonInfo();
        }

        #endregion

        #region Input Validation 
        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.SelectedIndex == 0)
            {
                if (!InputValidator.IsAllowedChar(e.KeyChar, InputValidator.enCharType.LettersAndDigits))
                {
                    e.Handled = true;
                    epErrorHandle.SetError(txtFilterValue, "Please Enter Only Letters and Digits");
                }
                else
                {
                    epErrorHandle.SetError(txtFilterValue, "");
                }
            }else if (cbFilterBy.SelectedIndex == 1)
            {
                if (!InputValidator.IsAllowedChar(e.KeyChar, InputValidator.enCharType.Digits))
                {
                    e.Handled = true;
                    epErrorHandle.SetError(txtFilterValue, "Please Enter Only Digits");
                }
                else
                {
                    epErrorHandle.SetError(txtFilterValue, "");
                }
            }
            
            if (cbFilterBy.SelectedIndex == -1)
            {
                e.Handled = true;
                epErrorHandle.SetError(cbFilterBy, "Please Selected Filter By");
            }
            else
            {
                epErrorHandle.SetError(cbFilterBy, "");
            }

        }

        private void cbFilterBy_TextChanged(object sender, EventArgs e)
        {
            txtFilterValue.Text = "";
            if (cbFilterBy.SelectedIndex == 1 || cbFilterBy.SelectedIndex == 0)
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

        private Result<Person> _GetPerson(string value)
        {
            Result<Person> result = new Result<Person>();

            if (cbFilterBy.SelectedIndex == 0)
            {
                result = PersonService.Find(value);
            }
            else if (cbFilterBy.SelectedIndex == 1)
            {
                int personID = Convert.ToInt32(value);
                result = PersonService.Find(personID);
            }
            else
            {
                result.FailureResult("Person Not Found");
            }

            return result;
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFilterValue.Text))
            {
                epErrorHandle.SetError(txtFilterValue,"Please Enter Values");
                return;
            }

           Result<Person> result = _GetPerson(txtFilterValue.Text);

           if (result.Success)
            {
                ctrlPersonInfo.AddPerson(result.Data);
                ctrlPersonInfo.LoadPersonData();
                personID = result.Data.PersonID;

            }


        }

        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson addUpdatePerson = new frmAddUpdatePerson();
            addUpdatePerson.BackData += BindDataFromAddNewPerson;
            addUpdatePerson.ShowDialog();
        }

        private void BindDataFromAddNewPerson(object sender,int personId )
        {
            this.personID = personId;
            
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
