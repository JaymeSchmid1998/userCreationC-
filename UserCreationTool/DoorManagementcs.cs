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
using System.IO;

namespace UserCreationTool
{
    public partial class DoorManagementcs : Form
    {
        DataTable datT = new DataTable();
        //this gives the location and the also the authentication password to firesharp
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "kMmaSpqjGX4IwLb4D6pQBZPlZsDU7zK4y6Z6x16Q",
            BasePath = "https://pineappleverification.firebaseio.com/",
        };
        IFirebaseClient C1;
        public DoorManagementcs()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            FirebaseResponse response1 = await C1.GetTaskAsync("DoorCount/node");
           DoorCount t1 = response1.ResultAs<DoorCount>();
            //  MessageBox.Show(t1.cnt);


            var data = new DoorData
            {
                DoorId = (Convert.ToInt32(t1.cnt) + 1).ToString(),
                placeName = label1.Text,
                DoorName = textBox1.Text,
                Status = comboBox2.Text,
                AuthLevel = comboBox1.Text

            };

            SetResponse response = await C1.SetTaskAsync("Doors/" + data.DoorId, data);
            Data result = response.ResultAs<Data>();

            var LocData = new LocationCounter
            {
                cnt = data.DoorId,

            };

            SetResponse resp1 = await C1.SetTaskAsync("DoorCount/node", LocData);
            //   button3.Visible = true;
            LoadData();
        }
       

        private  void button3_Click(object sender, EventArgs e)
        {
         /*   FirebaseResponse response1 = await C1.GetTaskAsync("DoorCount/node");
            DoorCount t2 = response1.ResultAs<DoorCount>();

            int checkV = Convert.ToInt32(t2.cnt);

            //this part updates 

            var data = new DoorData
            {
                placeName = label1.Text,
               DoorName= textBox1.Text,
                Status = comboBox2.Text,
                AuthLevel=comboBox1.Text

            };
            bool check = false;
            while (check == false && checkV > 0)
            {
                FirebaseResponse response2 = await C1.GetTaskAsync("Doors/" + checkV);
               DoorData t1 = response2.ResultAs<DoorData>();

                if (t1.placeName == label1.Text)
                {
                    if (t1.DoorName == textBox1.Text)
                    {
                        //SetResponse response = await C1.SetTaskAsync("Location/" + checkV, data);
                        //this deletes specified
                        FirebaseResponse response = await C1.DeleteTaskAsync("Doors/" + checkV);
                        //  Data result = response.ResultAs<Data>();
                        check = true;
                        MessageBox.Show("deleted");
                    }
                    
                }



                checkV--;
            }

            if (check == false)
            {
                MessageBox.Show("cannot delete as its not found");
            }
            */
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        async public void LoadData()
        {
            datT.Rows.Clear();
            FirebaseResponse response1 = await C1.GetTaskAsync("DoorCount/node");
            DoorCount t2 = response1.ResultAs<DoorCount>();


            int checkV = Convert.ToInt32(t2.cnt);

            while (true && checkV > 0)
            {
                try
                {
                    FirebaseResponse response2 = await C1.GetTaskAsync("Doors/" + checkV);
                    DoorData obj2 = response2.ResultAs<DoorData>();

                    if (label1.Text == obj2.placeName)
                    {
                        DataRow row = datT.NewRow();



                        row["PlaceName"] = obj2.placeName;
                        row["DoorName"] = obj2.DoorName;
                        row["Status"] = obj2.Status;
                        row["AuthLvl"] = obj2.AuthLevel;
                       
                       // row["Edit"] =buttonColumn;
                              
                        datT.Rows.Add(row);


                       
                    }
                }
                catch
                {

                }
                checkV--;
            }

        }



        private  async void button4_Click(object sender, EventArgs e)
        {
            FirebaseResponse response1 = await C1.GetTaskAsync("DoorCount/node");
           DoorCount t2 = response1.ResultAs<DoorCount>();

            int checkV = Convert.ToInt32(t2.cnt);
            //this part gets data from the data base 
            bool found = false;
            while (found == false && checkV > 0)
            {
                try
                {
                    FirebaseResponse response = await C1.GetTaskAsync("Doors/" + checkV);
                   DoorData t1 = response.ResultAs<DoorData>();
                    if (label1.Text == t1.placeName)
                    {
                        if (textBox1.Text==t1.DoorName)
                        {
                            comboBox2.Text = t1.Status;
                            comboBox1.Text = t1.AuthLevel;
                            MessageBox.Show(" found");
                            found = true;
                        //    button3.Visible = true;
                        }
                       
                       

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

        private void DoorManagementcs_Load(object sender, EventArgs e)
        {
            label1.Text = GlobalVar.GlobalVar1;
            C1 = new FireSharp.FirebaseClient(config);

            if (C1 != null)
            {

                Console.Write("connection successful");
            }
            else
            {
                Console.Write("connection successful");
            }

            datT.Columns.Add("PlaceName");
            datT.Columns.Add("DoorName");
            datT.Columns.Add("Status");
            datT.Columns.Add("AuthLvl");
           


            // datT.Columns.Add("Edit");
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
             DataGridViewButtonColumn btn4 = new DataGridViewButtonColumn();
            dataGridView1.Columns.Add(btn4);
            btn4.HeaderText = "Send data ";
            btn4.Text = "Send data ";
            btn4.Name = "btn4";
            btn4.UseColumnTextForButtonValue = true;



            LoadData();












        }

        private  void button2_Click_1(object sender, EventArgs e)
        {
            // dataGridView1.DataSource = null;
            datT.Rows.Clear();
            LoadData();


        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            LocationDashBoard ld = new LocationDashBoard();
            ld.ShowDialog();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //LoadData();
            if (e.ColumnIndex == 4)
            {
                // MessageBox.Show();
                String ABC = e.RowIndex.ToString();
               // MessageBox.Show(ABC);
                if (dataGridView1.SelectedCells.Count > 0)
                {
                    int selectedrowindex = e.RowIndex;
                    DataGridViewRow selectedRow = dataGridView1.Rows[selectedrowindex];
                    string placeName = Convert.ToString(selectedRow.Cells["PlaceName"].Value);
                    string doorN1 = textBox1.Text;
                    string status1 = comboBox2.Text;
                    string authl1 = comboBox1.Text;
                    if (textBox1.Text !=null&&textBox1.Text!=""&&comboBox1.Text !=""&&comboBox1.Text != "select one "&& comboBox2.Text != "lowest" && comboBox2.Text!= "highest" && comboBox2.Text !="" )
                    {
                        EditDoor(placeName, doorN1, status1, authl1);
                    }
                    else
                    {
                        MessageBox.Show("data cannot be invalid or blank!");
                    }
                   
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
                    string placeName = Convert.ToString(selectedRow.Cells["PlaceName"].Value);
                    string doorN1 = Convert.ToString(selectedRow.Cells["DoorName"].Value);
                    string status1 = Convert.ToString(selectedRow.Cells["Status"].Value);
                    string authl1 = Convert.ToString(selectedRow.Cells["AuthLvl"].Value);
                    DeleteDoor(placeName,doorN1,status1,authl1);
                }

            }
            if (e.ColumnIndex == 6)
            {
                // MessageBox.Show();
                String ABC = e.RowIndex.ToString();
                MessageBox.Show(ABC);
                if (dataGridView1.SelectedCells.Count > 0)
                {
                    int selectedrowindex = e.RowIndex;
                    DataGridViewRow selectedRow = dataGridView1.Rows[selectedrowindex];
                    string placeName = Convert.ToString(selectedRow.Cells["PlaceName"].Value);
                    string doorN1 = Convert.ToString(selectedRow.Cells["DoorName"].Value);
                    string status1 = Convert.ToString(selectedRow.Cells["Status"].Value);
                    string authl1 = Convert.ToString(selectedRow.Cells["AuthLvl"].Value);
                    textBox1.Text = doorN1;
                    comboBox2.Text = status1;
                    comboBox1.Text = authl1;
                }

            }
            if (e.ColumnIndex == 7)
            {
                // MessageBox.Show();
                String ABC = e.RowIndex.ToString();
                MessageBox.Show(ABC);
                if (dataGridView1.SelectedCells.Count > 0)
                {
                    int selectedrowindex = e.RowIndex;
                    DataGridViewRow selectedRow = dataGridView1.Rows[selectedrowindex];
                    string placeName = Convert.ToString(selectedRow.Cells["PlaceName"].Value);
                    string doorN1 = Convert.ToString(selectedRow.Cells["DoorName"].Value);
                    string status1 = Convert.ToString(selectedRow.Cells["Status"].Value);
                    string authl1 = Convert.ToString(selectedRow.Cells["AuthLvl"].Value);
                    // textBox1.Text = doorN1;
                    //  comboBox2.Text = status1;
                    // comboBox1.Text = authl1;
                    createDoor(placeName, doorN1, status1, authl1);
                }

            }
        }
        async void createDoor(string PlaceName, string DoorName, string Status, string AuthLvl)
        {
            string LocVar = GlobalVar.GlobalVar4;
            //CHECK IF ITS VALID 

            //checks if the data is in the db...


            FirebaseResponse response1 = await C1.GetTaskAsync("DoorCount/node");
            UserCounter t2 = response1.ResultAs<UserCounter>();

            int checkV = Convert.ToInt32(t2.cnt);
            //this part gets data from the data base 
            bool found = false;
            while (found == false && checkV > 0)
            {
                try
                {
                    FirebaseResponse response = await C1.GetTaskAsync("Doors/" + checkV);
                    DoorData t1 = response.ResultAs<DoorData>();
                    if (PlaceName == t1.placeName && DoorName == t1.DoorName && Status == t1.Status && AuthLvl == t1.AuthLevel)
                    {
                        //THIS IS WORKING
                         

                            string path = LocVar;

                            string fileName = path + @"\DoorCreation.txt";
                            MessageBox.Show(fileName);
                            //         fileName =fileName.Replace(@"\\",@"\");

                            try
                            {
                                // Check if file already exists. If yes, delete it.     
                                if (File.Exists(fileName))
                                {
                                    File.Delete(fileName);
                                }

                                // Create a new file     
                                using (StreamWriter FileWrite = File.CreateText(fileName))
                                {

                                    FileWrite.WriteLine("DoorName: {0:G}", t1.DoorName);

                                }

                                // Open the stream and read it back.    
                                using (StreamReader sr = File.OpenText(fileName))
                                {
                                    string s = "";
                                    while ((s = sr.ReadLine()) != null)
                                    {
                                        Console.WriteLine(s);
                                    }
                                }


                              
                                MessageBox.Show("door generated");

                            }
                            catch (Exception Ex)
                            {
                                Console.WriteLine(Ex.ToString());
                                MessageBox.Show(Ex.ToString());
                            }




                        
                        
                      


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










        async void DeleteDoor(string placeName,string doorName,string status,string AuthLevel)
        {
            FirebaseResponse response1 = await C1.GetTaskAsync("DoorCount/node");
            DoorCount t2 = response1.ResultAs<DoorCount>();

            int checkV = Convert.ToInt32(t2.cnt);

            //this part updates 

            var data = new DoorData
            {
                placeName = placeName,
                DoorName = doorName,
                Status = status,
                AuthLevel = AuthLevel

            };
            bool check = false;
            while (check == false && checkV > 0)
            {
                FirebaseResponse response2 = await C1.GetTaskAsync("Doors/" + checkV);
                DoorData t1 = response2.ResultAs<DoorData>();

                if (t1.placeName == label1.Text)
                {
                    if (t1.DoorName == textBox1.Text)
                    {
                        //SetResponse response = await C1.SetTaskAsync("Location/" + checkV, data);
                        //this deletes specified
                        FirebaseResponse response = await C1.DeleteTaskAsync("Doors/" + checkV);
                        //  Data result = response.ResultAs<Data>();
                        check = true;
                        MessageBox.Show("deleted");
                        LoadData();
                    }

                }



                checkV--;
            }

            if (check == false)
            {
                MessageBox.Show("cannot delete as its not found");
            }
        }



        async void EditDoor(string placeName, string doorName, string status, string AuthLevel)
        {
            FirebaseResponse response1 = await C1.GetTaskAsync("DoorCount/node");
            DoorCount t2 = response1.ResultAs<DoorCount>();

            int checkV = Convert.ToInt32(t2.cnt);

            //this part updates 

            var data = new DoorData
            {
                placeName = placeName,
                DoorName = doorName,
                Status = status,
                AuthLevel = AuthLevel

            };
            bool check = false;
            while (check == false && checkV > 0)
            {
                try
                {
                    FirebaseResponse response2 = await C1.GetTaskAsync("Doors/" + checkV);
                    DoorData t1 = response2.ResultAs<DoorData>();

                    if (t1.placeName == label1.Text)
                    {
                        if (t1.DoorName == textBox1.Text)
                        {
                            //SetResponse response = await C1.SetTaskAsync("Location/" + checkV, data);
                            //this deletes specified
                            FirebaseResponse response = await C1.UpdateTaskAsync("Doors/" + checkV, data);
                            Data result = response.ResultAs<Data>();
                            check = true;
                            MessageBox.Show("edited : " + t1.placeName);
                            LoadData();
                        }

                    }

                }
                catch 
                {
                    //this goes through the entire loop and it was null so dont crash.
                }

                checkV--;
            }

            if (check == false)
            {
                MessageBox.Show("cannot delete as its not found");
            }

        }
       
    }
}
