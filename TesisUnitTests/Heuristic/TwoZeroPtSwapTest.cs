using Main.Brkga;
using Main.BrkgaTop;
using Main.BrkgaTop.Decoders;
using Main.BrkgaTop.Encoders;
using Main.Factory;
using Main.GuidedLocalSearchHeuristics;
using Main.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace TesisUnitTests.Heuristic
{
    [TestClass]
    public class TwoZeroPtSwapTest
    {
        [TestMethod]
        public void Test_basic_Replace()
        {
            var profits = new List<int>() { 0, 3, 4, 0 };
            var coordinates = new List<Coordinate>()
            {
                new Coordinate(0, 0),
                new Coordinate(8, 8),
                new Coordinate(0, 8),
                new Coordinate(8, 0)
            };
            var descriptions = new List<string>()
            {
                "Start",
                "A1",
                "A2",
                "End"
            };

            var problemProvider = ProblemProviderFactory.CreateProblemProvider(profits, coordinates, descriptions, 1, 500);
            var problemDecoder = new GreedyVehicleDecoder(problemProvider);

            var randomVector = PopulationGenerator.GenerateRandomVector(profits.Count, 0, 2);
            var encodedSolution = new EncodedSolution(problemDecoder, randomVector);

            var routeOne = new Route(problemProvider.GetStartDestination(), problemProvider.GetEndingDestination());
            routeOne.AddDestination(problemProvider.GetDestinationByDescription("A1"));
            routeOne.AddDestination(problemProvider.GetDestinationByDescription("A2"));

            var routes = new List<Route>() { routeOne };
            encodedSolution = Encoder.UpdateEncodedSolution(encodedSolution, routes);

            var solution = encodedSolution.GetSolution;

            Assert.AreEqual(2, solution.VehicleFleet.Vehicles[0].Route.RouteLenght());
            Assert.AreEqual("A1", solution.VehicleFleet.Vehicles[0].Route.GetDestinations[0].Description);
            Assert.AreEqual("A2", solution.VehicleFleet.Vehicles[0].Route.GetDestinations[1].Description);

            var twoZeroPtSwapHeuristic = new TwoZeroPtSwap();
            twoZeroPtSwapHeuristic.ApplyHeuristic(ref encodedSolution);

            solution = encodedSolution.GetSolution;

            Assert.AreEqual(2, solution.VehicleFleet.Vehicles[0].Route.RouteLenght());
            Assert.AreEqual("A2", solution.VehicleFleet.Vehicles[0].Route.GetDestinations[0].Description);
            Assert.AreEqual("A1", solution.VehicleFleet.Vehicles[0].Route.GetDestinations[1].Description);
        }

        [TestMethod]
        public void Test_basic_Replace_2()
        {
            var profits = new List<int>() { 0, 3, 3, 3, 0 };
            var coordinates = new List<Coordinate>()
            {
                new Coordinate(0, 0),
                new Coordinate(2, 0),
                new Coordinate(4, 0),
                new Coordinate(6, 0),
                new Coordinate(8, 0)
            };
            var descriptions = new List<string>()
            {
                "Start",
                "A1",
                "A2",
                "A3",
                "End"
            };

            var problemProvider = ProblemProviderFactory.CreateProblemProvider(profits, coordinates, descriptions, 1, 1000);
            var problemDecoder = new GreedyVehicleDecoder(problemProvider);

            var randomVector = PopulationGenerator.GenerateRandomVector(profits.Count, 0, 2);
            var encodedSolution = new EncodedSolution(problemDecoder, randomVector);

            var routeOne = new Route(problemProvider.GetStartDestination(), problemProvider.GetEndingDestination());
            routeOne.AddDestination(problemProvider.GetDestinationByDescription("A3"));
            routeOne.AddDestination(problemProvider.GetDestinationByDescription("A2"));
            routeOne.AddDestination(problemProvider.GetDestinationByDescription("A1"));

            var routes = new List<Route>() { routeOne };
            encodedSolution = Encoder.UpdateEncodedSolution(encodedSolution, routes);

            var solution = encodedSolution.GetSolution;

            Assert.AreEqual(3, solution.VehicleFleet.Vehicles[0].Route.RouteLenght());
            Assert.AreEqual("A3", solution.VehicleFleet.Vehicles[0].Route.GetDestinations[0].Description);
            Assert.AreEqual("A2", solution.VehicleFleet.Vehicles[0].Route.GetDestinations[1].Description);
            Assert.AreEqual("A1", solution.VehicleFleet.Vehicles[0].Route.GetDestinations[2].Description);

            var twoZeroPtSwapHeuristic = new TwoZeroPtSwap();
            twoZeroPtSwapHeuristic.ApplyHeuristic(ref encodedSolution);

            solution = encodedSolution.GetSolution;

            Assert.AreEqual(3, solution.VehicleFleet.Vehicles[0].Route.RouteLenght());
            Assert.AreEqual("A1", solution.VehicleFleet.Vehicles[0].Route.GetDestinations[0].Description);
            Assert.AreEqual("A2", solution.VehicleFleet.Vehicles[0].Route.GetDestinations[1].Description);
            Assert.AreEqual("A3", solution.VehicleFleet.Vehicles[0].Route.GetDestinations[2].Description);
        }
    }
}
