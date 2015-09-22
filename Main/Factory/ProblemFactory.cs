using Main.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Main.Factory
{
    public static class ProblemFactory
    {
        public static Problem CreateProblem(double[] profits, double[][] distances, int amountOfVehicles, double vehicleMaxDistance)
        {
            if(!ValidateArgs(profits, distances))
                throw new Exception("Argumentos invalidos");

            var destinations = new List<Destination>();
            for (var index = 0; index < profits.Count(); index++)
                destinations.Add(new Destination(index, profits[index]));

            var map = new Map(distances, destinations);
            var vehicleFleet = new VehicleFleet();
            for (var index = 0; index < amountOfVehicles; index++)
            {
                var depot = new Destination(0, 0);
                var route = new Route(depot);
                var vehicle = new Vehicle(index, vehicleMaxDistance, route);
                vehicleFleet.Vehicles.Add(vehicle);
            }

            return new Problem(map, vehicleFleet);
        }

        private static bool ValidateArgs(double[] profits, double[][] distances)
        {
            return true;
        }
    }
}
