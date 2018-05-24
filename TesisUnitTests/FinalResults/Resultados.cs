using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Main.Repositories;
using Main.Entities;
using Main.Factory;
using TesisUnitTests.Helper;
using System.Collections.Generic;
using Main.GuidedLocalSearchHeuristics;
using Main.Model;

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
            //Test_Final_Results();
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

        [TestMethod]
        public void Test_Decos_With_Heuristics()
        {
            // 1. Para las 2 instancias mas grandes. Todo con deco simple. 5 configuraciones. 25 ejecuciones.
            var instanceRepository = new InstanceRepository(TopEntitiesManager.GetContext());

            var configs = UltimosResultadosFactory.GetConfigsWithDiferentDeco();

            foreach (var config in configs)
                config.Fase = 1011;

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

        [TestMethod]
        public void Test_Final_Results()
        {
            var instanceRepository = new InstanceRepository(TopEntitiesManager.GetContext());
            var exSolutionRepository = new ExternalSolutionsBdmRepository(TopEntitiesManager.GetContext());
            var solutionRepository = new SolutionRepository(TopEntitiesManager.GetContext());


            var config = UltimosResultadosFactory.GetFinalResultsConfig();
            config.Fase = 3015; // 1015 fue el que se ejecuto con el viejo hash
            var solutions = solutionRepository.GetAll().Where(s => s.Fase == config.Fase).ToList();
            var exSolutions = exSolutionRepository.GetAll().ToList();
            var instances = instanceRepository.GetAll().ToList().Where(i => exSolutions.Any(e => e.InstanceId == i.Id) && solutions.Where(s => s.InstanceId == i.Id).Count() < 3).ToList();
            foreach (var instance in instances)
            {
                while(solutionRepository.GetAll().Where(s => s.Fase == config.Fase && s.InstanceId == instance.Id).Count() < 3)
                //for (var index = 0; index < 1; index++)
                {
                    var brkga = BrkgaFactory.Get(instance, config);
                    brkga.Start();
                }
            }
        }

        [TestMethod]
        public void Test_Final_Results_Singular()
        {
            var instanceRepository = new InstanceRepository(TopEntitiesManager.GetContext());
            var exSolutionRepository = new ExternalSolutionsBdmRepository(TopEntitiesManager.GetContext());
            var solutionRepository = new SolutionRepository(TopEntitiesManager.GetContext());


            var config = UltimosResultadosFactory.GetFinalResultsConfig();
            config.Fase = 1014;
            var solutions = solutionRepository.GetAll().Where(s => s.Fase == config.Fase).ToList();
            var exSolutions = exSolutionRepository.GetAll().ToList();
            var instances = instanceRepository.GetAll().ToList().Where(i => i.Id == 671).ToList();
            foreach (var instance in instances)
            {
                while (solutionRepository.GetAll().Where(s => s.Fase == config.Fase && s.InstanceId == instance.Id).Count() < 3)
                //for (var index = 0; index < 1; index++)
                {
                    var brkga = BrkgaFactory.Get(instance, config);
                    brkga.Start();
                }
            }
        }

        [TestMethod]
        public void Test_Final_Results_Demo_Heuristica_Exponencial()
        {
            // 1. Para las 2 instancias mas grandes. Todo con deco simple. 5 configuraciones. 25 ejecuciones.
            var instanceRepository = new InstanceRepository(TopEntitiesManager.GetContext());

            var configs = new List<BrkgaConfiguration>();
            configs.Add(UltimosResultadosFactory.GetFinalResultsConfig_NoHeuristicsLong(UltimosResultadosFactory.Get_OSIRs()));
            configs.Add(UltimosResultadosFactory.GetFinalResultsConfig_NoHeuristicsLong(UltimosResultadosFactory.Get_OSIRsRma(2)));
            configs.Add(UltimosResultadosFactory.GetFinalResultsConfig_NoHeuristicsLong(UltimosResultadosFactory.Get_OSIRsRma(4)));
            configs.Add(UltimosResultadosFactory.GetFinalResultsConfig_NoHeuristicsLong(UltimosResultadosFactory.Get_OSIRsRma(8)));
            configs.Add(UltimosResultadosFactory.GetFinalResultsConfig_NoHeuristicsLong(UltimosResultadosFactory.Get_OSIRsRma(16)));
            configs.Add(UltimosResultadosFactory.GetFinalResultsConfig_NoHeuristicsLong(UltimosResultadosFactory.Get_OSIRsRma(32)));
            configs.Add(UltimosResultadosFactory.GetFinalResultsConfig_NoHeuristicsLong(UltimosResultadosFactory.Get_OSIRsRma(64)));
            configs.Add(UltimosResultadosFactory.GetFinalResultsConfig_NoHeuristicsLong(UltimosResultadosFactory.Get_OSIRsRma(128)));
            configs.Add(UltimosResultadosFactory.GetFinalResultsConfig_NoHeuristicsLong(UltimosResultadosFactory.Get_OSIRsRm()));

            foreach (var config in configs)
                config.Fase = 1022;

            var instances = new List<int>() { 400, 779 };
            //var instances = new List<int>() { 777 };

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

        [TestMethod]
        public void Test_Decoders_200_Random()
        {
            // Creo 200 soluciones al azar con el Greedy decoder y las persisto
            // Tomo cada una de estas soluciones, le aplico una heuristica y la persisto  
            // Finalmente mido en promedio cuanto mejoro la solucion. y cuanto disminuyo la ruta.
            // Busqueda Local | Tiempo Promedio de ejecucion | Incremento Porcentual Beneficio | Decremento Porcentual Distancia

            // 1. Creo las soluciones
            var instanceRepository = new InstanceRepository(TopEntitiesManager.GetContext());
            var intancesIds = Provider.GetSelectedInstancesForTesting();

            var intances = new List<Main.Entities.Instance>();
            foreach (var instanceId in intancesIds)
            {
                intances.Add(instanceRepository.GetById(instanceId));
            }            

            var simpleSolutions = PaperConfigsFactory.DecodersRun(intances, 1025, 1026, 200);
        }


        [TestMethod]
        public void Test_Defensa_6_Instances_Brkga_Puro_Y_BL()
        {
            var instanceRepository = new InstanceRepository(TopEntitiesManager.GetContext());
            var solutionRepository = new SolutionRepository(TopEntitiesManager.GetContext());

            var instanceIds = Provider.GetSelectedInstancesForTesting();
            var instances = instanceRepository.GetAll().ToList().Where(i => instanceIds.Any(x => x == i.Id)).ToList();

            var config = UltimosResultadosFactory.GetFinalResultsConfig_BrkgaPuro(Main.BrkgaTop.Decoders.DecoderEnum.Greedy);
            config.Fase = 3030;             

            foreach (var instance in instances)
            {
                while (solutionRepository.GetAll().Where(s => s.Fase == config.Fase && s.InstanceId == instance.Id).Count() < 10)
                {
                    var brkga = BrkgaFactory.Get(instance, config);
                    brkga.Start();
                }
            }

            config = UltimosResultadosFactory.GetFinalResultsConfig_BrkgaPuro(Main.BrkgaTop.Decoders.DecoderEnum.Simple);
            config.Fase = 3031;

            foreach (var instance in instances)
            {
                while (solutionRepository.GetAll().Where(s => s.Fase == config.Fase && s.InstanceId == instance.Id).Count() < 10)
                {
                    var brkga = BrkgaFactory.Get(instance, config);
                    brkga.Start();
                }
            }

            config = UltimosResultadosFactory.GetFinalResultsConfig();
            config.Fase = 3032;

            foreach (var instance in instances)
            {
                while (solutionRepository.GetAll().Where(s => s.Fase == config.Fase && s.InstanceId == instance.Id).Count() < 10)
                {
                    var brkga = BrkgaFactory.Get(instance, config);
                    brkga.Start();
                }
            }
        }

        //[TestMethod]
        //public void Test_Defensa_6_Instances_Brkga_With_BL()
        //{
        //    var instanceRepository = new InstanceRepository(TopEntitiesManager.GetContext());
        //    var solutionRepository = new SolutionRepository(TopEntitiesManager.GetContext());


        //    var config = UltimosResultadosFactory.GetFinalResultsConfig();
        //    config.Fase = 3032;

        //    var solutions = solutionRepository.GetAll().Where(s => s.Fase == config.Fase).ToList();

        //    var instanceIds = Provider.GetSelectedInstancesForTesting();
        //    var instances = instanceRepository.GetAll().ToList().Where(i => instanceIds.Any(x => x == i.Id)).ToList();

        //    foreach (var instance in instances)
        //    {
        //        while (solutionRepository.GetAll().Where(s => s.Fase == config.Fase && s.InstanceId == instance.Id).Count() < 10)
        //        {
        //            var brkga = BrkgaFactory.Get(instance, config);
        //            brkga.Start();
        //        }
        //    }
        //}
    }
}
