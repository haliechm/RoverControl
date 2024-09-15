using RoverControl.Core;
using RoverControl.Core.Enums;

namespace RoverControl.Tests
{
    [TestFixture]
    public class MissionTests
    {
        private Plateau _plateau;
        private Mission _mission;


        [SetUp]
        public void Setup()
        {
            _plateau = new Plateau(6, 6);
            _mission = new Mission(_plateau);
        }

        [Test]
        public void Mission_ShouldInitializeCorrectly()
        {
            Assert.Multiple(() =>
            {
                Assert.That(_mission.Plateau.Width, Is.EqualTo(6));
                Assert.That(_mission.Plateau.Height, Is.EqualTo(6));
                Assert.That(_mission.Rovers.Count, Is.EqualTo(0));
            });
        }

        [Test]
        public void Mission_ShouldAddRoversCorrectly()
        {
            _mission.AddRover(1, 2, Direction.N);
            _mission.AddRover(3, 3, Direction.E);

            Assert.That(_mission.Rovers.Count, Is.EqualTo(2));
        }

        [Test]
        public void Mission_RoverShouldMoveCorrectly_WithSequence_LMLMLMLMM()
        {
            var rover = _mission.AddRover(1, 2, Direction.N);
            Mission.SendInstructionsToRover(rover, "LMLMLMLMM");

            Assert.That(rover.Position.X, Is.EqualTo(1));
            Assert.That(rover.Position.Y, Is.EqualTo(3));
            Assert.That(rover.Position.Orientation, Is.EqualTo(Direction.N));
        }

        [Test]
        public void Mission_RoverShouldMoveCorrectly_WithSequence_MMRMMRMRRM()
        {
            var rover = _mission.AddRover(3, 3, Direction.E);
            Mission.SendInstructionsToRover(rover, "MMRMMRMRRM");

            Assert.That(rover.Position.X, Is.EqualTo(5));
            Assert.That(rover.Position.Y, Is.EqualTo(1));
            Assert.That(rover.Position.Orientation, Is.EqualTo(Direction.E));
        }

        [Test]
        public void Mission_RoverShouldMoveCorrectly_WithSequence_MMMMMLMMMMMLMMMMMLMMMMR()
        {
            var rover = _mission.AddRover(0, 0, Direction.E);
            Mission.SendInstructionsToRover(rover, "MMMMMLMMMMMLMMMMMLMMMMR");

            Assert.That(rover.Position.X, Is.EqualTo(0));
            Assert.That(rover.Position.Y, Is.EqualTo(1));
            Assert.That(rover.Position.Orientation, Is.EqualTo(Direction.W));
        }

        [Test]
        public void Mission_RoverShouldNotMoveOutsidePlateau()
        {
            var rover = _mission.AddRover(5, 5, Direction.E);
            Mission.SendInstructionsToRover(rover, "MLM");

            Assert.That(rover.Position.X, Is.EqualTo(5));
            Assert.That(rover.Position.Y, Is.EqualTo(5));
            Assert.That(rover.Position.Orientation, Is.EqualTo(Direction.N));
        }

        [Test]
        public void Mission_RoverShouldNotMoveOnSingleCellPlateau()
        {
            var mission = new Mission(new Plateau(1, 1));
            var rover = mission.AddRover(0, 0, Direction.E);
            Mission.SendInstructionsToRover(rover, "MRMMRLMML");

            Assert.That(rover.Position.X, Is.EqualTo(0));
            Assert.That(rover.Position.Y, Is.EqualTo(0));
            Assert.That(rover.Position.Orientation, Is.EqualTo(Direction.E));
        }

        [Test]
        public void Mission_RoverShouldSkipIncorrectCommand()
        {
            var rover = _mission.AddRover(1, 2, Direction.N);
            Mission.SendInstructionsToRover(rover, "OLMPSSSLMLMLMMX");

            Assert.That(rover.Position.X, Is.EqualTo(1));
            Assert.That(rover.Position.Y, Is.EqualTo(3));
            Assert.That(rover.Position.Orientation, Is.EqualTo(Direction.N));
        }

        [Test]
        public void Mission_RoverShouldNotMoveIfGivenEmptyInstruction()
        {
            var rover = _mission.AddRover(1, 2, Direction.N);
            Mission.SendInstructionsToRover(rover, "");

            Assert.That(rover.Position.X, Is.EqualTo(1));
            Assert.That(rover.Position.Y, Is.EqualTo(2));
            Assert.That(rover.Position.Orientation, Is.EqualTo(Direction.N));
        }
    }
}