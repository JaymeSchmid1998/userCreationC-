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
            button3.Visible = true;
        }
       

        private async void button3_Click(object sender, EventArgs e)
        {
            FirebaseResponse response1 = await C1.GetTaskAsync("DoorCount/node");
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
                            button3.Visible = true;
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
                MessageBox.Show(ABC);
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
                    string a = Convert.ToString(selectedRow.Cells["PlaceName"].Value);
                    MessageBox.Show(a);
                }

            }
        }
    }
}
