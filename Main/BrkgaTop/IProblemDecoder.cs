using System.Collections.Generic;
using System.Linq;
using Main.Model;
using System;

namespace Main.BrkgaTop
{
    public interface IProblemDecoder
    {
        Problem Decode(List<RandomKey> randomKeys);

        ProblemResourceProvider Provider { get; set; }
    }

    public class ProblemDecoder : IProblemDecoder
    {
        public ProblemDecoder(ProblemResourceProvider provider)
        {
            Provider = provider;
        }


        public Problem Decode(List<RandomKey> randomKeys)
        {
            var orderedRandomKeys = randomKeys.OrderBy(rk => rk.Key).ToList();

            var problem = Provider.GetFreshProblem();

            var keyIndex = 0;
            var vehicle = problem.VehicleFleet.Vehicles[0];
            while (keyIndex < randomKeys.Count)
            {
                var currentDestination = problem.Map.Destinations[orderedRandomKeys[keyIndex].Position];
                vehicle = GetNextAvailableVehicleFor(problem, currentDestination, vehicle.Id);
                if(vehicle == null)
                    break;

                vehicle.Route.AddDestination(currentDestination);
                keyIndex++;
            }

            return problem;
        }

        public ProblemResourceProvider Provider { get; set; }

        public Vehicle GetNextAvailableVehicleFor(Problem problem, Destination destination, int currentVehicleId)
        {
            while (currentVehicleId < problem.VehicleFleet.Vehicles.Count)
            {
                var currentVehicle = problem.VehicleFleet.GetById(currentVehicleId);

                var distanceFromCurrentDestinationToNewDestination = problem.Map.GetDistance(currentVehicle.Route.CurrentDestination, destination);
                var distanceFromNewDestinationToDepot = problem.Map.GetDistance(destination, currentVehicle.Route.Depot);

                if (currentVehicle.Route.GetDistanceWithoutFinalReturn(problem.Map) + distanceFromCurrentDestinationToNewDestination + distanceFromNewDestinationToDepot <= currentVehicle.MaxDistance)
                    return currentVehicle;

                currentVehicleId++;
            }
            return null;
        }
    }
}
