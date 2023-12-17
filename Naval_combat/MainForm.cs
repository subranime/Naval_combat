using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Naval_combat
{
    public partial class MainForm : Form
    {
        private TcpClient tcpClient;
        private SslStream sslStream;

        public MainForm()
        {
            InitializeComponent();
            this.tcpClient = new TcpClient();
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

                AppendToLog("Успешное подключение к серверу.");

                //TODO код для обмена данными с сервером
            }
            catch (Exception ex)
            {
                AppendToLog($"Ошибка подключения: {ex.Message}");
            }
        }

        private void AppendToLog(string message)
        {
            // TODO заменить на нормальное логгирование
            richTextBox1.AppendText($"{DateTime.Now:HH:mm:ss} - {message}\n");
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
    }
}
