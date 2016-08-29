using System.Collections.Generic;
using System.Linq;
using Main.Model;
using System;

namespace Main.BrkgaTop
{
    public interface IProblemDecoder
    {
        Solution Decode(EncodedSolution encodedSolution);

        ProblemResourceProvider Provider { get; set; }
    }

    public class ProblemDecoder : IProblemDecoder
    {
        public ProblemDecoder(ProblemResourceProvider provider)
        {
            Provider = provider;
        }


        public Solution Decode(EncodedSolution encodedSolution)
        {
            var orderedRandomKeys = encodedSolution.GetOrderedRandomKeys();

            var problem = Provider.GetFreshProblem();

            var keyIndex = 0;
            var vehicle = problem.VehicleFleet.Vehicles[0];
            while (keyIndex < orderedRandomKeys.Count)
            {
                var currentDestination = problem.Map.Destinations[orderedRandomKeys[keyIndex].Position];
                vehicle = GetNextAvailableVehicleFor(problem, currentDestination, vehicle.Number);
                if(vehicle == null)
                    break;

                vehicle.Route.AddDestination(currentDestination);
                keyIndex++;
            }

            return problem;
        }

        public ProblemResourceProvider Provider { get; set; }

        public Vehicle GetNextAvailableVehicleFor(Solution solution, Destination destination, int currentVehicleNumber)
        {
            while (currentVehicleNumber <= solution.VehicleFleet.Vehicles.Count)
            {
                var currentVehicle = solution.VehicleFleet.GetByNumber(currentVehicleNumber);

                var distanceFromCurrentDestinationToNewDestination = solution.Map.GetDistance(currentVehicle.Route.CurrentLastDestination, destination);
                var distanceFromNewDestinationToEnding = solution.Map.GetDistance(destination, currentVehicle.Route.EndingPoint);

                if (currentVehicle.Route.GetDistanceWithoutFinalReturn(solution.Map) + distanceFromCurrentDestinationToNewDestination + distanceFromNewDestinationToEnding <= currentVehicle.MaxDistance)
                    return currentVehicle;

                currentVehicleNumber++;
            }
            return null;
        }
    }
}
