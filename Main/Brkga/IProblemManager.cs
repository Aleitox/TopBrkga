using System;
using System.Collections.Generic;
using System.Linq;
using Main.BrkgaTop;
using Main.GuidedLocalSearchHeuristics;
using Main.Helpers;
using Main.Model;

namespace Main.Brkga
{
    public interface IProblemManager
    {
        bool StoppingRuleFulfilled { get; }

        Population Population { get; set; }

        IPopulationGenerator PopulationGenerator { get; set; }

        void InitializePopulation();

        void EvolvePopulation();

        bool LogPopulation { get; set; }

        int MinIterations { get; set; }

        int MinNoChanges { get; set; }

        int GenerationNumber { get; set; }

        List<EncodedSolution> HistoricalEncodedSolutions { get; set; }

        List<ILocalSearchHeuristic> HeuristicsShort { get; set; }
    }

    public class ProblemManager : IProblemManager
    {
        public ProblemManager(IPopulationGenerator populationGenerator, bool logPopulation = false, int minIterations = 100, int minNoChanges = 10)
        {
            PopulationGenerator = populationGenerator;
            HeuristicsShort = new List<ILocalSearchHeuristic>();
            ApplyHeuristicsToTop = 0;
            LogPopulation = logPopulation;
            MinIterations = minIterations;
            HistoricalEncodedSolutions = new List<EncodedSolution>();
            MinNoChanges = minNoChanges;
            LastProfits = new Queue<double>();
            GenerationNumber = 0;
        }

        public ProblemManager(IPopulationGenerator populationGenerator, List<ILocalSearchHeuristic> heuristics, List<ILocalSearchHeuristic> heuristicsLong, int applyHeuristicsToTop, bool logPopulation = false, int minIterations = 100, int minNoChanges = 10)
        {
            PopulationGenerator = populationGenerator;
            HeuristicsShort = heuristics;
            ApplyHeuristicsToTop = applyHeuristicsToTop;
            LogPopulation = logPopulation;
            MinIterations = minIterations;
            HistoricalEncodedSolutions = new List<EncodedSolution>();
            MinNoChanges = minNoChanges;
            LastProfits = new Queue<double>();
            HeuristicsLong = new List<ILocalSearchHeuristic>()
            {
                new SwapHeuristic(),
                new InsertHeuristic(),
                new SwapHeuristic(),
                new TwoZeroPtSwap(),
                ReplaceHeuristic.GetSuper(),
                new TwoZeroPtSwap(),
                new SwapHeuristic(),
                new TwoZeroPtSwap(),
                new InsertHeuristic(),
                ReplaceHeuristic.GetNormal(),
                new SwapHeuristic(),
                new InsertHeuristic(),
                new SwapHeuristic(),
                new TwoZeroPtSwap(),
                ReplaceHeuristic.GetSuper()
            };
        }

        public bool StoppingRuleFulfilled
        {
            get
            {
                return PopulationGenerator.Generation >= MinIterations && NoChanges();
            }
        }

        // TODO: Ver
        private bool NoChanges()
        {
            var currentProfit = HistoricalEncodedSolutions.Last().GetSolution.GetCurrentProfit;

            return LastProfits.All(p => p == currentProfit);
        }

        public Population Population { get; set; }

        public IPopulationGenerator PopulationGenerator { get; set; }

        public int MinNoChanges { get; set; }

        // TODO: Borrar si ya no se usa
        public List<EncodedSolution> HistoricalEncodedSolutions { get; set; }

        public Queue<double> LastProfits { get; set; }

        public List<ILocalSearchHeuristic> HeuristicsShort { get; set; }

        public List<ILocalSearchHeuristic> HeuristicsLong { get; set; }        

        public int ApplyHeuristicsToTop { get; set; }

        public void InitializePopulation()
        {
            Population = PopulationGenerator.Generate(PopulationGenerator.PopulationSize);
            foreach (var encodedProblem in Population.EncodedProblems)
            {
                encodedProblem.GetSolution.Generation = 0;
            }

            if (LogPopulation)
            {
                Logger.ClearLog();
                Logger.WriteOnFile(string.Format("Generation: {0}{1}{2}", PopulationGenerator.Generation, Environment.NewLine, Population));
            }
            Population.GetOrderByMostProfitable().First().GetSolution.BestInGeneration = true;

            LastProfits.Enqueue(Population.GetOrderByMostProfitable().First().GetSolution.GetCurrentProfit);
            HistoricalEncodedSolutions.Add(Population.GetOrderByMostProfitable().First());
        }

        public void ApplyLocalHeuristics()
        {
            var orderedSolutions = Population.GetOrderByMostProfitable();
            var bestSolutions = orderedSolutions.Where(es => !es.EnhancedByLocalHeuristics).Take(ApplyHeuristicsToTop).ToList();
            for (int index = 0; index < bestSolutions.Count; index++)
            {
                var solution = bestSolutions[index];
                LocalSearchHeuristicHelper.ApplyHeuristics(HeuristicsShort, ref solution);
                solution.EnhancedByLocalHeuristics = true;
            }

            if (GenerationNumber >= 10)
            {
                var solutions = orderedSolutions.Take(3).ToList();
                for (int index = 0; index < solutions.Count(); index++)
                {
                    var solution = solutions[index];
                    LocalSearchHeuristicHelper.ApplyHeuristics(HeuristicsLong, ref solution);
                    GenerationNumber = 1;
                }
            }               
        }

        public void EvolvePopulation()
        {
            Population = PopulationGenerator.Evolve(Population);

            GenerationNumber++;

            ApplyLocalHeuristics();

            //Delete posible duplicates generated by local search
            Population.EncodedProblems = Population.EncodedProblems.GroupBy(x => x.GetPseudoHash()).Select(y => y.First()).ToList();
            while (Population.EncodedProblems.Count() < PopulationGenerator.PopulationSize)
            {
                var mutant = PopulationGenerator.GenerateEncodedSolution(Population.EncodedProblems);
                Population.EncodedProblems.Add(mutant);
            }

            if (LogPopulation)
                Logger.WriteOnFile(string.Format("Generation: {0}{1}{2}", PopulationGenerator.Generation, Environment.NewLine, Population));

            LastProfits.Enqueue(Population.GetOrderByMostProfitable().First().GetSolution.GetCurrentProfit);
            if (LastProfits.Count > MinNoChanges)
                LastProfits.Dequeue();
            
            HistoricalEncodedSolutions.Add(Population.GetOrderByMostProfitable().First());
        }        

        public bool LogPopulation { get; set; }

        public int MinIterations { get; set; }

        public int GenerationNumber { get; set; }
    }
}
