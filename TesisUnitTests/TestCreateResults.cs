using System;
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
        [TestMethod]
        public void TestMethod()
        {
            var instanceRepository = new InstanceRepository(TopEntitiesManager.GetContext());
            //var intancesIds = Provider.GetSelectedInstancesForTesting();
            var intancesIds = new List<int>() { 628 };
            var intances = new List<Main.Entities.Instance>();
            foreach (var instanceId in intancesIds)
            {
                intances.Add(instanceRepository.GetById(instanceId));
            }

            PaperConfigsFactory.SimpleDecoderRun(intances, 3, 200);
            PaperConfigsFactory.GreedyDecoderRun(intances, 3, 200);
        }

        [TestMethod]
        public void Test_Basic_Configuration_On_One_Instance_2()
        {
            var instanceRepository = new InstanceRepository(TopEntitiesManager.GetContext());

            var config = PaperConfigsFactory.GetConfigOne();
            var instances = Provider.GetProblematicInstances();

            foreach(var intance in instances)
            {
                for (var index = 0; index < 100; index++)
                {
                    var instance = instanceRepository.GetById(intance);
                    var brkga = BrkgaFactory.Get(instance, config);
                    brkga.Start();
                }
            }
        }
    }
}
