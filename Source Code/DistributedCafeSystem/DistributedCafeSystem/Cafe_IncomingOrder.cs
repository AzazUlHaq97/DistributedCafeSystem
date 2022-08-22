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
    public partial class Cafe_IncomingOrder : Form
    {
        public Cafe_IncomingOrder()
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
                    using (SqlCommand cmdGetData = new SqlCommand("Select * from in_order", dbCon))
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
        SqlConnection dbcon = new SqlConnection(@"Data Source=HAIER-PC\SQLEXPRESS;Initial Catalog=DistributedCafe;Integrated Security=True");
        private void btnAdd_Click(object sender, EventArgs e)
        {
            int OrderNo = Convert.ToInt16(txtOrderNo.Text);
            string ItemName = txtItemName.Text;
            int Quantity= Convert.ToInt16(txtQuantity.Text);

            string query = "Insert into in_order (order_number,item_name,qty) values(@order_number,@item_name,@quantity)";
            using (SqlCommand command = new SqlCommand(query, dbcon))
            {
                command.Parameters.AddWithValue("@order_number", OrderNo);
                command.Parameters.AddWithValue("@item_name", ItemName);
                command.Parameters.AddWithValue("@quantity", Quantity);
                dbcon.Open();
                int result = command.ExecuteNonQuery();
                dbcon.Close();
                if (result < 0)
                    MessageBox.Show("Error inserting data into database");
                else
                    MessageBox.Show("Record Inserted");
            }
        }
        
        private void Cafe_IncomingOrder_Load(object sender, EventArgs e)
        {
            BindData();
        }
        int orderNo, Quantity;
        string itemName;

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int OrderNo = Convert.ToInt16(txtOrderNo.Text);
            string ItemName = txtItemName.Text;
            int Quantity = Convert.ToInt16(txtQuantity.Text);

            string query = "Update in_order set order_number=@order_number,item_name=@item_name,qty=@quantity where order_number=@order_number";
            using (SqlCommand command = new SqlCommand(query, dbcon))
            {
                command.Parameters.AddWithValue("@order_number", OrderNo);
                command.Parameters.AddWithValue("@item_name", ItemName);
                command.Parameters.AddWithValue("@quantity", Quantity);
                dbcon.Open();
                int result = command.ExecuteNonQuery();
                dbcon.Close();
                if (result < 0)
                    MessageBox.Show("Error in updating record");
                else
                    MessageBox.Show("Record Updated");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int selectedIndex = dataGridView1.CurrentCell.RowIndex;
            if (selectedIndex > -1)
            {
                dataGridView1.Rows.RemoveAt(selectedIndex);
            }
            string query = "Delete from in_order where order_number=" + txtOrderNo.Text;
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
            txtOrderNo.Clear();
            txtItemName.Clear();
            txtQuantity.Clear();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                orderNo = Convert.ToInt16(row.Cells[0].Value);
                itemName = Convert.ToString(row.Cells[1].Value);
                Quantity = Convert.ToInt16(row.Cells[2].Value);
                txtOrderNo.Text = orderNo.ToString();
                txtItemName.Text = itemName.ToString();
                txtQuantity.Text = Quantity.ToString();
            }
        }
    }
}
