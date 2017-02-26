using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Main.Model;

namespace Main.BrkgaTop.Decoders
{
    /*
     La idea es tomar un vehiculo y agregar de forma golosa y ordenada, todos los destinos que entren
     Luego el siguiente vehiculo con aquellos destinos que quedaron libres
     
     */

    public class GreedyVehicleDecoder : IProblemDecoder
    {
        public GreedyVehicleDecoder(ProblemResourceProvider provider)
        {
            Provider = provider;
        }

        public Solution Decode(EncodedSolution encodedSolution)
        {
            var orderedRandomKeys = encodedSolution.GetOrderedRandomKeys();

            var solution = Provider.GetFreshProblem();
            var usedDestinationsIds = new HashSet<int>();

            foreach (var vehicle in solution.VehicleFleet.Vehicles)
            {
                vehicle.Route = GetRoute(orderedRandomKeys, solution.Map, vehicle.MaxDistance, ref usedDestinationsIds);
            }

            return solution;
        }

        private Route GetRoute(List<RandomKey> orderedRandomKeys, IMap map, decimal maxDistance, ref HashSet<int> usedDestinationsIds)
        {
            var route = new Route(map.Destinations.First(), map.Destinations.Last());
            foreach (var randomKey in orderedRandomKeys)
            {
                var destination = map.Destinations[randomKey.Position];
                if (!usedDestinationsIds.Contains(destination.Id) && CanAddDestination(map, route, destination, maxDistance))
                {
                    route.AddDestination(destination);
                    usedDestinationsIds.Add(destination.Id);
                }
            }
            return route;
        }

        private bool CanAddDestination(IMap map, Route route, Destination destination, decimal maxDistance)
        {
            return maxDistance >= route.GetDistanceAdding(map, destination);
        }

        public ProblemResourceProvider Provider { get; set; }
    }
}
