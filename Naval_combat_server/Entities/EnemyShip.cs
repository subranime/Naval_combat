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
    public enum MovementDirection
    {
        Left,
        Right
    }

    public class EnemyShip
    {
        private static readonly Random random = new Random();

        public ShipSize ShipSize { get; set; }
        public MovementDirection Direction { get; set; }
        public int Speed { get; set; }
        public Coordinates Coordinates { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public EnemyShip()
        {
            ShipSize = GetRandomShipSize();
            GenerateRandomSize();
            Coordinates = GenerateRandomCoordinates();
            Speed = CalculateSpeedBasedOnSize();
            Direction = GenerateRandomDirection();
        }

        private ShipSize GetRandomShipSize()
        {
            Array shipSizes = Enum.GetValues(typeof(ShipSize));
            return (ShipSize)shipSizes.GetValue(random.Next(shipSizes.Length));
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
        private MovementDirection GenerateRandomDirection()
        {
            return (MovementDirection)random.Next(2);
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

    }
}
