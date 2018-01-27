using System.Collections.Generic;
using System.Diagnostics;
using Main.BrkgaTop;
using Main.Entities;
using Main.GuidedLocalSearchHeuristics;
using Main.Repositories;
using System.Linq;

namespace Main.Brkga
{
    // BIASED RANDOM-KEY GENETIC ALGORITHMS
    public class Brkga
    {
        public Brkga(IProblemManager problemManager)
        {
            ProblemManager = problemManager;
            SolutionRepository = new SolutionRepository(TopEntitiesManager.GetContext());
            Fase = 0;
        }
        
        public IProblemManager ProblemManager { get; set; }

        public SolutionRepository SolutionRepository { get; set; }

        public EncodedSolution EncodedSolution { get; set; }

        public int Fase { get; set; }

        public void Start()
        {
            var timer = new Stopwatch();

            timer.Start();

            ProblemManager.InitializePopulation();

            while (!ProblemManager.StoppingRuleFulfilled)
                ProblemManager.EvolvePopulation();

            ProblemManager.Population.MarkSolutionsAsFinals();

            timer.Stop();
            var timeElapsed = timer.ElapsedMilliseconds;

            var encodedSolution = ProblemManager.Population.GetMostProfitableSolution();

            encodedSolution.GetSolution.TimeElapsedInMilliseconds = timeElapsed;

            EncodedSolution = encodedSolution;

            var solution = EncodedSolution.GetSolution;
            solution.Fase = Fase;
            solution.ProfitEvolution = string.Join(";", ProblemManager.HistoricalEncodedSolutions.Select(s => s.GetSolution.GetCurrentProfit.ToString()).ToList());

            SolutionRepository.SaveSolution(solution);
        }
    }
}
