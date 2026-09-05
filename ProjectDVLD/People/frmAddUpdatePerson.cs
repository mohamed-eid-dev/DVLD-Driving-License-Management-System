using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD.Application;
using DVLD.Model;
using DVLD.Model.Countries;
using DVLD.Model.People;

namespace ProjectDVLD.People
{
    public partial class frmAddUpdatePerson : Form
    {
        #region Data Model

        private Person _person;

        //private delegate void BackDataHandel(object sender,int personID);

        //private event BackDataHandel BackData;
        #endregion

        #region Constructor
        public frmAddUpdatePerson()
        {
            InitializeComponent();
            _person = new Person();
        }

        public frmAddUpdatePerson(int personID)
        {
            InitializeComponent();
            var result = PersonService.Find(personID);
            if (result.Success)
                _person = result.Data;
            else
            {
                MessageBox.Show(result.Message, "Not Found Person", MessageBoxButtons.OK);
                this.Close();
            }
            
        }

        #endregion

        #region Methods 

        #region Initial Data to Form
        private void _ResetDefaultValues()
        {


            if (_person.Mode == enMode.AddNew)
            {
                lblTitle.Text = "Add New Person";
                llRemoveImage.Visible = false;
            }
            else
            {
                lblTitle.Text = "Update Person";
            }

            _FillCountriesInComboBox();

            dtpDateOfBirth.MaxDate = DateTime.Now.AddYears(-18);
            dtpDateOfBirth.MinDate = DateTime.Now.AddYears(-100);
            
            _LoadData(_person);

        }

        private void _FillCountriesInComboBox()
        {
            var Countries = CountryService.GetAll();

            if (!Countries.Success)
            {
                MessageBox.Show(Countries.Message);
                this.Close();
                return;
            }

            // Default Item (Select Country)
            var countries = Countries.Data;
            countries.Insert(0, new Country
            {
                CountryID = 0,
                CountryName = "Select Country"
            });

            cbCountry.DataSource = countries;
            cbCountry.DisplayMember = "CountryName";
            cbCountry.ValueMember = "CountryID";

        
        }
        private void _SetPersonImage()
        {
            if (string.IsNullOrWhiteSpace(_person.ImagePath))
            {
                return;
            }

            if (rbMale.Checked)
            {
                pbPersonImage.Image = Properties.Resources.Male_512;
            }
            else
            {
                pbPersonImage.Image = Properties.Resources.Female_512;
            }

            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,_person.ImagePath);

            pbPersonImage.ImageLocation = fullPath;

        }

        private void _LoadData(Person person)
        {

            if (person.PersonID != -1)
                lblPersonID.Text = person.PersonID.ToString();
            else
            { 
                lblPersonID.Text = "N/A";
            }

            txtFirstName.Text = person.FirstName;
            txtSecondName.Text = person.SecondName;
            txtThirdName.Text = person.ThirdName;
            txtLastName.Text = person.LastName;

            txtNationalNo.Text = person.NationalNo;
            txtEmail.Text = person.Email;

            if (person.Mode == enMode.AddNew)
            {

                dtpDateOfBirth.Value = dtpDateOfBirth.MaxDate;
                cbCountry.SelectedValue = 0;
            }
            else
            {
                cbCountry.SelectedValue = person.CountryInfo.CountryID;
                dtpDateOfBirth.Value = person.DateOfBirth;

            }

            txtPhone.Text = person.Phone;
            txtAddress.Text = person.Address;

            _SetPersonImage();

        }

        private void frmAddUpdatePerson_Load(object sender, EventArgs e)
        {

            _ResetDefaultValues();
        
        }

        #endregion

