using Main.BrkgaTop.Decoders;
using Main.GuidedLocalSearchHeuristics;
using Main.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesisUnitTests.FinalResults
{
    public class UltimosResultadosFactory
    {
        public static List<ILocalSearchHeuristic> Get_IRmRsOS()
        {
            return new List<ILocalSearchHeuristic>()
            {
                new InsertHeuristic(),
                ReplaceHeuristic.GetSuper(),
                ReplaceHeuristic.GetNormal(),
                new TwoZeroPtSwap(),
                new SwapHeuristic()
            };
        }

        public static List<ILocalSearchHeuristic> Get_SOIRsRm()
        {
            return new List<ILocalSearchHeuristic>()
            {
                new SwapHeuristic(),
                new TwoZeroPtSwap(),
                new InsertHeuristic(),
                ReplaceHeuristic.GetNormal(),
                ReplaceHeuristic.GetSuper(),
            };
        }

        public static List<ILocalSearchHeuristic> Get_SRsOIRm()
        {
            return new List<ILocalSearchHeuristic>()
            {
                new SwapHeuristic(),
                ReplaceHeuristic.GetNormal(),
                new TwoZeroPtSwap(),
                new InsertHeuristic(),
                ReplaceHeuristic.GetSuper(),
            };
        }

        public static List<ILocalSearchHeuristic> Get_ORsSIRm()
        {
            return new List<ILocalSearchHeuristic>()
                {
                    new TwoZeroPtSwap(),
                    ReplaceHeuristic.GetNormal(),
                    new SwapHeuristic(),
                    new InsertHeuristic(),
                    ReplaceHeuristic.GetSuper(),
                };
        }

        public static List<ILocalSearchHeuristic> Get_SIORsSORm()
        {
            return new List<ILocalSearchHeuristic>()
                {
                    new SwapHeuristic(),
                    new InsertHeuristic(),
                    new TwoZeroPtSwap(),
                    ReplaceHeuristic.GetNormal(),
                    new SwapHeuristic(),
                    new TwoZeroPtSwap(),
                    ReplaceHeuristic.GetSuper()
                };
        }

        // Interesante resultados
        public static List<ILocalSearchHeuristic> Get_SOSIRsSORm()
        {
            return new List<ILocalSearchHeuristic>()
                {
                    new SwapHeuristic(),
                    new TwoZeroPtSwap(),
                    new SwapHeuristic(),
                    new InsertHeuristic(),
                    ReplaceHeuristic.GetNormal(),
                    new SwapHeuristic(),
                    new TwoZeroPtSwap(),
                    ReplaceHeuristic.GetSuper()
                };
        }


        public static List<ILocalSearchHeuristic> Get_SOIORsRmSORm()
        {
            return new List<ILocalSearchHeuristic>()
            {
                new SwapHeuristic(),
                new TwoZeroPtSwap(),
                new InsertHeuristic(),
                new TwoZeroPtSwap(),
                ReplaceHeuristic.GetNormal(),
                ReplaceHeuristic.GetSuper(),
                new SwapHeuristic(),
                new TwoZeroPtSwap(),
                ReplaceHeuristic.GetSuper()
            };
        }

        internal static BrkgaConfiguration GetFinalResultsConfig()
        {
            var config = new BrkgaConfiguration()
            {
                MinIterations = 250,
                MinNoChanges = 100,
                PopulationSize = 100,
                ElitePercentage = 0.3m,
                MutantPercentage = 0.1m,
                EliteGenChance = 70,
                Heuristics = Get_SOSIRsSORm(),
                HeuristicsLong = new List<ILocalSearchHeuristic>()
                {
                    new TwoZeroPtSwap(),
                    new SwapHeuristic(),
                    new TwoZeroPtSwap(),
                    new InsertHeuristic(),
                    ReplaceHeuristic.GetSuper(),
                    new SwapHeuristic(),
                    new TwoZeroPtSwap(),
                    new SwapHeuristic(),
                    new InsertHeuristic(),
                    ReplaceHeuristic.GetNormal(),
                    new SwapHeuristic(),
                    new TwoZeroPtSwap(),
                    new SwapHeuristic(),
                    ReplaceHeuristic.GetSuper()
                },
                ApplyHeuristicsToTop = 2,
                DecoderType = DecoderEnum.Simple
            };
            config.SetDescription();
            return config;
        }

        public static BrkgaConfiguration GetBaseConfig(List<ILocalSearchHeuristic> heuristics)
        {
            var config = new BrkgaConfiguration()
            {
                MinIterations = 400,
                MinNoChanges = 100,
                PopulationSize = 150,
                ElitePercentage = 0.3m,
                MutantPercentage = 0.1m,
                EliteGenChance = 70,
                Heuristics = heuristics,
                //HeuristicsLong = new List<ILocalSearchHeuristic>()
                //{
                //    new SwapHeuristic(),
                //    new TwoZeroPtSwap(),
                //    new SwapHeuristic(),
                //    new InsertHeuristic(),
                //    ReplaceHeuristic.GetSuper(),
                //    new TwoZeroPtSwap(),
                //    new SwapHeuristic(),
                //    new TwoZeroPtSwap(),
                //    new InsertHeuristic(),
                //    ReplaceHeuristic.GetNormal(),
                //    new SwapHeuristic(),
                //    new TwoZeroPtSwap(),
                //    new SwapHeuristic(),
                //    ReplaceHeuristic.GetSuper()
                //},
                ApplyHeuristicsToTop = 2,
                DecoderType = DecoderEnum.Simple
            };
            config.SetDescription();
            return config;
        }

        #region Test_Local_Search_Order

        public static List<BrkgaConfiguration> GetConfigsForTestLocalSearchOrder()
        {
            return new List<BrkgaConfiguration>() {
                GetBaseConfig(Get_SOSIRsSORm()),
                GetBaseConfig(Get_IRmRsOS()),
                GetBaseConfig(Get_SOIRsRm()),
                GetBaseConfig(Get_SRsOIRm()),
                GetBaseConfig(Get_ORsSIRm()),
                GetBaseConfig(Get_SIORsSORm()),
                GetBaseConfig(Get_SOIORsRmSORm())
            };
        }

        public static List<BrkgaConfiguration> GetConfigsWithDiferentDeco()
        {
            var configDecoSimple = GetBaseConfig(Get_SOSIRsSORm());
            configDecoSimple.DecoderType = DecoderEnum.Simple;
            configDecoSimple.SetDescription();
            var configDecoGreedy = GetBaseConfig(Get_SOSIRsSORm());
            configDecoGreedy.DecoderType = DecoderEnum.Greedy;
            configDecoGreedy.SetDescription();
            return new List<BrkgaConfiguration>() {
                configDecoSimple,
                configDecoGreedy
            };
        }

        #endregion

    }
}
