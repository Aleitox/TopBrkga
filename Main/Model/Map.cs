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
            var from = Destinations.FirstOrDefault(d => d.Id == idFrom);
            var to = Destinations.FirstOrDefault(d => d.Id == idTo);

            if(from == null || to == null)
                throw new Exception(string.Format("Uno de los dos Ids esta fuera de rango. From: {0}. To: {1}. Cantidad de destinations: {2}", idFrom, idTo, Destinations.Count));

            return GetDistance(from, to);
        }

        public double GetDistance(Destination destinationFrom, Destination destinationTo)
        {
            return Math.Sqrt(Math.Pow(destinationFrom.Coordinate.X - destinationTo.Coordinate.X, 2) + Math.Pow(destinationFrom.Coordinate.Y - destinationTo.Coordinate.Y, 2));
        }

        public List<Destination> Destinations { get; private set; }
    }
}
