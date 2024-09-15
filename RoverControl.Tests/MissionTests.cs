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

    }
}