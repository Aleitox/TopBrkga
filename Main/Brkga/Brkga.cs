using System.Diagnostics;
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
            var timer = new Stopwatch();

            timer.Start();

            ProblemManager.InitializePopulation();

            while (!ProblemManager.StoppingRuleFulfilled)
                ProblemManager.EvolvePopulation();

            ProblemManager.Population.MarkSolutionsAsFinals();

            timer.Stop();
            var timeElapsed = timer.ElapsedMilliseconds;

            EncodedSolution = ProblemManager.Population.GetMostProfitableSolution();

            //var juan = ProblemManager.HistoricalEncodedSolutions;

            // TODO: Para salvar todas las cadenas hay que hacer que la misma solucion no pase de generacion en generacion, sino una copia
            //foreach (var encodedSolution in ProblemManager.HistoricalEncodedSolutions)
            //{
            //    SolutionRepository.SaveSolution(encodedSolution.GetSolution);
            //}
            EncodedSolution.GetSolution.TimeElapsedInMilliseconds = timeElapsed;
            SolutionRepository.SaveSolution(EncodedSolution.GetSolution);
        }
    }
}
