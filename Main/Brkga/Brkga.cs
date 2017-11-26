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

            //var juan = ProblemManager.HistoricalEncodedSolutions;

            // TODO: Para salvar todas las cadenas hay que hacer que la misma solucion no pase de generacion en generacion, sino una copia
            //foreach (var encodedSolution in ProblemManager.HistoricalEncodedSolutions)
            //{
            //    SolutionRepository.SaveSolution(encodedSolution.GetSolution);
            //}

            // No quiero intentar mejorar cuando no configure heuristicas locales
            if(ProblemManager.Heuristics.Count > 0)
                LastImprovementTry(ref encodedSolution);
            encodedSolution.GetSolution.TimeElapsedInMilliseconds = timeElapsed;

            EncodedSolution = encodedSolution;

            var solution = EncodedSolution.GetSolution;
            solution.Fase = Fase;
            solution.ProfitEvolution = string.Join(";", ProblemManager.HistoricalEncodedSolutions.Select(s => s.GetSolution.GetCurrentProfit.ToString()).ToList());

            SolutionRepository.SaveSolution(solution);
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
