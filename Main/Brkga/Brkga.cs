using Main.BrkgaTop;
using Main.Entities;
using Main.Repositories;

namespace Main.Brkga
{
    // BIASED RANDOM-KEY GENETIC ALGORITHMS
    public class Brkga
    {
        public Brkga(IProblemManager problemManager)
        {
            ProblemManager = problemManager;
            SolutionRepository = new SolutionRepository(TopEntitiesManager.GetContext());
        }
        
        public IProblemManager ProblemManager { get; set; }

        public SolutionRepository SolutionRepository { get; set; }

        public EncodedSolution EncodedSolution { get; set; }

        public void Start()
        {
            ProblemManager.InitializePopulation();

            while (!ProblemManager.StoppingRuleFulfilled)
                ProblemManager.EvolvePopulation();

            ProblemManager.Population.MarkSolutionsAsFinals();

            EncodedSolution = ProblemManager.Population.GetMostProfitableSolution();
            SolutionRepository.SaveSolution(EncodedSolution.GetSolution);
        }
    }
}
