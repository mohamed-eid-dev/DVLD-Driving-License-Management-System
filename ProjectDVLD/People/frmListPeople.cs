// frmListPeople.cs

using DVLD.Application;
using DVLD.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ProjectDVLD.People
{
    public partial class frmListPeople : Form
    {
        #region Faild
        private List<Person> _allPeople = new List<Person>();

        #endregion

        #region Constructor
        public frmListPeople()
        {
            InitializeComponent();


        }

        #endregion

        #region Methods

        #region Initial Data To Form

        private void _InitialFormSetting()
        {
            cbFilterBy.Text = "None";
            txtFilterValue.Visible = false;
            lblRecordsCount.Text = _allPeople.Count.ToString();
        }

        private void _LoadPeople()
        {
            var result = PersonService.GetAll();

            if (!result.Success)
            {
                MessageBox.Show(result.Message);
                return;
            }

            _allPeople = result.Data;

            _BindPeopleToGrid(result.Data);
        }

        private static Image _GetPersonImage(string imagePath, enGenderType genderType = enGenderType.Male)
        {
            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, imagePath);

            if (!string.IsNullOrWhiteSpace(imagePath) && File.Exists(fullPath))
            {
                using (Image source = Image.FromFile(fullPath))
                {
                    return new Bitmap(source);
                }
            }
            return genderType == enGenderType.Male
                       ? Properties.Resources.Male_512
                       : Properties.Resources.Female_512;
        }

        private void _AddProfileImageColumn()
        {
            DataGridViewImageColumn column =
                new DataGridViewImageColumn();

            column.Name = "ProfileImage";
            column.HeaderText = "Image";
            column.ImageLayout =
                DataGridViewImageCellLayout.Zoom;

            dgvPeople.Columns.Insert(0, column);
        }

        private void _BindPeopleToGrid(List<Person> people)
        {
            dgvPeople.DataSource = people;

            if (!dgvPeople.Columns.Contains("ProfileImage"))
            {
                _AddProfileImageColumn();
            }

            foreach (DataGridViewRow row in dgvPeople.Rows)
            {
                Person person = (Person)row.DataBoundItem;

                row.Cells["ProfileImage"].Value =
                    _GetPersonImage(
                        person.ImagePath,
                        person.Gender);
            }

            dgvPeople.Columns["ImagePath"].Visible = false;
            dgvPeople.Columns["FullName"].Visible = false;
            dgvPeople.Columns["CountryInfo"].Visible = false;
            dgvPeople.Columns["Gender"].Visible = false;
            dgvPeople.Columns["Mode"].Visible = false;

            dgvPeople.CurrentCell = dgvPeople.Rows[0].Cells[0];
        }

        private void frmListPeople_Load(object sender, EventArgs e)
        {
            _LoadPeople();
            _InitialFormSetting();
        }


        #endregion

        #region Input Validation 
        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Clear();

            if (cbFilterBy.Text == "None")
            {
                txtFilterValue.Visible = false;
                _BindPeopleToGrid(_allPeople);
                lblRecordsCount.Text = _allPeople.Count.ToString();
            }
            else
            {
                txtFilterValue.Visible = true;
                txtFilterValue.Focus();
            }
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            _ApplyFilter();
        }

        private void _ApplyFilter()
        {
            string filterBy = cbFilterBy.Text;
            string value = txtFilterValue.Text.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(value) || filterBy == "None")
            {
                _BindPeopleToGrid(_allPeople);
                lblRecordsCount.Text = _allPeople.Count.ToString();
                return;
            }

            List<Person> filteredPeople = new List<Person>();

            switch (filterBy)
            {
                case "Person ID":
                    filteredPeople = _allPeople
                        .Where(p => p.PersonID.ToString().Contains(value))
                        .ToList();
                    break;

                case "National No":
                    filteredPeople = _allPeople
                        .Where(p => p.NationalNo != null &&
                                    p.NationalNo.ToLower().Contains(value))
                        .ToList();
                    break;

                case "First Name":
                    filteredPeople = _allPeople
                        .Where(p => p.FirstName != null &&
                                    p.FirstName.ToLower().Contains(value))
                        .ToList();
                    break;

                case "Last Name":
                    filteredPeople = _allPeople
                        .Where(p => p.LastName != null &&
                                    p.LastName.ToLower().Contains(value))
                        .ToList();
                    break;

                case "Phone":
                    filteredPeople = _allPeople
                        .Where(p => p.Phone != null &&
                                    p.Phone.ToLower().Contains(value))
                        .ToList();
                    break;

                case "Email":
                    filteredPeople = _allPeople
                        .Where(p => p.Email != null &&
                                    p.Email.ToLower().Contains(value))
                        .ToList();
                    break;

                default:
                    filteredPeople = _allPeople;
                    break;
            }

            _BindPeopleToGrid(filteredPeople);
            lblRecordsCount.Text = filteredPeople.Count.ToString();
        }


        #endregion

        #region Buttons Close & Save & Action

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson();
            frm.ShowDialog();
            _LoadPeople();
            _ApplyFilter();

        }


        #endregion

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int personID = Convert.ToInt32(dgvPeople.CurrentRow.Cells["PersonID"].Value);

            frmAddUpdatePerson frm = new frmAddUpdatePerson(personID);
            frm.ShowDialog();

            _LoadPeople();
            _ApplyFilter();

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int personID = Convert.ToInt32(dgvPeople.CurrentRow.Cells["PersonID"].Value);

            Result result = PersonService.Delete(personID);

            if (!result.Success)
            {
                MessageBox.Show(result.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Delete Successfully", result.Message, MessageBoxButtons.OK, MessageBoxIcon.Information);

            _LoadPeople();
            _ApplyFilter();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int personID = Convert.ToInt32(dgvPeople.CurrentRow.Cells["PersonID"].Value);

            frmShowPersonInfo frm =
                new frmShowPersonInfo(personID);

            frm.ShowDialog();
            _LoadPeople();
            _ApplyFilter();
        }

        #endregion

    }
}