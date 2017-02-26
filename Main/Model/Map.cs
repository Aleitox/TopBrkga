using System;
using System.Collections.Generic;
using System.Linq;
using Main.Helpers;

namespace Main.Model
{
    public interface IMap
    {
        decimal GetDistance(int destinationFrom, int destinationTo);

        decimal GetDistance(Destination destinationFrom, Destination destinationTo);

        List<Destination> Destinations { get; }

        List<Destination> GetNonProfitDestinations();

        Destination GetStartingDestination();

        Destination GetEndingDestination();

        List<Destination> GetUnvisitedDestinations(List<Route> routes);
    }

    public class Map : IMap
    {
        public Map(List<Destination> destinations)
        {
            Destinations = destinations;
        }

        // TODO: Esta forma de calcular distancia se puede mejorar (igual solo se usa desde tests)
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
            return EuclidianCalculator.GetDistanceBetween(destinationFrom.Coordinate.X, destinationFrom.Coordinate.Y, destinationTo.Coordinate.X, destinationTo.Coordinate.Y);
        }

        public List<Destination> Destinations { get; private set; }

        private List<Destination> NonProfitDestinations { get; set; }

        public List<Destination> GetNonProfitDestinations()
        {
            return NonProfitDestinations ?? (NonProfitDestinations = Destinations.Where(d => d.Profit == 0).ToList());
        }

        private Destination StartingDestination { get; set; }

        public Destination GetStartingDestination()
        {
            return StartingDestination ?? (StartingDestination = Destinations.First(d => d.Profit == 0));
        }

        private Destination EndingDestination { get; set; }

        public Destination GetEndingDestination()
        {
            return EndingDestination ?? (EndingDestination = Destinations.Last(d => d.Profit == 0));
        }

        // TODO: Test Method
        public List<Destination> GetUnvisitedDestinations(List<Route> routes)
        {
            var visitedDestinations = new List<Destination>();
            foreach (var route in routes)
                visitedDestinations.AddRange(route.GetDestinations);
            
            return Destinations.Where(destination => destination.Profit != 0 && visitedDestinations.All(vd => vd.Id != destination.Id)).ToList();
        }
    }
}
