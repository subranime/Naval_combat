using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Naval_combat.Common;

namespace Naval_combat.Entities
{
    public class Field
    {
        public List<EnemyShip> EnemyShips { get; set; }

        // Конструктор без параметров
        public Field()
        {
            // Инициализация EnemyShips по умолчанию, чтобы избежать NullReferenceException
            EnemyShips = new List<EnemyShip>();
        }

        // Конструктор с параметрами
        public Field(List<EnemyShip> enemyShips)
        {
            EnemyShips = enemyShips;
        }

    }
}

