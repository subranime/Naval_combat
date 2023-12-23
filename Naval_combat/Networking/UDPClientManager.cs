using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Naval_combat.Common;
using Newtonsoft.Json;
using Naval_combat.Entities;

namespace Naval_combat.Networking
{
    public class UDPClientManager
    {
        private Socket udpClient;
        private Thread receiveThread;
        private object lockObject = new object();
        private bool isRunning = true;
        private static Logger staticLogger;
        private CancellationTokenSource cancellationTokenSource;

        public delegate void DataReceivedEventHandler(Game data);
        public event DataReceivedEventHandler DataReceived;

        public UDPClientManager(Logger logger)
        {
            staticLogger = logger;
            udpClient = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            // Привязываем сокет к конечной точке
            // Порт 49152 используется для принятия входящих пакетов на стороне UDP-сервера
            udpClient.Bind(new IPEndPoint(IPAddress.Any, 49152));

            cancellationTokenSource = new CancellationTokenSource();
            receiveThread = new Thread(() => ReceiveData(cancellationTokenSource.Token));
            receiveThread.Start();
        }

        private void ReceiveData(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    EndPoint serverEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    byte[] data = new byte[2048];
                    int receivedBytes;

                    try
                    {
                        receivedBytes = udpClient.ReceiveFrom(data, ref serverEndPoint);
                        string jsonString = Encoding.UTF8.GetString(data, 0, receivedBytes);

                        // Десериализация JSON в объект класса Game
                        var gameData = JsonConvert.DeserializeObject<Game>(jsonString);

                        OnDataReceived(gameData);

                    }
                    catch (SocketException ex) when (ex.SocketErrorCode == SocketError.Interrupted)
                    {
                        // Прервано из-за закрытия сокета
                        break;
                    }

                    
                }
            }
            catch (Exception ex)
            {
                // Логирование ошибки
                staticLogger.Log(LogLevel.Error, "Error in ReceiveData: " + ex.Message);
            }
        }

        protected virtual void OnDataReceived(Game data)
        {
            DataReceived?.Invoke(data);
        }

        public void Connect(string serverIP, int serverPort)
        {
            // Здесь serverPort — порт для отправки сообщений
            udpClient.Connect(serverIP, serverPort);
        }

        public void SendData(string data)
        {
            lock (lockObject)
            {
                if (!isRunning)
                    return;

                byte[] bytes = Encoding.UTF8.GetBytes(data);
                udpClient.Send(bytes, bytes.Length, SocketFlags.None);
            }
        }


        public void Close()
        {
            lock (lockObject)
            {
                if (!isRunning)
                    return;

                isRunning = false;

                // Закрытие соединения
                udpClient.Close();

                // Отмена токена для завершения потока
                cancellationTokenSource.Cancel();

                // Дождитесь завершения потока
                receiveThread.Join();
            }
        }
    }
}
