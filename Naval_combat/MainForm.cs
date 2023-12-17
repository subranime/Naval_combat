using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Naval_combat;

namespace Naval_combat
{
    public partial class MainForm : Form
    {
        private TcpClientManager tcpClientManager;
        private Logger logger;

        public MainForm()
        {
            InitializeComponent();
            this.logger = new Logger(LogLevel.Info, $"{AppSettings.LogPath}client_log.txt");
            this.tcpClientManager = new TcpClientManager(logger);
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            if (tcpClientManager.Connect("127.0.0.1", 7777))
            {
                ConnectButton.Enabled = false;
                SendDataButton.Enabled = true;
                //TODO код для обмена данными с сервером
            }
        }

        private void SendDataButton_Click(object sender, EventArgs e)
        {
            tcpClientManager.SendData("Hello, server!");
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Закрываем соединение при закрытии формы
            tcpClientManager.CloseConnection();
        }


    }
}
