using RoverControl.Core;
using RoverControl.Core.Enums;

namespace RoverControl.Tests
{
    [TestFixture]
    public class RoverTests
    {
        private Plateau _plateau;

        [SetUp]
        public void Setup()
        {
            _plateau = new Plateau(6, 6);
        }

        [Test]
        public void Rover_ShouldInitializeCorrectly()
        {
            var rover = new Rover(3, 4, Direction.E, _plateau);
            Assert.Multiple(() =>
            {
                Assert.That(rover.Position.X, Is.EqualTo(3));
                Assert.That(rover.Position.Y, Is.EqualTo(4));
                Assert.That(rover.Position.Orientation, Is.EqualTo(Direction.E));
                Assert.That(rover.PositionHistory, Has.Count.EqualTo(1));
                Assert.That(rover.PositionHistory.First().X, Is.EqualTo(3));
                Assert.That(rover.PositionHistory.First().Y, Is.EqualTo(4));
                Assert.That(rover.PositionHistory.First().Orientation, Is.EqualTo(Direction.E));
            });
        }

        [Test]
        public void Rover_ThrowsArgumentException_ForOutOfBoundsCoordinates()
        {
            Assert.Throws<ArgumentException>(() => new Rover(0, -1, Direction.S, _plateau));
            Assert.Throws<ArgumentException>(() => new Rover(-1, -0, Direction.S, _plateau));
            Assert.Throws<ArgumentException>(() => new Rover(6, 2, Direction.S, _plateau));
            Assert.Throws<ArgumentException>(() => new Rover(2, 6, Direction.S, _plateau));
        }

        [Test]
        public void Rover_ShouldMoveForward_North()
        {
            var rover = new Rover(1, 2, Direction.N, _plateau);
            rover.MoveForward();

            Assert.Multiple(() =>
            {
                Assert.That(rover.Position.X, Is.EqualTo(1));
                Assert.That(rover.Position.Y, Is.EqualTo(3));
                Assert.That(rover.Position.Orientation, Is.EqualTo(Direction.N));
            });
        }

        [Test]
        public void Rover_ShouldMoveForward_South()
        {
            var rover = new Rover(1, 2, Direction.S, _plateau);
            rover.MoveForward();

            Assert.Multiple(() =>
            {
                Assert.That(rover.Position.X, Is.EqualTo(1));
                Assert.That(rover.Position.Y, Is.EqualTo(1));
                Assert.That(rover.Position.Orientation, Is.EqualTo(Direction.S));
            });
        }

        [Test]
        public void Rover_ShouldMoveForward_East()
        {
            var rover = new Rover(1, 2, Direction.E, _plateau);
            rover.MoveForward();

            Assert.Multiple(() =>
            {
                Assert.That(rover.Position.X, Is.EqualTo(2));
                Assert.That(rover.Position.Y, Is.EqualTo(2));
                Assert.That(rover.Position.Orientation, Is.EqualTo(Direction.E));
            });
        }

        [Test]
        public void Rover_ShouldMoveForward_West()
        {
            var rover = new Rover(1, 2, Direction.W, _plateau);
            rover.MoveForward();

            Assert.Multiple(() =>
            {
                Assert.That(rover.Position.X, Is.EqualTo(0));
                Assert.That(rover.Position.Y, Is.EqualTo(2));
                Assert.That(rover.Position.Orientation, Is.EqualTo(Direction.W));
            });
        }

        [Test]
        public void Rover_ShouldTurn_Left()
        {
            var rover = new Rover(1, 2, Direction.W, _plateau);
            rover.TurnLeft();
            Assert.That(rover.Position.Orientation, Is.EqualTo(Direction.S));

            rover.TurnLeft();
            Assert.That(rover.Position.Orientation, Is.EqualTo(Direction.E));

            rover.TurnLeft();
            Assert.That(rover.Position.Orientation, Is.EqualTo(Direction.N));

            rover.TurnLeft();
            Assert.That(rover.Position.Orientation, Is.EqualTo(Direction.W));
        }

        [Test]
        public void Rover_ShouldTurn_Right()
        {
            var rover = new Rover(1, 2, Direction.W, _plateau);
            rover.TurnRight();
            Assert.That(rover.Position.Orientation, Is.EqualTo(Direction.N));

            rover.TurnRight();
            Assert.That(rover.Position.Orientation, Is.EqualTo(Direction.E));

            rover.TurnRight();
            Assert.That(rover.Position.Orientation, Is.EqualTo(Direction.S));

            rover.TurnRight();
            Assert.That(rover.Position.Orientation, Is.EqualTo(Direction.W));
        }

        [Test]
        public void Rover_ShouldNotFallOff_PlateauLeft()
        {
            var rover = new Rover(0, 2, Direction.W, _plateau);
            rover.MoveForward();
            Assert.That(rover.Position.X, Is.EqualTo(0));
        }

        [Test]
        public void Rover_ShouldNotFallOff_PlateauRight()
        {
            var rover = new Rover(5, 0, Direction.E, _plateau);
            rover.MoveForward();
            Assert.That(rover.Position.X, Is.EqualTo(5));
        }

