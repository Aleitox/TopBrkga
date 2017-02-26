using Main.Entities;
using Main.Model;
using System.Collections.Generic;
using System.Linq;

namespace Main.GuidedLocalSearchHeuristics
{
    public class LocalSearchHeuristic
    {
        public LocalSearchHeuristic(Model.Solution solution)
        {
            Solution = solution;
        }

        public Model.Solution Solution { get; set; }

        public void ApplySwap()
        {
        }

        public void ApplyInsert()
        {
            // Lista de todos los clientes no visitados
            var unvistedDestinations = Solution.GetCurrentUnvistedDestination;
            
            // Lista de rutas
            var vehicles = Solution.VehicleFleet.Vehicles;

            foreach (var vehicle in vehicles)
            {
                vehicle.Route.ActivateCog();
                unvistedDestinations = unvistedDestinations.OrderBy(x => vehicle.Route.CenterOfGravity.GetDistanceToCog(x)).ToList();

                var insertedDestinations = new List<Destination>();
                for (var index = 0; index < unvistedDestinations.Count; index++)
                {
                    var analizeResults = AnalizeInsert(vehicle, unvistedDestinations[index]);
                    if (analizeResults.CanBeInserted)  // TODO: Si hago variantes de insert, es en este punto donde se diferencia?
                    {
                        vehicle.Route.AddDestinationInPosition(unvistedDestinations[index], analizeResults.BestInsertPosition);
                        insertedDestinations.Add(unvistedDestinations[index]);
                    }
                }
                unvistedDestinations = unvistedDestinations.Where(ud => insertedDestinations.All(id => id.Id != ud.Id)).ToList();
            }

            // Por cada cliente no visitado se intenta insertar
        }

        // TODO: Test Method
        private PreInsertAnalisis AnalizeInsert(Model.Vehicle vehicle, Destination destination)
        {
            var currentDistanceCost = vehicle.Route.GetDistance(Solution.Map);
            var currentRouteLenght = vehicle.Route.RouteLenght() + 1;
            var currentDistanceAvg = currentDistanceCost/currentRouteLenght;

            var tracks = vehicle.Route.GetCurrentTracks();

            var bestPosition = -1;
            var bestInsertDistanceCost = decimal.MaxValue;

            for (var index = 0; index < tracks.Count; index++)
            {
                var trackDistance = Solution.Map.GetDistance(tracks[index].From, tracks[index].To);
                var distanceFromTrackStartTonNewDest = Solution.Map.GetDistance(tracks[index].From, destination);
                var distanceFromNewDestToTrackEnd = Solution.Map.GetDistance(destination, tracks[index].To);

                var insertDistance = distanceFromTrackStartTonNewDest + distanceFromNewDestToTrackEnd - trackDistance;
                if (bestInsertDistanceCost > insertDistance)
                {
                    bestInsertDistanceCost = insertDistance;
                    bestPosition = index;
                }

            }
            var preInsertAnalisis = new PreInsertAnalisis();
            preInsertAnalisis.BestInsertPosition = bestPosition;
            preInsertAnalisis.NewTimeBudgetAvg = currentDistanceCost + bestInsertDistanceCost/(currentRouteLenght + 1);
            preInsertAnalisis.CurrentTimeBudgetAvg = currentDistanceAvg;
            preInsertAnalisis.CanBeInserted = vehicle.MaxDistance >= currentDistanceCost + bestInsertDistanceCost;

            return preInsertAnalisis;
        }

        public void Apply2OptSwap()
        {
            //https://en.wikipedia.org/wiki/2-opt
        }

        public void ApplyReplace()
        {
            //for (var index = 0; index < Solution.VehicleFleet.Vehicles.Count; index++)
            //{
            //    ApplyReplaceHeuristic(Solution.VehicleFleet.Vehicles[index]);
            //}
        }

        private void ApplyReplace(Model.Vehicle vehicle)
        {
            // Necesito: 
            // Los que estan afuera (ordenados por que tan copados son)
            // Los que estan adentro (ordenados por que tan copados son)
            // Poder busar posicion ideal
            // Hacer el swap. El swap debe reflejarse en el codigo que. Es decir que cuando decodifique, debe quedar la solucion mejorada
        }

        

    }
}
