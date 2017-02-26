﻿using System.Collections.Generic;
using Main.Brkga;
using Main.BrkgaTop.Decoders;
using Main.Entities;
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
            var problemManager = new ProblemManager(populationGenerator, true, config.MinIerations, config.MinHistoricalChanges);
            var brkga = new Brkga.Brkga(problemManager);
            return brkga;
        }

        public static BrkgaConfiguration GetBasicConfig()
        {
            var config = new BrkgaConfiguration()
            {
                Description = "Basic:MI.400;MHC.50;PZ.100;EP.0,3;MP.0,1;EGC.70",
                MinIerations = 400,
                MinHistoricalChanges = 50,
                PopulationSize = 100,
                ElitePercentage = 0.3m,
                MutantPercentage = 0.1m,
                EliteGenChance = 70
            };
            return config;
        }

        public static BrkgaConfiguration GetMoreIterationsConfig()
        {
            var config = new BrkgaConfiguration()
            {
                Description = "MoreIterations:MI.1200;MHC.100;PZ.100;EP.0,3;MP.0,3;EGC.70",
                MinIerations = 1200,
                MinHistoricalChanges = 100,
                PopulationSize = 100,
                ElitePercentage = 0.3m,
                MutantPercentage = 0.3m,
                EliteGenChance = 70
            };
            return config;
        }

        public static BrkgaConfiguration GetUnBiasConfig()
        {
            var config = new BrkgaConfiguration()
            {
                Description = "UnBias:MI.400;MHC.50;PZ.100;EP.0,4;MP.0,2;EGC.50",
                MinIerations = 400,
                MinHistoricalChanges = 50,
                PopulationSize = 100,
                ElitePercentage = 0.4m,
                MutantPercentage = 0.2m,
                EliteGenChance = 50
            };
            return config;
        }

        public static BrkgaConfiguration GetFewIterationsConfig()
        {
            var config = new BrkgaConfiguration()
            {
                Description = "FewIterations:MI.100;MHC.25;PZ.100;EP.0,3;MP.0,1;EGC.70",
                MinIerations = 100,
                MinHistoricalChanges = 25,
                PopulationSize = 100,
                ElitePercentage = 0.3m,
                MutantPercentage = 0.1m,
                EliteGenChance = 70
            };
            return config;
        }
    }
}
