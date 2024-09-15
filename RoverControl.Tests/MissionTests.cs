using RoverControl.Core;
using RoverControl.Core.Enums;

namespace RoverControl.Tests
{
    [TestFixture]
    public class MissionTests
    {
        private Plateau _plateau;


        [SetUp]
        public void Setup()
        {
            _plateau = new Plateau(6, 6);
        }

        [Test]
        public void Mission_ShouldInitializeCorrectly()
        {
            var mission = new Mission(_plateau);
            Assert.Multiple(() =>
            {
                Assert.That(mission.Plateau.Width, Is.EqualTo(6));
                Assert.That(mission.Plateau.Height, Is.EqualTo(6));
                Assert.That(mission.Rovers.Count, Is.EqualTo(0));
            });
        }

        [Test]
        public void Mission_ShouldAddRoversCorrectly()
        {
            var mission = new Mission(_plateau);
            mission.AddRover(1, 2, Direction.N);
            mission.AddRover(3, 3, Direction.E);

            Assert.That(mission.Rovers.Count, Is.EqualTo(2));
        }

        [Test]
        public void Mission_RoverShouldMoveCorrectly_WithSequence_LMLMLMLMM()
        {
            var mission = new Mission(_plateau);
            var rover = mission.AddRover(1, 2, Direction.N);
            mission.SendInstructionsToRover(rover, "LMLMLMLMM");

            Assert.That(rover.Position.X, Is.EqualTo(1));
            Assert.That(rover.Position.Y, Is.EqualTo(3));
            Assert.That(rover.Position.Orientation, Is.EqualTo(Direction.N));
        }

        [Test]
        public void Mission_RoverShouldMoveCorrectly_WithSequence_MMRMMRMRRM()
        {
            var mission = new Mission(_plateau);
            var rover = mission.AddRover(3, 3, Direction.E);
            mission.SendInstructionsToRover(rover, "MMRMMRMRRM");

            Assert.That(rover.Position.X, Is.EqualTo(5));
            Assert.That(rover.Position.Y, Is.EqualTo(1));
            Assert.That(rover.Position.Orientation, Is.EqualTo(Direction.E));
        }

        [Test]
        public void Mission_RoverShouldMoveCorrectly_WithSequence_MMMMMLMMMMMLMMMMMLMMMMM()
        {
            var mission = new Mission(_plateau);
            var rover = mission.AddRover(0, 0, Direction.E);
            mission.SendInstructionsToRover(rover, "MMMMMLMMMMMLMMMMMLMMMMR");

            Assert.That(rover.Position.X, Is.EqualTo(0));
            Assert.That(rover.Position.Y, Is.EqualTo(1));
            Assert.That(rover.Position.Orientation, Is.EqualTo(Direction.W));
        }

        [Test]
        public void Mission_RoverShouldNotMoveOutsidePlateau()
        {
            var mission = new Mission(_plateau);
            var rover = mission.AddRover(5, 5, Direction.E);

            mission.SendInstructionsToRover(rover, "MLM");

            Assert.That(rover.Position.X, Is.EqualTo(5));
            Assert.That(rover.Position.Y, Is.EqualTo(5));
            Assert.That(rover.Position.Orientation, Is.EqualTo(Direction.N));
        }

        [Test]
        public void Mission_RoverShouldNotMoveOnSingleCellPlateau()
        {
            var mission = new Mission(new Plateau(1, 1));
            var rover = mission.AddRover(0, 0, Direction.E);
            mission.SendInstructionsToRover(rover, "MRMMRLMML");

            Assert.That(rover.Position.X, Is.EqualTo(0));
            Assert.That(rover.Position.Y, Is.EqualTo(0));
            Assert.That(rover.Position.Orientation, Is.EqualTo(Direction.E));
        }

        [Test]
        public void Mission_RoverShouldSkipIncorrectCommand()
        {
            var mission = new Mission(_plateau);
            var rover = mission.AddRover(1, 2, Direction.N);
            mission.SendInstructionsToRover(rover, "OLMPSSSLMLMLMMX");

            Assert.That(rover.Position.X, Is.EqualTo(1));
            Assert.That(rover.Position.Y, Is.EqualTo(3));
            Assert.That(rover.Position.Orientation, Is.EqualTo(Direction.N));
        }

        [Test]
        public void Mission_RoverShouldNotMoveIfGivenEmptyInstruction()
        {
            var mission = new Mission(_plateau);
            var rover = mission.AddRover(1, 2, Direction.N);
            mission.SendInstructionsToRover(rover, "");

            Assert.That(rover.Position.X, Is.EqualTo(1));
            Assert.That(rover.Position.Y, Is.EqualTo(2));
            Assert.That(rover.Position.Orientation, Is.EqualTo(Direction.N));
        }
    }
}