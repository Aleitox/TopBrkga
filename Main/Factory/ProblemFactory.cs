using Main.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Main.Factory
{
    public static class ProblemFactory
    {
        public static Problem CreateProblem(List<List<string>> input)
        {
            var amountOfVehicles = Convert.ToInt32(input[1][1]);
            var vehicleMaxDistance = Convert.ToDouble(input[2][1]);

            var profits = new List<double>();
            var coordinates = new List<Coordinate>();

            for (var index = 3; index < input.Count - 1; index++)
            {
                profits.Add(Convert.ToDouble(input[index][2]));
                coordinates.Add(new Coordinate(Convert.ToDouble(input[index][0]), Convert.ToDouble(input[index][1])));
            }

            return CreateProblem(profits, coordinates, amountOfVehicles, vehicleMaxDistance);
        }

        public static Problem CreateProblem(List<double> profits, List<Coordinate> coordinates, int amountOfVehicles, double vehicleMaxDistance)
        {
            if (!ValidateArgs(profits, coordinates))
                throw new Exception("Argumentos invalidos");

            var destinations = new List<Destination>();
            for (var index = 0; index < profits.Count(); index++)
                destinations.Add(new Destination(index, profits[index], coordinates[index]));

            var map = new Map(destinations);
            var vehicleFleet = new VehicleFleet();

            for (var index = 0; index < amountOfVehicles; index++)
            {
                var depot = destinations[0];
                var route = new Route(depot);
                var vehicle = new Vehicle(index, vehicleMaxDistance, route);
                vehicleFleet.Vehicles.Add(vehicle);
            }

            return new Problem(map, vehicleFleet);
        }

        private static bool ValidateArgs(List<double> profits, List<Coordinate> distances)
        {
            // Validar que solo hay dos puntos destinations con mismas coordenadas. El depot, primero y ultimo
            // Validar que primero y ultimo es depot (el mismo y con profit 0)

            return true;
        }
    }
}
