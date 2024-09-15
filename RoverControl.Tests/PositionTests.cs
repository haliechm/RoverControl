using RoverControl.Core;
using RoverControl.Core.Enums;

namespace RoverControl.Tests
{
    [TestFixture]
    public class PositionTests
    {
        [Test]
        public void Constructor_ValidArguments_ShouldSetProperties()
        {
            var position = new Position(1, 2, Direction.N);
            Assert.That(position.X, Is.EqualTo(1));
            Assert.That(position.Y, Is.EqualTo(2));
            Assert.That(position.Orientation, Is.EqualTo(Direction.N));
        }

        [Test]
        public void CopyConstructor_ShouldCopyValues()
        {
            var originalPosition = new Position(3, 4, Direction.E);
            var copiedPosition = new Position(originalPosition);

            Assert.That(copiedPosition.X, Is.EqualTo(originalPosition.X));
            Assert.That(copiedPosition.Y, Is.EqualTo(originalPosition.Y));
            Assert.That(copiedPosition.Orientation, Is.EqualTo(originalPosition.Orientation));

            Assert.That(copiedPosition, Is.Not.SameAs(originalPosition));
        }
    }
}
