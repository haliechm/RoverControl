namespace RoverControl.Core
{
    public class Plateau
    {
        public int Width { get; }
        public int Height { get; }

        public Plateau(int width, int height)
        {
            if (width <= 0 || height <= 0)
            {
                throw new ArgumentException("Width and height must be positive values.");
            }

            this.Width = width;
            this.Height = height;
        }
    }
}
