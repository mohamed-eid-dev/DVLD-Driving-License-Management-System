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

        public frmShowPersonInfo(int personID)
        {
            InitializeComponent();
            ctrlPersonCard1.LoadPersonData(personID);
           
        }

        public frmShowPersonInfo(string notionalNo)
        {
            InitializeComponent();
            ctrlPersonCard1.LoadPersonData(notionalNo);

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
