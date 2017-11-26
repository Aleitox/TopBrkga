using Main.BrkgaTop.Decoders;
using Main.GuidedLocalSearchHeuristics;
using System.Collections.Generic;
using System;

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

        public int Fase { get; set; }

        public void SetDescription()
        {
            var outterSeparator = ";";
            var descriptions = new List<string>();
            var innerSeparator = ".";

            var mi = string.Format("MI{0}{1}", innerSeparator, MinIterations);
            descriptions.Add(mi);
            var mnc = string.Format("MNC{0}{1}", innerSeparator, MinNoChanges);
            descriptions.Add(mnc);
            var ps = string.Format("PS{0}{1}", innerSeparator, PopulationSize);
            descriptions.Add(ps);
            var ep = string.Format("EP{0}{1}", innerSeparator, ElitePercentage);
            descriptions.Add(ep);
            var mp = string.Format("MP{0}{1}", innerSeparator, MutantPercentage);
            descriptions.Add(mp);
            var egc = string.Format("EGC{0}{1}", innerSeparator, EliteGenChance);
            descriptions.Add(egc);
            var heu = string.Format("HEU{0}{1}", innerSeparator, GetHeuristcsCode());
            descriptions.Add(heu);
            var top = string.Format("TOP{0}{1}", innerSeparator, ApplyHeuristicsToTop);
            descriptions.Add(top);
            var dt = string.Format("MI{0}{1}", innerSeparator, GetDecoderType());
            descriptions.Add(dt);
            Description = string.Join(outterSeparator, descriptions);
        }

        private object GetDecoderType()
        {
            var decoType = string.Empty;
            if (DecoderType == DecoderEnum.Greedy)
                decoType = "G";
            else if (DecoderType == DecoderEnum.Simple)
                decoType = "S";
            else
                throw new Exception("Missing decoder type");
            return decoType;
        }

        private string GetHeuristcsCode()
        {
            var heuristicCodes = string.Empty;
            foreach (var heuristic in Heuristics)
            {
                if (heuristic is SwapHeuristic)
                    heuristicCodes += "S";
                else if (heuristic is TwoZeroPtSwap)
                    heuristicCodes += "O";
                else if (heuristic is InsertHeuristic)
                    heuristicCodes += "I";
                else if (heuristic is ReplaceHeuristic)
                    heuristicCodes += "R";
                else
                    throw new Exception("Missing heuristic type");
            }
            return heuristicCodes;
        }
    }
}
