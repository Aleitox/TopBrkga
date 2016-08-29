
namespace Main.Model
{
    public class Coordinate
    {
        public Coordinate(decimal x, decimal y)
        {
            X = x;
            Y = y;
        }

        public decimal X { get; set; }

        public decimal Y { get; set; }

        public override string ToString()
        {
            return string.Format("({0},{1})", X, Y);
        }
    }
}
