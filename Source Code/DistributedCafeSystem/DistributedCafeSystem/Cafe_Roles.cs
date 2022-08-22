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
    public partial class Cafe_Roles : Form
    {
        SqlConnection dbcon = new SqlConnection(@"Data Source=HAIER-PC\SQLEXPRESS;Initial Catalog=DistributedCafe;Integrated Security=True");

        public Cafe_Roles()
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
                    using (SqlCommand cmdGetData = new SqlCommand("Select * from Role", dbCon))
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
            int role_id = Convert.ToInt16(txtRoleID.Text);
            string Role = txtRole.Text;

            string query = "Insert into Role (role_id,role) values(@role_id,@role)";
            using (SqlCommand command = new SqlCommand(query, dbcon))
            {
                command.Parameters.AddWithValue("@role_id", role_id);
                command.Parameters.AddWithValue("@role", Role);
                dbcon.Open();
                int result = command.ExecuteNonQuery();
                dbcon.Close();
                if (result < 0)
                    MessageBox.Show("Error inserting data into database");
                else
                    MessageBox.Show("Record Inserted");
            }
        }

        private void Cafe_Roles_Load(object sender, EventArgs e)
        {
            BindData();
        }
        int Role_ID;
        string Role;
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                Role_ID = Convert.ToInt16(row.Cells[0].Value);
                Role = Convert.ToString(row.Cells[1].Value);
                txtRoleID.Text = Role_ID.ToString();
                txtRole.Text = Role.ToString();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int selectedIndex = dataGridView1.CurrentCell.RowIndex;
            if (selectedIndex > -1)
            {
                dataGridView1.Rows.RemoveAt(selectedIndex);
            }
            string query = "Delete from Role where role_id=" + txtRoleID.Text;
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
            txtRole.Clear();
            txtRoleID.Clear();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int role_id = Convert.ToInt16(txtRoleID.Text);
            string Role = txtRole.Text;

            string query = "Update Role set role_id=@role_id,role=@role where role_id=@role_id";
            using (SqlCommand command = new SqlCommand(query, dbcon))
            {
                command.Parameters.AddWithValue("@role_id", role_id);
                command.Parameters.AddWithValue("@role", Role);
                dbcon.Open();
                int result = command.ExecuteNonQuery();
                dbcon.Close();
                if (result < 0)
                    MessageBox.Show("Error in updating record");
                else
                    MessageBox.Show("Record Updated");
            }
            dataGridView1.ClearSelection();
            BindData();
        }
    }
}
