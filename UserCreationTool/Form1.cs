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



    public partial class Form1 : Form
    {
        DataTable datT = new DataTable();
        //this gives the location and the also the authentication password to firesharp
        IFirebaseConfig config = new FirebaseConfig
        {
           AuthSecret= "kMmaSpqjGX4IwLb4D6pQBZPlZsDU7zK4y6Z6x16Q",
           BasePath= "https://pineappleverification.firebaseio.com/",
        };
        IFirebaseClient C1;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
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

            datT.Columns.Add("PLACE NAME");
            datT.Columns.Add("USERNAME NAME");
            datT.Columns.Add("PASSWORD");
            dataGridView1.DataSource = datT;

        }

        private async void button1_Click(object sender, EventArgs e)
        {

            FirebaseResponse response1 = await C1.GetTaskAsync("LocCount/node");
            LocationCounter t1 = response1.ResultAs<LocationCounter>();
            //  MessageBox.Show(t1.cnt);


            var data = new Data
            {
                LocId = (Convert.ToInt32(t1.cnt) + 1).ToString(),
                placeName = textBox3.Text,
                userName = textBox1.Text,
            password = textBox2.Text

        };

            SetResponse response = await C1.SetTaskAsync("Location/"+data.LocId, data);
            Data result = response.ResultAs<Data>();

            var LocData = new LocationCounter
            {
                cnt = data.LocId,

            };

            SetResponse resp1 = await C1.SetTaskAsync("LocCount/node",LocData);
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private async void button2_Click(object sender, EventArgs e)
        {
            FirebaseResponse response1 = await C1.GetTaskAsync("LocCount/node");
            LocationCounter t2 = response1.ResultAs<LocationCounter>();

            int checkV = Convert.ToInt32( t2.cnt);
            //this part gets data from the data base 
            bool found= false;
            while(found == false && checkV>0)
            {
                try
                {
                    FirebaseResponse response = await C1.GetTaskAsync("Location/" + checkV);
                    Data t1 = response.ResultAs<Data>();
                    if (textBox3.Text == t1.placeName)
                    {
                        textBox1.Text = t1.userName;
                        textBox2.Text = t1.password;
                        MessageBox.Show(" found");
                        found = true;
                            
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
            

        }

        private async void button3_Click(object sender, EventArgs e)
        {
            FirebaseResponse response1 = await C1.GetTaskAsync("LocCount/node");
            LocationCounter t2 = response1.ResultAs<LocationCounter>();

            int checkV = Convert.ToInt32(t2.cnt);

            //this part updates 

            var data = new Data
            {
                placeName = textBox3.Text,
                userName = textBox1.Text,
            password = textBox2.Text

        };
            bool check = false;
            while(check==false && checkV > 0)
            {
                FirebaseResponse response2 = await C1.GetTaskAsync("Location/" + checkV);
                Data t1 = response2.ResultAs<Data>();

                if (t1.placeName == textBox3.Text)
                {
                    SetResponse response = await C1.SetTaskAsync("Location/" + checkV, data);
                    Data result = response.ResultAs<Data>();
                    check = true;
                    MessageBox.Show("updated");
                }



                checkV--;
            }

            if (check == false)
            {
                MessageBox.Show("not found");
            }


            
        }

        private async void button4_Click(object sender, EventArgs e)
        {



            FirebaseResponse response1 = await C1.GetTaskAsync("LocCount/node");
            LocationCounter t2 = response1.ResultAs<LocationCounter>();

            int checkV = Convert.ToInt32(t2.cnt);

            //this part updates 

            var data = new Data
            {
                placeName = textBox3.Text,
                userName = textBox1.Text,
                password = textBox2.Text

            };
            bool check = false;
            while (check == false && checkV > 0)
            {
                FirebaseResponse response2 = await C1.GetTaskAsync("Location/" + checkV);
                Data t1 = response2.ResultAs<Data>();

                if (t1.placeName == textBox3.Text)
                {
                    //SetResponse response = await C1.SetTaskAsync("Location/" + checkV, data);
                    //this deletes specified
                    FirebaseResponse response = await C1.DeleteTaskAsync("Location/" + checkV);
                  //  Data result = response.ResultAs<Data>();
                    check = true;
                    MessageBox.Show("deleted");
                }



                checkV--;
            }

            if (check == false)
            {
                MessageBox.Show("cannot delete as its not found");
            }





          
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            //this deletes all
            FirebaseResponse response = await C1.DeleteTaskAsync("Location/");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            displayall();
        }
        private async void displayall()
        {
            
            FirebaseResponse response1 = await C1.GetTaskAsync("LocCount/node");
            LocationCounter t2 = response1.ResultAs<LocationCounter>();


            int checkV = Convert.ToInt32(t2.cnt);

            while (true&& checkV>0)
            {
                try
                {
                    FirebaseResponse response2 = await C1.GetTaskAsync("Location/" + checkV);
                    Data obj2 = response2.ResultAs<Data>();
                    DataRow row = datT.NewRow();

                  
                   
                    row["PLACE NAME"] = obj2.placeName;
                    row["USERNAME NAME"] = obj2.userName;
                    row["PASSWORD"] = obj2.password;

                    datT.Rows.Add(row);
                }
                catch
                {

                }
                checkV--;
            }





        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            AdminLogin al = new AdminLogin();
            al.ShowDialog();
        }
    }
}
