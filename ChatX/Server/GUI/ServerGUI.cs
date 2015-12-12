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
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            sc.StartServer("localhost", txtServiceIP.Text);
        }
        
        private void btnStop_Click(object sender, EventArgs e)
        {
            sc.StopServer();
        }
    }
}
