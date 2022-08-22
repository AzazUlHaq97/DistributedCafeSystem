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
    public partial class SignUp : Form
    {
        SqlConnection dbcon = new SqlConnection(@"Data Source=HAIER-PC\SQLEXPRESS;Initial Catalog=DistributedCafe;Integrated Security=True");

        public SignUp()
        {
            InitializeComponent();
        }

        private void btnSignp_Click(object sender, EventArgs e)
        {
            string UserName = txtUserName.Text;
            string Location = txtLocation.Text;
            int PhoneNum = Convert.ToInt32(txtPhoneNum.Text);
            string Email = txtEMail.Text;
            string Password = txtPassword.Text;
            
            int RoleID = Convert.ToInt16(txtRoleID.Text);
            string query = "Insert into users (user_name,location,phone_no,email,password,role_id) values(@user_name,@location,@phone_no,@email,@password,@role_id)";
            using (SqlCommand command = new SqlCommand(query, dbcon))
            {
                command.Parameters.AddWithValue("@user_name", UserName);
                command.Parameters.AddWithValue("@location", Location);
                command.Parameters.AddWithValue("@phone_no", PhoneNum);
                command.Parameters.AddWithValue("@email", Email);
                command.Parameters.AddWithValue("@password", Password);
                
                command.Parameters.AddWithValue("@role_id", RoleID);
                dbcon.Open();
                int result = command.ExecuteNonQuery();
                dbcon.Close();
                if (result < 0)
                    MessageBox.Show("Error!");
                else
                    MessageBox.Show("SignedUp Seuccessfuly");
            }
        }
    }
}
