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
    public partial class frmFindPerson : Form
    {

        #region Failds

        public event Action<object, int> DataBack;


        #endregion

        public frmFindPerson()
        {
            InitializeComponent();
        }

        private void frmFindPerson_Load(object sender, EventArgs e)
        {
            ctrlPersonCardWithFilter1.LoadControl();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (ctrlPersonCardWithFilter1.personID != -1)
            {
                DataBack?.Invoke(this, ctrlPersonCardWithFilter1.personID);
            }

            this.Close();
        }

    }
}
