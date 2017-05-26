using System;
using Main.Model;

namespace Main.Helpers
{
    public static class EuclidianCalculator
    {
        public static decimal GetDistanceBetween(decimal x1, decimal y1, decimal x2, decimal y2)
        {
            var xDiff = x1 - x2;
            var yDiff = y1 - y2;
            return Convert.ToDecimal(Math.Sqrt((double)(xDiff * xDiff + yDiff * yDiff)));
        }

        public static decimal GetDistanceBetween(Destination from, Destination to)
        {
            return GetDistanceBetween(from.Coordinate.X, from.Coordinate.Y, to.Coordinate.X, to.Coordinate.Y);
        }
    }
}
