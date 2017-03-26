using System;
using System.Collections.Generic;
using Main.Brkga;
using Main.BrkgaTop;
using Main.BrkgaTop.Decoders;
using Main.BrkgaTop.Encoders;
using Main.Factory;
using Main.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TesisUnitTests
{
    [TestClass]
    public class EncoderTest
    {
        [TestMethod]
        public void Test_UpdateEncodedSolution()
        {
            var profits = new List<int>() {0, 3, 3, 3, 3, 0};
            var coordinates = new List<Coordinate>()
            {
                new Coordinate(0, 0),
                new Coordinate(4, 10),
                new Coordinate(6, 10),
                new Coordinate(4, -10),
                new Coordinate(6, -10),
                new Coordinate(10 , 0)
            };
            var descriptions = new List<string>()
            {
                "Start",
                "A1",
                "A2",
                "B1",
                "B2",
                "End"
            };

            var problemProvider = ProblemProviderFactory.CreateProblemProvider(profits, coordinates, descriptions, 2, 100);
            var problemDecoder = new GreedyVehicleDecoder(problemProvider);

            var randomVector = PopulationGenerator.GenerateRandomVector(6, 0, 2);
            var encodedSolution = new EncodedSolution(problemDecoder, randomVector);

            var routeOne = new Route(problemProvider.GetStartDestination(), problemProvider.GetEndingDestination());
            routeOne.AddDestination(problemProvider.GetDestinationByDescription("A1"));
            routeOne.AddDestination(problemProvider.GetDestinationByDescription("B2"));

            var routeTwo = new Route(problemProvider.GetStartDestination(), problemProvider.GetEndingDestination());
            routeTwo.AddDestination(problemProvider.GetDestinationByDescription("B1"));
            routeTwo.AddDestination(problemProvider.GetDestinationByDescription("A2"));

            var solucionPrevia = encodedSolution.GetSolution;

            var routes = new List<Route>() { routeOne, routeTwo };
            encodedSolution = Encoder.UpdateEncodedSolution(encodedSolution, routes);

            var solution = encodedSolution.GetSolution;

            Assert.AreEqual(2, solution.VehicleFleet.Vehicles[0].Route.RouteLenght());
            Assert.AreEqual(2, solution.VehicleFleet.Vehicles[1].Route.RouteLenght());

            Assert.AreEqual("A1", solution.VehicleFleet.Vehicles[0].Route.GetDestinations[0].Description);
            Assert.AreEqual("B2", solution.VehicleFleet.Vehicles[0].Route.GetDestinations[1].Description);

            Assert.AreEqual("B1", solution.VehicleFleet.Vehicles[1].Route.GetDestinations[0].Description);
            Assert.AreEqual("A2", solution.VehicleFleet.Vehicles[1].Route.GetDestinations[1].Description);
        }
    }
}
