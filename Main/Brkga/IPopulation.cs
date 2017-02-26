using System;
using System.Collections.Generic;
using System.Linq;
using Main.BrkgaTop;
using Main.Model;

namespace Main.Brkga
{
    public class Population
    {
        public Population()
        {
            EncodedProblems = new List<EncodedSolution>();
        }

        public Population(List<EncodedSolution> encodedProblems, List<EncodedSolution> mutants)
        {
            EncodedProblems = encodedProblems;
            EncodedProblems.AddRange(mutants);
        }

        public List<EncodedSolution> EncodedProblems { get; set; }

        public List<EncodedSolution> GetOrderByMostProfitable()
        {
            return EncodedProblems.OrderByDescending(ep => ep.GetSolution.GetCurrentProfit).ToList();
        }

        public EncodedSolution GetMostProfitableSolution()
        {
            return GetOrderByMostProfitable().First();
        }

        public void MarkSolutionsAsFinals()
        {
            foreach (var encodedProblem in EncodedProblems)
            {
                encodedProblem.GetSolution.IsFinal = true;
            }
        }

        public int CurrentPopulationSize()
        {
            return EncodedProblems.Count;
        }

        public override string ToString()
        {
            var populationString = string.Empty;

            foreach (var encodedProblems in GetOrderByMostProfitable())
            {
                populationString += string.Format("{0}{1}", encodedProblems.GetSolution.ToString(), Environment.NewLine);
            }

            return populationString;
        }
    }
}
