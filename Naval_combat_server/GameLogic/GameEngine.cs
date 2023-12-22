using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Naval_combat_server.Entities;
using Naval_combat_server.Common;
using Naval_combat_server.GameLogic;
using System.Net;

namespace Naval_combat_server.GameLogic
{
    public class GameEngine
    {
        public Player Player { get; private set; }
        public List<EnemyShip> EnemyShips { get; private set; }

        public void InitializeGame(Player player, List<EnemyShip> enemyShips)
        {
            InitializePlayer(player);
            InitializeEnemyShips(enemyShips);
            // Другие шаги инициализации
        }

        private void InitializePlayer(Player player)
        {
            Player = player;
            // Другие параметры инициализации игрока
        }

        private void InitializeEnemyShips(List<EnemyShip> enemyShips)
        {
            EnemyShips = new List<EnemyShip>(enemyShips);
            // Другие параметры инициализации кораблей противника
        }

        public void ProcessReceivedData(IDataContainer jsonData, EndPoint clientEndPoint)
        {
            // Обработка полученных данных
            if (jsonData is Player playerData)
            {
                // Это данные игрока, обработайте их
                //UpdatePlayerData(playerData);
            }
            else if (jsonData is Field fieldData)
            {
                // Это данные игрового поля, обработайте их
                //UpdateFieldData(fieldData);
            }
        }
    }
}
