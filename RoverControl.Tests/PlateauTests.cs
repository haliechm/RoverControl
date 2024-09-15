using RoverControl.Core;

namespace RoverControl.Tests
{
    [TestFixture]
    public class PlateauTests
    {
        [Test]
        public void Constructor_ValidArguments_ShouldSetProperties()
        {
            var plateau = new Plateau(5, 10);
            Assert.That(plateau.Width, Is.EqualTo(5));
            Assert.That(plateau.Height, Is.EqualTo(10));
        }

        [Test]
        public void Constructor_NegativeWidth_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new Plateau(-1, 10));
        }

        [Test]
        public void Constructor_NegativeHeight_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new Plateau(5, -1));
        }
    }
}
