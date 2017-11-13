using System.Collections.Generic;
using Main.Brkga;
using Main.BrkgaTop.Decoders;
using Main.Entities;
using Main.GuidedLocalSearchHeuristics;
using Main.Model;

namespace Main.Factory
{
    public class BrkgaFactory
    {
        public static Main.Brkga.Brkga Get(Instance instance, BrkgaConfiguration config)
        {
            var problemResourceProvider = ProblemProviderFactory.CreateProblemProvider(instance, config.Description);
            var problemDecoder = new GreedyVehicleDecoder(problemResourceProvider);
            var populationGenerator = new PopulationGenerator(problemDecoder, problemResourceProvider.GetAmountOfNonProfitDestinations(), config.PopulationSize, config.ElitePercentage, config.MutantPercentage, config.EliteGenChance);
            var problemManager = new ProblemManager(populationGenerator, config.Heuristics, config.ApplyHeuristicsToTop, false, config.MinIerations, config.MinNoChanges);
            var brkga = new Brkga.Brkga(problemManager);
            return brkga;
        }

        public static BrkgaConfiguration GetBasicConfig()
        {
            var config = new BrkgaConfiguration()
            {
                Description = "Basic:MI.400;MNC.10;PZ.100;EP.0,3;MP.0,1;EGC.70",
                MinIerations = 400,
                MinNoChanges = 10,
                PopulationSize = 100,
                ElitePercentage = 0.3m,
                MutantPercentage = 0.1m,
                EliteGenChance = 70,
                Heuristics = new List<ILocalSearchHeuristic>(),
                ApplyHeuristicsToTop = 1
            };
            return config;
        }

        public static BrkgaConfiguration GetMoreIterationsConfig()
        {
            var config = new BrkgaConfiguration()
            {
                Description = "MoreIterations:MI.1200;MNC.10;PZ.100;EP.0,3;MP.0,3;EGC.70",
                MinIerations = 1200,
                MinNoChanges = 10,
                PopulationSize = 100,
                ElitePercentage = 0.3m,
                MutantPercentage = 0.3m,
                EliteGenChance = 70,
                Heuristics = new List<ILocalSearchHeuristic>(),
                ApplyHeuristicsToTop = 1
            };
            return config;
        }

        public static BrkgaConfiguration GetUnBiasConfig()
        {
            var config = new BrkgaConfiguration()
            {
                Description = "UnBias:MI.400;MNC.10;PZ.100;EP.0,4;MP.0,2;EGC.50",
                MinIerations = 400,
                MinNoChanges = 10,
                PopulationSize = 100,
                ElitePercentage = 0.4m,
                MutantPercentage = 0.2m,
                EliteGenChance = 50,
                Heuristics = new List<ILocalSearchHeuristic>(),
                ApplyHeuristicsToTop = 1
            };
            return config;
        }

        public static BrkgaConfiguration GetFewIterationsConfig()
        {
            var config = new BrkgaConfiguration()
            {
                Description = "FewIterations:MI.100;MNC.10;PZ.100;EP.0,3;MP.0,1;EGC.70",
                MinIerations = 100,
                MinNoChanges = 10,
                PopulationSize = 100,
                ElitePercentage = 0.3m,
                MutantPercentage = 0.1m,
                EliteGenChance = 70,
                Heuristics = new List<ILocalSearchHeuristic>(),
                ApplyHeuristicsToTop = 1
            };
            return config;
        }

        // WARNING Esta es una de las finales, y no quiero pisar su valor por ahora
        public static BrkgaConfiguration GetBasicConfigWithEuristicsFixed()
        {
            var config = new BrkgaConfiguration()
            {
                Description = "Basic:MI.200;MNC.10;PZ.100;EP.0,3;MP.0,1;EGC.70;HEU.STSIR;TOP.2",
                MinIerations = 200,
                MinNoChanges = 10,
                PopulationSize = 100,
                ElitePercentage = 0.3m,
                MutantPercentage = 0.1m,
                EliteGenChance = 70,
                Heuristics = new List<ILocalSearchHeuristic>() { new SwapHeuristic(), new TwoZeroPtSwap(), new SwapHeuristic(), new InsertHeuristic(), new ReplaceHeuristic() },
                ApplyHeuristicsToTop = 2
            };
            return config;
        }

        public static BrkgaConfiguration GetBasicConfigWithEuristics1()
        {
            var config = new BrkgaConfiguration()
            {
                Description = "Basic:MI.200;MNC.10;PZ.100;EP.0,3;MP.0,1;EGC.70;HEU.ISIRT;TOP.2",
                MinIerations = 200,
                MinNoChanges = 10,
                PopulationSize = 100,
                ElitePercentage = 0.3m,
                MutantPercentage = 0.1m,
                EliteGenChance = 70,
                Heuristics = new List<ILocalSearchHeuristic>() { new InsertHeuristic(), new SwapHeuristic(), new InsertHeuristic(), new ReplaceHeuristic(), new TwoZeroPtSwap() },
                ApplyHeuristicsToTop = 2
            };
            return config;
        }
        public static BrkgaConfiguration GetBasicConfigWithEuristics2()
        {
            var config = new BrkgaConfiguration()
            {
                Description = "Basic:MI.200;MNC.10;PZ.100;EP.0,3;MP.0,1;EGC.70;HEU.ITSIR;TOP.2",
                MinIerations = 200,
                MinNoChanges = 10,
                PopulationSize = 100,
                ElitePercentage = 0.3m,
                MutantPercentage = 0.1m,
                EliteGenChance = 70,
                Heuristics = new List<ILocalSearchHeuristic>() { new InsertHeuristic(), new TwoZeroPtSwap(), new SwapHeuristic(), new InsertHeuristic(), new ReplaceHeuristic() },
                ApplyHeuristicsToTop = 2
            };
            return config;
        }

        public static BrkgaConfiguration GetBasicConfigWithEuristics3()
        {
            var config = new BrkgaConfiguration()
            {
                Description = "Basic:MI.200;MNC.10;PZ.100;EP.0,3;MP.0,1;EGC.70;HEU.STIR;TOP.2",
                MinIerations = 200,
                MinNoChanges = 10,
                PopulationSize = 100,
                ElitePercentage = 0.3m,
                MutantPercentage = 0.1m,
                EliteGenChance = 70,
                Heuristics = new List<ILocalSearchHeuristic>() { new SwapHeuristic(), new TwoZeroPtSwap(), new InsertHeuristic(), new ReplaceHeuristic() },
                ApplyHeuristicsToTop = 2
            };
            return config;
        }

        public static BrkgaConfiguration GetBasicConfigWithEuristics()
        {
            var config = new BrkgaConfiguration()
            {
                Description = "Basic:MI.200;MNC.10;PZ.100;EP.0,3;MP.0,1;EGC.70;HEU.STSIR;TOP.2;Version.2",
                MinIerations = 200,
                MinNoChanges = 10,
                PopulationSize = 100,
                ElitePercentage = 0.3m,
                MutantPercentage = 0.1m,
                EliteGenChance = 70,
                Heuristics = new List<ILocalSearchHeuristic>() { new SwapHeuristic(), new TwoZeroPtSwap(), new SwapHeuristic(), new InsertHeuristic(), new ReplaceHeuristic() },
                ApplyHeuristicsToTop = 2
            };
            return config;
        }
    }
}
