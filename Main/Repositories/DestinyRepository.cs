using Main.Entities;
using Main.Model;
using System.Data.Entity;

namespace Main.Repositories
{
    public class DestinyRepository : Repository<Destiny>
    {
        public DestinyRepository(DbContext dataContext) : base(dataContext)
        {
        }

        public void SaveDestiny(Destination destination, Instance instance)
        {
            var destiny = new Destiny
            {
                Instance = instance,
                Profit = destination.Profit,
                X = destination.Coordinate.X,
                Y = destination.Coordinate.Y,
                Description = destination.Description
            };
            Insert(destiny);
            SaveChanges();
        }
    }
}
