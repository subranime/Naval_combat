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
using Naval_combat.Entities;
using Naval_combat.Networking;
using Newtonsoft.Json;
using Naval_combat.GameLogic;

namespace Naval_combat.UI
{
    public partial class MainForm : Form
    {
        private static Logger staticLogger;
        private UDPClientManager udpClientManager;
        private string playerName;
        private Game game;
        private GameEngine gameEngine;
        private Timer updateTimer;

        public MainForm(string playerName, UDPClientManager udpClientManager)
        {
            InitializeComponent();

            staticLogger = Login_form.StaticLogger;
            this.playerName = playerName;
            this.udpClientManager = udpClientManager;
            this.game = new Game(playerName);

            // Создаем экземпляр GameEngine
            this.gameEngine = new GameEngine(gamePictureBox);

            this.udpClientManager.DataReceived += OnDataReceived;

            // Отправка стартовых данных при инициализации формы
            SendStartGameData();

            // Инициализация таймера для обновлений каждые 1000 миллисекунд (1 секунда)
            updateTimer = new Timer();
            updateTimer.Interval = 2000;
            updateTimer.Tick += UpdateTimer_Tick;
            updateTimer.Start();
        }

        private async void SendStartGameData()
        {
            this.game.GameState = "StartGame";
            // Преобразование объекта в JSON
            string startGameMessage = JsonConvert.SerializeObject(this.game);

            // Отправка данных на сервер асинхронно
            await Task.Run(() => udpClientManager.SendData(startGameMessage));
        }


        private async void UpdateTimer_Tick(object sender, EventArgs e)
        {
            // Запрос на обновление данных с сервера
            this.game.GameState = "Update";
            // Преобразование объекта в JSON
            string startGameMessage = JsonConvert.SerializeObject(this.game);

            // Отправка данных на сервер асинхронно
            await Task.Run(() => udpClientManager.SendData(startGameMessage));
        }


        private void OnDataReceived(Game data)
        {
            // Передача объекта Game в метод GameEngine
            this.gameEngine.UpdateUI(data);
        }


        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Создаем JSON-событие "Disconnect"
            this.game.GameState = "Disconnect";
            // Преобразование объекта в JSON
            string disconnectMessage = JsonConvert.SerializeObject(this.game);
            // Отправка данных на сервер асинхронно
            udpClientManager.SendData(disconnectMessage);
            
            udpClientManager.Close();
        }

        private void Shoot_button_Click(object sender, EventArgs e)
        {

        }
    }
}
