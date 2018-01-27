using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Main.Repositories;
using Main.Entities;
using Main.Factory;
using TesisUnitTests.Helper;
using System.Collections.Generic;
using Main.GuidedLocalSearchHeuristics;

namespace TesisUnitTests.FinalResults
{
    [TestClass]
    public class Resultados
    {
        [TestMethod]
        public void RunAll()
        {
            //Test_Local_Search_Order();
            //Test_Decos_With_Heuristics();
            Test_Final_Results();
        }
        
        public static void Test_Local_Search_Order()
        {
            // 1. Para las 2 instancias mas grandes. Todo con deco simple. 5 configuraciones. 25 ejecuciones.
            var instanceRepository = new InstanceRepository(TopEntitiesManager.GetContext());

            var configs = UltimosResultadosFactory.GetConfigsForTestLocalSearchOrder();

            foreach (var config in configs)
                config.Fase = 1001;

            var instances = new List<int>() { 628, 776 };

            foreach (var intance in instances)
            {
                foreach (var config in configs)
                {
                    for (var index = 0; index < 25; index++)
                    {
                        var instance = instanceRepository.GetById(intance);
                        var brkga = BrkgaFactory.Get(instance, config);
                        brkga.Start();
                    }
                }
            }            
        }
        
        public static void Test_Decos_With_Heuristics()
        {
            // 1. Para las 2 instancias mas grandes. Todo con deco simple. 5 configuraciones. 25 ejecuciones.
            var instanceRepository = new InstanceRepository(TopEntitiesManager.GetContext());

            var configs = UltimosResultadosFactory.GetConfigsWithDiferentDeco();

            foreach (var config in configs)
                config.Fase = 1002;

            var instances = new List<int>() { 628, 776 };

            foreach (var intance in instances)
            {
                foreach (var config in configs)
                {
                    for (var index = 0; index < 10; index++)
                    {
                        var instance = instanceRepository.GetById(intance);
                        var brkga = BrkgaFactory.Get(instance, config);
                        brkga.Start();
                    }
                }
            }
        }

        public static void Test_Final_Results()
        {
            var instanceRepository = new InstanceRepository(TopEntitiesManager.GetContext());
            var config = UltimosResultadosFactory.GetFinalResultsConfig();
            config.Fase = 1010;
            var instances = instanceRepository.GetAll().Where(x => x.Id == 776).ToList();
            foreach (var instance in instances)
            {
                for (var index = 0; index < 3; index++)
                {
                    var brkga = BrkgaFactory.Get(instance, config);
                    brkga.Start();
                }
            }
        }

            //public static void Test_Local_Search_Missing()
            //{
            //    // Creo 200 soluciones al azar con el Greedy decoder y las persisto
            //    // Tomo cada una de estas soluciones, le aplico una heuristica y la persisto  
            //    // Finalmente mido en promedio cuanto mejoro la solucion. y cuanto disminuyo la ruta.
            //    // Busqueda Local | Tiempo Promedio de ejecucion | Incremento Porcentual Beneficio | Decremento Porcentual Distancia

            //    // 1. Creo las soluciones
            //    var instanceRepository = new InstanceRepository(TopEntitiesManager.GetContext());
            //    var intancesIds = Provider.GetSelectedInstancesForTesting();
            //    //var intancesIds = new List<int>() { 628 };
            //    var intances = new List<Main.Entities.Instance>();
            //    foreach (var instanceId in intancesIds)
            //    {
            //        intances.Add(instanceRepository.GetById(instanceId));
            //    }

            //    var simpleSolutions = PaperConfigsFactory.SimpleDecoderRun(intances, 1003, 200);
            //    var greedySolutions = PaperConfigsFactory.GreedyDecoderRun(intances, 1004, 200);

            //    //2. Tomar las soluciones
            //    var solutionRepository = new SolutionRepository(TopEntitiesManager.GetContext());

            //    var localSearch = new SwapHeuristic();
            //    for (var index = 0; index < simpleSolutions.Count; index++)
            //    {
            //        var s = simpleSolutions[index];
            //        s.Fase = 1004;
            //        s.Id = 0;
            //        localSearch.ApplyHeuristic(ref s.);
            //        solutionRepository.SaveSolution(simpleSolutions[index]);
            //    }
            //}
        }
}