        [Test]
        public void Rover_ShouldNotFallOff_PlateauTop()
        {
            var rover = new Rover(0, 5, Direction.N, _plateau);
            rover.MoveForward();
            Assert.That(rover.Position.Y, Is.EqualTo(5));
        }

        [Test]
        public void Rover_ShouldNotFallOff_PlateauBottom()
        {
            var rover = new Rover(0, 0, Direction.S, _plateau);
            rover.MoveForward();
            Assert.That(rover.Position.Y, Is.EqualTo(0));
        }

        [Test]
        public void Rover_RoverShouldMoveCorrectly()
        {
            var rover = new Rover(1, 2, Direction.N, _plateau);
            rover.TurnLeft();
            rover.MoveForward();
            rover.TurnLeft();
            rover.MoveForward();
            rover.TurnLeft();
            rover.MoveForward();
            rover.TurnLeft();
            rover.MoveForward();
            rover.MoveForward();

            Assert.That(rover.Position.X, Is.EqualTo(1));
            Assert.That(rover.Position.Y, Is.EqualTo(3));
            Assert.That(rover.Position.Orientation, Is.EqualTo(Direction.N));

            rover = new Rover(3, 3, Direction.E, _plateau);
            rover.MoveForward();
            rover.MoveForward();
            rover.TurnRight();
            rover.MoveForward();
            rover.MoveForward();
            rover.TurnRight();
            rover.MoveForward();
            rover.TurnRight();
            rover.TurnRight();
            rover.MoveForward();

            Assert.That(rover.Position.X, Is.EqualTo(5));
            Assert.That(rover.Position.Y, Is.EqualTo(1));
            Assert.That(rover.Position.Orientation, Is.EqualTo(Direction.E));
        }

        [Test]
        public void Rover_ShouldRecordPositionHistory()
        {
            var rover = new Rover(1, 2, Direction.N, _plateau);
            rover.MoveForward();
            rover.TurnLeft();
            rover.TurnLeft();
            rover.TurnLeft();
            rover.MoveForward();
            rover.TurnRight();
            rover.MoveForward();

            Assert.That(rover.PositionHistory.Count, Is.EqualTo(8));

            Assert.That(rover.PositionHistory[0].X, Is.EqualTo(1));
            Assert.That(rover.PositionHistory[0].Y, Is.EqualTo(2));
            Assert.That(rover.PositionHistory[0].Orientation, Is.EqualTo(Direction.N));

            Assert.That(rover.PositionHistory[1].X, Is.EqualTo(1));
            Assert.That(rover.PositionHistory[1].Y, Is.EqualTo(3));
            Assert.That(rover.PositionHistory[1].Orientation, Is.EqualTo(Direction.N));

            Assert.That(rover.PositionHistory[2].X, Is.EqualTo(1));
            Assert.That(rover.PositionHistory[2].Y, Is.EqualTo(3));
            Assert.That(rover.PositionHistory[2].Orientation, Is.EqualTo(Direction.W));

            Assert.That(rover.PositionHistory[3].X, Is.EqualTo(1));
            Assert.That(rover.PositionHistory[3].Y, Is.EqualTo(3));
            Assert.That(rover.PositionHistory[3].Orientation, Is.EqualTo(Direction.S));

            Assert.That(rover.PositionHistory[4].X, Is.EqualTo(1));
            Assert.That(rover.PositionHistory[4].Y, Is.EqualTo(3));
            Assert.That(rover.PositionHistory[4].Orientation, Is.EqualTo(Direction.E));

            Assert.That(rover.PositionHistory[5].X, Is.EqualTo(2));
            Assert.That(rover.PositionHistory[5].Y, Is.EqualTo(3));
            Assert.That(rover.PositionHistory[5].Orientation, Is.EqualTo(Direction.E));

            Assert.That(rover.PositionHistory[6].X, Is.EqualTo(2));
            Assert.That(rover.PositionHistory[6].Y, Is.EqualTo(3));
            Assert.That(rover.PositionHistory[6].Orientation, Is.EqualTo(Direction.S));

            Assert.That(rover.PositionHistory[7].X, Is.EqualTo(2));
            Assert.That(rover.PositionHistory[7].Y, Is.EqualTo(2));
            Assert.That(rover.PositionHistory[7].Orientation, Is.EqualTo(Direction.S));
        }

        [Test]
        public void Rover_ShouldReturnFirstPositionInHistory()
        {
            var rover = new Rover(3, 4, Direction.E, _plateau);
            rover.TurnRight();
            rover.MoveForward();
            rover.TurnLeft();
            rover.MoveForward();
            Assert.That(rover.GetInitialPosition().X, Is.EqualTo(3));
            Assert.That(rover.GetInitialPosition().Y, Is.EqualTo(4));
            Assert.That(rover.GetInitialPosition().Orientation, Is.EqualTo(Direction.E));
        }
    }
}