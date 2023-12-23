using System;
using System.Collections.Generic;
using System.Net;
using Naval_combat_server.Entities;
using Naval_combat_server.Networking;
using Newtonsoft.Json;

namespace Naval_combat_server.GameLogic
{
    public class GameEngine
    {
        private readonly UDPManager udpManager;
        private Random random = new Random();

        public Player Player { get; private set; }
        public List<EnemyShip> EnemyShips { get; private set; }

        public GameEngine(UDPManager udpManager)
        {
            this.udpManager = udpManager;
        }

        public void ProcessReceivedData(Game data, IPEndPoint clientEndPoint)
        {
            try
            {
                if (data.GameState == "StartGame")
                {
                    // Инициализация игры
                    InitializeGame(data);
                    Console.WriteLine($"Player {data.Player.NickName} connected... Start the game");
                    udpManager.SendData(data.ToJson(), clientEndPoint);
                }
                else if (data.GameState == "Shoot")
                {
                    // Обработка других состояний игры
                    Console.WriteLine("None");
                }
                else if (data.GameState == "Update")
                {
                    // Обновление поля
                    UpdateEnemyShips(data.Field.EnemyShips);
                    udpManager.SendData(data.ToJson(), clientEndPoint);
                    Console.WriteLine("Update field");
                }
                else if (data.GameState == "EndGame")
                {
                    // Обработка других состояний игры
                    Console.WriteLine("None");
                }
                else if (data.GameState == "Disconnect")
                {
                    // Обработка других состояний игры
                    Console.WriteLine($"User {data.Player.NickName} DISCONNECTED");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error processing received data: " + ex.Message);
            }
        }

        public void InitializeGame(Game data)
        {
            data.Field.GenerateEnemyShips();
        }

        public void UpdateEnemyShips(List<EnemyShip> enemyShips)
        {
            foreach (var enemyShip in enemyShips)
            {
                UpdateEnemyShipPosition(enemyShip);
            }
        }

        private void UpdateEnemyShipPosition(EnemyShip enemyShip)
        {
            if (enemyShip.Direction == MovementDirection.Left)
            {
                enemyShip.Coordinates.X -= enemyShip.Speed;
                // Проверка на выход за границы поля
                if (enemyShip.Coordinates.X < 0)
                {
                    enemyShip.Coordinates.X = 0;
                    enemyShip.Direction = MovementDirection.Right; // Изменение направления
                }
            }
            else
            {
                enemyShip.Coordinates.X += enemyShip.Speed;
                // Проверка на выход за границы поля
                if (enemyShip.Coordinates.X + enemyShip.Width > 500)
                {
                    enemyShip.Coordinates.X = 500 - enemyShip.Width;
                    enemyShip.Direction = MovementDirection.Left; // Изменение направления
                }
            }
        }



    }
}
