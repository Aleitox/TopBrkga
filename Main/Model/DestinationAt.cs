using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Model
{
    public class DestinationAt
    {
        public DestinationAt(Destination destination, int at)
        {
            Destination = destination;
            At = at;
        }

        public Destination Destination { get; set; }

        public int At { get; set; }
    }

    public class SetOfDestinationAt
    {
        public SetOfDestinationAt()
        {
            AcumProfit = 0;
            DestinationsAt = new List<DestinationAt>();
        }

        public SetOfDestinationAt(DestinationAt destinationAt)
        {
            AcumProfit = destinationAt.Destination.Profit;
            DestinationsAt = new List<DestinationAt>() { destinationAt };
        }

        public List<DestinationAt> DestinationsAt { get; set; }

        public int AcumProfit { get; set; }

        public void AddDestinationAt(DestinationAt destinationAt)
        {
            DestinationsAt.Add(destinationAt);
            AcumProfit += destinationAt.Destination.Profit;
        }

        public static SetOfDestinationAt Clone(SetOfDestinationAt setOfDestinationAt)
        {
            var clone = new SetOfDestinationAt();
            foreach (var destinationAt in setOfDestinationAt.DestinationsAt)
                clone.AddDestinationAt(destinationAt);
            return clone;
        }
    }
}
