using DVLD.BusinessLayer;
using DVLD.Model.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectDVLD.user
{
    public partial class frmListUsers : Form
    {
        private List<User> _users;

        public frmListUsers()
        {
            InitializeComponent();
            _users = new List<User>();
        }

        #region Methods

        #region Initial Data to Form

        private void _InitialFormSetting()
        {
            if(_users.Count == 0)
                return;
            

            cbUserTpye.Text = "None";
            //cbIsActive.Visible = false;
            //txtFilterValue.Visible = false;

            lblRecordsCount.Text = _users.Count.ToString();
        }

        private void _LoadData()
        {
            var result = UserService.GetAll();

            if (!result.Success)
            {
                MessageBox.Show(result.Message);
                return;
            }
            _users = result.Data;

            dgvUsers.DataSource = result.Data;
            dgvUsers.Columns["UserMode"].Visible = false;
            dgvUsers.CurrentCell = dgvUsers.Rows[0].Cells[0];
        }

        private void frmListUsers_Load(object sender, EventArgs e)
        {
            _LoadData();
            _InitialFormSetting();
        }

        #endregion

        #region Input Validation



        #endregion

        #region Active Buttons 

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            frmAddUpdateUser addUpdateUser = new frmAddUpdateUser();
            addUpdateUser.ShowDialog();
        }



        #endregion

        #endregion

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }

        private void pbPersonImage_Click(object sender, EventArgs e)
        {

        }

        private void pnlHeader_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void cbUserTpye_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
    }
}
