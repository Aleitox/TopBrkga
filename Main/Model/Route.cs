using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Main.Model
{
    public class Route
    {
        public Route(Destination startingPoint, Destination endingPoint)
        {
            Destinations = new List<Destination>();
            StartingPoint = startingPoint;
            EndingPoint = endingPoint;
        }

        public List<Destination> Destinations { get; set; }

        public Destination StartingPoint { get; set; }
        public Destination EndingPoint { get; set; }

        public Destination CurrentLastDestination {
            get
            {
                return Destinations.Count > 0 ? Destinations.Last() : StartingPoint;
            }
        }

        public double GetProfit()
        {
            return Destinations.Sum(d => d.Profit);
        }

        public decimal GetDistance(IMap map)
        {
            if (!Destinations.Any())
                return map.GetDistance(StartingPoint, EndingPoint);

            var distance = GetDistanceWithoutFinalReturn(map);
            distance += map.GetDistance(Destinations.Last(), EndingPoint);
            return distance;
        }

        public decimal GetDistanceWithoutFinalReturn(IMap map)
        {
            if (Destinations.Count == 0)
                return 0;

            var distance = map.GetDistance(StartingPoint, Destinations.First());

            for (var index = 0; index < Destinations.Count - 1; index++)
                distance += map.GetDistance(Destinations[index], Destinations[index + 1]);

            return distance;
        }

        public void AddDestination(Destination destination)
        {
            Destinations.Add(destination);
        }

        
        public override string ToString()
        {
            var routeString = string.Empty;

            for (var index = 0; index < Destinations.Count; index++)
            {
                routeString += Destinations[index].Coordinate.ToString();
                if (index < Destinations.Count - 1)
                    routeString += " -> ";
            }
            return routeString;
        }

        public bool IsEquivalentTo(Route anotherRoute)
        {
            if (Destinations.Count != anotherRoute.Destinations.Count)
                return false;

            return !Destinations.Where((t, index) => t.Id != anotherRoute.Destinations[index].Id).Any();
        }
    }
}
