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
    public partial class Faculty_Menu : Form

    {
        byte[] m_dataBuffer = new byte[10];
        IAsyncResult m_result;
        public AsyncCallback m_pfnCallBack;
        public Socket m_clientSocket;

        public Faculty_Menu()
        {
            InitializeComponent();
        }
        public void WaitForData()
        {
            try
            {
                if (m_pfnCallBack == null)
                {
                    m_pfnCallBack = new AsyncCallback(OnDataReceived);
                }
                SocketPacket theSocPkt = new SocketPacket();
                theSocPkt.thisSocket = m_clientSocket;
                // Start listening to the data asynchronously
                m_result = m_clientSocket.BeginReceive(theSocPkt.dataBuffer,
                                                        0, theSocPkt.dataBuffer.Length,
                                                        SocketFlags.None,
                                                        m_pfnCallBack,
                                                        theSocPkt);
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message);
            }

        }
        public class SocketPacket
        {
            public System.Net.Sockets.Socket thisSocket;
            public byte[] dataBuffer = new byte[1];
        }
        private delegate void SafeCallDelegate(string text);
        private void WriteTextSafe(string text)
        {
            if (label3.InvokeRequired)
            {
                var d = new SafeCallDelegate(WriteTextSafe);
                Invoke(d, new object[] { text });
            }
            else
            {
                label3.Text = text;
            }
        }
        public void OnDataReceived(IAsyncResult asyn)
        {
            try
            {
                SocketPacket theSockId = (SocketPacket)asyn.AsyncState;
                int iRx = theSockId.thisSocket.EndReceive(asyn);
                char[] chars = new char[iRx + 1];
                System.Text.Decoder d = System.Text.Encoding.UTF8.GetDecoder();
                int charLen = d.GetChars(theSockId.dataBuffer, 0, iRx, chars, 0);
                System.String szData = new System.String(chars);
                //richTextBox2.Text = ;
                // String abc = WriteTextSafe(richTextBox2.Text) + szData;
                WriteTextSafe(szData);
                WaitForData();
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
        private void UpdateControls(bool connected)
        {
            button2.Enabled = !connected;
            button3.Enabled = connected;
            string connectStatus = connected ? "Connected" : "Not Connected";
            label4.Text = connectStatus;
        }
        private void Faculty_Menu_Load(object sender, EventArgs e)
        {
            dataGridView1.Rows.Add("Azaz", "Ul", "Haq", "5000", "Add");
          //  textBox1.Enabled = false;
            foreach (var control in this.Controls)
            {
                var textbox = control as TextBox;
                if (1 == 2) textbox.Visible = true;
            }


            try
            {
                UpdateControls(false);
                // Create the socket instance
                m_clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // Cet the remote IP address
                IPAddress ip = IPAddress.Parse("192.168.1.1");
                int iPortNo = System.Convert.ToInt16("8888");
                // Create the end point 
                IPEndPoint ipEnd = new IPEndPoint(ip, iPortNo);
                // Connect to the remote host
                m_clientSocket.Connect(ipEnd);
                if (m_clientSocket.Connected)
                {

                    UpdateControls(true);
                    //Wait for data asynchronously 
                    WaitForData();
                }
            }
            catch (SocketException se)
            {
                string str;
                str = "\nConnection failed, is the server running?\n" + se.Message;
                MessageBox.Show(str);
                UpdateControls(false);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                Object objData = label3.Text;
                byte[] byData = System.Text.Encoding.ASCII.GetBytes(objData.ToString());
                if (m_clientSocket != null)
                {
                    m_clientSocket.Send(byData);
                }
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message);
            }
        }

      
    }
}
