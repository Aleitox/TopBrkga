using System;
using System.Collections.Generic;
using Main.Brkga;
using Main.Entities;
using Main.Factory;
using Main.Model;
using Main.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TesisUnitTests.Helper;

namespace TesisUnitTests
{
    [TestClass]
    public class BrkgaFactoryTest
    {

        // Corre
        [TestMethod]
        public void Test_Basic_Configuration_On_One_Instance()
        {
            var instanceRepository = new InstanceRepository(TopEntitiesManager.GetContext());

            var configs = Provider.GetSelectedConfigurationsForTesting();
            var intancesIds = Provider.GetSelectedInstancesForTesting();

            foreach (var config in configs)
            {
                foreach (var instanceId in intancesIds)
                {
                    var instance = instanceRepository.GetById(instanceId);
                    var brkga = BrkgaFactory.Get(instance, config);
                    brkga.Start();
                }
            }
        }
    }
}
