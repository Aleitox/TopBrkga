using Main.Model;
using System.Collections.Generic;

namespace Main.BrkgaTop.Decoders
{
    public interface IProblemDecoder
    {
        Solution Decode(EncodedSolution encodedSolution);

        Solution Decode(List<RandomKey> randomKeys);

        ProblemResourceProvider Provider { get; set; }
    }

    public enum DecoderEnum
    {
        Simple = 0,
        Greedy = 1
    }
}
