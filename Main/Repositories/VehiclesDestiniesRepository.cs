using Main.Entities;
using System.Data.Entity;
using Main.Model;

namespace Main.Repositories
{
    public class VehiclesDestiniesRepository : Repository<VehiclesDestiny>
    {
        public VehiclesDestiniesRepository(DbContext dataContext) : base(dataContext)
        {
        }

        public void SaveDestiny(Destination destination, Model.Vehicle modelVehicle, int order)
        {
            var vehiclesDestiny = new VehiclesDestiny()
            {
                DestinyId = destination.Id,
                Order = order,
                VehicleId = modelVehicle.Id
            };
            Insert(vehiclesDestiny);
            SaveChanges();
        }
    }
}
