using RoverControl.Core;
using RoverControl.Core.Enums;

namespace RoverControl.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Prompt user to enter the mission file path
                Console.Write("Enter the file path of your mission: ");
                var filePath = Console.ReadLine();

                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"Mission file not found: {filePath}");
                    return;
                }

                // Read the file lines into a queue for processing
                var missionLines = new Queue<string>(File.ReadLines(filePath));

                if (missionLines.Count == 0)
                {
                    Console.WriteLine("Mission data not found.");
                    return;
                }

                // Plateau set-up
                var plateauDimensions = missionLines.Dequeue().Split();

                if (plateauDimensions.Length != 2 || !int.TryParse(plateauDimensions[0], out int plateauUpperRightXCoord) || !int.TryParse(plateauDimensions[1], out int plateauUpperRightYCoord))
                {
                    Console.WriteLine("Invalid plateau definition. Ensure first line is two integers separated by a space.");
                    return;
                }

                if (plateauUpperRightXCoord < 0 || plateauUpperRightYCoord < 0)
                {
                    Console.WriteLine("Plateau coordinates cannot be negative.");
                    return;
                }

                var plateau = new Plateau(plateauUpperRightXCoord + 1, plateauUpperRightYCoord + 1);
                var mission = new Mission(plateau);

                // Rover instructions processing
                while (missionLines.Count > 0)
                {
                    // Each rover must have a line to indicate initial postion and a line for movement instructions
                    if (missionLines.Count < 2)
                    {
                        Console.WriteLine("Insufficient instructions given for rover initialization.");
                        break;
                    }

                    // Assumption: input always has rover position line followed by rover instruction line
                    var roverPosition = missionLines.Dequeue().Split();
                    var roverInstruction = missionLines.Dequeue();

                    if (roverPosition.Length != 3 || !int.TryParse(roverPosition[0], out int roverXCoord) || !int.TryParse(roverPosition[1], out int roverYCoord) || !Enum.TryParse(roverPosition[2], true, out Direction roverOrientation))
                    {
                        Console.WriteLine("Invalid rover position instruction.");
                        continue;
                    }

                    if (roverXCoord < 0 || roverXCoord >= plateau.Width || roverYCoord < 0 || roverYCoord >= plateau.Height)
                    {
                        Console.WriteLine("Initial rover position is out of bounds. Setting initial coordiates to (0, 0)");
                        roverXCoord = 0;
                        roverYCoord = 0;
                    }

                    // Add the rover to the mission and send it instructions
                    var rover = mission.AddRover(roverXCoord, roverYCoord, roverOrientation);
                    Mission.SendInstructionsToRover(rover, roverInstruction);
                }

                DisplayMission(mission);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}. Mission failed.");
                return;
            }
        }

        static void DisplayMission(Mission mission)
        {
            Console.Clear();

            Console.WriteLine("Searching for Mars plateau...");
            Thread.Sleep(2500); // Simulate search delay

            // Initial plateau display without any rovers
            DisplayPlateau(mission.Plateau, []);

            Console.WriteLine("Landing rovers on plateau...");
            Thread.Sleep(2500); // Simulate landing delay

            // Store initial positions of all rovers
            Position[] roverPositionDisplays = new Position[mission.Rovers.Count];
            for (int i = 0; i < mission.Rovers.Count; i++)
            {
                var rover = mission.Rovers.ElementAt(i);
                roverPositionDisplays[i] = rover.GetInitialPosition();
            }

            // Display plateau with initial rover positions
            DisplayPlateau(mission.Plateau, roverPositionDisplays);

            Console.WriteLine($"{mission.Rovers.Count} rovers have landed.");
            Thread.Sleep(2500); // Simulate delay before starting rover movements

            // Display each rover's movements step-by-step
            for (int i = 0; i < mission.Rovers.Count; i++)
            {
                var rover = mission.Rovers.ElementAt(i);
                foreach (var movement in rover.PositionHistory)
                {
                    roverPositionDisplays[i] = movement;
                    DisplayPlateau(mission.Plateau, roverPositionDisplays);
                    Console.WriteLine($"Rover {i + 1} receiving instructions...");
                    Thread.Sleep(750); // Simulate delay between each movement
                }
            }

            // Final plateau display after all movements are complete
            DisplayPlateau(mission.Plateau, roverPositionDisplays);

            Console.WriteLine("Final rover positions:");
            Console.WriteLine();

            foreach (var rover in mission.Rovers)
            {
                Console.ForegroundColor = GetDirectionColor(rover.Position.Orientation);
                Console.WriteLine($"{rover.Position.X} {rover.Position.Y} {rover.Position.Orientation}");
            }

            Console.ResetColor();
        }

        static void DisplayPlateau(Plateau plateau, IList<Position> roverPositions)
        {
            Console.Clear();
            Console.WriteLine();

            for (int y = plateau.Height - 1; y >= 0; y--)
            {
                for (int x = 0; x < plateau.Width; x++)
                {
                    // Check if there's a rover on the current coordinate (if multiple rovers on same coordinate, show last rover)
                    var roverOnCoordinate = roverPositions.Reverse().FirstOrDefault(p => p.X == x && p.Y == y);
                    if (roverOnCoordinate != null)
                    {
                        Console.ForegroundColor = GetDirectionColor(roverOnCoordinate.Orientation);
                        Console.Write($"   {roverOnCoordinate.Orientation}   ");
                        Console.ResetColor();
                    }
                    else Console.Write("   *   ");
                }
                Console.WriteLine();
                Console.WriteLine();

            }
            Console.WriteLine();
            Console.WriteLine();
        }

        static ConsoleColor GetDirectionColor(Direction direction)
        {
            return direction switch
            {
                Direction.N => ConsoleColor.Magenta,
                Direction.S => ConsoleColor.Green,
                Direction.E => ConsoleColor.Yellow,
                Direction.W => ConsoleColor.Blue,
                _ => ConsoleColor.White,
            };
        }
    }
}
