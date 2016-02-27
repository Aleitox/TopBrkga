using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

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

        public Destination CurrentDestination {
            get
            {
                if (Destinations.Count > 0)
                    return Destinations.Last();

                return Depot;
            } 
        }

        public double GetProfit()
        {
            return Destinations.Sum(d => d.Profit);
        }

        public double GetDistance(IMap map)
        {
            if (!Destinations.Any()) return 0;
            var distance = GetDistanceWithoutFinalReturn(map);
            distance += map.GetDistance(Destinations.Last(), Depot);
            return distance;
        }

        public double GetDistanceWithoutFinalReturn(IMap map)
        {
            if (Destinations.Count == 0)
                return 0;

            var distance = map.GetDistance(Depot, Destinations.First());

            for (var index = 0; index < Destinations.Count - 1; index++)
                distance += map.GetDistance(Destinations[index], Destinations[index + 1]);

            return distance;
        }

        public void AddDestination(Destination destination)
        {
            Destinations.Add(destination);
        }
    }
}
