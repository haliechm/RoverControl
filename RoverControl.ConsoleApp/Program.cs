using RoverControl.Core;
using RoverControl.Core.Enums;

namespace RoverControl.ConsoleApp
{
    internal class Program
    {

        // note to self:
        // another assumption: there is no collision between rovers if they are on same x,y coord

        static void Main(string[] args)
        {
            Console.WriteLine("Enter the file path of your mission:");

            var filePath = Console.ReadLine();

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Mission file not found: {filePath}");
                return;
            }

            // assumption: file is relatively small
            // would use streamreader to process each line instead of putting into memory otherwise
            var fileLines = File.ReadLines(filePath);
            var missionLines = new Queue<string>(fileLines);

            if (missionLines.Count == 0)
            {
                Console.WriteLine("Mission data not found.");
                return;
            }

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

            while (missionLines.Count > 0)
            {
                if (missionLines.Count < 2)
                {
                    Console.WriteLine("Insufficient instructions given for rover initialization.");
                    break;
                }

                // assumption: input always has rover instruction line following rover position line
                // e.g never 5 5 N \n 2 4 E \n LLMMLRML
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

                    // assumption: if invalid coords of rover are given, okay to initially set to (0, 0)
                    roverXCoord = 0;
                    roverYCoord = 0;
                }

                var rover = mission.AddRover(roverXCoord, roverYCoord, roverOrientation);
                mission.SendInstructionsToRover(rover, roverInstruction);
            }

            DisplayMission(mission);
        }

        static void DisplayMission(Mission mission)
        {
            Console.Clear();

            Console.WriteLine("Searching for Mars plateau...");
            Thread.Sleep(2500);

            DisplayPlateau(mission.Plateau, []);

            Console.WriteLine("Landing rovers on plateau...");
            Thread.Sleep(2500);

            Position[] roverPositionDisplays = new Position[mission.Rovers.Count];
            for (int i = 0; i < mission.Rovers.Count; i++)
            {
                var rover = mission.Rovers.ElementAt(i);
                roverPositionDisplays[i] = rover.GetInitialPosition();
            }

            DisplayPlateau(mission.Plateau, roverPositionDisplays);
            Console.WriteLine($"{mission.Rovers.Count} rovers have landed.");

            Thread.Sleep(2500);

            for (int i = 0; i < mission.Rovers.Count; i++)
            {
                var rover = mission.Rovers.ElementAt(i);
                foreach (var movement in rover.PositionHistory)
                {
                    roverPositionDisplays[i] = movement;
                    DisplayPlateau(mission.Plateau, roverPositionDisplays);
                    Console.WriteLine($"Rover {i + 1} receiving instructions...");

                    Thread.Sleep(750);
                }
            }

            DisplayPlateau(mission.Plateau, roverPositionDisplays);

            Console.WriteLine("Final rover positions:");
            Console.WriteLine();

            foreach (var rover in mission.Rovers)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
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
                    var roverOnCoordinate = roverPositions.Reverse().FirstOrDefault(p => p.X == x && p.Y == y);
                    if (roverOnCoordinate != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
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
    }
}
