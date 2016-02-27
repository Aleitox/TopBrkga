namespace Main.Brkga
{
    // BIASED RANDOM-KEY GENETIC ALGORITHMS
    public class Brkga
    {
        public Brkga(IPopulationGenerator populationGenerator, IProblemManager problemManager)
        {
            PopulationGenerator = populationGenerator;
            ProblemManager = problemManager;
        }

        public IPopulationGenerator PopulationGenerator { get; set; }

        public IProblemManager ProblemManager { get; set; }

        public void Start()
        {
            ProblemManager.Population = PopulationGenerator.Generate();
            ProblemManager.IterationNumber = 0;

            while (!ProblemManager.StoppingRuleFulfilled)
            {
                PopulationGenerator.Evolve(ProblemManager.Population);
                ProblemManager.IterationNumber++;
            }
        }
    }
}
