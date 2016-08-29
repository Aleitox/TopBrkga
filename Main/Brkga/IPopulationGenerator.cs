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

        int Generation { get; set; }

        Population Evolve(Population population);
    }

    public class PopulationGenerator : IPopulationGenerator
    {
        public PopulationGenerator(IProblemDecoder problemDecoder, int nonProfitDestinations)
        {
            ProblemDecoder = problemDecoder;
            AmountOfDestinations = problemDecoder.Provider.GetAmountOfDestinations();
            PopulationSize = 8;
            Generation = 0;
            NonProfitDestinations = nonProfitDestinations;
        }

        private int NonProfitDestinations;

        public IProblemDecoder ProblemDecoder { get; set; }

        public int AmountOfDestinations { get; set; }

        public int PopulationSize { get; set; }

        public int Generation { get; set; }

        public int EliteSize { get { return PopulationSize * 3 / 10; } }

        public int NonEliteSize { get { return PopulationSize - EliteSize; } }

        public int EliteGenChance {
            get { return 50; }
        }

        public Population Generate()
        {
            var population = new Population();

            var randomGenerator = new Random();
            for (var index = 0; index < PopulationSize; index++)
            {
                EncodedSolution encodedSolution;
                do
                {
                    var randomVector = GenerateRandomVector(AmountOfDestinations, randomGenerator.Next());
                    encodedSolution = new EncodedSolution(ProblemDecoder, randomVector);
                } while (population.EncodedProblems.Any(ep => ep.IsEquivalenteTo(encodedSolution))); 

                population.EncodedProblems.Add(encodedSolution);
            }

            return population;
        }

        private List<RandomKey> GenerateRandomVector(int amountOfDestinations, int seed)
        {
            var randomVector = new List<RandomKey>();
            var randomGenerator = new Random(seed);
            for (var index = NonProfitDestinations; index < amountOfDestinations; index++) // index != 0 => El depot siempre va a ser considerado como salida y llegada.
            {
                var nextRandom = randomGenerator.Next(0, 1000);
                while (randomVector.Any(r => r.Key == nextRandom))
                    nextRandom++;
                randomVector.Add(new RandomKey(nextRandom, index));
            }
            return randomVector;
        }


        public Population Evolve(Population population)
        {
            population.EncodedProblems = population.GetOrderByMostProfitable();

            var elitePopulation = population.EncodedProblems.Take(EliteSize).ToList();
            var nonElitePopulation = population.EncodedProblems.Skip(EliteSize).Take(NonEliteSize).ToList();

            var evolvedPopulation = new Population(elitePopulation);

            while (evolvedPopulation.CurrentPopulationSize() < PopulationSize)
                evolvedPopulation.EncodedProblems.Add(Mate(GetRandomItem(elitePopulation), GetRandomItem(nonElitePopulation)));
            
            Generation++;

            return evolvedPopulation;
        }

        private EncodedSolution Mate(EncodedSolution eliteParent, EncodedSolution nonEliteParent)
        {
            var child = new EncodedSolution(eliteParent.ProblemDecoder, new List<RandomKey>());

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

        private EncodedSolution GetRandomItem(List<EncodedSolution> elitePopulation)
        {
            var randomGenerator = new Random();
            return elitePopulation[randomGenerator.Next(elitePopulation.Count())];
        }
    }
}
