using System.Collections.Generic;
using System.Security.AccessControl;
using System.Linq;

namespace Main.Model
{
    public class Vehicle
    {
        public Vehicle(short number, decimal maxDistance, Destination startingPoint, Destination endingPoint)
        {
            Number = number;
            MaxDistance = maxDistance;
            Route = new Route(startingPoint, endingPoint);
        }

        public Vehicle(short number, decimal maxDistance, Route route)
        {
            Number = number;
            MaxDistance = maxDistance;
            Route = route;
        }

        public int Id { get; set; }

        public short Number { get; set; }

        public decimal MaxDistance { get; set; }

        public Route Route { get; set; }

        public string GetHash()
        {
            return string.Join("@", Route.GetDestinations.Select(x => x.PositionIndex));
        }
    }
}
