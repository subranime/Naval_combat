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
using Naval_combat.Common;
using Naval_combat.Networking;
using Newtonsoft.Json;

namespace Naval_combat.UI
{
    public partial class MainForm : Form
    {
        private static Logger staticLogger;
        private UDPClientManager udpClientManager;

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
            // Создание JSON-события "StartGame"
            JsonEvent startGameEvent = new JsonEvent { EventType = "StartGame" };
            string startGameMessage = JsonConvert.SerializeObject(startGameEvent);
            udpClientManager.SendData(startGameMessage);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Создаем JSON-событие "Disconnect"
            var disconnectEvent = new { EventType = "Disconnect" };

            // Преобразуем объект в JSON
            string disconnectMessage = JsonConvert.SerializeObject(disconnectEvent);

            // Отправляем сообщение на сервер о том, что клиент отключается
            udpClientManager.SendData(disconnectMessage);

            udpClientManager.Close();
        }
    }
}
