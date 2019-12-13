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
    public partial class Logs : Form
    {
        DataTable datT = new DataTable();
        //this gives the location and the also the authentication password to firesharp
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "kMmaSpqjGX4IwLb4D6pQBZPlZsDU7zK4y6Z6x16Q",
            BasePath = "https://pineappleverification.firebaseio.com/",
        };
        IFirebaseClient C1;
      
        public Logs()
        {
            InitializeComponent();
        }

        private  async void Logs_Load(object sender, EventArgs e)
        {
            // label1.Text = GlobalVar.GlobalVar1;

            string PlaceName = GlobalVar.GlobalVar1;

            C1 = new FireSharp.FirebaseClient(config);

            if (C1 != null)
            {

                Console.Write("connection successful");
            }
            else
            {
                Console.Write("connection successful");
            }

            datT.Columns.Add("Door_Name");
            datT.Columns.Add("First name");
            datT.Columns.Add("Last Name");
            datT.Columns.Add("DateAndTime");
            datT.Columns.Add("LogDesc");
            datT.Columns.Add("PlaceName");

            dataGridView1.DataSource = datT;





            FirebaseResponse response1 = await C1.GetTaskAsync("LogCount/node");
            LogCount t2 = response1.ResultAs<LogCount>();


            int checkV = Convert.ToInt32(t2.cnt);

            while (true && checkV > 0)
            {
                try
                {
                    FirebaseResponse response2 = await C1.GetTaskAsync("Logs/" + checkV);
                    LogData obj2 = response2.ResultAs<LogData>();

                    if (PlaceName== obj2.PlaceName)
                    {
                        DataRow row = datT.NewRow();

                        /*datT.Columns.Add("Door Name");
                        datT.Columns.Add("First name");
                        datT.Columns.Add("Last Name");

                        datT.Columns.Add("DateAndTime");
                        datT.Columns.Add("LogDesc");
                        datT.Columns.Add("PlaceName");*/


                        row["Door_Name"] = obj2.DoorName;
                        row["First name"] = obj2.FirstName;
                        row["Last Name"] = obj2.LastName;
                        row["DateAndTime"] = obj2.DateAndTime;
                        row["LogDesc"] = obj2.LogDesc;
                        row["PlaceName"] = obj2.PlaceName;
                        datT.Rows.Add(row);
                    }
                }
                catch
                {

                }
                checkV--;
            }








        }

        private void button1_Click(object sender, EventArgs e)
        {
            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = string.Format("Door_Name = '{0}'", textBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            LocationDashBoard LD = new LocationDashBoard();
            LD.Show();
        }
    }
    }
