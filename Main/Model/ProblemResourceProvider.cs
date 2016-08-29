
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;

namespace Main.Model
{
    public class ProblemResourceProvider
    {
        public ProblemResourceProvider(IMap map, IVehicleFleet vehicleFleet)
        {
            Map = map;
            VehicleFleet = vehicleFleet;
        }
        
        private IMap Map { get; set; }

        private IVehicleFleet VehicleFleet { get; set; }

        public int GetAmountOfNonProfitDestinations()
        {
            return Map.GetNonProfitDestinations().Count;
        }


        public IVehicleFleet GetFreshVehicleFleet()
        {
            var vehicleFleet = new VehicleFleet();
            short vehicleNumber = 1;
            foreach (var vehicle in VehicleFleet.Vehicles)
            {
                vehicleFleet.Vehicles.Add(new Vehicle(vehicleNumber, vehicle.MaxDistance, vehicle.Route.StartingPoint, vehicle.Route.EndingPoint));
                vehicleNumber++;
            }
            return vehicleFleet;
        }

        public List<Destination> GetDestinations()
        {
            return Map.Destinations;
        }

        public Solution GetFreshProblem()
        {
            return new Solution(Map, GetFreshVehicleFleet());
        }

        public int GetAmountOfDestinations()
        {
            return Map.Destinations.Count;
        }

        public int GetAmountOfVehicles()
        {
            return VehicleFleet.Vehicles.Count;
        }

        public decimal GetVehiclesMaxDistance()
        {
            return VehicleFleet.Vehicles.First().MaxDistance;
        }
    }
}
