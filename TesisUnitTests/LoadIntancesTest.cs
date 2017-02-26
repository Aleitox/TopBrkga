using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using Main.Entities;
using Main.Factory;
using Main.FileIO;
using Main.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TesisUnitTests
{
    [TestClass]
    public class LoadIntancesTest
    {
        [TestMethod]
        public void LoadAlCases()
        {
            //const string path = @"D:\Desarrollo\VisualStudio\Tesis\TopBrkga\Main\Cases\Tsiligirides\Set_21_234";
            //return;

            var partialPaths = new List<string>()
            {
                @"D:\Desarrollo\VisualStudio\Tesis\TopBrkga\Main\Cases\Tsiligirides\Set_21_234",
                @"D:\Desarrollo\VisualStudio\Tesis\TopBrkga\Main\Cases\Tsiligirides\Set_32_234",
                @"D:\Desarrollo\VisualStudio\Tesis\TopBrkga\Main\Cases\Tsiligirides\Set_33_234",
                @"D:\Desarrollo\VisualStudio\Tesis\TopBrkga\Main\Cases\Chao\Set_64_234",
                @"D:\Desarrollo\VisualStudio\Tesis\TopBrkga\Main\Cases\Chao\Set_66_234",
                @"D:\Desarrollo\VisualStudio\Tesis\TopBrkga\Main\Cases\Chao\Set_100_234",
                @"D:\Desarrollo\VisualStudio\Tesis\TopBrkga\Main\Cases\Chao\Set_102_234"
            };

            foreach (var path in partialPaths)
            {
                var set = path.Substring(path.LastIndexOf("\\", StringComparison.Ordinal) + 1);
                var pathWithoutSet = path.Replace("\\" + set, "");

                var author = pathWithoutSet.Substring(pathWithoutSet.LastIndexOf("\\", StringComparison.Ordinal) + 1);

                var directoryInfo = new DirectoryInfo(path);

                var instanceRepository = new InstanceRepository(TopEntitiesManager.GetContext());

                foreach (var file in directoryInfo.GetFiles("*.txt"))
                {
                    //if (file.Name != "p2.2.a.txt")
                    //    continue;

                    var fileManager = new FileManager(file.Name, path);
                    var caso = fileManager.ReadFile();
                    var problemProvider = ProblemProviderFactory.CreateProblemProvider(caso);
                    var name = Path.GetFileNameWithoutExtension(file.Name);
                    if (instanceRepository.GetAll().All(x => x.Name != name))
                        instanceRepository.SaveInstance(problemProvider, name, author, set);
                }
            }
        }

        [TestMethod]
        public void TestLoadedInstance()
        {
            var instanceRepository = new InstanceRepository(TopEntitiesManager.GetContext());
            var intance = instanceRepository.GetBy("Tsiligirides", "Set_21_234", "p2.2.a.txt");

            Assert.AreEqual(21, intance.DestiniesCount);
            Assert.AreEqual(2, intance.Vehicles);
            Assert.AreEqual(Convert.ToDecimal(7.5), intance.TMax);

            Assert.AreEqual(21, intance.Destinies.Count);

            var aDestiny = intance.Destinies.First(d => d.Id == 1);
            Assert.AreEqual(1, aDestiny.InstanceId);
            Assert.AreEqual(Convert.ToDecimal(4.60), aDestiny.X);
            Assert.AreEqual(Convert.ToDecimal(7.10), aDestiny.Y);
            Assert.AreEqual(0, aDestiny.Profit);

            var anotherDestiny = intance.Destinies.First(d => d.Id == 14);
            Assert.AreEqual(1, anotherDestiny.InstanceId);
            Assert.AreEqual(Convert.ToDecimal(6.70), anotherDestiny.X);
            Assert.AreEqual(Convert.ToDecimal(5.80), anotherDestiny.Y);
            Assert.AreEqual(25, anotherDestiny.Profit);

        }
    }
}
