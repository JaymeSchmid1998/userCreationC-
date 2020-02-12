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
    public partial class UserManagementcs : Form
    {
        DataTable datT = new DataTable();
        //this gives the location and the also the authentication password to firesharp
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "kMmaSpqjGX4IwLb4D6pQBZPlZsDU7zK4y6Z6x16Q",
            BasePath = "https://pineappleverification.firebaseio.com/",
        };
        IFirebaseClient C1;
        public UserManagementcs()
        {
            InitializeComponent();
        }

        private void UserManagementcs_Load(object sender, EventArgs e)
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

            datT.Columns.Add("Place name");
            datT.Columns.Add("First name");
            datT.Columns.Add("last name");
            datT.Columns.Add("authcode ");
            datT.Columns.Add("status");
            datT.Columns.Add("authlevel");
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
            btn4.HeaderText = "Create id card";
            btn4.Text = "Create id card";
            btn4.Name = "btn4";
            btn4.UseColumnTextForButtonValue = true;

            LoadUserData();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == ""|| textBox1.Text == " "|| textBox1.Text == null|| textBox2.Text == "" || textBox2.Text == " " || textBox2.Text == null)
            {
                MessageBox.Show("please enter in a valid username and password");

            }
            else if (comboBox1.Text== "select one " || comboBox1.Text == "" || comboBox1.Text == null)
            {
                MessageBox.Show("please select a valid status");
            }
            else
            {
                FirebaseResponse response1 = await C1.GetTaskAsync("UserCount/node");
                LocationCounter t1 = response1.ResultAs<LocationCounter>();
                //  MessageBox.Show(t1.cnt);

               



                FirebaseResponse response2 = await C1.GetTaskAsync("UserCount/node");
                UserCounter t3 = response2.ResultAs<UserCounter>();
                int checkV = Convert.ToInt32(t3.cnt);

                long randgen = getrand1();
                bool isrand = false;
                while (isrand == false && checkV > 0 )
                {
                    randgen = getrand1();

                    try
                    {
                        FirebaseResponse response3 = await C1.GetTaskAsync("Users/" + checkV);
                        UserData t13 = response3.ResultAs<UserData>();
                        if (randgen.Equals(t13.AuthCode))
                        {
                            isrand = true;
                            randgen = 0;
                        }
                    }
                    catch
                    {

                    }
                    checkV--;


                }
                if (isrand == false && randgen != 0)
                {
                    MessageBox.Show("not found");
                    //save this to the global variable
                    GlobalVar.GlobalVar2 = randgen;
                }























                var data = new UserData
                {
                    UserId = (Convert.ToInt32(t1.cnt) + 1).ToString(),
                    //  placeName = textBox3.Text,
                    FirstName = textBox1.Text,
                    LastName = textBox2.Text,
                    PlaceName = label1.Text,
                    //authcode
                    CodeUsed = "No",
                    AuthCode = GlobalVar.GlobalVar2.ToString(),

                    //status
                    Status = comboBox1.Text,
                    //auth level
                    AuthLevel = comboBox2.Text,

                };

                 SetResponse response = await C1.SetTaskAsync("Users/" + data.UserId, data);
                Data result = response.ResultAs<Data>();

                var LocData = new LocationCounter
                {
                    cnt = data.UserId,

                };

                SetResponse resp1 = await C1.SetTaskAsync("UserCount/node", LocData);
                label9.Text = data.AuthCode;
            //    button2.Visible = true;
              //  button3.Visible = true;
            }


            LoadUserData();
        
        }
     private long getrand1()
        {
            long rand;
            Random random = new Random();
            rand = random.Next(100000000, 999999999);
            return rand;
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            FirebaseResponse response1 = await C1.GetTaskAsync("UserCount/node");
            LocationCounter t2 = response1.ResultAs<LocationCounter>();

            int checkV = Convert.ToInt32(t2.cnt);

            //this part updates 

            var data = new UserData
            {
                PlaceName = label1.Text,
                FirstName = textBox1.Text,
                LastName = textBox2.Text

            };
            bool check = false;
            while (check == false && checkV > 0)
            {
                FirebaseResponse response2 = await C1.GetTaskAsync("Users/" + checkV);
               UserData t1 = response2.ResultAs<UserData>();

                if (t1.PlaceName == label1.Text)
                {
                    if (t1.FirstName == textBox1.Text)
                    {
                        if (t1.LastName == textBox2.Text)
                        {
                            FirebaseResponse response = await C1.DeleteTaskAsync("Users/" + checkV);
                            //  Data result = response.ResultAs<Data>();
                            check = true;
                            MessageBox.Show("deleted");
                         //   button2.Visible = false;
                         //   button3.Visible = false;
                            textBox1.Text = "";
                            textBox2.Text = "";
                            comboBox1.Text = "";
                            comboBox2.Text = "";
                            label9.Text = "no code";
                        }
                    }
                    //SetResponse response = await C1.SetTaskAsync("Location/" + checkV, data);
                    //this deletes specified
                    
                }



                checkV--;
            }

            if (check == false)
            {
                MessageBox.Show("cannot delete as its not found");
            }


        }

       private void button4_Click(object sender, EventArgs e)
        {
          /*  FirebaseResponse response1 = await C1.GetTaskAsync("UserCount/node");
            UserCounter t2 = response1.ResultAs<UserCounter>();

            int checkV = Convert.ToInt32(t2.cnt);
            //this part gets data from the data base 
            bool found = false;
            while (found == false && checkV > 0)
            {
                try
                {
                    FirebaseResponse response = await C1.GetTaskAsync("Users/" + checkV);
                   UserData t1 = response.ResultAs<UserData>();
                    if (label1.Text == t1.PlaceName)
                    {
                        if (textBox1.Text == t1.FirstName)
                        {
                            if (textBox2.Text == t1.LastName)
                            {
                                comboBox1.Text = t1.Status;
                                comboBox2.Text = t1.AuthLevel;
                                label9.Text = t1.AuthCode;

                                MessageBox.Show(" found");
                                found = true;
                               // button3.Visible = true;
                                button2.Visible = true;


                            }
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
            }*/
        }

        private  void button5_Click(object sender, EventArgs e)
        {
            LoadUserData();
           /* FirebaseResponse response1 = await C1.GetTaskAsync("UserCount/node");
            LocationCounter t2 = response1.ResultAs<LocationCounter>();


            int checkV = Convert.ToInt32(t2.cnt);

            while (true && checkV > 0)
            {
                try
                {
                    FirebaseResponse response2 = await C1.GetTaskAsync("Users/" + checkV);
                    UserData obj2 = response2.ResultAs<UserData>();
                  

                     datT.Columns.Add("Place name");
                     datT.Columns.Add("First name");
                     datT.Columns.Add("last name");
                     datT.Columns.Add("authcode ");
                     datT.Columns.Add("status");
                     datT.Columns.Add("authlevel");
                    if (label1.Text == obj2.PlaceName)
                    {
                        DataRow row = datT.NewRow();
                        row["Place name"] = obj2.PlaceName;
                        row["First name"] = obj2.FirstName;
                        row["last name"] = obj2.LastName;
                        row["authcode "] = obj2.AuthCode;
                        row["status"] = obj2.Status;
                        row["authlevel"] = obj2.AuthLevel;

                        datT.Rows.Add(row);

                        

                    }
                   
                }
                catch
                {

                }
                checkV--;
            }*/
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            string LocVar = GlobalVar.GlobalVar3;
            //CHECK IF ITS VALID 
            if (textBox1.Text == "" || textBox1.Text == " " || textBox1.Text == null || textBox2.Text == "" || textBox2.Text == " " || textBox2.Text == null)
            {
                MessageBox.Show("please enter in a valid username and password");

            }
            else if (comboBox1.Text == "select one " || comboBox1.Text == "" || comboBox1.Text == null|| comboBox2.Text == "lowest" || comboBox2.Text == "highest" || comboBox2.Text == "" || comboBox2.Text == null)
            {
                MessageBox.Show("please select a valid status or auth level");
            }
            else if(label9.Text== "no code")
            {

            }
            else
            {
                //checks if the data is in the db...


                FirebaseResponse response1 = await C1.GetTaskAsync("UserCount/node");
                UserCounter t2 = response1.ResultAs<UserCounter>();

                int checkV = Convert.ToInt32(t2.cnt);
                //this part gets data from the data base 
                bool found = false;
                while (found == false && checkV > 0)
                {
                    try
                    {
                        FirebaseResponse response = await C1.GetTaskAsync("Users/" + checkV);
                        UserData t1 = response.ResultAs<UserData>();
                        if (label1.Text == t1.PlaceName && textBox1.Text == t1.FirstName&& textBox2.Text == t1.LastName&& comboBox1.Text==t1.Status&&comboBox2.Text==t1.AuthLevel &&label9.Text==t1.AuthCode)
                        {
                          //THIS IS WORKING
                            if (t1.CodeUsed == "No")
                            {//send out code 
                                
                                string path = LocVar;

                                string fileName = path+@"\UserCreation.txt";

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

                                        FileWrite.WriteLine("AuthCode: {0:G}", t1.AuthCode);
                                      
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


                                    var data = new UserData
                                    {
                                        UserId = checkV.ToString(),
                                        //  placeName = textBox3.Text,
                                        FirstName = textBox1.Text,
                                        LastName = textBox2.Text,
                                        PlaceName = label1.Text,
                                        //authcode
                                        CodeUsed = "Yes",
                                        AuthCode = t1.AuthCode,

                                        //status
                                        Status = comboBox1.Text,
                                        //auth level
                                        AuthLevel = comboBox2.Text,

                                    };

                                    SetResponse response6 = await C1.SetTaskAsync("Users/" + checkV, data);
                                    Data result = response6.ResultAs<Data>();                       
                                    MessageBox.Show("updated");

                                }
                                catch (Exception Ex)
                                {
                                    Console.WriteLine(Ex.ToString());
                                }





                            }
                            else
                            {
                                //generate a new code then write it to the file


                                FirebaseResponse response2 = await C1.GetTaskAsync("UserCount/node");
                                UserCounter t3 = response2.ResultAs<UserCounter>();
                               int checkV1 = Convert.ToInt32(t3.cnt);

                                long randgen = getrand1();
                                bool isrand = false;
                                while (isrand == false && checkV1 > 0)
                                {
                                    randgen = getrand1();

                                    try
                                    {
                                        FirebaseResponse response3 = await C1.GetTaskAsync("Users/" + checkV1);
                                        UserData t13 = response3.ResultAs<UserData>();
                                        if (randgen.Equals(t13.AuthCode))
                                        {
                                            isrand = true;
                                            randgen = 0;
                                        }
                                    }
                                    catch
                                    {

                                    }
                                    checkV1--;


                                }
                                if (isrand == false && randgen != 0)
                                {
                                    MessageBox.Show("not found");
                                    //save this to the global variable
                                    GlobalVar.GlobalVar2 = randgen;
                                }
















                                string path = LocVar;

                                string fileName = path + @"\UserCreation.txt";

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

                                        FileWrite.WriteLine("AuthCode: {0:G}", GlobalVar.GlobalVar2.ToString());

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


                                    var data = new UserData
                                    {
                                        UserId = checkV.ToString(),
                                        //  placeName = textBox3.Text,
                                        FirstName = textBox1.Text,
                                        LastName = textBox2.Text,
                                        PlaceName = label1.Text,
                                        //authcode
                                        CodeUsed = "Yes",
                                        AuthCode = GlobalVar.GlobalVar2.ToString(),

                                        //status
                                        Status = comboBox1.Text,
                                        //auth level
                                        AuthLevel = comboBox2.Text,

                                    };

                                    SetResponse response6 = await C1.SetTaskAsync("Users/" + checkV, data);
                                    Data result = response6.ResultAs<Data>();
                                    MessageBox.Show("updated");

                                }
                                catch (Exception Ex)
                                {
                                    Console.WriteLine(Ex.ToString());
                                }























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

            //CHECK IF I CAN WRITE TEH DATA TO A FILE

            //IF ITS NOT VALID THEN  GENERATE A NEW NUMEBR

            //AND THEN SAVE IT TO THE LOCATION 

        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            LocationDashBoard ld = new LocationDashBoard();
            ld.ShowDialog();
        }

        async void  deleteUser(string PlaceName1, string firstName1, string lastname1)
        {
            FirebaseResponse response1 = await C1.GetTaskAsync("UserCount/node");
            LocationCounter t2 = response1.ResultAs<LocationCounter>();

            int checkV = Convert.ToInt32(t2.cnt);

            //this part updates 

            var data = new UserData
            {
                PlaceName = PlaceName1,
                FirstName = firstName1,
                LastName = lastname1

            };
            bool check = false;
            while (check == false && checkV > 0)
            {
                FirebaseResponse response2 = await C1.GetTaskAsync("Users/" + checkV);
                UserData t1 = response2.ResultAs<UserData>();

                if (t1.PlaceName == PlaceName1)
                {
                    if (t1.FirstName == firstName1)
                    {
                        if (t1.LastName == lastname1)
                        {
                            FirebaseResponse response = await C1.DeleteTaskAsync("Users/" + checkV);
                            //  Data result = response.ResultAs<Data>();
                            check = true;
                            MessageBox.Show("deleted");
                           // button2.Visible = false;
                           // button3.Visible = false;
                            textBox1.Text = "";
                            textBox2.Text = "";
                            comboBox1.Text = "";
                            comboBox2.Text = "";
                            label9.Text = "no code";
                        }
                    }
                    //SetResponse response = await C1.SetTaskAsync("Location/" + checkV, data);
                    //this deletes specified

                }



                checkV--;
            }

            if (check == false)
            {
                MessageBox.Show("cannot delete as its not found");
            }
            LoadUserData();
        }
async void LoadUserData()
        {
            datT.Rows.Clear();
            FirebaseResponse response1 = await C1.GetTaskAsync("UserCount/node");
            LocationCounter t2 = response1.ResultAs<LocationCounter>();


            int checkV = Convert.ToInt32(t2.cnt);

            while (true && checkV > 0)
            {
                try
                {
                    FirebaseResponse response2 = await C1.GetTaskAsync("Users/" + checkV);
                    UserData obj2 = response2.ResultAs<UserData>();


                    /* datT.Columns.Add("Place name");
                     datT.Columns.Add("First name");
                     datT.Columns.Add("last name");
                     datT.Columns.Add("authcode ");
                     datT.Columns.Add("status");
                     datT.Columns.Add("authlevel");*/
                    if (label1.Text == obj2.PlaceName)
                    {
                        DataRow row = datT.NewRow();
                        row["Place name"] = obj2.PlaceName;
                        row["First name"] = obj2.FirstName;
                        row["last name"] = obj2.LastName;
                        row["authcode "] = obj2.AuthCode;
                        row["status"] = obj2.Status;
                        row["authlevel"] = obj2.AuthLevel;

                        datT.Rows.Add(row);



                    }

                }
                catch
                {

                }
                checkV--;
            }
        }

async void EditUserData(string placeName,string firstName,string lastName,string Authcode,string AuthLevel,string status )
        {
           


            FirebaseResponse response1 = await C1.GetTaskAsync("UserCount/node");
            LocationCounter t2 = response1.ResultAs<LocationCounter>();

            int checkV = Convert.ToInt32(t2.cnt);

            //this part updates 

            var data = new UserData
            {
                UserId = (Convert.ToInt32(t2.cnt) + 1).ToString(),
                //  placeName = textBox3.Text,
                FirstName = textBox1.Text,
                LastName = textBox2.Text,
                PlaceName = label1.Text,
                //authcode
                CodeUsed = "No",
                AuthCode = Authcode,

                //status
                Status = comboBox1.Text,
                //auth level
                AuthLevel = comboBox2.Text,

            };
            bool check = false;
            while (check == false && checkV > 0)
            {
                try
                {
                    FirebaseResponse response2 = await C1.GetTaskAsync("Users/" + checkV);
                    UserData t1 = response2.ResultAs<UserData>();

                    if (t1.PlaceName == placeName)
                    {
                        if (t1.FirstName == firstName)
                        {
                            if (t1.LastName == lastName)
                            {
                                FirebaseResponse response = await C1.UpdateTaskAsync("Users/" + checkV, data);
                                //  Data result = response.ResultAs<Data>();
                                check = true;
                                MessageBox.Show("Updated");
                                // button2.Visible = false;
                                // button3.Visible = false;
                                textBox1.Text = "";
                                textBox2.Text = "";
                                comboBox1.Text = "";
                                comboBox2.Text = "";
                                label9.Text = "no code";
                            }
                        }
                        //SetResponse response = await C1.SetTaskAsync("Location/" + checkV, data);
                        //this deletes specified

                    }
                }
                catch
                {

                }


                checkV--;
            }

            if (check == false)
            {
                MessageBox.Show("cannot delete as its not found");
            }
            LoadUserData();






















        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            //LoadData();
            if (e.ColumnIndex == 6)
            {
                // MessageBox.Show();
                String ABC = e.RowIndex.ToString();
                // MessageBox.Show(ABC);
                if (dataGridView1.SelectedCells.Count > 0)
                {
                    int selectedrowindex = e.RowIndex;
                    DataGridViewRow selectedRow = dataGridView1.Rows[selectedrowindex];
                    string placeName = Convert.ToString(selectedRow.Cells["Place name"].Value);
                    string firstN1 = textBox1.Text;
                    string lastN1 = textBox2.Text;
                    string status1 = comboBox1.Text;
                    string authC1 = comboBox2.Text;
                    string auth1 = label9.Text;
                    if (textBox1.Text != null && textBox1.Text != "" && textBox2.Text != null && textBox2.Text != "" && comboBox1.Text != "" && comboBox1.Text != "select one " && comboBox2.Text != "lowest" && comboBox2.Text != "highest" && comboBox2.Text != ""&&label9.Text !="no code")
                    {
                        EditUserData(placeName,firstN1,lastN1,auth1,authC1,status1);
                    }
                    else
                    {
                        MessageBox.Show("data cannot be invalid or blank!");
                    }

                }
            }
            if (e.ColumnIndex == 7)
            {
                // MessageBox.Show();
                String ABC = e.RowIndex.ToString();
              //  MessageBox.Show(ABC);
                if (dataGridView1.SelectedCells.Count > 0)
                {
                    int selectedrowindex = e.RowIndex;
                    DataGridViewRow selectedRow = dataGridView1.Rows[selectedrowindex];
                    string placeName = Convert.ToString(selectedRow.Cells["Place name"].Value);
                    string firstName = Convert.ToString(selectedRow.Cells["First name"].Value);
                    string lastName = Convert.ToString(selectedRow.Cells["last name"].Value);
                    //    
                    deleteUser(placeName, firstName, lastName);
                }

            }
            if (e.ColumnIndex == 8)
            {
                // MessageBox.Show();
                String ABC = e.RowIndex.ToString();
                //MessageBox.Show(ABC);
                if (dataGridView1.SelectedCells.Count > 0)
                {


                    /*
                       row["Place name"] = obj2.PlaceName;
                                row["First name"] = obj2.FirstName;
                                row["last name"] = obj2.LastName;
                                row["authcode "] = obj2.AuthCode;
                                row["status"] = obj2.Status;
                                row["authlevel"] = obj2.AuthLevel; 




                    */




                    int selectedrowindex = e.RowIndex;
                    DataGridViewRow selectedRow = dataGridView1.Rows[selectedrowindex];
                    string placeName = Convert.ToString(selectedRow.Cells["Place name"].Value);
                    string firstN1 = Convert.ToString(selectedRow.Cells["First name"].Value);
                    string lastN1 = Convert.ToString(selectedRow.Cells["last name"].Value);
                    string status1 = Convert.ToString(selectedRow.Cells["status"].Value);
                    string authC1 = Convert.ToString(selectedRow.Cells["authcode "].Value);
                    string auth1 = Convert.ToString(selectedRow.Cells["authlevel"].Value);
                    textBox1.Text = firstN1;
                    textBox2.Text = lastN1;
                    comboBox2.Text = auth1;
                    comboBox1.Text = status1;
                    label9.Text = authC1;
                }

            }
            if (e.ColumnIndex == 9)
            {
                // MessageBox.Show();
                String ABC = e.RowIndex.ToString();
               // MessageBox.Show(ABC);
                if (dataGridView1.SelectedCells.Count > 0)
                {
                    int selectedrowindex = e.RowIndex;
                    DataGridViewRow selectedRow = dataGridView1.Rows[selectedrowindex];
                    string placeName = Convert.ToString(selectedRow.Cells["Place name"].Value);
                    string firstName = Convert.ToString(selectedRow.Cells["First name"].Value);
                    string lastName = Convert.ToString(selectedRow.Cells["last name"].Value);
                    string status1 = Convert.ToString(selectedRow.Cells["status"].Value);
                    string authC1 = Convert.ToString(selectedRow.Cells["authcode "].Value);
                    string auth1 = Convert.ToString(selectedRow.Cells["authlevel"].Value);
                    //    
                    createId(placeName,firstName,lastName,status1,auth1,authC1);
                    
                }
                LoadUserData();
               
            }
        }

        async void createId(string PlaceName,string FirstName,string LastName,string status,string authlevel,string authcode)
        {
            string LocVar = GlobalVar.GlobalVar3;
            //CHECK IF ITS VALID 
        
                //checks if the data is in the db...


                FirebaseResponse response1 = await C1.GetTaskAsync("UserCount/node");
                UserCounter t2 = response1.ResultAs<UserCounter>();

                int checkV = Convert.ToInt32(t2.cnt);
                //this part gets data from the data base 
                bool found = false;
                while (found == false && checkV > 0)
                {
                    try
                    {
                        FirebaseResponse response = await C1.GetTaskAsync("Users/" + checkV);
                        UserData t1 = response.ResultAs<UserData>();
                        if (PlaceName == t1.PlaceName && FirstName == t1.FirstName && LastName == t1.LastName && status == t1.Status && authlevel == t1.AuthLevel && authcode == t1.AuthCode)
                        {
                            //THIS IS WORKING
                            if (t1.CodeUsed == "No")
                            {//send out code 

                                string path = LocVar;

                                string fileName = path + @"\UserCreation.txt";
                          //  MessageBox.Show(fileName);
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

                                        FileWrite.WriteLine("AuthCode: {0:G}", t1.AuthCode);

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


                                    var data = new UserData
                                    {
                                        UserId = checkV.ToString(),
                                        //  placeName = textBox3.Text,
                                        FirstName = FirstName,
                                        LastName = LastName,
                                        PlaceName = PlaceName,
                                        //authcode
                                        CodeUsed = "Yes",
                                        AuthCode = authcode,

                                        //status
                                        Status = status,
                                        //auth level
                                        AuthLevel = authlevel,

                                    };

                                    SetResponse response6 = await C1.SetTaskAsync("Users/" + checkV, data);
                                    Data result = response6.ResultAs<Data>();
                                  //  MessageBox.Show("updated");

                                }
                                catch (Exception Ex)
                                {
                                    Console.WriteLine(Ex.ToString());
                                MessageBox.Show(Ex.ToString());
                                }





                            }
                            else
                            {
                                //generate a new code then write it to the file


                                FirebaseResponse response2 = await C1.GetTaskAsync("UserCount/node");
                                UserCounter t3 = response2.ResultAs<UserCounter>();
                                int checkV1 = Convert.ToInt32(t3.cnt);

                                long randgen = getrand1();
                                bool isrand = false;
                                while (isrand == false && checkV1 > 0)
                                {
                                    randgen = getrand1();

                                    try
                                    {
                                        FirebaseResponse response3 = await C1.GetTaskAsync("Users/" + checkV1);
                                        UserData t13 = response3.ResultAs<UserData>();
                                        if (randgen.Equals(t13.AuthCode))
                                        {
                                            isrand = true;
                                            randgen = 0;
                                        }
                                    }
                                    catch
                                    {

                                    }
                                    checkV1--;


                                }
                                if (isrand == false && randgen != 0)
                                {
                                   // MessageBox.Show("not found");
                                    //save this to the global variable
                                    GlobalVar.GlobalVar2 = randgen;
                                }
















                                string path = LocVar;

                                string fileName = path + @"\UserCreation.txt";

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

                                        FileWrite.WriteLine("AuthCode: {0:G}", GlobalVar.GlobalVar2.ToString());

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


                                    var data = new UserData
                                    {
                                        UserId = checkV.ToString(),
                                        //  placeName = textBox3.Text,
                                        FirstName = FirstName,
                                        LastName = LastName,
                                        PlaceName = PlaceName,
                                        //authcode
                                        CodeUsed = "Yes",
                                        AuthCode = GlobalVar.GlobalVar2.ToString(),

                                        //status
                                        Status = status,
                                        //auth level
                                        AuthLevel = authlevel,

                                    };

                                    SetResponse response6 = await C1.SetTaskAsync("Users/" + checkV, data);
                                    Data result = response6.ResultAs<Data>();
                                  //  MessageBox.Show("updated");

                                }
                                catch (Exception Ex)
                                {
                                    Console.WriteLine(Ex.ToString());
                                }























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
                  //  MessageBox.Show("not found");
                }


















            
        }
    }
}
