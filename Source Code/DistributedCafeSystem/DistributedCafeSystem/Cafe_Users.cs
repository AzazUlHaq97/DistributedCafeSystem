using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DistributedCafeSystem
{
    public partial class Cafe_Users : Form
    {
        SqlConnection dbcon = new SqlConnection(@"Data Source=HAIER-PC\SQLEXPRESS;Initial Catalog=DistributedCafe;Integrated Security=True");
        public Cafe_Users()
        {
            InitializeComponent();
        }

        public void BindData()
        {
            try
            {
                DataTable dtData = new System.Data.DataTable();
                using (SqlConnection dbCon = new SqlConnection(@"Data Source=HAIER-PC\SQLEXPRESS;Initial Catalog=DistributedCafe;Integrated Security=True"))
                {
                    using (SqlCommand cmdGetData = new SqlCommand("Select * from users", dbCon))
                    {

                        if (dbCon.State == ConnectionState.Closed)
                            dbCon.Open();
                        using (SqlDataReader drGetData = cmdGetData.ExecuteReader())
                        {
                            dtData.Load(drGetData);
                            for (int iCount = 0; iCount < dtData.Rows.Count; iCount++)
                            {
                                DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
                                row.Cells[0].Value = dtData.Rows[iCount][0];
                                row.Cells[1].Value = dtData.Rows[iCount][1];
                                row.Cells[2].Value = dtData.Rows[iCount][2];
                                row.Cells[3].Value = dtData.Rows[iCount][3];
                                row.Cells[4].Value = dtData.Rows[iCount][4];
                                row.Cells[5].Value = dtData.Rows[iCount][5];
                                row.Cells[6].Value = dtData.Rows[iCount][6];
                                dataGridView1.Rows.Add(row);
                                dataGridView1.Rows[iCount].Height = 20;

                            }
                            dataGridView1.AllowUserToAddRows = false;
                            dataGridView1.AutoGenerateColumns = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            { }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string UserName = txtUserName.Text;
            string Location = txtLocation.Text;
            int PhoneNum = Convert.ToInt32(txtPhoneNo.Text);
            string Email = txtEmail.Text;
            string Password = txtPassword.Text;
            string TimeJoined = txtTimeJoined.Text;
            int RoleID = Convert.ToInt16(txtRoleID.Text);
            string query = "Insert into users (user_name,location,phone_no,email,password,time_joined,role_id) values(@user_name,@location,@phone_no,@email,@password,@time_joined,@role_id)";
            using (SqlCommand command = new SqlCommand(query, dbcon))
            {
                command.Parameters.AddWithValue("@user_name", UserName);
                command.Parameters.AddWithValue("@location", Location);
                command.Parameters.AddWithValue("@phone_no", PhoneNum);
                command.Parameters.AddWithValue("@email", Email);
                command.Parameters.AddWithValue("@password", Password);
                command.Parameters.AddWithValue("@time_joined", TimeJoined);
                command.Parameters.AddWithValue("@role_id", RoleID);
                dbcon.Open();
                int result = command.ExecuteNonQuery();
                dbcon.Close();
                if (result < 0)
                    MessageBox.Show("Error inserting data into database");
                else
                    MessageBox.Show("Record Inserted");
            }
        }

        private void btnUpddate_Click(object sender, EventArgs e)
        {
            string UserName = txtUserName.Text;
            string Location = txtLocation.Text;
            int PhoneNum = Convert.ToInt32(txtPhoneNo.Text);
            string Email = txtEmail.Text;
            string Password = txtPassword.Text;
            string TimeJoined = txtTimeJoined.Text;
            int RoleID = Convert.ToInt16(txtRoleID.Text);

            string query = "Update users set user_name=@user_name,location=@location,phone_no=@phone_no,email=@email,password=@password,time_joined=@time_joined,role_id=@role_id where user_name=@user_name";
            using (SqlCommand command = new SqlCommand(query, dbcon))
            {
                command.Parameters.AddWithValue("@user_name", UserName);
                command.Parameters.AddWithValue("@location", Location);
                command.Parameters.AddWithValue("@phone_no", PhoneNum);
                command.Parameters.AddWithValue("@email", Email);
                command.Parameters.AddWithValue("@password", Password);
                command.Parameters.AddWithValue("@time_joined", TimeJoined);
                command.Parameters.AddWithValue("@role_id", RoleID);
                dbcon.Open();
                int result = command.ExecuteNonQuery();
                dbcon.Close();
                if (result < 0)
                    MessageBox.Show("Error in updating record");
                else
                    MessageBox.Show("Record Updated");
            }
        }

        string User_Name, Location, E_Mail, Password, Timejoined;

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string id = txtUserName.Text;
            int selectedIndex = dataGridView1.CurrentCell.RowIndex;
            if (selectedIndex > -1)
            {
                dataGridView1.Rows.RemoveAt(selectedIndex);
            }
            string query = "Delete from users where user_name='" + id + "'";
            using (SqlCommand command = new SqlCommand(query, dbcon))
            {
                dbcon.Open();
                int result = command.ExecuteNonQuery();
                dbcon.Close();
                if (result < 0)
                    MessageBox.Show("Error deleting Record from Database");
                else
                    MessageBox.Show("Record Deleted");
            }
            txtUserName.Clear();
            txtLocation.Clear();
            txtPhoneNo.Clear();
            txtEmail.Clear();
            txtPassword.Clear();
            txtTimeJoined.Clear();
            txtRoleID.Clear();
        }

        int role_ID, phone_Num;
        private void dataGridView1_SelectionChanged_1(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                User_Name = Convert.ToString(row.Cells[0].Value);
                Location = Convert.ToString(row.Cells[1].Value);
                phone_Num = Convert.ToInt32(row.Cells[2].Value);
                E_Mail = Convert.ToString(row.Cells[3].Value);
                Password = Convert.ToString(row.Cells[4].Value);
                Timejoined = Convert.ToString(row.Cells[5].Value);
                role_ID = Convert.ToInt16(row.Cells[6].Value);

                txtUserName.Text = User_Name.ToString();
                txtLocation.Text = Location.ToString();
                txtPhoneNo.Text = phone_Num.ToString();
                txtEmail.Text = E_Mail.ToString();
                txtPassword.Text = Password.ToString();
                txtTimeJoined.Text = Timejoined.ToString();
                txtRoleID.Text = role_ID.ToString();
            }
        }

        private void Cafe_Users_Load(object sender, EventArgs e)
        {
            BindData();
        }
    }
}
