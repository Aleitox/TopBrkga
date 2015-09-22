using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Main.Model
{
    public interface IMap
    {
        Double GetDistance(Destination destinationFrom, Destination destinationTo);

        List<Destination> Destinations { get; }
    }

    public class Map : IMap
    {
        public Map(double[][] distances, List<Destination> destinations)
        {
            Distances = distances;
        }

        private double[][] Distances { get; set; }

        public double GetDistance(Destination destinationFrom, Destination destinationTo)
        {
            return Distances[destinationFrom.Id][destinationTo.Id];
        }

        public List<Destination> Destinations { get; private set; }
    }
}
