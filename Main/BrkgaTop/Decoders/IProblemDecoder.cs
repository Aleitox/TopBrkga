using Main.Model;

namespace Main.BrkgaTop.Decoders
{
    public interface IProblemDecoder
    {
        Solution Decode(EncodedSolution encodedSolution);

        ProblemResourceProvider Provider { get; set; }
    }
}
