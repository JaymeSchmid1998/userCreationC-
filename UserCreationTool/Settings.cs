using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserCreationTool
{
    public partial class Settings: Form
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            LocationDashBoard ld = new LocationDashBoard();
            ld.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //save to global variables

            GlobalVar.GlobalVar3= textBox1.Text;
            MessageBox.Show("updated");
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            textBox1.Text = GlobalVar.GlobalVar3;
        }
    }
}
