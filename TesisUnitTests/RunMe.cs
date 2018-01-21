using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Main.Repositories;
using Main.Factory;
using TesisUnitTests.Helper;
using Main.Entities;

namespace TesisUnitTests
{
    [TestClass]
    public class RunMe
    {
        [TestMethod]
        public void TestMethod1()
        {
            var instanceRepository = new InstanceRepository(TopEntitiesManager.GetContext());
            
            for (var index = 0; index < 10; index++)
            {
                var config = PaperConfigsFactory.GetSimpleTestConfig();
                var instance = instanceRepository.GetById(Provider.GetBigInstanceId());
                var brkga = BrkgaFactory.Get(instance, config);
                brkga.Start();
            }
        }
    }
}
