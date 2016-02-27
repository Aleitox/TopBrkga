using System.Collections.Generic;
using Main.BrkgaTop;
using Main.Model;

namespace Main.Brkga
{
    public class Population
    {
        public Population()
        {
            EncodedProblems = new List<EncodedProblem>();
        }

        public Population(List<EncodedProblem> encodedProblems)
        {
            EncodedProblems = encodedProblems;
        }

        public List<EncodedProblem> EncodedProblems { get; set; }
    }
}