        #region Input Validation 
        private void txtName_Validating(object sender, CancelEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                errorProvider1.SetError(textBox,"Please Enter Name");
            }
            else
            {
                errorProvider1.SetError(textBox, "");
            }
        }

        private void txtFirstName_KeyPress(object sender, KeyPressEventArgs e)
        {
          if (!InputValidator.IsAllowedChar(e.KeyChar, InputValidator.enCharType.LettersWithSpace))
                e.Handled = true;
            
        }

        private void txtNationalNo_Validating(object sender, CancelEventArgs e)
        {
            string nationalNo = txtNationalNo.Text.Trim();

            if (string.IsNullOrWhiteSpace(nationalNo))
            {
                errorProvider1.SetError(txtNationalNo, "National Number is required!");
                return;
            }

            if (PersonService.Exists(nationalNo))
            {
                errorProvider1.SetError(txtNationalNo, "National Number already exists!");
            }
            else
            {
                errorProvider1.SetError(txtNationalNo, "");
            }
        }

        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!InputValidator.IsAllowedChar(e.KeyChar, InputValidator.enCharType.Digits))
                e.Handled = true;
            
        }

        private void txtNationalNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!InputValidator.IsAllowedChar(e.KeyChar, InputValidator.enCharType.LettersAndDigits))
                e.Handled = true;
        }

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            if (!InputValidator.ValidateStringByType(txtEmail.Text, InputValidator.enStringType.Email, true))
                errorProvider1.SetError(txtEmail, "Email Not Valid");
            else
                errorProvider1.SetError(txtEmail, "");
        }

        private void rbMale_CheckedChanged(object sender, EventArgs e)
        {
            _SetPersonImage();
        }

        private void llRemoveImage_Click(object sender, EventArgs e)
        {
            _person.ImagePath = "";
            _SetPersonImage();
            llRemoveImage.Visible = false;
        }

        private void llSetImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog oft = new OpenFileDialog())
            {
                oft.Filter = "Image File|*.png;*.jpg;*.jpeg;*.bmp";
                if (oft.ShowDialog() == DialogResult.OK)
                { 
                    pbPersonImage.ImageLocation = oft.FileName;
                    llRemoveImage.Visible = true;

                }
            }
        }

        private void cbCountry_Validating(object sender, CancelEventArgs e)
        {
            if (Convert.ToInt32(cbCountry.SelectedValue) < 1)
            {
                errorProvider1.SetError(cbCountry, "Please Choose Country!");
            }
            else
            {
                errorProvider1.SetError(cbCountry, "");
            }
        }

        private void llRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }
        #endregion

        /// ////////////////////////////////////////////////////
        #region Buttons Save & Close and Load Data Of Form

        private void _LoadDataFromForm()
        {
         

            _person.FirstName = txtFirstName.Text.Trim();
            _person.SecondName = txtSecondName.Text.Trim();
            _person.ThirdName = txtThirdName.Text.Trim();
            _person.LastName = txtLastName.Text.Trim();

            _person.NationalNo = txtNationalNo.Text.Trim();
            _person.DateOfBirth = dtpDateOfBirth.Value;

            if (rbMale.Checked)
                _person.Gender = enGenderType.Male;
            else
                _person.Gender = enGenderType.Female;

            _person.Phone = txtPhone.Text.Trim();
            _person.Email = txtEmail.Text.Trim();

            _person.CountryInfo.CountryID = Convert.ToInt32(cbCountry.SelectedValue);
            _person.Address = txtAddress.Text.Trim();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _LoadDataFromForm();

            Result validator = PersonValidator.Validate(_person);

            if (!validator.Success)
            {
                MessageBox.Show(validator.Message,"",MessageBoxButtons.OK);
                return;
            }


            Result<Person> result = PersonService.Save(_person,pbPersonImage.ImageLocation);

                if (result.Success)
                {
                MessageBox.Show( result.Message);
                DialogResult = DialogResult.OK;
                _person = result.Data;
                _ResetDefaultValues();
                //BackData?.Invoke(this, result.Data.PersonID);
                }
                else
                {
                    MessageBox.Show(result.Message);
                }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }




        #endregion

        #endregion


    }
}
