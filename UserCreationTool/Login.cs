using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

namespace UserCreationTool
{
    public partial class Login : Form
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "kMmaSpqjGX4IwLb4D6pQBZPlZsDU7zK4y6Z6x16Q",
            BasePath = "https://pineappleverification.firebaseio.com/",
        };
        IFirebaseClient C1;
        public Login()
        {
            InitializeComponent();
        }
        //login button click
        private async void button1_Click(object sender, EventArgs e)
        {
            FirebaseResponse response1 = await C1.GetTaskAsync("LocCount/node");
            LocationCounter t2 = response1.ResultAs<LocationCounter>();

            int checkV = Convert.ToInt32(t2.cnt);
            //this part gets data from the data base 
            bool found = false;
            while (found == false && checkV > 0)
            {
                try
                {
                    FirebaseResponse response = await C1.GetTaskAsync("Location/" + checkV);
                    Data t1 = response.ResultAs<Data>();
                    if (textBox3.Text == t1.placeName && textBox1.Text == t1.userName && textBox2.Text == t1.password)
                    {
                        //textBox1.Text = t1.userName;
                        // textBox2.Text = t1.password;
                        // MessageBox.Show(" found");
                        found = true;
                        GlobalVar.GlobalVar1 = textBox3.Text;
                        


                    }
                }
                catch
                {

                }


                checkV--;
            }
            if (found == false)
            {
                MessageBox.Show("not found");
            }
            else if (found == true)
            {
                MessageBox.Show(" you have logged in");
                //change form
                this.Visible = false;
                LocationDashBoard f1 = new LocationDashBoard();
                f1.ShowDialog();

            }

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            PageSelection PS = new PageSelection();
            PS.ShowDialog();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            C1 = new FireSharp.FirebaseClient(config);

            if (C1 != null)
            {

                Console.Write("connection successful");
            }
            else
            {
                Console.Write("connection successful");
            }
        }
    }
}
