using Main.Model;
using System.Collections.Generic;
using System.Linq;

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
                //var destination = map.Destinations.First(d => d.Id == randomKey.DestinationId); // TODO Refactor Aca hay un abuso que se rompe con las instancias. Asume que la posicion en la lista es igual a su Id. Traer por ID pero en O(1)
                var destination = map.Destinations[randomKey.PositionIndex];
                if (!usedDestinationsIds.Contains(destination.Id) && CanAddDestination(map, route, destination, maxDistance))
                {
                    route.AddDestination(destination);
                    usedDestinationsIds.Add(destination.Id);
                    // Es usado para garantizar que la solucion obtenida de una busqueda local se obtenga del decode
                    if (randomKey.ForceVehicleChangeAfterThis)
                        break;
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
