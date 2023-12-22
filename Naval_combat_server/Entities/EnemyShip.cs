using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Naval_combat_server.Common;
using Newtonsoft.Json;

namespace Naval_combat_server.Entities
{
    public enum ShipSize
    {
        Tiny,
        Small,
        Medium,
        Large
    }

    public class EnemyShip : IDataContainer
    {
        private static readonly Random random = new Random();

        public ShipSize ShipSize { get; set; }
        public int Speed { get; set; }
        public Coordinates Coordinates { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public EnemyShip()
        {
            GenerateRandomSize();
            Coordinates = GenerateRandomCoordinates();
            Speed = CalculateSpeedBasedOnSize();
        }

        private void GenerateRandomSize()
        {
            switch (ShipSize)
            {
                case ShipSize.Tiny:
                    Width = random.Next(30, 50);
                    Height = random.Next(20, 30);
                    break;
                case ShipSize.Small:
                    Width = random.Next(50, 70);
                    Height = random.Next(30, 40);
                    break;
                case ShipSize.Medium:
                    Width = random.Next(70, 90);
                    Height = random.Next(40, 60);
                    break;
                case ShipSize.Large:
                    Width = random.Next(90, 120);
                    Height = random.Next(60, 80);
                    break;
            }
        }

        private Coordinates GenerateRandomCoordinates()
        {
            return new Coordinates
            {
                X = random.Next(501 - Width),  // От 0 до (500 - Width)
                Y = random.Next(251 - Height)  // От 0 до (250 - Height)
            };
        }

        private int CalculateSpeedBasedOnSize()
        {
            switch (ShipSize)
            {
                case ShipSize.Tiny:
                    return 5;
                case ShipSize.Small:
                    return 4;
                case ShipSize.Medium:
                    return 2;
                case ShipSize.Large:
                    return 1;
                default:
                    return 1;
            }
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
            EnemyShip enemyShip = JsonConvert.DeserializeObject<EnemyShip>(json);

            // Копирование данных из преобразованного объекта в текущий объект
            ShipSize = enemyShip.ShipSize;
            Speed = enemyShip.Speed;
            Coordinates = enemyShip.Coordinates;
            Width = enemyShip.Width;
            Height = enemyShip.Height;
        }
    }
}
