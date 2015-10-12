
namespace Main.Model
{
    public class Destination
    {
        public Destination(int id, double profit, Coordinate coordinate)
        {
            Id = id;
            Profit = profit;
            Coordinate = coordinate;
        }

        public int Id { get; set; }

        public double Profit { get; set; }

        public Coordinate Coordinate { get; set; }
    }
}
