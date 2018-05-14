using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Main.Repositories;
using Main.Entities;
using Main.Factory;
using TesisUnitTests.Helper;
using System.Collections.Generic;
using Main.GuidedLocalSearchHeuristics;
using Main.Model;
using TesisUnitTests.FinalResults;

namespace TesisUnitTests
{
    [TestClass]
    public class Test_Brkga_Puro
    {
        [TestMethod]
        public void Test_Sin_BL_Solo_Iteraciones_Minimas()
        {
            // 1. Para las 2 instancias mas grandes. Todo con deco simple. 5 configuraciones. 25 ejecuciones.
            var instanceRepository = new InstanceRepository(TopEntitiesManager.GetContext());

            var config = UltimosResultadosFactory.GetFinalResultsConfig_NoHeuristicsLong(new List<ILocalSearchHeuristic>());
            config.Fase = 1030;

            //foreach (var config in configs)
            //    config.Fase = 1022;

            var instances = new List<int>() { 776 };

            foreach (var intance in instances)
            {
                //for (var index = 0; index < 25; index++)
                //{
                    var instance = instanceRepository.GetById(intance);
                    var brkga = BrkgaFactory.Get(instance, config);
                    brkga.Start();
                //}
            }
        }
    }
}
