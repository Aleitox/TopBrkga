
using Main.BrkgaTop;

namespace Main.GuidedLocalSearchHeuristics
{
    public interface ILocalSearchHeuristic
    {
        void ApplyHeuristic(ref EncodedSolution encodedSolution);
    }
}
