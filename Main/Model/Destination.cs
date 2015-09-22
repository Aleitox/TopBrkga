
namespace Main.Model
{
    public class Destination
    {
        public Destination(int id, double profit)
        {
            Id = id;
            Profit = profit;
        }

        public int Id { get; set; }

        public double Profit { get; set; }
    }
}
