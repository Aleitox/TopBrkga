using Main.Brkga;
using Main.BrkgaTop;
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
        public static List<Model.Solution> DecodersRun(List<Instance> instances, int fase1, int fase2, int sampleSize)
        {
            var generatedSolutions = new List<Model.Solution>();
            var solutionRepository = new SolutionRepository(TopEntitiesManager.GetContext());

            var solutionName1 = "Decoder Simple Alone";
            var solutionName2 = "Decoder Greedy Alone";

            var problemProviders1 = new List<ProblemResourceProvider>();
            var problemProviders2 = new List<ProblemResourceProvider>();

            var decodersSimple = new List<IProblemDecoder>();
            var decodersGreedy = new List<IProblemDecoder>();

            foreach (var instance in instances)
                problemProviders1.Add(ProblemProviderFactory.CreateProblemProvider(instance, solutionName1));
            foreach (var instance in instances)
                problemProviders2.Add(ProblemProviderFactory.CreateProblemProvider(instance, solutionName2));

            foreach (var problemProvider in problemProviders1)
                decodersSimple.Add(new FirstSimpleDecoder(problemProvider));
            foreach (var problemProvider in problemProviders2)
                decodersGreedy.Add(new GreedyVehicleDecoder(problemProvider));

            for (var index = 0; index < decodersSimple.Count; index++)
            {
                var amountOfDestinations = decodersSimple[index].Provider.GetAmountOfDestinations();
                var randomKeysList = CreateRandomKeyVector(amountOfDestinations, sampleSize);

                Model.Solution s;
                foreach (var randomKeys in randomKeysList)
                {
                    s = decodersSimple[index].Decode(randomKeys);
                    s.Fase = fase1;
                    generatedSolutions.Add(s);
                    solutionRepository.SaveSolution(s);
                }
                foreach (var randomKeys in randomKeysList)
                {
                    s = decodersGreedy[index].Decode(randomKeys);
                    s.Fase = fase2;
                    generatedSolutions.Add(s);
                    solutionRepository.SaveSolution(s);
                }
            }
            return generatedSolutions;
        }


        public static List<EncodedSolution> GetSomeSolutionsOneDecoder(Instance instance, int fase1, int sampleSize, string deco)
        {
            var generatedSolutions = new List<EncodedSolution>();
            var solutionRepository = new SolutionRepository(TopEntitiesManager.GetContext());

            var solutionName1 = "Decoder Simple Alone";
            var problemProviders1 = new List<ProblemResourceProvider>();
            var decodersSimple = new List<IProblemDecoder>();
            
            problemProviders1.Add(ProblemProviderFactory.CreateProblemProvider(instance, solutionName1));

            if (deco == "Simple")
            {
                foreach (var problemProvider in problemProviders1)
                    decodersSimple.Add(new FirstSimpleDecoder(problemProvider));
            }
            else
            {
                foreach (var problemProvider in problemProviders1)
                    decodersSimple.Add(new GreedyVehicleDecoder(problemProvider));
            }

            var id = 1;

            for (var index = 0; index < decodersSimple.Count; index++)
            {
                var amountOfDestinations = decodersSimple[index].Provider.GetAmountOfDestinations();
                var randomKeysList = CreateRandomKeyVector(amountOfDestinations, sampleSize);
                //var otherRandomKeys = new List<RandomKey>();
                //var firstOrdered = randomKeysList.First().OrderBy(x => x.Key).ToList();
                //for (var j = 0; j < firstOrdered.Count(); j++)
                //{
                //    if (j < firstOrdered.Count() - 2)
                //    {
                //        var randomKey = new RandomKey(firstOrdered[j].Key, firstOrdered[j].PositionIndex);
                //        otherRandomKeys.Add(randomKey);
                //    }
                //    else
                //    {
                //        if (j == firstOrdered.Count() - 2)
                //        {
                //            var randomKey = new RandomKey(firstOrdered[j+1].Key, firstOrdered[j].PositionIndex);
                //            otherRandomKeys.Add(randomKey);
                //        }
                //        else if (j == firstOrdered.Count() - 1)
                //        {
                //            var randomKey = new RandomKey(firstOrdered[j-1].Key, firstOrdered[j].PositionIndex);
                //            otherRandomKeys.Add(randomKey);
                //        }
                //    }
                //}
                //randomKeysList.Add(otherRandomKeys);

                EncodedSolution simpleEncodedSolution;
                Model.Solution simpleSolution;

                foreach (var randomKeys in randomKeysList)
                {
                    simpleEncodedSolution = new EncodedSolution(decodersSimple[index], randomKeys);
                    simpleSolution = simpleEncodedSolution.GetSolution;
                    simpleSolution.Fase = fase1;
                    simpleSolution.Id = id;
                    id++;
                    generatedSolutions.Add(simpleEncodedSolution);
                }
            }
            return generatedSolutions;
        }

        public static List<Model.Solution> GetSomeSolutions(List<Instance> instances, int fase1, int fase2, int sampleSize)
        {
            var generatedSolutions = new List<Model.Solution>();
            var solutionRepository = new SolutionRepository(TopEntitiesManager.GetContext());

            var solutionName1 = "Decoder Simple Alone";
            var solutionName2 = "Decoder Greedy Alone";

            var problemProviders1 = new List<ProblemResourceProvider>();
            var problemProviders2 = new List<ProblemResourceProvider>();

            var decodersSimple = new List<IProblemDecoder>();
            var decodersGreedy = new List<IProblemDecoder>();

            foreach (var instance in instances)
                problemProviders1.Add(ProblemProviderFactory.CreateProblemProvider(instance, solutionName1));
            foreach (var instance in instances)
                problemProviders2.Add(ProblemProviderFactory.CreateProblemProvider(instance, solutionName2));

            foreach (var problemProvider in problemProviders1)
                decodersSimple.Add(new FirstSimpleDecoder(problemProvider));
            foreach (var problemProvider in problemProviders2)
                decodersGreedy.Add(new GreedyVehicleDecoder(problemProvider));

            for (var index = 0; index < decodersSimple.Count; index++)
            {
                var amountOfDestinations = decodersSimple[index].Provider.GetAmountOfDestinations();
                var randomKeysList = CreateRandomKeyVector(amountOfDestinations, sampleSize);

                EncodedSolution simpleEncodedSolution;
                Model.Solution simpleSolution;
                EncodedSolution greedyEncodedSolution;
                Model.Solution greedySolution;

                foreach (var randomKeys in randomKeysList)
                {
                    simpleEncodedSolution = new EncodedSolution(decodersSimple[index], randomKeys);
                    simpleSolution = simpleEncodedSolution.GetSolution;
                    simpleSolution.Fase = fase1;
                    generatedSolutions.Add(simpleSolution);
                    //solutionRepository.SaveSolution(s);


                    var simpleOldHash = simpleEncodedSolution.OldHash();
                    var simpleStrongHash = simpleEncodedSolution.StrongHash();
                }
                foreach (var randomKeys in randomKeysList)
                {
                    greedyEncodedSolution = new EncodedSolution(decodersGreedy[index], randomKeys);
                    greedySolution = greedyEncodedSolution.GetSolution;
                    greedySolution.Fase = fase2;
                    generatedSolutions.Add(greedySolution);
                    //solutionRepository.SaveSolution(s);

                    var greedyOldHash = greedyEncodedSolution.OldHash();
                    var greedyStrongHash = greedyEncodedSolution.StrongHash();
                }
            }
            return generatedSolutions;
        }


        public static List<Model.Solution> SimpleDecoderRun(List<Instance> instances, int fase, int sampleSize)
        {
            var generatedSolutions = new List<Model.Solution>();
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
                var randomKeysList = CreateRandomKeyVector(decoder.Provider.GetAmountOfDestinations(), sampleSize);
                Model.Solution s;
                foreach (var randomKeys in randomKeysList)
                {
                    s = decoder.Decode(randomKeys);
                    s.Fase = fase;
                    generatedSolutions.Add(s);
                    solutionRepository.SaveSolution(s);
                }
            }
            return generatedSolutions;
        }

        public static List<List<RandomKey>> CreateRandomKeyVector(int amountOfDestinations, int amount)
        {
            var random = new Random();
            var randomKeysList = new List<List<RandomKey>>();
            for (var index = 0; index < amount; index++)
            {
                var list = PopulationGenerator.GenerateRandomVector(amountOfDestinations, random.Next(10000, 100000), 2);
                randomKeysList.Add(list);
            }

            return randomKeysList;
        }

        public static List<Model.Solution> GreedyDecoderRun(List<Instance> instances, int fase, int sampleSize)
        {
            var generatedSolutions = new List<Model.Solution>();
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
                var randomKeysList = CreateRandomKeyVector(decoder.Provider.GetAmountOfDestinations(), sampleSize);
                Model.Solution s;
                foreach (var randomKeys in randomKeysList)
                {
                    s = decoder.Decode(randomKeys);
                    s.Fase = fase;
                    generatedSolutions.Add(s);
                    solutionRepository.SaveSolution(s);
                }
            }
            return generatedSolutions;
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
                Heuristics = new List<ILocalSearchHeuristic>() { new SwapHeuristic(), new TwoZeroPtSwap(), new InsertHeuristic(), ReplaceHeuristic.GetNormal(), ReplaceHeuristic.GetSuper() },
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
                Heuristics = new List<ILocalSearchHeuristic>() { new SwapHeuristic(), new TwoZeroPtSwap(), new InsertHeuristic(), ReplaceHeuristic.GetNormal(), ReplaceHeuristic.GetSuper() },
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
                Heuristics = new List<ILocalSearchHeuristic>() { new SwapHeuristic(), new TwoZeroPtSwap(), new InsertHeuristic(), new SwapHeuristic(), new TwoZeroPtSwap(), new InsertHeuristic(), ReplaceHeuristic.GetNormal(), ReplaceHeuristic.GetSuper() },
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
                Heuristics = new List<ILocalSearchHeuristic>() { new SwapHeuristic(), new InsertHeuristic(), new TwoZeroPtSwap(), ReplaceHeuristic.GetNormal(), new SwapHeuristic(), new TwoZeroPtSwap(), ReplaceHeuristic.GetSuper() },
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
                Heuristics = new List<ILocalSearchHeuristic>() { new SwapHeuristic(), new InsertHeuristic(), new TwoZeroPtSwap(), ReplaceHeuristic.GetNormal(), new SwapHeuristic(), new TwoZeroPtSwap(), ReplaceHeuristic.GetSuper() },
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
                Heuristics = new List<ILocalSearchHeuristic>() { new InsertHeuristic(), ReplaceHeuristic.GetNormal(), ReplaceHeuristic.GetSuper(), new TwoZeroPtSwap(), new SwapHeuristic() },
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
