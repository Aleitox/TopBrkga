namespace Main.Brkga
{
    // BIASED RANDOM-KEY GENETIC ALGORITHMS
    public class Brkaga
    {
        public Brkaga(IPopulation population, IInitialKeyGenerator initialKeyGenerator, IProblemManager problemManager, IGenerationEvolver generationEvolver)
        {
            Population = population;
            InitialKeyGenerator = initialKeyGenerator;
            ProblemManager = problemManager;
            GenerationEvolver = GenerationEvolver;
        }

        public IPopulation Population { get; set; }

        public IInitialKeyGenerator InitialKeyGenerator { get; set; }

        public IProblemManager ProblemManager { get; set; }

        public IGenerationEvolver GenerationEvolver { get; set; }

        public void Start()
        {
            Population = InitialKeyGenerator.GeneratePopulation();
            ProblemManager.Decode(Population);
            while (!ProblemManager.StoppingRuleFulfilled)
            {
                GenerationEvolver.Evolve(Population);
                ProblemManager.Decode(Population);
            }
        }
    }
}
