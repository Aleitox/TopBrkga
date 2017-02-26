using System;
using Main.Entities;
using Main.Factory;
using Main.GuidedLocalSearchHeuristics;
using Main.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TesisUnitTests.Helper;

namespace TesisUnitTests.Heuristic
{
    [TestClass]
    public class InsertTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var instanceRepository = new InstanceRepository(TopEntitiesManager.GetContext());

            var instance = instanceRepository.GetById(Provider.GetBigInstanceId());
            var brkga = BrkgaFactory.Get(instance, BrkgaFactory.GetBasicConfig());
            brkga.ProblemManager.InitializePopulation();
            var solution = brkga.ProblemManager.Population.GetMostProfitableSolution().GetSolution;

            var profitBefore = solution.GetCurrentProfit;
            var heuristic = new LocalSearchHeuristic(solution);
            heuristic.ApplyInsert();
            var profitAfter = heuristic.Solution.GetCurrentProfit;

        }
    }
}
