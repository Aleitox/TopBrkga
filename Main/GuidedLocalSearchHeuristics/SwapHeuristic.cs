using System.Collections.Generic;
using Main.BrkgaTop;
using Main.BrkgaTop.Encoders;
using Main.Model;
using System;

namespace Main.GuidedLocalSearchHeuristics
{
    public class SwapHeuristic : ILocalSearchHeuristic
    {
        public void ApplyHeuristic(ref EncodedSolution encodedSolution)
        {
            var solution = encodedSolution.GetSolution;
            // TODO necesita permutaciones 1,1 y 2,1 que GetCombinationsFor no genera
            var combinations = LocalSearchHeuristicHelper.GetCombinationsFor(solution.VehicleFleet.Vehicles.Count);
            foreach (var combination in combinations)
            {
                // TODO: Test que se modifican en la solucion final
                SwapDestinationsBetween(solution.VehicleFleet.GetRoute(combination.Item1), solution.VehicleFleet.GetRoute(combination.Item2));
            }

            encodedSolution = Encoder.UpdateEncodedSolution(encodedSolution, solution.GetCurrentRoutes);
        }

        // TODO: Alternativa sin Banns
        public void SwapDestinationsBetween(Vehicle leftRoute, Vehicle rightRoute)
        {
            var bannedLeftDestinations = new Dictionary<int, bool>();
            var bannedRightDestinations = new Dictionary<int, bool>();

            for (var i = 0; i < leftRoute.Route.RouteLenght(); i++)
            {
                if(bannedLeftDestinations.ContainsKey(i)) continue;

                for (var j = 0; j < rightRoute.Route.RouteLenght(); j++)
                {
                    if (bannedLeftDestinations.ContainsKey(j)) continue;

                    if (!Swaps(i, j, ref leftRoute, ref rightRoute)) continue;

                    if (!bannedLeftDestinations.ContainsKey(i))
                        bannedLeftDestinations.Add(i, true);
                    else
                    {
                        var juan = 1;
                    }
                    if (!bannedRightDestinations.ContainsKey(j))
                        bannedRightDestinations.Add(j, true);
                    else
                    {
                        var juan = 1;
                    }
                    break;
                }
            }
        }

        public bool Swaps(int i, int j, ref Vehicle leftVehicle, ref Vehicle rightVehicle)
        {
            
            var leftCurrentTracks = leftVehicle.Route.GetTracksForDestinationAt(i);
            var rightCurrentTracks = rightVehicle.Route.GetTracksForDestinationAt(j);

            var leftCurrentCosts = leftCurrentTracks.Item1.From.GetDistanceTo(leftCurrentTracks.Item1.To);
            leftCurrentCosts += leftCurrentTracks.Item2.From.GetDistanceTo(leftCurrentTracks.Item2.To);
            var rightCurrentCosts = rightCurrentTracks.Item1.From.GetDistanceTo(rightCurrentTracks.Item1.To);
            rightCurrentCosts += rightCurrentTracks.Item2.From.GetDistanceTo(rightCurrentTracks.Item2.To);

            var leftSwapedCosts = leftCurrentTracks.Item1.From.GetDistanceTo(rightCurrentTracks.Item1.To);
            leftSwapedCosts += rightCurrentTracks.Item2.From.GetDistanceTo(leftCurrentTracks.Item2.To);
            var rightSwapedCosts = rightCurrentTracks.Item1.From.GetDistanceTo(leftCurrentTracks.Item1.To);
            rightSwapedCosts += leftCurrentTracks.Item2.From.GetDistanceTo(rightCurrentTracks.Item2.To);

            var swaps = (leftCurrentCosts + rightCurrentCosts > leftSwapedCosts + rightSwapedCosts) &&
                        leftVehicle.Route.GetDistance() + leftSwapedCosts - leftCurrentCosts <= leftVehicle.MaxDistance &&
                        rightVehicle.Route.GetDistance() + rightSwapedCosts - rightCurrentCosts <= rightVehicle.MaxDistance;

            if (!swaps) return false;

            leftVehicle.Route.RemoveDestinationAt(i);
            rightVehicle.Route.RemoveDestinationAt(j);

            leftVehicle.Route.AddDestinationAt(rightCurrentTracks.Item1.To, i);
            rightVehicle.Route.AddDestinationAt(leftCurrentTracks.Item1.To, j);

            return true;
        }
    }
}
