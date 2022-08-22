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
    public partial class Cafe_Menu : Form
    {
        SqlConnection dbcon = new SqlConnection(@"Data Source=HAIER-PC\SQLEXPRESS;Initial Catalog=DistributedCafe;Integrated Security=True");
        public Cafe_Menu()
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
                    using (SqlCommand cmdGetData = new SqlCommand("Select * from Menu_Items", dbCon))
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
            string ItemName = txtItemName.Text;
            string Description = txtDescription.Text;
            int category_ID = Convert.ToInt16(txtCategoryID.Text);
            int Price = Convert.ToInt16(txtPrice.Text);
            string Active = txtActive.Text;

            string query = "Insert into Menu_Items (item_name,description,category_id,price,active) values(@item_name,@description,@category_id,@price,@active)";
            using (SqlCommand command = new SqlCommand(query, dbcon))
            {
                command.Parameters.AddWithValue("@item_name", ItemName);
                command.Parameters.AddWithValue("@description", Description);
                command.Parameters.AddWithValue("@category_id", category_ID);
                command.Parameters.AddWithValue("@price", Price);
                command.Parameters.AddWithValue("@active", Active);
                dbcon.Open();
                int result = command.ExecuteNonQuery();
                dbcon.Close();
                if (result < 0)
                    MessageBox.Show("Error inserting data into database");
                else
                    MessageBox.Show("Record Inserted");
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string ItemName = txtItemName.Text;
            string Description = txtDescription.Text;
            int category_ID = Convert.ToInt16(txtCategoryID.Text);
            int Price = Convert.ToInt16(txtPrice.Text);
            string Active = txtActive.Text;

            string query = "Update Menu_Items set item_name=@itemname,description=@description,category_id=@category_id,price=@price,active=@active where item_name=@itemname";
            using (SqlCommand command = new SqlCommand(query, dbcon))
            {
                command.Parameters.AddWithValue("@itemname", ItemName);
                command.Parameters.AddWithValue("@description", Description);
                command.Parameters.AddWithValue("@category_id", category_ID);
                command.Parameters.AddWithValue("@price", Price);
                command.Parameters.AddWithValue("@active", Active);
                dbcon.Open();
                int result = command.ExecuteNonQuery();
                dbcon.Close();
                if (result < 0)
                    MessageBox.Show("Error in updating record");
                else
                    MessageBox.Show("Record Updated");
            }
            
        }

        private void Cafe_Menu_Load(object sender, EventArgs e)
        {
            BindData();
        }

        string itemName, Description, Active;

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string id = txtItemName.Text;
                int selectedIndex = dataGridView1.CurrentCell.RowIndex;
                if (selectedIndex > -1)
                {
                    dataGridView1.Rows.RemoveAt(selectedIndex);
                }
                string query = "Delete from Menu_Items where item_name='" + id+"'";
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
            txtItemName.Clear();
            txtDescription.Clear();
            txtCategoryID.Clear();
            txtPrice.Clear();
            txtActive.Clear();
        }

        int Category_id, Price;
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            foreach(DataGridViewRow row in dataGridView1.SelectedRows)
            {
                itemName = Convert.ToString(row.Cells[0].Value);
                Description = Convert.ToString(row.Cells[1].Value);
                Category_id = Convert.ToInt16(row.Cells[2].Value);
                Price = Convert.ToInt32(row.Cells[3].Value);
                Active = Convert.ToString(row.Cells[4].Value);
                
               
                txtItemName.Text = itemName.ToString();
                txtDescription.Text = Description.ToString();
                
                txtCategoryID.Text = Category_id.ToString();
                txtPrice.Text = Price.ToString();
                txtActive.Text = Active.ToString();
            }
        }

        


    }
}
