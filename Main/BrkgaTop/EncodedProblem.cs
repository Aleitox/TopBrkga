using Main.Model;
using System.Collections.Generic;

namespace Main.BrkgaTop
{
    public class EncodedProblem
    {
        public EncodedProblem(IProblemDecoder problemDecoder, List<RandomKey> randomKeys)
        {
            ProblemDecoder = problemDecoder;
            RandomKeys = randomKeys;
        }

        public IProblemDecoder ProblemDecoder { get; set; }

        public List<RandomKey> RandomKeys { get; set; }

        private Problem Problem { get; set; }

        public Problem GetProblem {
            get { return Problem ?? (Problem = ProblemDecoder.Decode(RandomKeys)); }
        }

    }
}
