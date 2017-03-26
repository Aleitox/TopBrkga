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

        int MinHistoricalChanges { get; set; }

        List<EncodedSolution> HistoricalEncodedSolutions { get; set; }

        List<ILocalSearchHeuristic> Heuristics { get; set; }
    }

    public class ProblemManager : IProblemManager
    {
        public ProblemManager(IPopulationGenerator populationGenerator, bool logPopulation = false, int minIterations = 100, int minHistoricalChanges = 50)
        {
            PopulationGenerator = populationGenerator;
            LogPopulation = logPopulation;
            MinIterations = minIterations;
            HistoricalEncodedSolutions = new List<EncodedSolution>();
            MinHistoricalChanges = minHistoricalChanges;
            Heuristics = new List<ILocalSearchHeuristic>();
        }

        public bool StoppingRuleFulfilled 
        {
            get
            {
                return PopulationGenerator.Generation >= MinIterations || NoChanges();
            } 
        }

        private bool NoChanges()
        {
            if (HistoricalEncodedSolutions.Count <= MinIterations)
                return false;

            var currentProfit = HistoricalEncodedSolutions.Last().GetSolution.GetCurrentProfit;
            for (int index = HistoricalEncodedSolutions.Count - 2; index > HistoricalEncodedSolutions.Count - (2 + MinHistoricalChanges); index--)
            {
                if (currentProfit != HistoricalEncodedSolutions[index].GetSolution.GetCurrentProfit)
                    return false;
            }
            return true;
        }

        public Population Population { get; set; }

        public IPopulationGenerator PopulationGenerator { get; set; }

        public int MinHistoricalChanges { get; set; }

        public List<EncodedSolution> HistoricalEncodedSolutions { get; set; }

        public List<ILocalSearchHeuristic> Heuristics { get; set; }

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
            HistoricalEncodedSolutions.Add(Population.GetOrderByMostProfitable().First());
        }

        public void ApplyLocalHeuristics()
        {
            var bestSolutions = Population.GetOrderByMostProfitable().Take(1).ToList();
            for (int index = 0; index < bestSolutions.Count; index++)
            {
                var topSolution = bestSolutions[index];
                foreach (var heuristic in Heuristics)
                    heuristic.ApplyHeuristic(ref topSolution);
            }
        }

        public void EvolvePopulation()
        {
            Population = PopulationGenerator.Evolve(Population);

            ApplyLocalHeuristics();

            if (LogPopulation)
                Logger.WriteOnFile(string.Format("Generation: {0}{1}{2}", PopulationGenerator.Generation, Environment.NewLine, Population));
            HistoricalEncodedSolutions.Add(Population.GetOrderByMostProfitable().First());
        }

        public bool LogPopulation { get; set; }

        public int MinIterations { get; set; }
    }
}
