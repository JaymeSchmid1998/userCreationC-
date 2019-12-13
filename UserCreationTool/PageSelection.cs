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
    public partial class PageSelection : Form
    {
        public PageSelection()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            AdminLogin al = new AdminLogin();
            al.Show();
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            Login al = new Login();
            al.ShowDialog();
            
        }
    }
}
