using Main.Entities;
using Main.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Vehicle = Main.Model.Vehicle;

namespace Main.Factory
{
    public static class ProblemProviderFactory
    {
        public static ProblemResourceProvider CreateProblemProvider(List<List<string>> input)
        {
            var amountOfVehicles = Convert.ToInt32(input[1][1]);
            var vehicleMaxDistance = Convert.ToDecimal(input[2][1]);

            var profits = new List<int>();
            var coordinates = new List<Coordinate>();

            for (var index = 3; index < input.Count; index++)
            {
                profits.Add(Convert.ToInt32(input[index][2]));
                coordinates.Add(new Coordinate(Convert.ToDecimal(input[index][0]), Convert.ToDecimal(input[index][1])));
            }

            return CreateProblemProvider(profits, coordinates, amountOfVehicles, vehicleMaxDistance);
        }

        private static ProblemResourceProvider CreateProblemProvider(List<int> profits, List<Coordinate> coordinates, int amountOfVehicles, decimal vehicleMaxDistance)
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
                var route = new Route(destinations.First(), destinations.Last());
                var vehicle = new Vehicle(Convert.ToInt16(index + 1), vehicleMaxDistance, route);
                vehicleFleet.Vehicles.Add(vehicle);
            }

            return new ProblemResourceProvider(map, vehicleFleet);
        }

        public static ProblemResourceProvider CreateProblemProvider(Instance instance)
        {
            var destinations = instance.Destinies.Select(destiny => new Destination(destiny)).ToList();
            var map = new Map(destinations);
            var vehicleFleet = new VehicleFleet();

            for (var index = 0; index < instance.Vehicles; index++)
            {
                var route = new Route(destinations.First(), destinations.Last());
                var vehicle = new Vehicle(Convert.ToInt16(index + 1), instance.TMax, route);
                vehicleFleet.Vehicles.Add(vehicle);
            }

            return new ProblemResourceProvider(map, vehicleFleet);
        }

        private static bool ValidateArgs(List<int> profits, List<Coordinate> distances)
        {
            // Validar que solo hay dos puntos destinations con mismas coordenadas. El depot, primero y ultimo
            // Validar que primero y ultimo es depot (el mismo y con profit 0)

            // TODO: Importante. Validar que todo punto es alcanzable. Es decir, que para todo Destino d, tmax >= distancia(depot, d) * 2. Luego excluir estos destinos, ya que estan al pedo
            return true;
        }
    }
}
