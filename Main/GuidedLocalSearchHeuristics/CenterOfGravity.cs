using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Main.Helpers;
using Main.Model;

namespace Main.GuidedLocalSearchHeuristics
{
    public class CenterOfGravity
    {
        // TODO TESTS
        // 1. Xcog Ycog para puntos obvios
        // 2. 

        public CenterOfGravity(List<Destination> includedLocations)
        {
            XcogUpperSum = 0;
            YcogUpperSum = 0;
            RouteProfitSum = 0;
            includedLocations.ForEach(d =>
            {
                XcogUpperSum += d.Coordinate.X * d.Profit;
                YcogUpperSum += d.Coordinate.Y * d.Profit;
                RouteProfitSum += d.Profit;
            });
            LocationsOrderedByA = includedLocations;
            UpdateValues();
        }

        protected List<Destination> LocationsOrderedByA { get; set; }
        
        public decimal Xcog { get; protected set; }

        public decimal Ycog { get; protected set; }

        public decimal GetDistanceToCog(Destination destination)
        {
            return EuclidianCalculator.GetDistanceBetween(Xcog, Ycog, destination.Coordinate.X, destination.Coordinate.Y);
        }

        public void AddDestination(Destination destination)
        {
            XcogUpperSum += destination.Coordinate.X * destination.Profit;
            YcogUpperSum += destination.Coordinate.Y * destination.Profit;
            RouteProfitSum += destination.Profit;

            LocationsOrderedByA.Add(destination);

            UpdateValues();
        }

        public void RemoveDestination(Destination destination)
        {
            var count = LocationsOrderedByA.Count;

            LocationsOrderedByA = LocationsOrderedByA.Where(x => x.Id != destination.Id).ToList();

            if (count == LocationsOrderedByA.Count)
                return;

            XcogUpperSum -= destination.Coordinate.X * destination.Profit;
            YcogUpperSum -= destination.Coordinate.Y * destination.Profit;
            RouteProfitSum -= destination.Profit;

            UpdateValues();
        }

        private void UpdateValues()
        {
            Xcog = XcogUpperSum / RouteProfitSum;
            Ycog = YcogUpperSum / RouteProfitSum;
            LocationsOrderedByA = LocationsOrderedByA.OrderBy(GetDistanceToCog).ToList();
        }

        protected decimal XcogUpperSum { get; set; }
        protected decimal RouteProfitSum { get; set; }
        protected decimal YcogUpperSum { get; set; }

        //public decimal Get
    }
}
