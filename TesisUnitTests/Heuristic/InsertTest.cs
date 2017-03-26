using System.Collections.Generic;
using System.Linq.Expressions;
using Main.Brkga;
using Main.BrkgaTop;
using Main.BrkgaTop.Decoders;
using Main.BrkgaTop.Encoders;
using Main.Entities;
using Main.Factory;
using Main.GuidedLocalSearchHeuristics;
using Main.Model;
using Main.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TesisUnitTests.Helper;

namespace TesisUnitTests.Heuristic
{
    [TestClass]
    public class InsertTest
    {
        [TestMethod]
        public void Test_Nothing()
        {
            var instanceRepository = new InstanceRepository(TopEntitiesManager.GetContext());

            var instance = instanceRepository.GetById(Provider.GetBigInstanceId());
            var brkga = BrkgaFactory.Get(instance, BrkgaFactory.GetBasicConfig());
            brkga.ProblemManager.InitializePopulation();
            
            var encodedSolution = brkga.ProblemManager.Population.GetMostProfitableSolution();

            var sol1 = encodedSolution.GetSolution;
            var profitBefore = encodedSolution.GetSolution.GetCurrentProfit;

            var heuristic = new InsertHeuristic();
            heuristic.ApplyHeuristic(ref encodedSolution);
            
            var sol2 = encodedSolution.GetSolution;
            var profitAfter = encodedSolution.GetSolution.GetCurrentProfit;

            var heuristicSwap = new SwapHeuristic();
            heuristicSwap.ApplyHeuristic(ref encodedSolution);
            
            var sol3 = encodedSolution.GetSolution;
            var profitFinal = encodedSolution.GetSolution.GetCurrentProfit;


            heuristic.ApplyHeuristic(ref encodedSolution);

            var sol4 = encodedSolution.GetSolution;
            var profitVeryFinal = encodedSolution.GetSolution.GetCurrentProfit;
        }

        [TestMethod]
        public void Test_basic_Insert()
        {
            var profits = new List<int>() { 0, 3, 3, 0 };
            var coordinates = new List<Coordinate>()
            {
                new Coordinate(0, 0),
                new Coordinate(4, 3),
                new Coordinate(8, 3),
                new Coordinate(8, 0)
            };
            var descriptions = new List<string>()
            {
                "Start",
                "A1",
                "A2",
                "End"
            };

            var problemProvider = ProblemProviderFactory.CreateProblemProvider(profits, coordinates, descriptions, 1, 100);
            var problemDecoder = new GreedyVehicleDecoder(problemProvider);

            var randomVector = PopulationGenerator.GenerateRandomVector(profits.Count, 0, 2);
            var encodedSolution = new EncodedSolution(problemDecoder, randomVector);

            var routeOne = new Route(problemProvider.GetStartDestination(), problemProvider.GetEndingDestination());
            routeOne.AddDestination(problemProvider.GetDestinationByDescription("A1"));

            var routes = new List<Route>() { routeOne };
            encodedSolution = Encoder.UpdateEncodedSolution(encodedSolution, routes);

            var solution = encodedSolution.GetSolution;

            Assert.AreEqual(1, solution.VehicleFleet.Vehicles[0].Route.RouteLenght());
            Assert.AreEqual("A1", solution.VehicleFleet.Vehicles[0].Route.GetDestinations[0].Description);

            var insertHeuristic = new InsertHeuristic();
            insertHeuristic.ApplyHeuristic(ref encodedSolution);

            solution = encodedSolution.GetSolution;

            Assert.AreEqual(2, solution.VehicleFleet.Vehicles[0].Route.RouteLenght());
            Assert.AreEqual("A1", solution.VehicleFleet.Vehicles[0].Route.GetDestinations[0].Description);
            Assert.AreEqual("A2", solution.VehicleFleet.Vehicles[0].Route.GetDestinations[1].Description);
        }

        [TestMethod]
        public void Test_basic_Insert_2()
        {
            var profits = new List<int>() { 0, 3, 3, 30, 0 };
            var coordinates = new List<Coordinate>()
            {
                new Coordinate(0, 0),
                new Coordinate(4, 3),
                new Coordinate(8, 3),
                new Coordinate(1000, 1000),
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

            var problemProvider = ProblemProviderFactory.CreateProblemProvider(profits, coordinates, descriptions, 1, 100);
            var problemDecoder = new GreedyVehicleDecoder(problemProvider);

            var randomVector = PopulationGenerator.GenerateRandomVector(profits.Count, 0, 2);
            var encodedSolution = new EncodedSolution(problemDecoder, randomVector);

            var routeOne = new Route(problemProvider.GetStartDestination(), problemProvider.GetEndingDestination());
            routeOne.AddDestination(problemProvider.GetDestinationByDescription("A1"));

            var routes = new List<Route>() { routeOne };
            encodedSolution = Encoder.UpdateEncodedSolution(encodedSolution, routes);

            var solution = encodedSolution.GetSolution;

            Assert.AreEqual(1, solution.VehicleFleet.Vehicles[0].Route.RouteLenght());
            Assert.AreEqual("A1", solution.VehicleFleet.Vehicles[0].Route.GetDestinations[0].Description);

            var insertHeuristic = new InsertHeuristic();
            insertHeuristic.ApplyHeuristic(ref encodedSolution);

            solution = encodedSolution.GetSolution;

            Assert.AreEqual(2, solution.VehicleFleet.Vehicles[0].Route.RouteLenght());
            Assert.AreEqual("A1", solution.VehicleFleet.Vehicles[0].Route.GetDestinations[0].Description);
            Assert.AreEqual("A2", solution.VehicleFleet.Vehicles[0].Route.GetDestinations[1].Description);
        }
    }


}
