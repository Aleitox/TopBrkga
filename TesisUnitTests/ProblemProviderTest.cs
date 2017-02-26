using System;
using System.Collections.Generic;
using System.Linq;
using Main.Brkga;
using Main.BrkgaTop;
using Main.BrkgaTop.Decoders;
using Main.Entities;
using Main.Factory;
using Main.Model;
using Main.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TesisUnitTests
{
    [TestClass]
    public class ProblemProviderTest
    {
        [TestMethod]
        public void CreateProblemFromDbIntance()
        {
            var instanceRepository = new InstanceRepository(TopEntitiesManager.GetContext());
            //var instance = instanceRepository.GetBy("Tsiligirides", "Set_21_234", "p2.2.a.txt");
            var instance = instanceRepository.GetById(431);

            var problemResourceProvider = ProblemProviderFactory.CreateProblemProvider(instance, null);
            var problemDecoder = new GreedyVehicleDecoder(problemResourceProvider);
            var populationGenerator = new PopulationGenerator(problemDecoder,
                problemResourceProvider.GetAmountOfNonProfitDestinations());
            var problemManager = new ProblemManager(populationGenerator, true);
            var brkga = new Brkga(problemManager);

            brkga.Start();
        }

        [TestMethod]
        public void CreateProblemFromDbIntance3()
        {
            var instanceRepository = new InstanceRepository(TopEntitiesManager.GetContext());
            //var instance = instanceRepository.GetBy("Tsiligirides", "Set_21_234", "p2.2.a.txt");
            var instance = instanceRepository.GetById(522); // 407, 400, 776, 721, 532

            var problemResourceProvider = ProblemProviderFactory.CreateProblemProvider(instance, null);
            var problemDecoder = new GreedyVehicleDecoder(problemResourceProvider);
            var populationGenerator = new PopulationGenerator(problemDecoder, problemResourceProvider.GetAmountOfNonProfitDestinations());
            var problemManager = new ProblemManager(populationGenerator, true);
            var brkga = new Brkga(problemManager);

            brkga.Start();
        }

        [TestMethod]
        public void CreateProblemFromDbIntance2()
        {
            var instanceRepository = new InstanceRepository(TopEntitiesManager.GetContext());
            var instance = instanceRepository.GetById(431);

            var problemResourceProvider = ProblemProviderFactory.CreateProblemProvider(instance, null);
            var problemDecoder = new GreedyVehicleDecoder(problemResourceProvider);
            var populationGenerator = new PopulationGenerator(problemDecoder, problemResourceProvider.GetAmountOfNonProfitDestinations());

            var randomGenerator = new Random();
            var encodedSolutions = new List<EncodedSolution>();

            var encodedSolution = populationGenerator.GenerateEncodedSolution(randomGenerator, encodedSolutions);
            var solution = problemDecoder.Decode(encodedSolution);
            solution.InstanceId = instance.Id;
            AssertSolution(solution);

            var solutionRepository = new SolutionRepository(TopEntitiesManager.GetContext());
            solutionRepository.SaveSolution(solution);
        }

        public void AssertSolution(Main.Model.Solution solution)
        {
            Assert.IsTrue(solution.GetCurrentProfit > 0);
            foreach (var vehicle in solution.VehicleFleet.Vehicles)
            {
                Assert.IsTrue(vehicle.Route.GetProfit() > 0);
                var currentDestination = solution.Map.GetStartingDestination();
                decimal distance = 0;
                
                for (var order = 0; order < vehicle.Route.RouteLenght(); order++)
                {
                    var destination = vehicle.Route.GetDestinationAt(order);
                    distance += solution.Map.GetDistance(currentDestination, destination);
                    currentDestination = destination;
                }
                distance += solution.Map.GetDistance(currentDestination, solution.Map.GetEndingDestination());
                Assert.IsTrue(distance <= vehicle.MaxDistance);
            }
        }
    }
}
