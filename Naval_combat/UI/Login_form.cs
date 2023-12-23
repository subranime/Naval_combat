using Naval_combat.Common;
using Naval_combat.Networking;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Naval_combat.UI
{
    public partial class Login_form : Form
    {
        private UDPClientManager udpClientManager;
        private static Logger staticLogger;
        public static Logger StaticLogger => staticLogger;

        public Login_form()
        {
            InitializeComponent();
            staticLogger = new Logger(LogLevel.Info, $"{AppSettings.LogPath}client_log.txt");
            this.udpClientManager = new UDPClientManager(staticLogger);

            // Подключение к серверу
            this.udpClientManager.Connect("127.0.0.1", 49153);
        }

        private void Play_button_Click(object sender, EventArgs e)
        {
            // Получение ника игрока из текстового поля
            string playerName = NickName_textBox.Text;

            // После успешной авторизации, открываем главное окно
            using (MainForm mainForm = new MainForm(playerName, udpClientManager))
            {
                mainForm.ShowDialog();
            }

            // Скрываем окно авторизации после завершения игры
            Hide();
        }
    }
}
