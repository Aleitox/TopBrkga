using Main.BrkgaTop;
using Main.BrkgaTop.Encoders;
using Main.Model;
using System.Collections.Generic;
using System.Linq;

namespace Main.GuidedLocalSearchHeuristics
{
    public class InsertHeuristic : ILocalSearchHeuristic
    {
        public void ApplyHeuristic(ref EncodedSolution encodedSolution)
        {
            var solution = encodedSolution.GetSolution;

            // Lista de todos los clientes no visitados
            var unvistedDestinations = solution.GetCurrentUnvistedDestination;
            
            // Lista de rutas
            var vehicles = solution.VehicleFleet.Vehicles;

            foreach (var vehicle in vehicles)
            {
                vehicle.Route.ActivateCog();
                unvistedDestinations = unvistedDestinations.OrderBy(x => vehicle.Route.CenterOfGravity.GetDistanceToCog(x)).ToList();

                var insertedDestinations = new List<Destination>();
                for (var index = 0; index < unvistedDestinations.Count; index++)
                {
                    var analizeResults = LocalSearchHeuristicHelper.AnalizeInsert(solution, vehicle, unvistedDestinations[index]);
                    if (analizeResults.CanBeInserted)  // TODO: VARIACION de Insert ACA
                    {
                        // TODO Test hubo un cambio
                        vehicle.Route.AddDestinationAt(unvistedDestinations[index], analizeResults.BestInsertPosition);
                        insertedDestinations.Add(unvistedDestinations[index]);
                    }
                }
                unvistedDestinations = unvistedDestinations.Where(ud => insertedDestinations.All(id => id.Id != ud.Id)).ToList();
            }
        }
    }
}
