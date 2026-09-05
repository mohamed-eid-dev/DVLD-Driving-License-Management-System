using DVLD.Application;
using DVLD.Model;
using ProjectDVLD.People.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectDVLD.People
{
    public partial class frmShowPersonInfo : Form
    {
        private int _personID;
        public frmShowPersonInfo(int personID)
        {
            InitializeComponent();

            _personID =  personID;
            
        }

        private void frmShowPersonInfo_Load(object sender, EventArgs e)
        {
            ctrlPersonCard1.LoadPersonData(_personID);
        }

    }
}
