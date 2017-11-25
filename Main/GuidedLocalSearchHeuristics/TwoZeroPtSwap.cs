using System;
using Main.BrkgaTop;
using Main.BrkgaTop.Encoders;
using Main.Model;

namespace Main.GuidedLocalSearchHeuristics
{
    public class TwoZeroPtSwap : ILocalSearchHeuristic
    {
        //https://en.wikipedia.org/wiki/2-opt
        public void ApplyHeuristic(ref EncodedSolution encodedSolution)
        {
            var solution = encodedSolution.GetSolution;

            var index = 0;

            while (index < solution.VehicleFleet.Vehicles.Count)
            {
                var currentDistance = solution.VehicleFleet.Vehicles[index].Route.GetDistance();

                var changed = Do2OptSwap(solution.VehicleFleet.Vehicles[index]);
                if (changed)
                {
                    var swapedDistance = solution.VehicleFleet.Vehicles[index].Route.GetDistance();
                    if (currentDistance <= swapedDistance)
                        throw new Exception("What's up doc?");

                    continue;
                }

                index++;
            }

            encodedSolution = Encoder.UpdateEncodedSolution(encodedSolution, solution.GetCurrentRoutes);
        }

        private bool Do2OptSwap(Vehicle vehicle)
        {
            var changed = false;
            var combinations = LocalSearchHeuristicHelper.GetCombinationsFor(vehicle.Route.RouteLenght());
            // TODO ojo que aca cambi recientemente
            //foreach (var combination in combinations)
            //{
            //    var swaped = vehicle.Route.SwapIfImprovesDistance(combination.Item1 - 1, combination.Item2 - 1);
            //    if (!swaped)
            //        continue;

            //    return true;
            //}
            var index = 0;
            while ( index < combinations.Count)
            {
                var swaped = vehicle.Route.SwapIfImprovesDistance(combinations[index].Item1 - 1, combinations[index].Item2 - 1);
                if (swaped)
                {
                    index = 0;
                    changed = true;
                }
                else
                    index++;
            }
            return changed;
        }
    }
}
