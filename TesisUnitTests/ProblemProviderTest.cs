using System;
using System.Linq;
using Main.Brkga;
using Main.BrkgaTop;
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
            var intance = instanceRepository.GetBy("Tsiligirides", "Set_21_234", "p2.2.a.txt");

            var problemResourceProvider = ProblemProviderFactory.CreateProblemProvider(intance);
            var problemDecoder = new ProblemDecoder(problemResourceProvider);
            var populationGenerator = new PopulationGenerator(problemDecoder, problemResourceProvider.GetAmountOfNonProfitDestinations());
            var problemManager = new ProblemManager(populationGenerator, true) {MaxIterations = 1};
            var brkga = new Brkga(problemManager);

            brkga.Start();
        }
    }
}
