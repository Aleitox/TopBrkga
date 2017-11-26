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

        List<EncodedSolution> HistoricalEncodedSolutions { get; set; }

        List<ILocalSearchHeuristic> Heuristics { get; set; }
    }

    public class ProblemManager : IProblemManager
    {
        public ProblemManager(IPopulationGenerator populationGenerator, bool logPopulation = false, int minIterations = 100, int minNoChanges = 10)
        {
            PopulationGenerator = populationGenerator;
            Heuristics = new List<ILocalSearchHeuristic>();
            ApplyHeuristicsToTop = 0;
            LogPopulation = logPopulation;
            MinIterations = minIterations;
            HistoricalEncodedSolutions = new List<EncodedSolution>();
            MinNoChanges = minNoChanges;
            LastProfits = new Queue<double>();
        }

        public ProblemManager(IPopulationGenerator populationGenerator, List<ILocalSearchHeuristic> heuristics, int applyHeuristicsToTop, bool logPopulation = false, int minIterations = 100, int minNoChanges = 10)
        {
            PopulationGenerator = populationGenerator;
            Heuristics = heuristics;
            ApplyHeuristicsToTop = applyHeuristicsToTop;
            LogPopulation = logPopulation;
            MinIterations = minIterations;
            HistoricalEncodedSolutions = new List<EncodedSolution>();
            MinNoChanges = minNoChanges;
            LastProfits = new Queue<double>();
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

        public List<ILocalSearchHeuristic> Heuristics { get; set; }

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
            var bestSolutions = Population.GetOrderByMostProfitable().Where(es => !es.EnhancedByLocalHeuristics).Take(ApplyHeuristicsToTop).ToList();
            for (int index = 0; index < bestSolutions.Count; index++)
            {
                var topSolution = bestSolutions[index];
                foreach (var heuristic in Heuristics)
                    heuristic.ApplyHeuristic(ref topSolution);
                topSolution.EnhancedByLocalHeuristics = true;
            }
        }

        public void EvolvePopulation()
        {
            Population = PopulationGenerator.Evolve(Population);

            ApplyLocalHeuristics();

            if (LogPopulation)
                Logger.WriteOnFile(string.Format("Generation: {0}{1}{2}", PopulationGenerator.Generation, Environment.NewLine, Population));

            LastProfits.Enqueue(Population.GetOrderByMostProfitable().First().GetSolution.GetCurrentProfit);
            if (LastProfits.Count > MinNoChanges)
                LastProfits.Dequeue();
            
            HistoricalEncodedSolutions.Add(Population.GetOrderByMostProfitable().First());
        }        

        public bool LogPopulation { get; set; }

        public int MinIterations { get; set; }
    }
}
