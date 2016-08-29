
using Main.Entities;

namespace Main.Model
{
    public class Destination
    {
        public Destination(int id, int profit, Coordinate coordinate)
        {
            Id = id;
            Profit = profit;
            Coordinate = coordinate;
        }

        public Destination(Destiny destiny)
        {
            Id = destiny.Id;
            Profit = destiny.Profit;
            Coordinate = new Coordinate(destiny.X, destiny.Y);
        }

        public int Id { get; set; }

        public int Profit { get; set; }

        public Coordinate Coordinate { get; set; }
    }
}
