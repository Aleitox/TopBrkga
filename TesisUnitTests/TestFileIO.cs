using Main.Factory;
using Main.FileIO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;

namespace TesisUnitTests
{
    [TestClass]
    public class TestFileIO
    {
        // TESTS obsoletos, perdi el archivo de prueba fuente. Tendria que volver a crearlos... y paja man

        [TestMethod]
        public void Test_Basic_File_Distances()
        {
            var fileManager = new FileManager(ConfigurationManager.AppSettings["fileNameTestDistances"], ConfigurationManager.AppSettings["filePath"]);
            var caso = fileManager.ReadFile();
            var problemaProvider = ProblemProviderFactory.CreateProblemProvider(caso);
            var problema = problemaProvider.GetFreshProblem();

            // Cuadrante Positivo
            Assert.AreEqual(problema.Map.GetDistance(0, 1), problema.Map.Destinations[1].Profit);
            Assert.AreEqual(problema.Map.GetDistance(0, 2), problema.Map.Destinations[2].Profit);
            Assert.AreEqual(problema.Map.GetDistance(0, 3), problema.Map.Destinations[3].Profit);

            // Cuadrante negativo
            Assert.AreEqual(problema.Map.GetDistance(0, 4), problema.Map.Destinations[4].Profit);
            Assert.AreEqual(problema.Map.GetDistance(0, 5), problema.Map.Destinations[5].Profit);
            Assert.AreEqual(problema.Map.GetDistance(0, 6), problema.Map.Destinations[6].Profit);
            
            // Contra su espejado
            Assert.AreEqual(problema.Map.GetDistance(1, 4), 2*problema.Map.Destinations[4].Profit);
            Assert.AreEqual(problema.Map.GetDistance(2, 5), 2*problema.Map.Destinations[5].Profit);
            Assert.AreEqual(problema.Map.GetDistance(3, 6), 2*problema.Map.Destinations[6].Profit);
        }

        [TestMethod]
        public void Test_Basic_File_Vehicles()
        {
            var fileManager = new FileManager(ConfigurationManager.AppSettings["fileNameTestDistances"], ConfigurationManager.AppSettings["filePath"]);
            var caso = fileManager.ReadFile();
            var problemaProvider = ProblemProviderFactory.CreateProblemProvider(caso);
            var problema = problemaProvider.GetFreshProblem();

            Assert.AreEqual(2, problema.VehicleFleet.Vehicles.Count);

            foreach (var vehicle in problema.VehicleFleet.Vehicles)
            {
                Assert.AreEqual(7.5, vehicle.MaxDistance);
                Assert.AreEqual(0, vehicle.Route.StartingPoint.Coordinate.X);
                Assert.AreEqual(0, vehicle.Route.StartingPoint.Coordinate.Y);
                Assert.AreEqual(0, vehicle.Route.GetProfit());
                Assert.AreEqual(0, vehicle.Route.RouteLenght());
            }
        }

        [TestMethod]
        public void Test_Basic_File_Problem()
        {
            var fileManager = new FileManager(ConfigurationManager.AppSettings["fileNameTestDistances"], ConfigurationManager.AppSettings["filePath"]);
            var caso = fileManager.ReadFile();
            var problemaProvider = ProblemProviderFactory.CreateProblemProvider(caso);
            var problema = problemaProvider.GetFreshProblem();

            Assert.AreEqual(7, problema.Map.Destinations.Count);

            Assert.IsTrue(problema.IsSolution);
            Assert.AreEqual(0, problema.GetCurrentProfit);
            Assert.AreEqual(2, problema.GetCurrentRoutes.Count);

            foreach (var route in problema.GetCurrentRoutes)
            {
                Assert.AreEqual(0, route.GetProfit());
                Assert.AreEqual(0, route.RouteLenght());
            }
        }
    }
}
