using System.Collections.Generic;

namespace Main.Model
{
    public class Vehicle
    {
        public Vehicle(int id, double maxDistance, Route route)
        {
            Id = id;
            MaxDistance = maxDistance;
            Route = route;
        }

        public int Id { get; set; }

        public double MaxDistance { get; set; }

        public Route Route { get; set; }
    }
}
