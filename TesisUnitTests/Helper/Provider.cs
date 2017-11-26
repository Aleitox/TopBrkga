using Main.Factory;
using Main.Model;
using System.Collections.Generic;

namespace TesisUnitTests.Helper
{
    public class Provider
    {
        public static int GetSmallInstanceId()
        {
            return 400;
        }
        public static int GetBigInstanceId()
        {
            return 776;
        }

        public static List<int> GetProblematicInstances()
        {
            return new List<int>() { 776, 628 };
        }

        public static List<int> GetSelectedInstancesForTesting()
        {
            return new List<int>() { 628, 407, 400, 776, 721, 532 };
        }

        public static List<BrkgaConfiguration> GetSelectedConfigurationsForTesting()
        {
            //return new List<BrkgaConfiguration>() { BrkgaFactory.GetBasicConfigWithEuristics() };
            return new List<BrkgaConfiguration>() { BrkgaFactory.GetBasicConfig(), BrkgaFactory.GetFewIterationsConfig(), BrkgaFactory.GetMoreIterationsConfig(), BrkgaFactory.GetUnBiasConfig() };
        }

        public static List<BrkgaConfiguration> GetHeuristicsConfigurationsForTesting()
        {
            return new List<BrkgaConfiguration>() { BrkgaFactory.GetBasicConfigWithEuristics() };
            //return new List<BrkgaConfiguration>() { BrkgaFactory.GetBasicConfig(), BrkgaFactory.GetFewIterationsConfig(), BrkgaFactory.GetMoreIterationsConfig(), BrkgaFactory.GetUnBiasConfig() };
        }
    }
}
