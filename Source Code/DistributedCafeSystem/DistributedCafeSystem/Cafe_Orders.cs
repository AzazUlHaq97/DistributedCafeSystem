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

    public partial class Cafe_Orders : Form
    {
        SqlConnection dbcon = new SqlConnection(@"Data Source=HAIER-PC\SQLEXPRESS;Initial Catalog=DistributedCafe;Integrated Security=True");
        public Cafe_Orders()
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
                    using (SqlCommand cmdGetData = new SqlCommand("Select * from orders", dbCon))
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
            int orderNo = Convert.ToInt16(txtOrderNo.Text);
            string UserName = txtUserName.Text;
            string OrderTime= txtOrderTime.Text;
            string ActualDeliveryTime=txtActualDelTime.Text;
            int TotalBill=Convert.ToInt32(txtTotalBill.Text);
            string Comments=txtComments.Text;
            string query = "Insert into orders (order_number,user_name,order_time,actual_delivery_time,total_bill,comment) values(@order_number,@user_name,@order_time,@actual_delivery_time,@total_bill,@comment)";
            using (SqlCommand command = new SqlCommand(query, dbcon))
            {
                command.Parameters.AddWithValue("@order_number", orderNo);
                command.Parameters.AddWithValue("@user_name", UserName);
                command.Parameters.AddWithValue("@order_time", OrderTime);
                command.Parameters.AddWithValue("@actual_delivery_time", ActualDeliveryTime);
                command.Parameters.AddWithValue("@total_bill", TotalBill);
                command.Parameters.AddWithValue("@comment", Comments);
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
            int orderNo = Convert.ToInt16(txtOrderNo.Text);
            string UserName = txtUserName.Text;
            string OrderTime = txtOrderTime.Text;
            string ActualDeliveryTime = txtActualDelTime.Text;
            int TotalBill = Convert.ToInt32(txtTotalBill.Text);
            string Comments = txtComments.Text;

            string query = "Update orders set order_number=@order_number,user_name=@user_name,order_time=@order_time,actual_delivery_time=@actual_delivery_time,total_bill=@total_bill,comment=@comment where order_number=@order_number";
            using (SqlCommand command = new SqlCommand(query, dbcon))
            {
                command.Parameters.AddWithValue("@order_number", orderNo);
                command.Parameters.AddWithValue("@user_name", UserName);
                command.Parameters.AddWithValue("@order_time", OrderTime);
                command.Parameters.AddWithValue("@actual_delivery_time", ActualDeliveryTime);
                command.Parameters.AddWithValue("@total_bill", TotalBill);
                command.Parameters.AddWithValue("@comment", Comments);
                dbcon.Open();
                int result = command.ExecuteNonQuery();
                dbcon.Close();
                if (result < 0)
                    MessageBox.Show("Error in updating record");
                else
                    MessageBox.Show("Record Updated");
            }
        }
        private void Cafe_Orders_Load(object sender, EventArgs e)
        {
            BindData();
        }

        string UserName, OrderTime, ActualDeliveryTime, Comments;

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int selectedIndex = dataGridView1.CurrentCell.RowIndex;
            if (selectedIndex > -1)
            {
                dataGridView1.Rows.RemoveAt(selectedIndex);
            }
            string query = "Delete from orders where order_number=" + txtOrderNo.Text;
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
            txtUserName.Clear();
            txtOrderTime.Clear();
            txtActualDelTime.Clear();
            txtTotalBill.Clear();
            txtComments.Clear();
        }

        int orderNo, TotalBill;
        private void dataGridView1_SelectionChanged_1(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                orderNo = Convert.ToInt16(row.Cells[0].Value);
                UserName = Convert.ToString(row.Cells[1].Value);
                OrderTime = Convert.ToString(row.Cells[2].Value);
                ActualDeliveryTime = Convert.ToString(row.Cells[3].Value);
                TotalBill = Convert.ToInt32(row.Cells[4].Value);
                Comments = Convert.ToString(row.Cells[5].Value);
                txtOrderNo.Text = orderNo.ToString();
                txtUserName.Text = UserName.ToString();
                txtOrderTime.Text = OrderTime.ToString();
                txtActualDelTime.Text = ActualDeliveryTime.ToString();
                txtTotalBill.Text = TotalBill.ToString();
                txtComments.Text = Comments.ToString();
            }
        }
    }
}
