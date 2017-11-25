using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Main.Model;

namespace Main.BrkgaTop.Decoders
{
    /*
        Este fue el primer decoder.
        Logica simple. Toma destinos en orden. Por cada destino, intenta asignarlo al primer auto disponible.
        Si entra, lo agrega y toma otro destino.
        Si no entra, toma otro auto.
        Si no hay autos termina.
        
        Pro: Lineal en funcion del suma autos + destinos
        Contra: Vehiculos sin destino por la aparicion de un destino inalcanzable.
    */

    public class FirstSimpleDecoder : IProblemDecoder
    {
        public FirstSimpleDecoder(ProblemResourceProvider provider)
        {
            Provider = provider;
        }


        public Solution Decode(EncodedSolution encodedSolution)
        {
            return Decode(encodedSolution.RandomKeys);
        }

        public Solution Decode(List<RandomKey> randomKeys)
        {
            var orderedRandomKeys = randomKeys.OrderBy(rk => rk.Key).ToList();

            var solution = Provider.GetFreshProblem();

            var keyIndex = 0;
            var vehicle = solution.VehicleFleet.Vehicles[0];
            var vehicleNumber = vehicle.Number;
            while (keyIndex < orderedRandomKeys.Count)
            {
                var currentDestination = solution.Map.Destinations[orderedRandomKeys[keyIndex].PositionIndex];
                vehicle = GetNextAvailableVehicleFor(solution, currentDestination, vehicleNumber);
                if (vehicle == null)
                    break;

                vehicleNumber = vehicle.Number;

                vehicle.Route.AddDestination(currentDestination);

                if (orderedRandomKeys[keyIndex].ForceVehicleChangeAfterThis)
                    vehicleNumber++;

                keyIndex++;
            }

            return solution;
        }

        public ProblemResourceProvider Provider { get; set; }

        public Vehicle GetNextAvailableVehicleFor(Solution solution, Destination destination, int currentVehicleNumber)
        {
            while (currentVehicleNumber <= solution.VehicleFleet.Vehicles.Count)
            {
                var currentVehicle = solution.VehicleFleet.GetByNumber(currentVehicleNumber);

                var distanceFromCurrentDestinationToNewDestination = solution.Map.GetDistance(currentVehicle.Route.CurrentLastDestination, destination);
                var distanceFromNewDestinationToEnding = solution.Map.GetDistance(destination, currentVehicle.Route.EndingPoint);

                if (currentVehicle.Route.GetDistanceWithoutFinalReturn() + distanceFromCurrentDestinationToNewDestination + distanceFromNewDestinationToEnding <= currentVehicle.MaxDistance)
                    return currentVehicle;

                currentVehicleNumber++;
            }
            return null;
        }
    }
}
