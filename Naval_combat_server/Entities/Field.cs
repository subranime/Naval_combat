using Naval_combat_server.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naval_combat_server.Entities
{
    public class Field : IDataContainer
    {
        public Player Player { get; set; }
        public List<EnemyShip> EnemyShips { get; set; }

        public Field(Player player, List<EnemyShip> enemyShips)
        {
            Player = player;
            EnemyShips = enemyShips;
        }

        // Реализация методов интерфейса IDataContainer
        public string ToJson()
        {
            // Преобразование объекта в JSON
            return JsonConvert.SerializeObject(this);
        }

        public void FromJson(string json)
        {
            // Преобразование JSON в объект
            Field field = JsonConvert.DeserializeObject<Field>(json);

            // Копирование данных из преобразованного объекта в текущий объект
            Player = field.Player;
            EnemyShips = field.EnemyShips;
        }
    }
}
