using System.Collections.Generic;
using System.Diagnostics;
using Main.BrkgaTop;
using Main.Entities;
using Main.GuidedLocalSearchHeuristics;
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
            var timer = new Stopwatch();

            timer.Start();

            ProblemManager.InitializePopulation();

            while (!ProblemManager.StoppingRuleFulfilled)
                ProblemManager.EvolvePopulation();0

            ProblemManager.Population.MarkSolutionsAsFinals();

            timer.Stop();
            var timeElapsed = timer.ElapsedMilliseconds;

            var encodedSolution = ProblemManager.Population.GetMostProfitableSolution();

            //var juan = ProblemManager.HistoricalEncodedSolutions;

            // TODO: Para salvar todas las cadenas hay que hacer que la misma solucion no pase de generacion en generacion, sino una copia
            //foreach (var encodedSolution in ProblemManager.HistoricalEncodedSolutions)
            //{
            //    SolutionRepository.SaveSolution(encodedSolution.GetSolution);
            //}

            LastImprovementTry(ref encodedSolution);
            encodedSolution.GetSolution.TimeElapsedInMilliseconds = timeElapsed;

            EncodedSolution = encodedSolution;

            SolutionRepository.SaveSolution(EncodedSolution.GetSolution);
        }

        private void LastImprovementTry(ref EncodedSolution encodedSolution)
        {
            var heuristics = new List<ILocalSearchHeuristic>()
            {
                new SwapHeuristic(),
                new InsertHeuristic(),
                new SwapHeuristic(),
                new ReplaceHeuristic(),
                new SwapHeuristic(),
                new TwoZeroPtSwap(),
                new InsertHeuristic(),
                new ReplaceHeuristic()
            };

            foreach (var heuristic in heuristics)
            {
                heuristic.ApplyHeuristic(ref encodedSolution);
            }
        }
    }
}
