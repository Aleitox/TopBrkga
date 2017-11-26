using System.Globalization;
using Main.Entities;
using Main.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Vehicle = Main.Model.Vehicle;
using Main.Helpers;

namespace Main.Factory
{
    public static class ProblemProviderFactory
    {
        public static ProblemResourceProvider CreateProblemProvider(List<List<string>> input)
        {
            var amountOfVehicles = Convert.ToInt32(input[1][1]);
            var culture = new CultureInfo("en-US");
            var vehicleMaxDistance = Convert.ToDecimal(input[2][1], culture);

            var profits = new List<int>();
            var coordinates = new List<Coordinate>();

            for (var index = 3; index < input.Count; index++)
            {
                profits.Add(Convert.ToInt32(input[index][2]));
                coordinates.Add(new Coordinate(Convert.ToDecimal(input[index][0], culture), Convert.ToDecimal(input[index][1], culture)));
            }

            return CreateProblemProvider(profits, coordinates, GetGenericDescriptions(coordinates.Count), amountOfVehicles, vehicleMaxDistance);
        }

        public static List<string> GetGenericDescriptions(int size)
        {
            if(size < 2)
                throw new Exception("Error 66");

            var descriptions = Enumerable.Repeat("Customers", size).ToList();
            descriptions[0] = "StartingPoint";
            descriptions[size - 1] = "EndingPoint";
            return descriptions;
        } 

        public static ProblemResourceProvider CreateProblemProvider(List<int> profits, List<Coordinate> coordinates, List<string> descriptions, int amountOfVehicles, decimal vehicleMaxDistance)
        {
            if (!ValidateArgs(profits, coordinates))
                throw new Exception("Argumentos invalidos");

            var destinations = new List<Destination>();
            for (var index = 0; index < profits.Count(); index++)
            {
                destinations.Add(new Destination(index, profits[index], coordinates[index], descriptions[index], index));
            }

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

        public static ProblemResourceProvider CreateProblemProvider(Instance instance, string solutionName)
        {
            var destinations = new List<Destination>();
            var dest = instance.Destinies.ToList();

            var startPoint = dest.First();
            var endPoint = dest.Last();

            var index = 0;
            var positionIndex = 0;
            while(index < dest.Count)
            {
                if (CanBeReached(dest[index], startPoint, endPoint, instance.TMax))
                {
                    destinations.Add(new Destination(dest[index], positionIndex));
                    positionIndex++;
                }
                index++;
            }

            var map = new Map(destinations);
            var vehicleFleet = new VehicleFleet();

            for (var i = 0; i < instance.Vehicles; i++)
            {
                var route = new Route(destinations.First(), destinations.Last());
                var vehicle = new Vehicle(Convert.ToInt16(i + 1), instance.TMax, route);
                vehicleFleet.Vehicles.Add(vehicle);
            }

            return new ProblemResourceProvider(map, vehicleFleet, instance.Id, solutionName);
        }

        private static bool CanBeReached(Destiny destiny, Destiny startPoint, Destiny endPoint, decimal tMax)
        {
            return EuclidianCalculator.GetDistanceBetween(startPoint, destiny) + EuclidianCalculator.GetDistanceBetween(destiny, endPoint) <= tMax;
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
