using Naval_combat_server.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naval_combat_server.Entities
{
    public class Field
    {
        public List<EnemyShip> EnemyShips { get; set; }

        // Конструктор без параметров
        public Field()
        {
        }

        // Конструктор с параметрами
        public Field(List<EnemyShip> enemyShips)
        {
            EnemyShips = enemyShips;
        }

        public void UpdateEnemyShips(List<EnemyShip> newEnemyShips)
        {
            EnemyShips = newEnemyShips;
        }

        public void GenerateEnemyShips()
        {
            // Генерация случайных вражеских кораблей
            List<EnemyShip> enemyShips = new List<EnemyShip>();
            for (int i = 0; i < 10; i++)
            {
                enemyShips.Add(new EnemyShip());
            }
            this.EnemyShips = enemyShips;
        }
    }

}
