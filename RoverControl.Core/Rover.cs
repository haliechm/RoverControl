using RoverControl.Core.Enums;

namespace RoverControl.Core
{
    public class Rover
    {
        public Plateau Plateau { get; }
        public Position Position { get; }
        public IList<Position> PositionHistory { get; } = new List<Position>();

        public Rover(int x, int y, Direction orientation, Plateau plateau)
        {
            if (x < 0 || x >= plateau.Width || y < 0 || y >= plateau.Height)
            {
                throw new ArgumentException("Initial position is out of plateau bounds.");
            }

            this.Plateau = plateau ?? throw new ArgumentNullException(nameof(plateau), "Rover must be assigned to a plateau.");
            this.Position = new Position(x, y, orientation);

            RecordCurrentPosition();
        }
        public Position GetInitialPosition() => PositionHistory.First();

        public void MoveForward()
        {
            switch (Position.Orientation)
            {
                case Direction.N:
                    if (Position.Y < Plateau.Height - 1) Position.Y++;
                    break;
                case Direction.S:
                    if (Position.Y > 0) Position.Y--;
                    break;
                case Direction.E:
                    if (Position.X < Plateau.Width - 1) Position.X++;
                    break;
                case Direction.W:
                    if (Position.X > 0) Position.X--;
                    break;
            }

            RecordCurrentPosition();
        }

        public void TurnLeft()
        {
            Position.Orientation = Position.Orientation switch
            {
                Direction.N => Direction.W,
                Direction.W => Direction.S,
                Direction.S => Direction.E,
                Direction.E => Direction.N,
                _ => Position.Orientation
            };

            RecordCurrentPosition();
        }

        public void TurnRight()
        {
            Position.Orientation = Position.Orientation switch
            {
                Direction.N => Direction.E,
                Direction.E => Direction.S,
                Direction.S => Direction.W,
                Direction.W => Direction.N,
                _ => Position.Orientation
            };

            RecordCurrentPosition();
        }

        private void RecordCurrentPosition()
        {
            PositionHistory.Add(new Position(Position));
        }
    }
}
