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
using System.Diagnostics;

namespace Naval_combat_server.Networking
{
    public class UDPManager
    {
        private Socket udpSocket;
        private Thread receiveThread;
        private CancellationTokenSource cancellationTokenSource;
        private GameEngine gameEngine;

        public delegate void DataReceivedEventHandler(Game data, IPEndPoint clientEndPoint);
        public event DataReceivedEventHandler DataReceived;

        public UDPManager(int port)
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
                    byte[] data = new byte[2048];
                    int receivedBytes = udpSocket.ReceiveFrom(data, ref clientEndPoint);

                    string jsonString = Encoding.UTF8.GetString(data, 0, receivedBytes);

                    // Попытка десериализации JSON
                    try
                    {
                        // Десериализация JSON в объект класса Game
                        var gameData = JsonConvert.DeserializeObject<Game>(jsonString);

                        // Генерирование события для уведомления внешнего кода
                        OnDataReceived(gameData, (IPEndPoint)clientEndPoint);

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

        private void ProcessReceivedData(Game gameData, IPEndPoint clientEndPoint)
        {
            // Оповестите о получении данных
            OnDataReceived(gameData, clientEndPoint);
        }


        private void OnDataReceived(Game gameData, IPEndPoint clientEndPoint)
        {
            DataReceived?.Invoke(gameData, clientEndPoint);
        }



        private void CloseConnection(IPEndPoint clientEndPoint)
        {
            Console.WriteLine($"Client {clientEndPoint} disconnected.");
            // Выполните дополнительные действия, если необходимо
        }

        public void SendData(string data, IPEndPoint clientEndPoint)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data);

            // Создаем буфер с размером, равным длине массива байт
            byte[] buffer = new byte[bytes.Length];

            // Копируем данные в буфер
            Array.Copy(bytes, buffer, bytes.Length);

            udpSocket.SendTo(buffer, clientEndPoint);
        }



        public void Close()
        {
            udpSocket.Close();
            cancellationTokenSource.Cancel();
            receiveThread.Join();
        }
    }
    public class DataReceivedEventArgs : EventArgs
    {
        public Game Data { get; }
        public IPEndPoint ClientEndPoint { get; }

        public DataReceivedEventArgs(Game data, IPEndPoint clientEndPoint)
        {
            Data = data;
            ClientEndPoint = clientEndPoint;
        }
    }



}
