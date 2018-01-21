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

        public static List<BrkgaConfiguration> Get6Configs()
        {
            return new List<BrkgaConfiguration>() { GetConfigAOne(), GetConfigATwo(), GetConfigAThree(), GetConfigAFour(), GetConfigAFive(), GetConfigASix() };
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

        public static BrkgaConfiguration GetConfigTwo()
        {
            var config = new BrkgaConfiguration()
            {
                MinIterations = 250,
                MinNoChanges = 10,
                PopulationSize = 100,
                ElitePercentage = 0.3m,
                MutantPercentage = 0.1m,
                EliteGenChance = 70,
                Heuristics = new List<ILocalSearchHeuristic>(),
                ApplyHeuristicsToTop = 1,
                DecoderType = DecoderEnum.Greedy,
                Fase = 5
            };
            config.SetDescription();
            return config;
        }

        public static BrkgaConfiguration GetConfigAOne()
        {
            var config = new BrkgaConfiguration()
            {
                MinIterations = 150,
                MinNoChanges = 70,
                PopulationSize = 200,
                ElitePercentage = 0.3m,
                MutantPercentage = 0.1m,
                EliteGenChance = 70,
                Heuristics = new List<ILocalSearchHeuristic>(),
                ApplyHeuristicsToTop = 0,
                DecoderType = DecoderEnum.Greedy,
                Fase = 6
            };
            config.SetDescription();
            return config;
        }

        public static BrkgaConfiguration GetConfigATwo()
        {
            var config = new BrkgaConfiguration()
            {
                MinIterations = 150,
                MinNoChanges = 70,
                PopulationSize = 200,
                ElitePercentage = 0.3m,
                MutantPercentage = 0.1m,
                EliteGenChance = 70,
                Heuristics = new List<ILocalSearchHeuristic>(),
                ApplyHeuristicsToTop = 0,
                DecoderType = DecoderEnum.Simple,
                Fase = 6
            };
            config.SetDescription();
            return config;
        }

        public static BrkgaConfiguration GetConfigAThree()
        {
            var config = new BrkgaConfiguration()
            {
                MinIterations = 150,
                MinNoChanges = 30,
                PopulationSize = 200,
                ElitePercentage = 0.25m,
                MutantPercentage = 0.05m,
                EliteGenChance = 60,
                Heuristics = new List<ILocalSearchHeuristic>(),
                ApplyHeuristicsToTop = 0,
                DecoderType = DecoderEnum.Greedy,
                Fase = 6
            };
            config.SetDescription();
            return config;
        }

        public static BrkgaConfiguration GetConfigAFour()
        {
            var config = new BrkgaConfiguration()
            {
                MinIterations = 250,
                MinNoChanges = 50,
                PopulationSize = 250,
                ElitePercentage = 0.15m,
                MutantPercentage = 0.05m,
                EliteGenChance = 50,
                Heuristics = new List<ILocalSearchHeuristic>(),
                ApplyHeuristicsToTop = 0,
                DecoderType = DecoderEnum.Greedy,
                Fase = 6
            };
            config.SetDescription();
            return config;
        }

        public static BrkgaConfiguration GetConfigAFive()
        {
            var config = new BrkgaConfiguration()
            {
                MinIterations = 250,
                MinNoChanges = 50,
                PopulationSize = 250,
                ElitePercentage = 0.15m,
                MutantPercentage = 0.05m,
                EliteGenChance = 50,
                Heuristics = new List<ILocalSearchHeuristic>(),
                ApplyHeuristicsToTop = 0,
                DecoderType = DecoderEnum.Greedy,
                Fase = 6
            };
            config.SetDescription();
            return config;
        }

        public static BrkgaConfiguration GetConfigASix()
        {
            var config = new BrkgaConfiguration()
            {
                MinIterations = 100,
                MinNoChanges = 100,
                PopulationSize = 500,
                ElitePercentage = 0.30m,
                MutantPercentage = 0.05m,
                EliteGenChance = 70,
                Heuristics = new List<ILocalSearchHeuristic>(),
                ApplyHeuristicsToTop = 0,
                DecoderType = DecoderEnum.Greedy,
                Fase = 6
            };
            config.SetDescription();
            return config;
        }
        
        public static List<BrkgaConfiguration> Get5Configs()
        {
            return new List<BrkgaConfiguration>() { GetConfigBThreeBis(), GetConfigBZero(), GetConfigBOne(), GetConfigBTwo(), GetConfigBThree(), GetConfigBFour() };
        }

        public static BrkgaConfiguration GetConfigBZero()
        {
            var config = new BrkgaConfiguration()
            {
                MinIterations = 200,
                MinNoChanges = 50,
                PopulationSize = 150,
                ElitePercentage = 0.3m,
                MutantPercentage = 0.1m,
                EliteGenChance = 70,
                Heuristics = new List<ILocalSearchHeuristic>() { new SwapHeuristic(), new TwoZeroPtSwap(), new InsertHeuristic(), new ReplaceHeuristic() },
                ApplyHeuristicsToTop = 2,
                DecoderType = DecoderEnum.Simple,
                Fase = 7
            };
            config.SetDescription();
            return config;
        }

        public static BrkgaConfiguration GetConfigBOne()
        {
            var config = new BrkgaConfiguration()
            {
                MinIterations = 200,
                MinNoChanges = 50,
                PopulationSize = 150,
                ElitePercentage = 0.3m,
                MutantPercentage = 0.1m,
                EliteGenChance = 70,
                Heuristics = new List<ILocalSearchHeuristic>() { new SwapHeuristic(), new TwoZeroPtSwap(), new InsertHeuristic(), new ReplaceHeuristic()},
                ApplyHeuristicsToTop = 2,
                DecoderType = DecoderEnum.Greedy,
                Fase = 7
            };
            config.SetDescription();
            return config;
        }

        public static BrkgaConfiguration GetConfigBTwo()
        {
            var config = new BrkgaConfiguration()
            {
                MinIterations = 200,
                MinNoChanges = 50,
                PopulationSize = 150,
                ElitePercentage = 0.3m,
                MutantPercentage = 0.1m,
                EliteGenChance = 70,
                Heuristics = new List<ILocalSearchHeuristic>() { new SwapHeuristic(), new TwoZeroPtSwap(), new InsertHeuristic(), new SwapHeuristic(), new TwoZeroPtSwap(), new InsertHeuristic(), new ReplaceHeuristic() },
                ApplyHeuristicsToTop = 2,
                DecoderType = DecoderEnum.Greedy,
                Fase = 7
            };
            config.SetDescription();
            return config;
        }

        public static BrkgaConfiguration GetConfigBThree()
        {
            var config = new BrkgaConfiguration()
            {
                MinIterations = 200,
                MinNoChanges = 50,
                PopulationSize = 150,
                ElitePercentage = 0.3m,
                MutantPercentage = 0.1m,
                EliteGenChance = 70,
                Heuristics = new List<ILocalSearchHeuristic>() { new SwapHeuristic(), new InsertHeuristic(), new TwoZeroPtSwap(), new ReplaceHeuristic(), new SwapHeuristic(), new TwoZeroPtSwap(), new ReplaceHeuristic() },
                ApplyHeuristicsToTop = 2,
                DecoderType = DecoderEnum.Greedy,
                Fase = 7
            };
            config.SetDescription();
            return config;
        }

        public static BrkgaConfiguration GetConfigBThreeBis()
        {
            var config = new BrkgaConfiguration()
            {
                MinIterations = 200,
                MinNoChanges = 50,
                PopulationSize = 150,
                ElitePercentage = 0.3m,
                MutantPercentage = 0.1m,
                EliteGenChance = 70,
                Heuristics = new List<ILocalSearchHeuristic>() { new SwapHeuristic(), new InsertHeuristic(), new TwoZeroPtSwap(), new ReplaceHeuristic(), new SwapHeuristic(), new TwoZeroPtSwap(), new ReplaceHeuristic() },
                ApplyHeuristicsToTop = 2,
                DecoderType = DecoderEnum.Simple,
                Fase = 7
            };
            config.SetDescription();
            return config;
        }

        public static BrkgaConfiguration GetConfigBFour()
        {
            var config = new BrkgaConfiguration()
            {
                MinIterations = 200,
                MinNoChanges = 50,
                PopulationSize = 150,
                ElitePercentage = 0.3m,
                MutantPercentage = 0.1m,
                EliteGenChance = 70,
                Heuristics = new List<ILocalSearchHeuristic>() { new InsertHeuristic(), new ReplaceHeuristic(), new TwoZeroPtSwap(), new SwapHeuristic() },
                ApplyHeuristicsToTop = 2,
                DecoderType = DecoderEnum.Greedy,
                Fase = 7
            };
            config.SetDescription();
            return config;
        }

        public static BrkgaConfiguration GetFinalConfig()
        {
            var config = new BrkgaConfiguration()
            {
                MinIterations = 200,
                MinNoChanges = 50,
                PopulationSize = 150,
                ElitePercentage = 0.3m,
                MutantPercentage = 0.1m,
                EliteGenChance = 70,
                Heuristics = new List<ILocalSearchHeuristic>() { new SwapHeuristic(), new InsertHeuristic(), new TwoZeroPtSwap(), ReplaceHeuristic.GetNormal(), new SwapHeuristic(), new TwoZeroPtSwap(), ReplaceHeuristic.GetSuper() },
                ApplyHeuristicsToTop = 2,
                DecoderType = DecoderEnum.Simple,
                Fase = 8
            };
            config.SetDescription();
            return config;
        }

        public static BrkgaConfiguration GetSimpleTestConfig()
        {
            var config = new BrkgaConfiguration()
            {
                MinIterations = 200,
                MinNoChanges = 50,
                PopulationSize = 150,
                ElitePercentage = 0.3m,
                MutantPercentage = 0.1m,
                EliteGenChance = 70,
                Heuristics = new List<ILocalSearchHeuristic>() { new SwapHeuristic(), new InsertHeuristic(), new TwoZeroPtSwap(), ReplaceHeuristic.GetNormal(), new SwapHeuristic(), new TwoZeroPtSwap(), ReplaceHeuristic.GetSuper() },
                ApplyHeuristicsToTop = 2,
                DecoderType = DecoderEnum.Simple,
                Fase = 9
            };
            config.SetDescription();
            return config;
        }
    }
}
