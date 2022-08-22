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
    public partial class Cafe_Category : Form
    {
        SqlConnection dbcon = new SqlConnection(@"Data Source=HAIER-PC\SQLEXPRESS;Initial Catalog=DistributedCafe;Integrated Security=True");
        public Cafe_Category()
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
                    using (SqlCommand cmdGetData = new SqlCommand("Select * from Category", dbCon))
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

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int category_id = Convert.ToInt16(txtCategoryID.Text);
            string category_name = txtCategoryName.Text;

            string query = "Update Category set category_id=@Categoy_ID, category_name=@Category_name where category_id=@Categoy_ID";
            using (SqlCommand command = new SqlCommand(query,dbcon))
            {
                command.Parameters.AddWithValue("@Categoy_ID", category_id);
                command.Parameters.AddWithValue("@Category_name", category_name);
                dbcon.Open();
                int result = command.ExecuteNonQuery();
                dbcon.Close();
                if (result < 0)
                    MessageBox.Show("Error in updating record");
                else
                    MessageBox.Show("Record Updated");
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            int category_id=Convert.ToInt16(txtCategoryID.Text);
            string category_name=txtCategoryName.Text;

            string query = "Insert into Category (category_id,category_name) values(@CategoryID,@CategoryName)";
            using (SqlCommand command = new SqlCommand(query, dbcon))
            {
                command.Parameters.AddWithValue("@CategoryID",category_id);
                command.Parameters.AddWithValue("@CategoryName", category_name);
                dbcon.Open();
                int result = command.ExecuteNonQuery();
                dbcon.Close();
                if (result < 0)
                    MessageBox.Show("Error inserting data into database");
                else
                    MessageBox.Show("Record Inserted");
            }
        }
        int categoryID;
        string Category_Name;

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
           
            foreach(DataGridViewRow row in dataGridView1.SelectedRows)
            {
                categoryID = Convert.ToInt16(row.Cells[0].Value);
                Category_Name = Convert.ToString(row.Cells[1].Value);
                txtCategoryID.Text = categoryID.ToString();
                txtCategoryName.Text = Category_Name.ToString();
            }
        }

        private void Cafe_Category_Load(object sender, EventArgs e)
        {
            BindData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int selectedIndex = dataGridView1.CurrentCell.RowIndex;
            if(selectedIndex>-1)
            {
                dataGridView1.Rows.RemoveAt(selectedIndex);
            }
            string query = "Delete from Category where category_id=" + txtCategoryID.Text;
            using (SqlCommand command = new SqlCommand(query,dbcon))
            {
                dbcon.Open();
                int result= command.ExecuteNonQuery();
                dbcon.Close();
                if (result < 0)
                    MessageBox.Show("Error deleting Record from Database");
                else
                    MessageBox.Show("Record Deleted");
            }
            txtCategoryID.Clear();
            txtCategoryName.Clear();
        }
    }
}
