
using System;
using System.Collections.Generic;
using System.Linq;

namespace Main.Model
{
    public class Solution
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int InstanceId { get; set; }

        public int Step { get; set; }

        public bool IsFinal { get; set; }

        public Solution(IMap map, IVehicleFleet vehicleFleet, int instanceId, string name)
        {
            Map = map;
            VehicleFleet = vehicleFleet;
            InstanceId = instanceId;
            Name = name;
        }

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

        public override string ToString()
        {
            return string.Format("{0}{1}", PrintProfit(), PrintVehicleFleetSate());
        }

        public string PrintProfit()
        {
            return string.Format("Profit: {0}{1}", GetCurrentProfit, Environment.NewLine);
        }

        public string PrintVehicleFleetSate()
        {
            var problemString = string.Empty;
            foreach (var vehicle in VehicleFleet.Vehicles)
            {
                problemString += string.Format("VehicleId: {0} - {1}{2}", vehicle.Id, vehicle.Route.ToString(), Environment.NewLine);
            }

            return problemString;
        }
    }
}
