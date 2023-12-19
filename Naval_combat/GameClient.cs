using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Naval_combat
{
    public class GameClient
    {
        private TcpClientManager tcpClientManager;
        private PictureBox pictureBox;
        private List<Ship> playerShips;

        public GameClient(TcpClientManager tcpClientManager, PictureBox pictureBox)
        {
            this.tcpClientManager = tcpClientManager;
            this.pictureBox = pictureBox;
            this.playerShips = new List<Ship>();

            // Подписываемся на событие получения сообщений от сервера
            tcpClientManager.MessageReceived += HandleServerMessage;
        }

        public async Task<bool> ConnectAsync(string ipAddress, int port, string userNickname)
        {
            return await tcpClientManager.ConnectAsync(ipAddress, port, userNickname);
        }

        public void StartGame()
        {
            // Запрос у сервера начального распределения кораблей
            tcpClientManager.SendData("requestInitialShips");
        }

        private void HandleServerMessage(string message)
        {
            try
            {
                dynamic jsonMessage = JsonConvert.DeserializeObject(message);
                string messageType = jsonMessage.type;

                switch (messageType)
                {
                    case "initialShips":
                        // Обработка начального распределения кораблей
                        JArray shipsArray = jsonMessage.ships;
                        playerShips = shipsArray.ToObject<List<Ship>>();
                        UpdatePictureBox();
                        break;

                    case "updateShips":
                        // Обработка обновленного распределения кораблей
                        JArray updatedShipsArray = jsonMessage.ships;
                        playerShips = updatedShipsArray.ToObject<List<Ship>>();
                        UpdatePictureBox();
                        break;

                    // Другие типы сообщений...

                    default:
                        // Обработка других типов сообщений от сервера
                        break;
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибок при обработке сообщения от сервера
                Console.WriteLine($"Ошибка при обработке сообщения от сервера: {ex.Message}");
            }
        }

        private void UpdatePictureBox()
        {
            // Очистка PictureBox
            pictureBox.Image = new Bitmap(pictureBox.Width, pictureBox.Height);

            // Отрисовка кораблей на PictureBox
            using (Graphics g = Graphics.FromImage(pictureBox.Image))
            {
                foreach (var ship in playerShips)
                {
                    // Пример: рисуем прямоугольник для каждого корабля
                    g.FillRectangle(Brushes.Blue, ship.Column * 3, ship.Row * 3, ship.Width * 3, ship.Height * 3);
                }
            }

            // Обновление PictureBox
            pictureBox.Invalidate();
        }
    }

}
