using RoverControl.Core.Enums;

namespace RoverControl.Core
{
    public class Mission
    {
        public Plateau Plateau { get; }
        public IList<Rover> Rovers { get; }

        public Mission(Plateau plateau)
        {
            this.Plateau = plateau ?? throw new ArgumentNullException(nameof(plateau), "Mission must have a plateau.");
            this.Rovers = new List<Rover>();
        }

        public Rover AddRover(int x, int y, Direction orientation)
        {
            var rover = new Rover(x, y, orientation, Plateau);
            Rovers.Add(rover);
            return rover;
        }

        public void MoveRover(Rover rover, string instruction)
        {
            if (rover == null) throw new ArgumentNullException(nameof(rover));
            if (instruction == null) throw new ArgumentNullException(nameof(instruction));

            foreach (char character in instruction)
            {
                switch (character)
                {
                    case 'M':
                        rover.MoveForward();
                        break;
                    case 'L':
                        rover.TurnLeft();
                        break;
                    case 'R':
                        rover.TurnRight();
                        break;
                }
            }
        }
    }
}
