using System.Globalization;
using System.Linq;
using Main.BrkgaTop.Decoders;
using Main.Model;
using System.Collections.Generic;

namespace Main.BrkgaTop
{
    public class EncodedSolution
    {
        public EncodedSolution(IProblemDecoder problemDecoder, List<RandomKey> randomKeys)
        {
            ProblemDecoder = problemDecoder;
            RandomKeys = randomKeys;
            EnhancedByLocalHeuristics = false;
        }

        public IProblemDecoder ProblemDecoder { get; set; }

        public List<RandomKey> RandomKeys { get; protected set; }

        public bool EnhancedByLocalHeuristics { get; set; }

        public void SetRandomKeys(List<RandomKey> randomKeys)
        {
            OrderedRandomKeys = null;
            Solution = null;
            pseudoHash = null;
            RandomKeys = randomKeys;
        }

        private List<RandomKey> OrderedRandomKeys { get; set; }

        public List<RandomKey> GetOrderedRandomKeys()
        {
            return OrderedRandomKeys ?? (OrderedRandomKeys = RandomKeys.OrderBy(rk => rk.Key).ToList());
        }

        private string pseudoHash;

        private string GetPseudoHash()
        {
            if (string.IsNullOrEmpty(pseudoHash))
                pseudoHash = string.Join("@", GetOrderedRandomKeys().Select(k => k.PositionIndex.ToString(CultureInfo.InvariantCulture)));

            return pseudoHash;
        }

        public bool IsEquivalenteTo(EncodedSolution encodedSolution)
        {
            return GetPseudoHash() == encodedSolution.GetPseudoHash();
        }

        private Solution Solution { get; set; }

        public Solution GetSolution {
            get { return Solution ?? (Solution = ProblemDecoder.Decode(this)); }
        }

        public override string ToString()
        {
            return string.Format("{2} Profit:{0} Distance:{3} Hash:{1}", GetSolution.GetCurrentProfit, GetPseudoHash(), EnhancedByLocalHeuristics, GetSolution.PrintRouteDistances());
        }
    }
}
