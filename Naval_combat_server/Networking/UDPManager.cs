using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Newtonsoft.Json;
using Naval_combat_server.Common;
using Naval_combat_server.GameLogic;
using Naval_combat_server.Entities;

namespace Naval_combat_server.Networking
{
    public class UDPManager
    {
        private Socket udpSocket;
        private Thread receiveThread;
        private CancellationTokenSource cancellationTokenSource;
        private GameEngine gameEngine;

        public delegate void DataReceivedEventHandler(string data, IPEndPoint clientEndPoint);
        public event DataReceivedEventHandler DataReceived;

        public UDPManager(int port, GameEngine gameEngine)
        {
            this.gameEngine = gameEngine;

            udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            udpSocket.Bind(new IPEndPoint(IPAddress.Any, port));

            cancellationTokenSource = new CancellationTokenSource();
            receiveThread = new Thread(() => ReceiveData(cancellationTokenSource.Token));
            receiveThread.Start();
        }

        private void ReceiveData(CancellationToken cancellationToken)
        {
            try
            {
                while (true)
                {
                    EndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    byte[] data = new byte[1024];
                    int receivedBytes = udpSocket.ReceiveFrom(data, ref clientEndPoint);

                    string jsonString = Encoding.UTF8.GetString(data, 0, receivedBytes);

                    // Попытка десериализации JSON
                    try
                    {
                        // Парсинг JSON-объекта
                        var jsonData = JsonConvert.DeserializeObject<IDataContainer>(jsonString);

                        // Обработка полученных данных
                        // Передача данных в GameEngine для обработки
                        gameEngine.ProcessReceivedData(jsonData, clientEndPoint);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error deserializing JSON: " + ex.Message);
                    }
                }
            }
            catch (SocketException ex)
            {
                if (ex.SocketErrorCode == SocketError.Interrupted)
                {
                    // Поток прерван, возможно, из-за закрытия сокета
                }
                else
                {
                    Console.WriteLine("Error in ReceiveData: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in ReceiveData: " + ex.Message);
            }
        }

        private void ProcessReceivedData(IDataContainer jsonData, IPEndPoint clientEndPoint)
        {
            // Оповестите о получении данных
            //OnDataReceived(jsonData.ToJson(), clientEndPoint);
        }

        protected virtual void OnDataReceived(string data, IPEndPoint clientEndPoint)
        {
            DataReceived?.Invoke(data, clientEndPoint);
        }

        private void CloseConnection(IPEndPoint clientEndPoint)
        {
            Console.WriteLine($"Client {clientEndPoint} disconnected.");
            // Выполните дополнительные действия, если необходимо
        }

        public void SendData(string data, IPEndPoint clientEndPoint)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            udpSocket.SendTo(bytes, clientEndPoint);
        }

        public void Close()
        {
            udpSocket.Close();
            cancellationTokenSource.Cancel();
            receiveThread.Join();
        }
    }

}
