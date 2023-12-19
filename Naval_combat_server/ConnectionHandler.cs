using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Naval_combat_server;
using System.Net.Security;
using Newtonsoft.Json;

namespace Naval_combat_server
{
    public class ConnectionHandler
    {
        private TcpListener tcpListener;
        private List<TcpClient> connectedClients;
        private List<Ship> playerShips;
        private X509Certificate serverCertificate;

        public ConnectionHandler(X509Certificate serverCertificate)
        {
            this.tcpListener = new TcpListener(System.Net.IPAddress.Any, 7777);
            this.connectedClients = new List<TcpClient>();
            this.playerShips = new List<Ship>();
            this.serverCertificate = serverCertificate;
        }

        public void Start()
        {
            this.tcpListener.Start();
            Console.WriteLine("Сервер запущен. Ожидание подключений...");

            while (true)
            {
                try
                {
                    TcpClient client = this.tcpListener.AcceptTcpClient();
                    connectedClients.Add(client);

                    // Создаем новый поток для обработки клиента
                    Task.Run(() => HandleClient(client));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при ожидании подключений: {ex.Message}");
                }
            }
        }

        private void HandleClient(TcpClient tcpClient)
        {
            try
            {
                using (NetworkStream stream = tcpClient.GetStream())
                {
                    // Отправляем начальное распределение кораблей при подключении нового клиента
                    SendInitialShips(tcpClient);
                    Console.WriteLine($"Клиенту отправлены начальные расположения кораблей");
                    
                    byte[] buffer = new byte[4096];
                    int bytesRead;

                    while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        Console.WriteLine($"Получено от клиента {tcpClient.Client.RemoteEndPoint}: {receivedData}");

                        // Добавьте здесь логику обработки сообщений от клиента

                        // Пример: обновляем распределение кораблей и отправляем обновление клиентам
                        UpdateShips();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка обработки клиента {tcpClient.Client.RemoteEndPoint}: {ex.Message}");
            }
            finally
            {
                connectedClients.Remove(tcpClient);
                tcpClient.Close();
            }
        }

        private void SendInitialShips(TcpClient tcpClient)
        {
            // Отправляем начальное распределение кораблей новому клиенту
            string initialShipsJson = JsonConvert.SerializeObject(new { type = "initialShips", ships = playerShips });
            SendDataToClient(tcpClient, initialShipsJson);
        }

        private void UpdateShips()
        {
            // Обновляем распределение кораблей и отправляем обновление всем клиентам
            string updatedShipsJson = JsonConvert.SerializeObject(new { type = "updateShips", ships = playerShips });
            BroadcastData(updatedShipsJson);
        }

        private void SendDataToClient(TcpClient tcpClient, string data)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            tcpClient.GetStream().Write(dataBytes, 0, dataBytes.Length);
        }

        private void BroadcastData(string data)
        {
            // Отправляем данные всем подключенным клиентам
            foreach (var client in connectedClients)
            {
                SendDataToClient(client, data);
            }
        }
    }
}