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
            var intancesIds = Provider.GetSelectedInstancesForTesting();
            var intances = new List<Main.Entities.Instance>();
            foreach (var instanceId in intancesIds)
            {
                intances.Add(instanceRepository.GetById(instanceId));
            }
            PaperConfigsFactory.SimpleDecoderRun(intances, 2);
        }
    }
}
