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
        private TcpClient tcpClient;
        private SslStream sslStream;
        private Logger logger;

        public MainForm()
        {
            InitializeComponent();
            this.tcpClient = new TcpClient();
            this.logger = new Logger(LogLevel.Info, $"{AppSettings.LogPath}client_log.txt");
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.tcpClient.Connect("127.0.0.1", 7777);

                this.sslStream = new SslStream(
                    this.tcpClient.GetStream(),
                    false,
                    new RemoteCertificateValidationCallback(ValidateServerCertificate),
                    null
                );

                this.sslStream.AuthenticateAsClient("YourServerName");

                logger.Log(LogLevel.Info, "Успешное подключение к серверу.");


                Task.Run(() => ListenForMessages());

                ConnectButton.Enabled = false;
                SendDataButton.Enabled = true;
                //TODO код для обмена данными с сервером
            }
            catch (Exception ex)
            {
                logger.LogException(ex, "Ошибка подключения");
            }
        }

        private bool ValidateServerCertificate(
            object sender,
            X509Certificate certificate,
            X509Chain chain,
            SslPolicyErrors sslPolicyErrors
        )
        {
            //TODO код для проверки сертификата сервера
            return true;
        }

        private void SendDataButton_Click(object sender, EventArgs e)
        {
            try
            {
                string dataToSend = "Hello, server!";
                byte[] dataBytes = Encoding.UTF8.GetBytes(dataToSend);

                // Отправка данных на сервер
                sslStream.Write(dataBytes, 0, dataBytes.Length);
                sslStream.Flush();

                logger.Log(LogLevel.Info, $"Отправлено на сервер: {dataToSend}");
            }
            catch (IOException ex) when (ex.InnerException is System.Net.Sockets.SocketException)
            {
                logger.LogException(ex, "Ошибка отправки данных: Соединение с сервером разорвано.");
            }
            catch (Exception ex)
            {
                logger.LogException(ex, "Ошибка отправки данных");
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Закрываем соединение при закрытии формы
            if (tcpClient != null && tcpClient.Connected)
            {
                tcpClient.Close();
            }
        }

        private void ListenForMessages()
        {
            try
            {
                byte[] buffer = new byte[4096];
                int bytesRead;

                while ((bytesRead = sslStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    logger.Log(LogLevel.Info, $"Получено от сервера: {receivedData}");
                }
            }
            catch (ObjectDisposedException)
            {
                logger.Log(LogLevel.Error, "Соединение с сервером разорвано");
            }
            catch (Exception ex)
            {
                logger.LogException(ex, "Ошибка при чтении данных");
            }
        }
    }
}
