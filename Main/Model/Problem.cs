
using System.Collections.Generic;
using System.Linq;

namespace Main.Model
{
    public class Problem
    {
        public Problem(IMap map, IVehicleFleet vehicleFleet)
        {
            Map = map;
            VehicleFleet = vehicleFleet;
        }

        public IMap Map { get; set; }

        public IVehicleFleet VehicleFleet { get; set; }

        public List<Route> CurrentSolution 
        {
            get {return VehicleFleet.Vehicles.Select(v => v.Route).ToList(); }
        }

        public bool IsSolution
        {
            get { return VehicleFleet.Vehicles.All(v => v.MaxDistance >= v.Route.GetDistance(Map)); }
        }
    }
}
