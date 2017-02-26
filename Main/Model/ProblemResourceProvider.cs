
using System.Collections.Generic;
using System.Linq;

namespace Main.Model
{
    public class ProblemResourceProvider
    {
        public ProblemResourceProvider(IMap map, IVehicleFleet vehicleFleet)
        {
            Map = map;
            VehicleFleet = vehicleFleet;
            InstanceId = 0;
        }

        public ProblemResourceProvider(IMap map, IVehicleFleet vehicleFleet, int instanceId, string solutionName)
        {
            Map = map;
            VehicleFleet = vehicleFleet;
            InstanceId = instanceId;
            SolutionName = solutionName;
        }

        private int InstanceId { get; set; }

        private IMap Map { get; set; }

        private IVehicleFleet VehicleFleet { get; set; }

        private string SolutionName { get; set; }

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
            return new Solution(Map, GetFreshVehicleFleet(), InstanceId, SolutionName);
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
