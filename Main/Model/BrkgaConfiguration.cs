using Main.BrkgaTop.Decoders;
using Main.GuidedLocalSearchHeuristics;
using System.Collections.Generic;

namespace Main.Model
{
    public class BrkgaConfiguration
    {
        public string Description { get; set; }

        public int MinIterations { get; set; }

        public int MinNoChanges { get; set; }

        public int PopulationSize { get; set; }

        public decimal ElitePercentage { get; set; }

        public decimal MutantPercentage { get; set; }

        public int EliteGenChance { get; set; }

        public List<ILocalSearchHeuristic> Heuristics { get; set; }

        public int ApplyHeuristicsToTop { get; set; }

        public DecoderEnum DecoderType { get; set; }
    }
}
