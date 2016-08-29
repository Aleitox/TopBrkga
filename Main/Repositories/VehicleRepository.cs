using Main.Entities;
using System;
using System.Data.Entity;
using System.Linq;

namespace Main.Repositories
{
    public class VehicleRepository : Repository<Vehicle>
    {
        public VehiclesDestiniesRepository VehiclesDestiniesRepository { get; set; }

        public VehicleRepository(DbContext dataContext) : base(dataContext)
        {
            VehiclesDestiniesRepository = new VehiclesDestiniesRepository(dataContext);
        }

        public void SaveVehicle(Model.Vehicle modelVehicle, Model.Solution modelSolution)
        {
            var vehicle = GetAll().FirstOrDefault(v => v.SolutionId == modelSolution.Id && v.SolutionStep == modelSolution.Step && v.Number == modelVehicle.Number) ??
                          new Vehicle()
                                {
                                    Distance = modelVehicle.Route.GetDistance(modelSolution.Map),
                                    SolutionId = modelSolution.Id,
                                    IsFinalSolution = modelSolution.IsFinal,
                                    SolutionStep = modelSolution.Step,
                                    SumProfit = Convert.ToInt32(modelVehicle.Route.GetProfit()),
                                    TMax = modelVehicle.MaxDistance,
                                };

            if (vehicle.Id == 0)
                Insert(vehicle);
            else
            {
                vehicle.Distance = modelVehicle.Route.GetDistance(modelSolution.Map);
                vehicle.SolutionId = modelSolution.Id;
                vehicle.IsFinalSolution = modelSolution.IsFinal;
                vehicle.SolutionStep = modelSolution.Step;
                vehicle.SumProfit = Convert.ToInt32(modelVehicle.Route.GetProfit());
                vehicle.TMax = modelVehicle.MaxDistance;

                foreach (var vehicleDestinies in vehicle.VehiclesDestinies)
                {
                    VehiclesDestiniesRepository.Delete(vehicleDestinies);
                }
            }

            SaveChanges();

            for (var order = 0; order <  modelVehicle.Route.Destinations.Count; order++)
                VehiclesDestiniesRepository.SaveDestiny(modelVehicle.Route.Destinations[order], modelVehicle, order);
        }
    }
}
