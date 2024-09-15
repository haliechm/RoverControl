using RoverControl.Core.Enums;

namespace RoverControl.Core
{
    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Direction Orientation { get; set; }

        public Position(int x, int y, Direction orientation)
        {
            this.X = x;
            this.Y = y;
            this.Orientation = orientation;
        }

        public Position(Position other) : this(other.X, other.Y, other.Orientation)
        {
        }
    }
}
