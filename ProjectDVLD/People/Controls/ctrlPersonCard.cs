using DVLD.Application;
using DVLD.Model;
using ProjectDVLD.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectDVLD.People.Controls
{
    public partial class ctrlPersonCard : UserControl
    {
        private  Person _person;
        private int _personID = -1;
        public Person Person { get { return _person; } }
        public int PersonID { get {return _personID; } }
        public ctrlPersonCard()
        {
            InitializeComponent();

        }

        public void LoadPersonData(int personID)
        {
            Result<Person> result = PersonService.Find(personID);

            if (!result.Success)
            {
                _person = null;
                ResetPersonInfo();
                MessageBox.Show("Error: "+result.Message, "", MessageBoxButtons.OK);
                return;
            }
            _person = result.Data;

            _FillPersonInfo();

        }

        public void LoadPersonData(string nationalNo)
        {
            Result<Person> result = PersonService.Find(nationalNo);

            if (!result.Success)
            {
                _person = null;
                ResetPersonInfo();
                MessageBox.Show("Error: " + result.Message, "", MessageBoxButtons.OK);
                return;
            }
            _person = result.Data;

            _FillPersonInfo();

        }

        public void ResetPersonInfo()
        {
            llEditPersonInfo.Visible = false;

            _personID = -1;
            lblPersonID.Text = "[????]";
            lblFullName.Text = "[????]";
            lblNationalNo.Text = "[????]";
            lblGendor.Text = "[????]";
            lblEmail.Text = "[????]";
            lblAddress.Text = "[????]";
            lblDateOfBirth.Text = "[????]";
            lblPhone.Text = "[????]";
            lblCountry.Text = "[????]";
            pbPersonImage.Image = Resources.Male_512;

        }

        private void _LoadPersonImage()
        {

            if (_person.Gender == enGenderType.Male)
            {
                lblGendor.Text = "Male";
                pbPersonImage.Image = Resources.Male_512;
            }
            else
            {
                lblGendor.Text = "Female";
                pbPersonImage.Image = Resources.Female_512;
            }

            if (string.IsNullOrWhiteSpace(_person.ImagePath))
                return;

           string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _person.ImagePath);

           if (File.Exists(imagePath))
           {
                pbPersonImage.ImageLocation = imagePath;
           }else
                MessageBox.Show("Could not find this image: = " + imagePath, "Error", MessageBoxButtons.OK);
       

        }

        private void _FillPersonInfo()
        {
            llEditPersonInfo.Visible = true;

            _personID = _person.PersonID;
            lblPersonID.Text = _person.PersonID.ToString();
            lblFullName.Text = _person.FullName;
            lblNationalNo.Text = _person.NationalNo;

            lblEmail.Text = _person.Email;
            lblAddress.Text = _person.Address;
            lblDateOfBirth.Text = _person.DateOfBirth.ToShortDateString();
            lblPhone.Text = _person.Phone;
            lblCountry.Text = _person.CountryInfo.CountryName;

            _LoadPersonImage();

        }

        private void llEditPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson(_personID);
            frm.ShowDialog();

            if (frm.DialogResult == DialogResult.OK)
            {
                LoadPersonData(_personID);
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
