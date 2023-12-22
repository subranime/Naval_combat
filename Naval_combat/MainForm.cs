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
        private static Logger staticLogger;
        private UDPClientManager udpClientManager;
        private GameClient gameClient;

        public MainForm()
        {
            InitializeComponent();

            staticLogger = new Logger(LogLevel.Info, $"{AppSettings.LogPath}client_log.txt");
            this.udpClientManager = new UDPClientManager(staticLogger);

            this.udpClientManager.DataReceived += OnDataReceived;

            // Подключение к серверу
            this.udpClientManager.Connect("127.0.0.1", 49153);
        }

        private static void OnDataReceived(string data)
        {
            staticLogger.Log(LogLevel.Info, $"Received data: {data}");
        }

        private void StartGameButton_Click(object sender, EventArgs e)
        {
            string startGameMessage = "StartGame";
            udpClientManager.SendData(startGameMessage);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Отправляем сообщение на сервер о том, что клиент отключается
            string disconnectMessage = "Disconnecting";
            udpClientManager.SendData(disconnectMessage);

            udpClientManager.Close();
        }
    }
}
