using System;
using System.Collections.Generic;
using System.Linq;

namespace Main.Model
{
    public interface IMap
    {
        decimal GetDistance(int destinationFrom, int destinationTo);

        decimal GetDistance(Destination destinationFrom, Destination destinationTo);

        List<Destination> Destinations { get; }

        List<Destination> GetNonProfitDestinations();
    }

    public class Map : IMap
    {
        public Map(List<Destination> destinations)
        {
            Destinations = destinations;
        }

        public decimal GetDistance(int idFrom, int idTo)
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

        public decimal GetDistance(Destination destinationFrom, Destination destinationTo)
        {
            var xDiff = destinationFrom.Coordinate.X - destinationTo.Coordinate.X;
            var yDiff = destinationFrom.Coordinate.Y - destinationTo.Coordinate.Y;
            return Convert.ToDecimal(Math.Sqrt((double)(xDiff * xDiff + yDiff * yDiff)));
        }

        public List<Destination> Destinations { get; private set; }

        private List<Destination> nonProfitDestinations;

        public List<Destination> GetNonProfitDestinations()
        {
            return nonProfitDestinations ?? (nonProfitDestinations = Destinations.Where(d => d.Profit == 0).ToList());
        }
    }
}
