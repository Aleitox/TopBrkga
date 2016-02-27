using Main.BrkgaTop;
using System;
using System.Collections.Generic;
using System.Linq;
using Main.Model;

namespace Main.Brkga
{
    public interface IPopulationGenerator
    {
        IProblemDecoder ProblemDecoder { get; set; }

        Population Generate();

        void Evolve(Population population);
    }

    public class PopulationGenerator : IPopulationGenerator
    {
        public PopulationGenerator(IProblemDecoder problemDecoder)
        {
            ProblemDecoder = problemDecoder;
            AmountOfDestinations = problemDecoder.Provider.GetAmountOfDestinations();
        }

        public IProblemDecoder ProblemDecoder { get; set; }

        public int AmountOfDestinations { get; set; }

        public int PopulationSize { get; set; }

        public int EliteSize { get { return PopulationSize * 3 / 10; } }

        public int NonEliteSize { get { return PopulationSize - EliteSize; } }

        public int EliteGenChance {
            get { return 70; }
        }

        public Population Generate()
        {
            var population = new Population();

            for (var index = 0; index < PopulationSize; index++)
            {
                var randomVector = GenerateRandomVector(AmountOfDestinations);
                var encodedProblem = new EncodedProblem(ProblemDecoder, randomVector);
                population.EncodedProblems.Add(encodedProblem);
            }

            return population;
        }

        private List<RandomKey> GenerateRandomVector(int amountOfDestinations)
        {
            var randomVector = new List<RandomKey>();
            var randomGenerator = new Random();
            for (var index = 0; index < amountOfDestinations; index++)
                randomVector.Add(new RandomKey(randomGenerator.Next(0, 10000), index));
            return randomVector;
        }


        public void Evolve(Population population)
        {
            population.EncodedProblems = population.EncodedProblems.OrderBy(ep => ep.GetProblem.CurrentProfit).ToList();
            var elitePopulation = population.EncodedProblems.Take(EliteSize).ToList();
            var nonElitePopulation = population.EncodedProblems.Skip(EliteSize).Take(NonEliteSize).ToList();

            var evolvedPopulation = new List<EncodedProblem>();
            evolvedPopulation.AddRange(elitePopulation);

            while (evolvedPopulation.Count < PopulationSize)
                evolvedPopulation.Add(Mate(GetRandomItem(elitePopulation), GetRandomItem(nonElitePopulation)));
            
            population.EncodedProblems = evolvedPopulation;
        }

        private EncodedProblem Mate(EncodedProblem eliteParent, EncodedProblem nonEliteParent)
        {
            var child = new EncodedProblem(eliteParent.ProblemDecoder, new List<RandomKey>());

            for (var index = 0; index < eliteParent.RandomKeys.Count; index ++)
                child.RandomKeys.Insert(index, TakeOne(eliteParent.RandomKeys[index].Key, nonEliteParent.RandomKeys[index].Key, index));

            return child;
        }

        private RandomKey TakeOne(int eliteKey, int nonElitekey, int position)
        {
            var randomGenerator = new Random();
            var key = randomGenerator.Next(100) >= EliteGenChance ? nonElitekey : eliteKey;
            return new RandomKey(key, position);
        }

        private EncodedProblem GetRandomItem(List<EncodedProblem> elitePopulation)
        {
            var randomGenerator = new Random();
            return elitePopulation[randomGenerator.Next(elitePopulation.Count())];
        }
    }
}
