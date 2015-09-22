using System.Collections.Generic;
using System.Linq;

namespace Main.Model
{
    public class Route
    {
        public Route(Destination depot)
        {
            Destinations = new List<Destination>();
            Depot = depot;
        }

        public List<Destination> Destinations { get; set; }

        public Destination Depot { get; set; }

        public double GetProfit()
        {
            return Destinations.Sum(d => d.Profit);
        }

        public double GetDistance(IMap map)
        {
            if (Destinations.Count == 0)
                return 0;

            var distance = map.GetDistance(Depot, Destinations.First());

            for (var index = 0; index < Destinations.Count - 1; index++)
                distance += map.GetDistance(Destinations[index], Destinations[index + 1]);

            distance += map.GetDistance(Destinations.Last(), Depot);

            return distance;
        }
    }
}
