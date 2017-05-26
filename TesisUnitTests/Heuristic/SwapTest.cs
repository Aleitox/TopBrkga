using System;
using System.Security.Cryptography;
using Main.BrkgaTop;
using Main.BrkgaTop.Decoders;
using Main.GuidedLocalSearchHeuristics;
using Main.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TesisUnitTests.Heuristic
{
    [TestClass]
    public class SwapTest
    {
        [TestMethod]
        public void Test_Swaps()
        {
            var starPoint = new Destination(0, 0, new Coordinate(0, 0), "start", 0);
            var endPoint = new Destination(0, 0, new Coordinate(10, 0), "end", 0);

            var aVehicle = new Vehicle(1, 100, starPoint, endPoint);
            var anotherVehicle = new Vehicle(2, 100, starPoint, endPoint);

            var destinationAone = new Destination(0, 0, new Coordinate(4, 10), "a1", 0);
            var destinationAtwo = new Destination(0, 0, new Coordinate(6, 10), "a2", 0);

            var destinationBone = new Destination(0, 0, new Coordinate(4, -10), "b1", 0);
            var destinationBtwo = new Destination(0, 0, new Coordinate(6, -10), "b2", 0);

            aVehicle.Route.AddDestination(destinationAone);
            aVehicle.Route.AddDestination(destinationBtwo);

            anotherVehicle.Route.AddDestination(destinationBone);
            anotherVehicle.Route.AddDestination(destinationAtwo);

            var heuristic = new SwapHeuristic();

            // Hace el swap
            Assert.IsTrue(heuristic.Swaps(0, 0, ref aVehicle, ref anotherVehicle));

            Assert.AreEqual(destinationBone.Description, aVehicle.Route.GetDestinationAt(0).Description);
            Assert.AreEqual(destinationBtwo.Description, aVehicle.Route.GetDestinationAt(1).Description);

            Assert.AreEqual(destinationAone.Description, anotherVehicle.Route.GetDestinationAt(0).Description);
            Assert.AreEqual(destinationAtwo.Description, anotherVehicle.Route.GetDestinationAt(1).Description);
            
            // No hace el swap
            Assert.IsFalse(heuristic.Swaps(0, 0, ref aVehicle, ref anotherVehicle));

            Assert.AreEqual(destinationBone.Description, aVehicle.Route.GetDestinationAt(0).Description);
            Assert.AreEqual(destinationBtwo.Description, aVehicle.Route.GetDestinationAt(1).Description);

            Assert.AreEqual(destinationAone.Description, anotherVehicle.Route.GetDestinationAt(0).Description);
            Assert.AreEqual(destinationAtwo.Description, anotherVehicle.Route.GetDestinationAt(1).Description);
        }

        [TestMethod]
        public void Test_Swaps_two()
        {
            var starPoint = new Destination(0, 0, new Coordinate(0, 0), "start", 0);
            var endPoint = new Destination(0, 0, new Coordinate(10, 0), "end", 0);

            var aVehicle = new Vehicle(1, 100, starPoint, endPoint);
            var anotherVehicle = new Vehicle(2, 100, starPoint, endPoint);

            var destinationAone = new Destination(0, 0, new Coordinate(4, 10), "a1", 0);
            var destinationAtwo = new Destination(0, 0, new Coordinate(6, 10), "a2", 0);

            var destinationBone = new Destination(0, 0, new Coordinate(4, -10), "b1", 0);
            var destinationBtwo = new Destination(0, 0, new Coordinate(6, -10), "b2", 0);

            aVehicle.Route.AddDestination(destinationAone);
            aVehicle.Route.AddDestination(destinationBtwo);

            anotherVehicle.Route.AddDestination(destinationBone);
            anotherVehicle.Route.AddDestination(destinationAtwo);

            var heuristic = new SwapHeuristic();

            // Hace el swap
            Assert.IsTrue(heuristic.Swaps(1, 1, ref aVehicle, ref anotherVehicle));

            Assert.AreEqual(destinationAone.Description, aVehicle.Route.GetDestinationAt(0).Description);
            Assert.AreEqual(destinationAtwo.Description, aVehicle.Route.GetDestinationAt(1).Description);

            Assert.AreEqual(destinationBone.Description, anotherVehicle.Route.GetDestinationAt(0).Description);
            Assert.AreEqual(destinationBtwo.Description, anotherVehicle.Route.GetDestinationAt(1).Description);

            // No hace el swap
            Assert.IsFalse(heuristic.Swaps(1, 1, ref aVehicle, ref anotherVehicle));

            Assert.AreEqual(destinationAone.Description, aVehicle.Route.GetDestinationAt(0).Description);
            Assert.AreEqual(destinationAtwo.Description, aVehicle.Route.GetDestinationAt(1).Description);

            Assert.AreEqual(destinationBone.Description, anotherVehicle.Route.GetDestinationAt(0).Description);
            Assert.AreEqual(destinationBtwo.Description, anotherVehicle.Route.GetDestinationAt(1).Description);
        }

        [TestMethod]
        public void Test_Combinations()
        {
            var heuristic = new SwapHeuristic();

            var combinations = LocalSearchHeuristicHelper.GetCombinationsFor(3);
            Assert.AreEqual(combinations.Count, 3);

            combinations = LocalSearchHeuristicHelper.GetCombinationsFor(5);
            Assert.AreEqual(combinations.Count, 10);
        }


        [TestMethod]
        public void Test_ApplyHeuristic()
        {
            //var starPoint = new Destination(0, 0, new Coordinate(0, 0), "start");
            //var endPoint = new Destination(0, 0, new Coordinate(10, 0), "end");

            //var aVehicle = new Vehicle(1, 100, starPoint, endPoint);
            //var anotherVehicle = new Vehicle(2, 100, starPoint, endPoint);

            //var destinationAone = new Destination(0, 0, new Coordinate(4, 10), "a1");
            //var destinationAtwo = new Destination(0, 0, new Coordinate(6, 10), "a2");

            //var destinationBone = new Destination(0, 0, new Coordinate(4, -10), "b1");
            //var destinationBtwo = new Destination(0, 0, new Coordinate(6, -10), "b2");

            //aVehicle.Route.AddDestination(destinationAone);
            //aVehicle.Route.AddDestination(destinationBtwo);

            //anotherVehicle.Route.AddDestination(destinationBone);
            //anotherVehicle.Route.AddDestination(destinationAtwo);

            //var heuristic = new SwapHeuristic();

            //var problem

            //var encodedSolution = new EncodedSolution(new GreedyVehicleDecoder(),)

            //heuristic.ApplyHeuristic();

            //// Hace el swap
            //Assert.IsTrue(heuristic.Swaps(1, 1, ref aVehicle, ref anotherVehicle));

            //Assert.AreEqual(destinationAone.Description, aVehicle.Route.GetDestinationAt(0).Description);
            //Assert.AreEqual(destinationAtwo.Description, aVehicle.Route.GetDestinationAt(1).Description);

            //Assert.AreEqual(destinationBone.Description, anotherVehicle.Route.GetDestinationAt(0).Description);
            //Assert.AreEqual(destinationBtwo.Description, anotherVehicle.Route.GetDestinationAt(1).Description);
        }
    }
}
