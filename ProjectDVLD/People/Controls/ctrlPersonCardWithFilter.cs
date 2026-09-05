using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectDVLD.People.Controls
{
    public partial class ctrlPersonCardWithFilter : UserControl
    {

        private bool _FilterDisable = false;
        public bool FilterDisabe { 
            get { return _FilterDisable; } 
            set { _FilterDisable = value; } }



        public ctrlPersonCardWithFilter()
        {
            InitializeComponent();
        }

        private void ctrlPersonCardWithFilter_Load(object sender, EventArgs e)
        {

        }
    }
}
