using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DistributedCafeSystem
{
    public partial class Cafe_Home : Form
    {
        public Cafe_Home()
        {
            InitializeComponent();
        }

        const int MAX_CLIENTS = 10;

        AsyncCallback pfnWorkerCallBack;
        Socket m_mainSocket;
        Socket[] m_workerSocket = new Socket[10];
        int m_clientCount = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Aray waah!");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == this.del.Index)
                {
                    MessageBox.Show("del");
                    dataGridView1.Rows.RemoveAt(e.RowIndex);
                }
                else if (e.ColumnIndex == this.Edit.Index)
                {
                    //MessageBox.Show("edit");
                    if (e.RowIndex >= 0)
                    {
                        textBox1.Enabled = true;
                        //gets a collection that contains all the rows
                        DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                        //populate the textbox from specific value of the coordinates of column and row.
                        textBox1.Text = row.Cells[0].Value.ToString();
                    }
                }
            }
        }





        private void UpdateControls(bool listening)
        {
            button4.Enabled = !listening;
            button5.Enabled = listening;
        }

        private delegate void SafeCallDelegate(string text);
        private void WriteTextSafe(string text)
        {
            if (richTextBox1.InvokeRequired)
            {
                var d = new SafeCallDelegate(WriteTextSafe);
                Invoke(d, new object[] { text });
            }
            else
            {
                richTextBox1.AppendText(text);
                //label3.Text = text;
            }
        }

        // This is the call back function, which will be invoked when a client is connected
        public void OnClientConnect(IAsyncResult asyn)
        {
            try
            {
                // Here we complete/end the BeginAccept() asynchronous call
                // by calling EndAccept() - which returns the reference to
                // a new Socket object
                m_workerSocket[m_clientCount] = m_mainSocket.EndAccept(asyn);
                // Let the worker Socket do the further processing for the 
                // just connected client
                WaitForData(m_workerSocket[m_clientCount]);
                // Now increment the client count
                ++m_clientCount;
                // Display this client connection as a status message on the GUI	
                String str = String.Format("Client # {0} connected", m_clientCount);
                //textBox2.Text = str;
                WriteTextSafe(str);

                // Since the main Socket is now free, it can go back and wait for
                // other clients who are attempting to connect
                m_mainSocket.BeginAccept(new AsyncCallback(OnClientConnect), null);
            }
            catch (ObjectDisposedException)
            {
                System.Diagnostics.Debugger.Log(0, "1", "\n OnClientConnection: Socket has been closed\n");
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message);
            }

        }

        public class SocketPacket
        {
            public System.Net.Sockets.Socket m_currentSocket;
            public byte[] dataBuffer = new byte[1];
        }
        // Start waiting for data from the client
        public void WaitForData(System.Net.Sockets.Socket soc)
        {
            try
            {
                if (pfnWorkerCallBack == null)
                {
                    // Specify the call back function which is to be 
                    // invoked when there is any write activity by the 
                    // connected client
                    pfnWorkerCallBack = new AsyncCallback(OnDataReceived);
                }
                SocketPacket theSocPkt = new SocketPacket();
                theSocPkt.m_currentSocket = soc;
                // Start receiving any data written by the connected client
                // asynchronously
                soc.BeginReceive(theSocPkt.dataBuffer, 0,
                                   theSocPkt.dataBuffer.Length,
                                   SocketFlags.None,
                                   pfnWorkerCallBack,
                                   theSocPkt);
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message);
            }

        }
        // This the call back function which will be invoked when the socket
        // detects any client writing of data on the stream
        public void OnDataReceived(IAsyncResult asyn)
        {
            try
            {
                SocketPacket socketData = (SocketPacket)asyn.AsyncState;

                int iRx = 0;
                // Complete the BeginReceive() asynchronous call by EndReceive() method
                // which will return the number of characters written to the stream 
                // by the client
                iRx = socketData.m_currentSocket.EndReceive(asyn);
                char[] chars = new char[iRx + 1];
                System.Text.Decoder d = System.Text.Encoding.UTF8.GetDecoder();
                int charLen = d.GetChars(socketData.dataBuffer,
                                         0, iRx, chars, 0);
                System.String szData = new System.String(chars);
                //richTextBox1.AppendText(szData);
                WriteTextSafe(szData);
                // Continue the waiting for data on the Socket
                WaitForData(socketData.m_currentSocket);
            }
            catch (ObjectDisposedException)
            {
                System.Diagnostics.Debugger.Log(0, "1", "\nOnDataReceived: Socket has been closed\n");
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message);
            }
        }

        private void Cafe_Home_Load(object sender, EventArgs e)
        {
            dataGridView1.Rows.Add("Azaz","Ul","Haq","del","edit");
          /**  textBox1.Enabled = false;
            foreach (var control in this.Controls)
            {
                var textbox = control as TextBox;
                if(1==2) textbox.Visible = true;
            } **/



            try
            {

                string portStr = "8888";
                int port = System.Convert.ToInt32(portStr);
                // Create the listening socket...
                m_mainSocket = new Socket(AddressFamily.InterNetwork,
                                          SocketType.Stream,
                                          ProtocolType.Tcp);
                IPEndPoint ipLocal = new IPEndPoint(IPAddress.Any, port);
                // Bind to local IP Address...
                m_mainSocket.Bind(ipLocal);
                // Start listening...
                m_mainSocket.Listen(4);
                // Create the call back for any client connections...
                m_mainSocket.BeginAccept(new AsyncCallback(OnClientConnect), null);

                UpdateControls(true);

            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message);
            }

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
