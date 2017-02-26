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

        public override void Delete(Vehicle entity)
        {
            var vehicleDestinies = entity.VehiclesDestinies.ToList();
            foreach (var t in vehicleDestinies)
                VehiclesDestiniesRepository.Delete(t);

            base.Delete(entity);
        }

        public void SaveVehicle(Model.Vehicle modelVehicle, Model.Solution modelSolution)
        {
            var vehicle = modelVehicle.Id != 0 ? UpdateVehicle(modelVehicle, modelSolution) : CreateNewVehicle(modelVehicle, modelSolution);

            SaveChanges();

            if (modelVehicle.Id == 0)
                modelVehicle.Id = vehicle.Id;
            else
            {
                var vehicleDestinies = vehicle.VehiclesDestinies.ToList();
                foreach (var t in vehicleDestinies)
                    VehiclesDestiniesRepository.Delete(t);
                
                SaveChanges();
            }
            
            for (var order = 0; order < modelVehicle.Route.RouteLenght() ; order++)
                VehiclesDestiniesRepository.SaveDestiny(modelVehicle.Route.GetDestinationAt(order), modelVehicle, order);
        }

        private Vehicle CreateNewVehicle(Model.Vehicle modelVehicle, Model.Solution modelSolution)
        {
            var vehicle = new Vehicle();
            vehicle.SolutionId = modelSolution.Id;
            vehicle.Number = modelVehicle.Number;
            vehicle.TMax = modelVehicle.MaxDistance;
            vehicle.Distance = modelVehicle.Route.GetDistance(modelSolution.Map);
            vehicle.SumProfit = modelVehicle.Route.GetProfit();
            Insert(vehicle);
            return vehicle;
        }

        private Vehicle UpdateVehicle(Model.Vehicle modelVehicle, Model.Solution modelSolution)
        {
            var vehicle = GetById(modelVehicle.Id);
            vehicle.SolutionId = modelSolution.Id;
            vehicle.Number = modelVehicle.Number;
            vehicle.TMax = modelVehicle.MaxDistance;
            vehicle.Distance = modelVehicle.Route.GetDistance(modelSolution.Map);
            vehicle.SumProfit = modelVehicle.Route.GetProfit();
            return vehicle;
        }
    }
}
