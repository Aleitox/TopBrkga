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
                    var analizeResults = AnalizeInsert(solution, vehicle, unvistedDestinations[index]);
                    if (analizeResults.CanBeInserted)  // TODO: VARIACION de Insert ACA
                    {
                        // TODO Test hubo un cambio
                        vehicle.Route.AddDestinationAt(unvistedDestinations[index], analizeResults.BestInsertPosition);
                        insertedDestinations.Add(unvistedDestinations[index]);
                    }
                }
                unvistedDestinations = unvistedDestinations.Where(ud => insertedDestinations.All(id => id.Id != ud.Id)).ToList();
            }

            encodedSolution = Encoder.UpdateEncodedSolution(encodedSolution, solution.GetCurrentRoutes);
        }

        private PreInsertAnalisis AnalizeInsert(Model.Solution solution, Model.Vehicle vehicle, Destination destination)
        {
            var currentDistanceCost = vehicle.Route.GetDistance();
            // NOTA: currentRouteLenght se usa para TimeBudggeAvg. Si no se visitan clientes, esta bien que sea 1 ya que seria los tracks total
            var currentTracks = vehicle.Route.RouteLenght() + 1;
            var currentDistanceAvg = currentDistanceCost/currentTracks;

            var tracks = vehicle.Route.GetCurrentTracks();

            var bestPosition = -1;
            var minimumInsertCost = decimal.MaxValue;

            for (var index = 0; index < tracks.Count; index++)
            {
                var trackDistance = solution.Map.GetDistance(tracks[index].From, tracks[index].To);
                var distanceFromTrackStartTonNewDest = solution.Map.GetDistance(tracks[index].From, destination);
                var distanceFromNewDestToTrackEnd = solution.Map.GetDistance(destination, tracks[index].To);

                var insertCost = distanceFromTrackStartTonNewDest + distanceFromNewDestToTrackEnd - trackDistance; // TODO: rename insertCost
                if (minimumInsertCost > insertCost)
                {
                    minimumInsertCost = insertCost;
                    bestPosition = index;
                }

            }
            var preInsertAnalisis = new PreInsertAnalisis();
            preInsertAnalisis.BestInsertPosition = bestPosition;
            preInsertAnalisis.NewTimeBudgetAvg = (currentDistanceCost + minimumInsertCost)/(currentTracks + 1);
            preInsertAnalisis.CurrentTimeBudgetAvg = currentDistanceAvg;
            preInsertAnalisis.CanBeInserted = vehicle.MaxDistance >= currentDistanceCost + minimumInsertCost;

            return preInsertAnalisis;
        }
    }
}
