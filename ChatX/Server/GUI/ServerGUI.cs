using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Server.Controller;

namespace Server
{
    public partial class ServerGUI : Form
    {
        private ServerController sc;
        
        public ServerGUI()
        {
            InitializeComponent();
            sc = ServerController.GetInstance();

            ChatSocket.ConnectionManager.GetInstance().OnMessageSend += (msg) =>
            {
                WriteLine(msg);
            };
        }

        private void WriteLine(string msg)
        {
            if (txtIO.InvokeRequired)
            {
                Action<string> thisMethod = WriteLine;

                txtIO.Invoke(thisMethod, msg);
            }
            else
            {
                txtIO.Text += "\n " + msg;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            sc.StartServer("localhost", txtServiceIP.Text);
            txtIO.Text += "\n Server Started!";
        }
        
        private void btnStop_Click(object sender, EventArgs e)
        {
            sc.StopServer();
        }
    }
}
