using Main.Brkga;
using Main.BrkgaTop.Decoders;
using Main.Entities;
using Main.GuidedLocalSearchHeuristics;
using Main.Model;
using Main.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Factory
{
    public class PaperConfigsFactory
    {
        public static void SimpleDecoderRun(List<Instance> instances, int fase, int sampleSize)
        {
            var solutionRepository = new SolutionRepository(TopEntitiesManager.GetContext());

            var solutionName = "Decoder Simple Alone";
            var problemProviders = new List<ProblemResourceProvider>();
            var decoders = new List<IProblemDecoder>();
            foreach (var instance in instances)
                problemProviders.Add(ProblemProviderFactory.CreateProblemProvider(instance, solutionName));
            foreach (var problemProvider in problemProviders)
                decoders.Add(new FirstSimpleDecoder(problemProvider));
            foreach (var decoder in decoders)
            {
                var randomKeysList = CreateRandomKeyVector(decoder, sampleSize);
                Model.Solution s;
                foreach (var randomKeys in randomKeysList)
                {
                    s = decoder.Decode(randomKeys);
                    s.Fase = fase;
                    solutionRepository.SaveSolution(s);
                }
            }
        }

        public static List<List<RandomKey>> CreateRandomKeyVector(IProblemDecoder decoder, int amount)
        {
            var random = new Random();
            var randomKeysList = new List<List<RandomKey>>();
            for (var index = 0; index < amount; index++)
            {
                var list = PopulationGenerator.GenerateRandomVector(decoder.Provider.GetAmountOfDestinations(), random.Next(1000), 2);
                randomKeysList.Add(list);
            }

            return randomKeysList;
        }

        public static void GreedyDecoderRun(List<Instance> instances, int fase, int sampleSize)
        {
            var solutionRepository = new SolutionRepository(TopEntitiesManager.GetContext());

            var solutionName = "Decoder Greedy Alone";
            var problemProviders = new List<ProblemResourceProvider>();
            var decoders = new List<IProblemDecoder>();
            foreach (var instance in instances)
                problemProviders.Add(ProblemProviderFactory.CreateProblemProvider(instance, solutionName));
            foreach (var problemProvider in problemProviders)
                decoders.Add(new GreedyVehicleDecoder(problemProvider));
            foreach (var decoder in decoders)
            {
                var randomKeysList = CreateRandomKeyVector(decoder, sampleSize);
                Model.Solution s;
                foreach (var randomKeys in randomKeysList)
                {
                    s = decoder.Decode(randomKeys);
                    s.Fase = fase;
                    solutionRepository.SaveSolution(s);
                }
            }
        }

        public static BrkgaConfiguration GetConfigOne()
        {
            var config = new BrkgaConfiguration()
            {
                MinIterations = 400,
                MinNoChanges = 10,
                PopulationSize = 100,
                ElitePercentage = 0.3m,
                MutantPercentage = 0.1m,
                EliteGenChance = 70,
                Heuristics = new List<ILocalSearchHeuristic>(),
                ApplyHeuristicsToTop = 1,
                DecoderType = DecoderEnum.Greedy,
                Fase = 4
            };
            config.SetDescription();
            return config;
        }
    }
}
