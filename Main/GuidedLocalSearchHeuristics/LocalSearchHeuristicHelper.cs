﻿using System.Collections.Generic;
using System.Linq;
using Main.Helpers;
using Main.Model;
using System;

namespace Main.GuidedLocalSearchHeuristics
{
    public static class LocalSearchHeuristicHelper
    {
        public static void ApplyHeuristics(List<ILocalSearchHeuristic> heuristics, ref Main.BrkgaTop.EncodedSolution encodedSolution)
        {
            foreach (var heuristic in heuristics)
                heuristic.ApplyHeuristic(ref encodedSolution);

            encodedSolution = BrkgaTop.Encoders.Encoder.UpdateEncodedSolution(encodedSolution, encodedSolution.GetSolution.GetCurrentRoutes);
        }

        public static PreInsertAnalisis AnalizeInsert(Model.Solution solution, Model.Vehicle vehicle, Destination destination)
        {
            var currentDistanceCost = vehicle.Route.GetDistance();
            // NOTA: currentRouteLenght se usa para TimeBudggeAvg. Si no se visitan clientes, esta bien que sea 1 ya que seria los tracks total
            var currentTracks = vehicle.Route.RouteLenght() + 1;
            var currentDistanceAvg = currentDistanceCost / currentTracks;

            var tracks = vehicle.Route.GetCurrentTracks();

            var bestPosition = -1;
            var minimumInsertCost = decimal.MaxValue;

            for (var index = 0; index < tracks.Count; index++)
            {
                var trackDistance = solution.Map.GetDistance(tracks[index].From, tracks[index].To);
                var distanceFromTrackStartTonNewDest = solution.Map.GetDistance(tracks[index].From, destination);
                var distanceFromNewDestToTrackEnd = solution.Map.GetDistance(destination, tracks[index].To);

                var insertCost = distanceFromTrackStartTonNewDest + distanceFromNewDestToTrackEnd - trackDistance; // TODO: rename insertCost
                if (minimumInsertCost > insertCost)
                {
                    minimumInsertCost = insertCost;
                    bestPosition = index;
                }

            }
            var preInsertAnalisis = new PreInsertAnalisis();
            preInsertAnalisis.BestInsertPosition = bestPosition;
            preInsertAnalisis.NewTimeBudgetAvg = (currentDistanceCost + minimumInsertCost) / (currentTracks + 1);
            preInsertAnalisis.CurrentTimeBudgetAvg = currentDistanceAvg;
            preInsertAnalisis.CanBeInserted = vehicle.MaxDistance >= currentDistanceCost + minimumInsertCost;

            return preInsertAnalisis;
        }

        // TODO Test
        public static Destination RemoveWorstOrDefault(Vehicle vehicle, Destination defaultDestination)
        {
            var destinationsNeighbors = vehicle.Route.GetAllDestinationNeighbors();

            decimal maxDistanceSaved = 0;
            var currentProfitToLose = 0;
            var destinationIdToRemove = defaultDestination.Id;

            foreach (var destinationNeighbors in destinationsNeighbors)
            {
                if (destinationNeighbors.Destination.Profit > defaultDestination.Profit)
                    continue;

                var distanceSaved =
                    EuclidianCalculator.GetDistanceBetween(destinationNeighbors.From, destinationNeighbors.Destination) +
                    EuclidianCalculator.GetDistanceBetween(destinationNeighbors.Destination, destinationNeighbors.To) -
                    EuclidianCalculator.GetDistanceBetween(destinationNeighbors.From, destinationNeighbors.To);

                if (distanceSaved > maxDistanceSaved || (distanceSaved == maxDistanceSaved && destinationNeighbors.Destination.Profit < currentProfitToLose))
                {
                    destinationIdToRemove = destinationNeighbors.Destination.Id;
                    currentProfitToLose = destinationNeighbors.Destination.Profit;
                    maxDistanceSaved = distanceSaved;
                }
            }

            var destinationRemoved = destinationsNeighbors.First(x => x.Destination.Id == destinationIdToRemove).Destination;
            vehicle.Route.RemoveDestination(destinationRemoved);

            return destinationRemoved;
        }


        public static List<Tuple<int, int>> GetCombinationsFor(int count)
        {
            var combinations = new List<Tuple<int, int>>();

            for (var i = 1; i <= count; i++)
            {
                for (var j = i + 1; j <= count; j++)
                {
                    combinations.Add(new Tuple<int, int>(i, j));
                }
            }
            return combinations;
        }
    }
}
