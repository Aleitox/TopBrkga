using Main.Brkga;
using Main.BrkgaTop.Decoders;
using Main.Entities;
using Main.Model;
using Main.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Factory
{
    public class PaperConfigsFactory
    {
        public static void SimpleDecoderRun(List<Instance> instances, int fase)
        {
            var solutionRepository = new SolutionRepository(TopEntitiesManager.GetContext());

            var solutionName = "Decoder Simple Alone";
            var problemProviders = new List<ProblemResourceProvider>();
            var decoders = new List<IProblemDecoder>();
            foreach (var instance in instances)
                problemProviders.Add(ProblemProviderFactory.CreateProblemProvider(instance, solutionName));
            foreach (var problemProvider in problemProviders)
                decoders.Add(new FirstSimpleDecoder(problemProvider));
            foreach (var decoder in decoders)
            {
                var randomKeysList = CreateRandomKeyVector(decoder, 5);
                Model.Solution s;
                foreach (var randomKeys in randomKeysList)
                {
                    s = decoder.Decode(randomKeys);
                    s.Fase = fase;
                    solutionRepository.SaveSolution(s);
                }
            }
        }

        public static List<List<RandomKey>> CreateRandomKeyVector(IProblemDecoder decoder, int amount)
        {
            var random = new Random();
            var randomKeysList = new List<List<RandomKey>>();
            for (var index = 0; index < amount; index++)
            {
                var list = PopulationGenerator.GenerateRandomVector(decoder.Provider.GetAmountOfDestinations(), random.Next(1000), 2);
                randomKeysList.Add(list);
            }

            return randomKeysList;
        }
    }
}
