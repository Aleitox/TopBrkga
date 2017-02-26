using System;

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
    }
}
