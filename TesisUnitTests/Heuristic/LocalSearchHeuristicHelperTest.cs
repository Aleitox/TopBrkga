using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Main.Model;
using Main.GuidedLocalSearchHeuristics;
using System.Collections.Generic;

namespace TesisUnitTests.Heuristic
{
    [TestClass]
    public class LocalSearchHeuristicHelperTest
    {
        [TestMethod]
        public void TestRemoveWorstTeamOrDefault()
        {
            var start = new Destination(0, 0, new Coordinate(0, 0), "Start", 0);
            var primero = new Destination(2, 3, new Coordinate(0, 1), "primero", 0);
            var segundoBorrado = new Destination(3, 2, new Coordinate(3, 5), "segundo borrado", 0);
            var terceroBorrado = new Destination(4, 1, new Coordinate(3, 7), "tercero borrado", 0);
            var agregado = new Destination(5, 10, new Coordinate(-3, 6), "agregado", 0);
            var unltimoMayorProfit = new Destination(6, 11, new Coordinate(0, 11), "ultimo nunca considerado", 0);
            var end = new Destination(1, 0, new Coordinate(0, 12), "End", 0);

            var route = new Route(start, end);

            route.AddDestination(primero);
            route.AddDestination(segundoBorrado);
            route.AddDestination(terceroBorrado);
            route.AddDestination(unltimoMayorProfit);

            var vehicle = new Vehicle(1, 14, route);

            var position = 3;
            vehicle.Route.AddDestinationAt(agregado, position);
            var destinationAt = new DestinationAt(agregado, position);

            var algo = LocalSearchHeuristicHelper.RemoveWorstTeamOrDefault(vehicle, destinationAt);

            Assert.AreEqual(true, algo);
            Assert.AreEqual(24, vehicle.Route.GetProfit());
            Assert.AreEqual(3, vehicle.Route.GetDestinations.Count);
            Assert.AreEqual(2, vehicle.Route.GetDestinationAt(0).Id);
            Assert.AreEqual(5, vehicle.Route.GetDestinationAt(1).Id);
            Assert.AreEqual(6, vehicle.Route.GetDestinationAt(2).Id);

        }

        [TestMethod]
        public void TestGetDistanceWithout()
        {
            var start = new Destination(0, 0, new Coordinate(0, 0), "Start", 0);
            var primero = new Destination(2, 3, new Coordinate(0, 1), "primero", 0);
            var segundoBorrado = new Destination(3, 2, new Coordinate(3, 5), "segundo borrado", 0);
            var terceroBorrado = new Destination(4, 1, new Coordinate(3, 7), "tercero borrado", 0);
            var agregado = new Destination(5, 10, new Coordinate(-3, 6), "agregado", 0);
            var unltimoMayorProfit = new Destination(0, 11, new Coordinate(0, 11), "ultimo nunca considerado", 0);
            var end = new Destination(1, 0, new Coordinate(0, 12), "End", 0);

            var route = new Route(start, end);
            
            route.AddDestination(segundoBorrado);

            var forbidden = new List<int>() { 0 };
            var dist = route.GetDistanceWithout(forbidden);
            Assert.AreEqual(12, dist);

            route.AddDestination(terceroBorrado);

            forbidden = new List<int>() { 0, 1 };
            dist = route.GetDistanceWithout(forbidden);
            Assert.AreEqual(12, dist);

        }

        [TestMethod]
        public void TestGetDistanceWithout2()
        {
            var start = new Destination(0, 0, new Coordinate(0, 0), "Start", 0);
            var one = new Destination(2, 3, new Coordinate(0, 4), "1", 0);
            var two = new Destination(3, 2, new Coordinate(3, 4), "2", 0);
            var end = new Destination(1, 0, new Coordinate(3, 8), "End", 0);

            var route = new Route(start, end);

            route.AddDestination(one);
            route.AddDestination(two);

            var forbidden = new List<int>() {  };
            var dist = route.GetDistanceWithout(forbidden);
            Assert.AreEqual(11, dist);

            forbidden = new List<int>() { 0 };
            dist = route.GetDistanceWithout(forbidden);
            Assert.AreEqual(9, dist);

            forbidden = new List<int>() { 1 };
            dist = route.GetDistanceWithout(forbidden);
            Assert.AreEqual(9, dist);

            forbidden = new List<int>() { 0, 1 };
            dist = route.GetDistanceWithout(forbidden);
            double delta = 0.00001d;
            Assert.AreEqual(8.54400374532d,Convert.ToDouble(dist), delta);

        }
    }
}
