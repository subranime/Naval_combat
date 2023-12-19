using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naval_combat
{
    public class Ship
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Speed { get; set; }
        public Direction Direction { get; set; }

        public Ship(int row, int column, int width, int height, int speed, Direction direction)
        {
            Row = row;
            Column = column;
            Width = width;
            Height = height;
            Speed = speed;
            Direction = direction;
        }
    }

    public enum Direction
    {
        Left,
        Right
    }

}
