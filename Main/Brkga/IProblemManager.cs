using System;
using Main.Helpers;

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

        int MaxIterations { get; set; }
    }

    public class ProblemManager : IProblemManager
    {
        public ProblemManager(IPopulationGenerator populationGenerator, bool logPopulation = false)
        {
            PopulationGenerator = populationGenerator;
            LogPopulation = logPopulation;
            MaxIterations = 100;
        }

        public bool StoppingRuleFulfilled { get { return PopulationGenerator.Generation >= MaxIterations; } }

        public Population Population { get; set; }

        public IPopulationGenerator PopulationGenerator { get; set; }

        public void InitializePopulation()
        {
            Population = PopulationGenerator.Generate();
            if (LogPopulation)
            {
                Logger.ClearLog();
                Logger.WriteOnFile(string.Format("Generation: {0}{1}{2}", PopulationGenerator.Generation, Environment.NewLine, Population));
            }
        }

        public void EvolvePopulation()
        {
            Population = PopulationGenerator.Evolve(Population);
            if (LogPopulation)
                Logger.WriteOnFile(string.Format("Generation: {0}{1}{2}", PopulationGenerator.Generation, Environment.NewLine, Population));
        }

        public bool LogPopulation { get; set; }

        public int MaxIterations { get; set; }
    }
}
