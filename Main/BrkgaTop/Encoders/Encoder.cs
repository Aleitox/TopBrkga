using Main.Model;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Main.BrkgaTop.Encoders
{
    public class Encoder
    {
        public static EncodedSolution UpdateEncodedSolution(EncodedSolution encodedSolution, List<Route> newRoutes)
        {
            var keys = encodedSolution.GetOrderedRandomKeys().Select(k => k.Key).ToList();
            var positionIndexes = new List<int>();
            foreach (var route in newRoutes)
                positionIndexes.AddRange(route.GetDestinations.Select(d => d.PositionIndex));
            var unvistedPositionIndexes = GetUnvisited(encodedSolution.GetOrderedRandomKeys(), newRoutes); // TODO REVISAR parecen estar todas, no solo los univisted
            positionIndexes.AddRange(unvistedPositionIndexes);

            var breaks = new Queue(newRoutes.Select(r => r.GetDestinations.Count).ToList());

            var newRandomKeys = new List<RandomKey>();
            var forceVehicleChangeAfterThis = false;
            var acumBreak = 0;
            for (var index = 0; index < keys.Count; index++)
            {
                if (breaks.Count > 0)
                {
                    forceVehicleChangeAfterThis = index + 1 == (int)breaks.Peek() + acumBreak;
                    if (forceVehicleChangeAfterThis)
                        acumBreak += (int)breaks.Dequeue();
                }

                var randomKey = new RandomKey(keys[index], positionIndexes[index], forceVehicleChangeAfterThis);
                newRandomKeys.Add(randomKey);
            }
            encodedSolution.SetRandomKeys(newRandomKeys);
            return encodedSolution;
        }

        private static List<int> GetUnvisited(List<RandomKey> randomKeys, List<Route> newRoutes)
        {
            var unvisited = new List<int>();

            foreach (var randomKey in randomKeys)
            {
                if(newRoutes.All(r => r.GetDestinations.All(d => d.PositionIndex != randomKey.PositionIndex)))
                    unvisited.Add(randomKey.PositionIndex);
            }

            return unvisited;
        }
    }
}
