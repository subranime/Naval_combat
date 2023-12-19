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
        private GameClient gameClient;

        public MainForm()
        {
            InitializeComponent();
            this.logger = new Logger(LogLevel.Info, $"{AppSettings.LogPath}client_log.txt");
            this.tcpClientManager = new TcpClientManager(logger);
            this.gameClient = new GameClient(tcpClientManager, gamePictureBox);

            ConnectToServer();
        }

        private async void ConnectToServer()
        {
            if (await gameClient.ConnectAsync("127.0.0.1", 7777, AppSettings.NickName))
            {
                Start_game_button.Enabled = true;
            }
        }

        private void StartGameButton_Click(object sender, EventArgs e)
        {
            gameClient.StartGame();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Закрываем соединение при закрытии формы
            tcpClientManager.CloseConnection();
        }


    }
}
