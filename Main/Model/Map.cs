using System;
using System.Collections.Generic;
using System.Linq;

namespace Main.Model
{
    public interface IMap
    {
        Double GetDistance(int destinationFrom, int destinationTo);

        Double GetDistance(Destination destinationFrom, Destination destinationTo);

        List<Destination> Destinations { get; }
    }

    public class Map : IMap
    {
        public Map(List<Destination> destinations)
        {
            Destinations = destinations;
        }

        public double GetDistance(int idFrom, int idTo)
        {
            var from = GetDestination(idFrom);
            var to = GetDestination(idTo);
            return GetDistance(from, to);
        }

        private Destination GetDestination(int id)
        {
            var dest = Destinations.FirstOrDefault(d => d.Id == id);
            if (dest == null)
                throw new Exception(string.Format("Destination out of range. Id: {0}. Amount of destinations: {1}", id, Destinations.Count));
            return dest;
        }

        public double GetDistance(Destination destinationFrom, Destination destinationTo)
        {
            return Math.Sqrt(Math.Pow(destinationFrom.Coordinate.X - destinationTo.Coordinate.X, 2) + Math.Pow(destinationFrom.Coordinate.Y - destinationTo.Coordinate.Y, 2));
        }

        public List<Destination> Destinations { get; private set; }
    }
}
