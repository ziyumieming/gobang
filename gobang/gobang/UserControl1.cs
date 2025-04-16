using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gobang
{
    public partial class SelectionForm : Form
    {
        public bool? auto;
        public SelectionForm()
        {
            InitializeComponent();
        }

        private void SelectionForm_Load(object sender, EventArgs e)
        {
            auto = null;
            radioButton1.Checked = false;
            radioButton2.Checked = false;
        }

        private void confirm_button_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
                auto = true;
            else if (radioButton2.Checked == true)
                auto = false;
            this.Close();
        }
    }
}
