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
                button2.Visible = true;
                button3.Visible = true;
            }



        
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
                            button2.Visible = false;
                            button3.Visible = false;
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

        private async void button4_Click(object sender, EventArgs e)
        {
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
                                button3.Visible = true;
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
            }
        }

        private async void button5_Click(object sender, EventArgs e)
        {
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
    }
}
