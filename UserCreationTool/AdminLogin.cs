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
    public partial class AdminLogin : Form
    {
        public AdminLogin()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            PageSelection PS = new PageSelection();
            PS.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text=="" || textBox2.Text == ""|| textBox1.Text == " " || textBox2.Text == " "|| textBox1.Text == null || textBox2.Text == null)
            {
                MessageBox.Show("please enter a valid username and password ");
            }
            else if (textBox1.Text == "admin1234" && textBox2.Text == "password1234")
            {
                this.Visible = false;
                Form1 fs = new Form1();
                this.Close();
                fs.ShowDialog();
            }
            else
            {
                MessageBox.Show("please enter a valid username and password ");
            }
        }
    }
}
