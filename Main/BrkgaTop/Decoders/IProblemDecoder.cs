using Main.Model;

namespace Main.BrkgaTop.Decoders
{
    public interface IProblemDecoder
    {
        Solution Decode(EncodedSolution encodedSolution);

        ProblemResourceProvider Provider { get; set; }
    }

    public enum DecoderEnum
    {
        Simple = 0,
        Greedy = 1
    }
}
