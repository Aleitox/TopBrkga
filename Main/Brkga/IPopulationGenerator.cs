using Main.BrkgaTop;
using System;
using System.Collections.Generic;
using System.Linq;
using Main.BrkgaTop.Decoders;
using Main.Model;

namespace Main.Brkga
{
    public interface IPopulationGenerator
    {
        IProblemDecoder ProblemDecoder { get; set; }

        Population Generate(int amountToGenerate);

        int Generation { get; set; }

        Population Evolve(Population population);

        int PopulationSize { get; set; }

        EncodedSolution GenerateEncodedSolution(List<EncodedSolution> encodedSolutions);
    }

    public class PopulationGenerator : IPopulationGenerator
    {
        public PopulationGenerator(IProblemDecoder problemDecoder, int nonProfitDestinations, int populationSize = 100, decimal elitePercentage = 0, decimal mutantPercentage = 0, int eliteGenChance = 50)
        {
            ProblemDecoder = problemDecoder;
            
            AmountOfDestinations = problemDecoder.Provider.GetAmountOfDestinations();
            
            AllowDuplicatesOnRandomCreation = problemDecoder.Provider.GetDestinations().Count() - 2 < 7;
            PopulationSize = populationSize;

            Generation = 0;
            NonProfitDestinations = nonProfitDestinations;
            ElitePercentage = elitePercentage == 0 ? 0.3m : elitePercentage;
            MutantPercentage = mutantPercentage == 0 ? 0.1m : mutantPercentage;
            EliteGenChance = eliteGenChance;
            Random = new Random();
        }

        private int NonProfitDestinations;

        public IProblemDecoder ProblemDecoder { get; set; }

        public int AmountOfDestinations { get; set; }

        public int PopulationSize { get; set; }

        public int Generation { get; set; }

        private int eliteSize { get; set; }

        private Random Random { get; set; }

        public decimal ElitePercentage { get; set; }

        public decimal MutantPercentage { get; set; }

        public bool AllowDuplicatesOnRandomCreation { get; set; }

        public int EliteSize {
            get
            {
                if (eliteSize == 0)
                    eliteSize = Convert.ToInt32(PopulationSize * ElitePercentage);
                return eliteSize;
            } 
        }

        private int nonEliteSize { get; set; }

        public int NonEliteSize
        {
            get
            {
                if (nonEliteSize == 0)
                    nonEliteSize = (PopulationSize + MutatansSize) - EliteSize;
                return nonEliteSize;
            }
        }
        private int mutatansSize { get; set; }

        public int MutatansSize
        {
            get
            {
                if (mutatansSize == 0)
                    mutatansSize = Convert.ToInt32(PopulationSize * MutantPercentage);
                return mutatansSize;
            }
        }


        public int EliteGenChance { get; set; }

        public Population Generate(int amountToGenerate)
        {
            var population = new Population();

            var randomGenerator = new Random();
            for (var index = 0; index < amountToGenerate; index++)
            {
                var encodedSolution = GenerateEncodedSolution(population.EncodedProblems);
                population.EncodedProblems.Add(encodedSolution);
            }

            return population;
        }

        public EncodedSolution GenerateEncodedSolution(List<EncodedSolution> encodedSolutions)
        {
            EncodedSolution encodedSolution;
            do
            {
                var randomVector = GenerateRandomVector(AmountOfDestinations, Random.Next(), NonProfitDestinations);
                encodedSolution = new EncodedSolution(ProblemDecoder, randomVector);
            } while (!AllowDuplicatesOnRandomCreation && encodedSolutions.Any(ep => ep.IsEquivalenteTo(encodedSolution)));

            return encodedSolution;
        }

        // TODO Refactor: Sacar a un helper?
        public static List<RandomKey> GenerateRandomVector(int amountOfDestinations, int seed, int nonProfitDestinations)
        {
            var randomVector = new List<RandomKey>();
            var randomGenerator = new Random(seed);
            for (var index = 1; index < amountOfDestinations - 1; index++) // index != 0 && index != amountOfDestinations - 1 => 0 es inicio, amountOfDestinations - 1 es fin
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
            var mutatants = Generate(MutatansSize).EncodedProblems;

            var evolvedPopulation = new Population(elitePopulation, mutatants);

            var childs = 0;
            var randoms = 0;

            while (evolvedPopulation.CurrentPopulationSize() < PopulationSize)
            {
                var childSolution = Mate(GetRandomItem(elitePopulation), GetRandomItem(nonElitePopulation));
                if (evolvedPopulation.EncodedProblems.Any(x => x.IsEquivalenteTo(childSolution)))
                {
                    var mutant = GenerateEncodedSolution(evolvedPopulation.EncodedProblems);
                    evolvedPopulation.EncodedProblems.Add(mutant);
                    randoms++;
                }
                else
                {
                    evolvedPopulation.EncodedProblems.Add(childSolution);
                    childs++;
                }
            }

            evolvedPopulation.GetMostProfitableSolution().GetSolution.BestInGeneration = true;
            Generation++;

            return evolvedPopulation;
        }

        private EncodedSolution Mate(EncodedSolution eliteParent, EncodedSolution nonEliteParent)
        {
            var child = new EncodedSolution(eliteParent.ProblemDecoder, new List<RandomKey>());

            for (var index = 0; index < eliteParent.RandomKeys.Count; index ++)
                child.RandomKeys.Insert(index, TakeOne(eliteParent.RandomKeys[index].Key, nonEliteParent.RandomKeys[index].Key, index));

            child.GetSolution.FatherId = eliteParent.GetSolution.Id;
            child.GetSolution.MotherId = nonEliteParent.GetSolution.Id;

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
            return elitePopulation[Random.Next(elitePopulation.Count())];
        }

        public int Factorial(int number)
        {
            var result = 1;
            while (number > 1)
            {
                result = result * number;
                number--;
            }
            return result;
        }
    }
}
