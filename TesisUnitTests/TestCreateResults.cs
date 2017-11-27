using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Main.Factory;
using TesisUnitTests.Helper;
using Main.Repositories;
using System.Collections.Generic;
using Main.Entities;

namespace TesisUnitTests
{
    [TestClass]
    public class TestCreateResults
    {
        [TestMethod] // *** Decos
        public void DecodersResult()
        {
            var instanceRepository = new InstanceRepository(TopEntitiesManager.GetContext());
            var intancesIds = Provider.GetSelectedInstancesForTesting();
            //var intancesIds = new List<int>() { 628 };
            var intances = new List<Main.Entities.Instance>();
            foreach (var instanceId in intancesIds)
            {
                intances.Add(instanceRepository.GetById(instanceId));
            }

            PaperConfigsFactory.SimpleDecoderRun(intances, 3, 200);
            PaperConfigsFactory.GreedyDecoderRun(intances, 3, 200);
        }

        [TestMethod]
        public void RunAllTests()
        {
            AllSixIntances20TimesGenericConfig();
            ConfigTestOverBig();
            ConfigTestOverAllWithHeuristicsVarience();
        }

        [TestMethod] // (60) Elegir una config y hacer 10 veces para cada una de las 6 intancias y ver mismo que antes
        public void AllSixIntances20TimesGenericConfig()
        {
            var instanceRepository = new InstanceRepository(TopEntitiesManager.GetContext());

            var config = PaperConfigsFactory.GetConfigTwo();
            var instances = Provider.GetSelectedInstancesForTesting();

            foreach (var intance in instances)
            {
                for (var index = 0; index < 10; index++)
                {
                    var instance = instanceRepository.GetById(intance);
                    var brkga = BrkgaFactory.Get(instance, config);
                    brkga.Start();
                }
            }
        }

        [TestMethod] //(120) Elegir 6 configs y hacer 10 veces para las 2 instancias molestas
        public void ConfigTestOverBig()
        {
            var instanceRepository = new InstanceRepository(TopEntitiesManager.GetContext());

            var configs = PaperConfigsFactory.Get6Configs();
            var instances = Provider.GetProblematicInstances();

            foreach(var intance in instances)
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

        [TestMethod] //(180) Elegir una config y hacer 10 veces para cada una de las 6. En el config variar 4 ordenes de euristicas
        public void ConfigTestOverAllWithHeuristicsVarience()
        {
            var instanceRepository = new InstanceRepository(TopEntitiesManager.GetContext());

            var configs = PaperConfigsFactory.Get5Configs();
            var instances = Provider.GetSelectedInstancesForTesting();

            foreach (var intance in instances)
            {
                foreach (var config in configs)
                {
                    for (var index = 0; index < 8; index++)
                    {
                        var instance = instanceRepository.GetById(intance);
                        var brkga = BrkgaFactory.Get(instance, config);
                        brkga.Start();
                    }
                }
            }
        }


        [TestMethod] //Final 199 * 10
        public void FinalResults()
        {
            var instanceRepository = new InstanceRepository(TopEntitiesManager.GetContext());
            var externalSolutionsRepository = new ExternalSolutionsRepository(TopEntitiesManager.GetContext());
            var instances = externalSolutionsRepository.GetAll().Select(e => e.InstanceId).Distinct().ToList();

            var config = PaperConfigsFactory.GetFinalConfig();

            foreach (var intance in instances.Where(i => i != null))
            {
                for (var index = 0; index < 10; index++)
                {
                    var instance = instanceRepository.GetById(intance.Value);
                    var brkga = BrkgaFactory.Get(instance, config);
                    brkga.Start();
                }
            }
        }


    }
}
