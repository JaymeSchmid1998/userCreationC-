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
            DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
            dataGridView1.Columns.Add(btn);
            btn.HeaderText = "Edit";
            btn.Text = "Edit";
            btn.Name = "btn";
            btn.UseColumnTextForButtonValue = true;
            DataGridViewButtonColumn btn2 = new DataGridViewButtonColumn();
            dataGridView1.Columns.Add(btn2);
            btn2.HeaderText = "Delete";
            btn2.Text = "Delete";
            btn2.Name = "btn2";
            btn2.UseColumnTextForButtonValue = true;
            DataGridViewButtonColumn btn3 = new DataGridViewButtonColumn();
            dataGridView1.Columns.Add(btn3);
            btn3.HeaderText = "Retrieve data";
            btn3.Text = "Retrieve data";
            btn3.Name = "btn3";
            btn3.UseColumnTextForButtonValue = true;
            displayall();

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
            displayall();
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
            displayall();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            displayall();
        }
        private async void displayall()
        {
            datT.Rows.Clear();
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //LoadData();
            if (e.ColumnIndex == 3)
            {
                // MessageBox.Show();
                String ABC = e.RowIndex.ToString();
                // MessageBox.Show(ABC);
                if (dataGridView1.SelectedCells.Count > 0)
                {
                    int selectedrowindex = e.RowIndex;
                    DataGridViewRow selectedRow = dataGridView1.Rows[selectedrowindex];
                    string placeName = Convert.ToString(selectedRow.Cells["Place Name"].Value);
                    string UserN = Convert.ToString(selectedRow.Cells["USERNAME NAME"].Value);
                    string PswN = Convert.ToString(selectedRow.Cells["PASSWORD"].Value);
                    if (textBox1.Text != null && textBox1.Text != "" && textBox2.Text != null && textBox2.Text != "" && textBox3.Text != null && textBox3.Text != "")
                    {
                        editPlace(placeName, UserN, PswN);
                    }
                    else
                    {
                        MessageBox.Show("data cannot be invalid or blank!");
                    }

                }
            }
            if (e.ColumnIndex == 4)
            {
                // MessageBox.Show();
                String ABC = e.RowIndex.ToString();
                MessageBox.Show(ABC);
                if (dataGridView1.SelectedCells.Count > 0)
                {
                    int selectedrowindex = e.RowIndex;
                    DataGridViewRow selectedRow = dataGridView1.Rows[selectedrowindex];
                    string placeName = Convert.ToString(selectedRow.Cells["Place Name"].Value);
                    string UserN = Convert.ToString(selectedRow.Cells["USERNAME NAME"].Value);
                    string PswN = Convert.ToString(selectedRow.Cells["PASSWORD"].Value);
                    
                    deletePlace(placeName, UserN,PswN);
                }

            }
            if (e.ColumnIndex == 5)
            {
                // MessageBox.Show();
                String ABC = e.RowIndex.ToString();
                MessageBox.Show(ABC);
                if (dataGridView1.SelectedCells.Count > 0)
                {
                    int selectedrowindex = e.RowIndex;
                    DataGridViewRow selectedRow = dataGridView1.Rows[selectedrowindex];
                    string placeName = Convert.ToString(selectedRow.Cells["Place Name"].Value);
                    string UserN = Convert.ToString(selectedRow.Cells["USERNAME NAME"].Value);
                    string PswN = Convert.ToString(selectedRow.Cells["PASSWORD"].Value);
                    textBox1.Text = placeName;
                    textBox2.Text = UserN;
                    textBox3.Text = PswN;
                }

            }
        }
        async void editPlace(String PlaceName, string userName, string password)
        {
            FirebaseResponse response1 = await C1.GetTaskAsync("LocCount/node");
            LocationCounter t2 = response1.ResultAs<LocationCounter>();

            int checkV = Convert.ToInt32(t2.cnt);

            //this part updates 

            var data = new Data
            {
                placeName = PlaceName,
                userName = userName,
                password = textBox2.Text

            };
            bool check = false;
            while (check == false && checkV > 0)
            {
                try
                {
                    FirebaseResponse response2 = await C1.GetTaskAsync("Location/" + checkV);
                    Data t1 = response2.ResultAs<Data>();

                    if (t1.placeName == PlaceName)
                    {
                        //SetResponse response = await C1.SetTaskAsync("Location/" + checkV, data);
                        //this deletes specified
                        FirebaseResponse response = await C1.UpdateTaskAsync("Location/" + checkV, data);
                        Data result = response.ResultAs<Data>();
                        check = true;
                        MessageBox.Show("edited place");
                    }
                }
                catch
                {

                }



                checkV--;
            }

            if (check == false)
            {
                MessageBox.Show("cannot update as its not found");
            }
            displayall();
            
        }
        async void deletePlace(String PlaceName,string userName,string password)
        {
            FirebaseResponse response1 = await C1.GetTaskAsync("LocCount/node");
            LocationCounter t2 = response1.ResultAs<LocationCounter>();

            int checkV = Convert.ToInt32(t2.cnt);

            //this part updates 

            var data = new Data
            {
                placeName = PlaceName,
                userName = userName,
                password = password

            };
            bool check = false;
            while (check == false && checkV > 0)
            {
                FirebaseResponse response2 = await C1.GetTaskAsync("Location/" + checkV);
                Data t1 = response2.ResultAs<Data>();

                if (t1.placeName == PlaceName)
                {
                    //SetResponse response = await C1.SetTaskAsync("Location/" + checkV, data);
                    //this deletes specified
                    FirebaseResponse response = await C1.DeleteTaskAsync("Location/" + checkV);
                    //  Data result = response.ResultAs<Data>();
                    check = true;
                    MessageBox.Show("deleted place");
                }



                checkV--;
            }

            if (check == false)
            {
                MessageBox.Show("cannot delete as its not found");
            }
            displayall();
        }
    }
}
