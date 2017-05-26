
using System.Runtime.Remoting.Services;
using Main.Helpers;

namespace Main.Model
{
    public class Tract
    {
        public Tract()
        {
        }


        public Tract(Destination from, Destination to)
        {
            From = from;
            To = to;
        }

        public Destination From { get; set; }

        public Destination To { get; set; }

        public decimal GetDistance()
        {
            return EuclidianCalculator.GetDistanceBetween(From, To);
        }
    }

    public class DestinationNeighbors
    {
        public DestinationNeighbors(Destination from, Destination destination, Destination to)
        {
            From = from;
            Destination = destination;
            To = to;
        }

        public Destination From { get; set; }

        public Destination Destination { get; set; }

        public Destination To { get; set; }
    }
}
