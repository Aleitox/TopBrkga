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
        }

        public IProblemDecoder ProblemDecoder { get; set; }

        public List<RandomKey> RandomKeys { get; protected set; }

        public void SetRandomKeys(List<RandomKey> randomKeys)
        {
            OrderedRandomKeys = null;
            Solution = null;
            RandomKeys = randomKeys;
        }

        private List<RandomKey> OrderedRandomKeys { get; set; }

        public List<RandomKey> GetOrderedRandomKeys()
        {
            if (OrderedRandomKeys == null)
                OrderedRandomKeys = RandomKeys.OrderBy(rk => rk.Key).ToList();

            return OrderedRandomKeys;
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
            //get { return Solution != null ? Solution : (Solution = ProblemDecoder.Decode(this)); }
            get
            {
                Solution = ProblemDecoder.Decode(this);
                return Solution;
            }
        }

    }
}
