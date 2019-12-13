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
    public partial class LocationDashBoard : Form
    {
        public LocationDashBoard()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            UserManagementcs UM = new UserManagementcs();
            UM.ShowDialog();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            Settings UM = new Settings();
            UM.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            DoorManagementcs UM = new DoorManagementcs();
            UM.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            Logs UM = new Logs();
            UM.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            Login L = new Login();
            L.ShowDialog();
        }
    }
}
