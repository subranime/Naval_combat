using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Naval_combat
{
    // Полный код TcpClientManager
    public class TcpClientManager
    {
        private TcpClient tcpClient;
        private SslStream sslStream;
        private Logger logger;
        private string nickname;

        // Событие для оповещения о получении сообщения от сервера
        public event Action<string> MessageReceived;

        public TcpClientManager(Logger logger)
        {
            this.tcpClient = new TcpClient();
            this.logger = logger;
        }

        public async Task<bool> ConnectAsync(string ipAddress, int port, string userNickname)
        {
            try
            {
                await tcpClient.ConnectAsync(ipAddress, port);

                sslStream = new SslStream(
                    tcpClient.GetStream(),
                    false,
                    new RemoteCertificateValidationCallback(ValidateServerCertificate),
                    null
                );

                await sslStream.AuthenticateAsClientAsync("YourServerName");

                logger.Log(LogLevel.Info, "Успешное подключение к серверу.");

                // Запускаем асинхронный метод для прослушивания сообщений
                Task.Run(() => ListenForMessages());

                nickname = userNickname;

                return true;
            }
            catch (Exception ex)
            {
                logger.LogException(ex, "Ошибка подключения");
                return false;
            }
        }

        public void SendData(string data)
        {
            try
            {
                // Добавляем никнейм к данным перед отправкой
                string dataWithNickname = $"{nickname}: {data}";
                byte[] dataBytes = Encoding.UTF8.GetBytes(dataWithNickname);

                sslStream.Write(dataBytes, 0, dataBytes.Length);
                sslStream.Flush();

                logger.Log(LogLevel.Info, $"Отправлено на сервер: {data}");
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

        private void ListenForMessages()
        {
            try
            {
                byte[] buffer = new byte[4096];
                int bytesRead;

                while ((bytesRead = sslStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    MessageReceived?.Invoke(receivedData);
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

        public void CloseConnection()
        {
            try
            {
                if (tcpClient != null && tcpClient.Connected)
                {
                    tcpClient.Close();
                    logger.Log(LogLevel.Info, "Соединение закрыто.");
                }
            }
            catch (Exception ex)
            {
                logger.LogException(ex, "Ошибка при закрытии соединения");
            }
        }
    }

}
