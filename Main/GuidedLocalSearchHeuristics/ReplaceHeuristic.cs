﻿using System.Collections.Generic;
using System.Linq;
using Main.BrkgaTop;
using Main.Model;
using System;
using Main.BrkgaTop.Encoders;

namespace Main.GuidedLocalSearchHeuristics
{
    public class ReplaceHeuristic : ILocalSearchHeuristic
    {
        public bool Acotado { get; set; }
        public int Cota { get; set; }

        public static ReplaceHeuristic GetSuper(int cota = 0)
        {
            var replace = new ReplaceHeuristic();
            replace.SuperReplace = true;
            if (cota != 0)
            {
                replace.Acotado = true;
                replace.Cota = cota;
            }
            return replace;
        }
        public static ReplaceHeuristic GetNormal()
        {
            return new ReplaceHeuristic() { SuperReplace = false, Acotado = false };
        }

        public bool SuperReplace { get; set; }
        // Necesito: 
        // Los que estan afuera (ordenados por que tan copados son)
        // Los que estan adentro (ordenados por que tan copados son)
        // Poder busar posicion ideal
        // Hacer el swap. El swap debe reflejarse en el codigo que. Es decir que cuando decodifique, debe quedar la solucion mejorada
        public void ApplyHeuristic(ref EncodedSolution encodedSolution)
        {
            var solution = encodedSolution.GetSolution;

            var unvisited = solution.GetCurrentUnvistedDestination;
            var vehicles = solution.VehicleFleet.Vehicles;

            foreach (var vehicle in vehicles)
            {
                var changed = ReplaceIfEnabled(solution, vehicle, unvisited);
                if(changed)
                    unvisited = solution.GetCurrentUnvistedDestination;
            }
        }

        private bool ReplaceIfEnabled(Solution solution, Vehicle vehicle, List<Destination> unvisited)
        {
            var changed = false;
            vehicle.Route.ActivateCog();
            // TODO, test this order by
            unvisited = unvisited.OrderBy(x => vehicle.Route.CenterOfGravity.GetDistanceToCog(x)).ToList();

            foreach (var destination in unvisited)
            {
                var analizedInsertion = LocalSearchHeuristicHelper.AnalizeInsert(solution, vehicle, destination);
                vehicle.Route.AddDestinationAt(destination, analizedInsertion.BestInsertPosition);
                if (!analizedInsertion.CanBeInserted)
                {
                    var destinationAt = new DestinationAt(destination, analizedInsertion.BestInsertPosition);
                    var justChange = SuperReplace ?
                        LocalSearchHeuristicHelper.RemoveWorstTeamOrDefault(vehicle, destinationAt, Acotado, Cota) :
                        LocalSearchHeuristicHelper.RemoveWorstOrDefault(vehicle, destinationAt);
                    changed = changed || justChange;
                }
                else
                    changed = true;
            }
            return changed;
        }

        public override string ToString()
        {
            return this.GetType().Name;
        }  
    }
}
