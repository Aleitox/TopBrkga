
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


        public IVehicleFleet GetFreshVehicleFleet()
        {
            var vehicleFleet = new VehicleFleet();
            foreach (var vehicle in VehicleFleet.Vehicles)
                vehicleFleet.Vehicles.Add(new Vehicle(vehicle.Id, vehicle.MaxDistance, vehicle.Route.Depot));
            return vehicleFleet;
        }

        public Problem GetFreshProblem()
        {
            return new Problem(Map, GetFreshVehicleFleet());
        }

        public int GetAmountOfDestinations()
        {
            return Map.Destinations.Count;
        }
    }
}
