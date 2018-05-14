
using System;
using System.Collections.Generic;
using System.Linq;

namespace Main.Model
{
    public class Solution
    {
        public Solution()
        {
            CreationDate = DateTime.Now;
        }

        public Solution(IMap map, IVehicleFleet vehicleFleet, int instanceId, string name)
        {
            Map = map;
            VehicleFleet = vehicleFleet;
            InstanceId = instanceId;
            Name = name;
            CreationDate = DateTime.Now;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int InstanceId { get; set; }

        public int Step { get; set; }

        public bool IsFinal { get; set; }

        public DateTime CreationDate { get; set; }

        public IMap Map { get; set; }

        public IVehicleFleet VehicleFleet { get; set; }

        public List<Route> GetCurrentRoutes 
        {
            get {return VehicleFleet.Vehicles.Select(v => v.Route).ToList(); }
        }

        public double GetCurrentProfit
        {
            get { return VehicleFleet.Vehicles.Select(v => v.Route).ToList().Sum(r => r.GetProfit()); }
        }

        public List<Destination> GetCurrentUnvistedDestination
        {
            get { return Map.GetUnvisitedDestinations(GetCurrentRoutes); }
        }

        public bool IsSolution
        {
            get { return VehicleFleet.Vehicles.All(v => v.MaxDistance >= v.Route.GetDistance()); }
        }

        public int Generation { get; set; }
        public int? FatherId { get; set; }
        public int? MotherId { get; set; }
        public bool Mutant { get; set; }
        public bool BestInGeneration { get; set; }
        public bool BestOfAll { get; set; }
        public long TimeElapsedInMilliseconds { get; set; }
        public int Fase { get; set; }
        public int Run { get; internal set; }
        public string ProfitEvolution { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1} Routes Distances: {2}", PrintProfit(), PrintRouteDistances(), PrintVehicleFleetSate());
        }

        public string PrintProfit()
        {
            return string.Format("Profit: {0}{1} ", GetCurrentProfit, Environment.NewLine);
        }

        public string PrintRouteDistances()
        {
            var problemString = string.Empty;
            decimal distanceAcum = 0;
            
            foreach (var vehicle in VehicleFleet.Vehicles)
            {
                distanceAcum += vehicle.Route.GetDistance();
            }
            problemString += String.Format("{0:0.00}", distanceAcum);

            return problemString;
        }

        public string PrintVehicleFleetSate()
        {
            var problemString = string.Empty;
            foreach (var vehicle in VehicleFleet.Vehicles)
            {
                problemString += string.Format("VehicleId: {0} - Distance: {3} - {1}{2}", vehicle.Id, vehicle.Route.ToString(), Environment.NewLine, vehicle.Route.GetDistance());
            }

            return problemString;
        }

        public string GetHash()
        {
            var orderedVehicles = VehicleFleet.Vehicles.Where(x => x.Route.RouteLenght() > 0).OrderBy(x => x.Route.GetDestinationAt(0).Id);
            var hash = string.Join("#", orderedVehicles.Select(x => x.GetHash()));
            return hash;
        }
    }
}
