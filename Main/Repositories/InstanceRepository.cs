using System;
using System.Linq;
using Main.Entities;
using Main.Model;
using System.Data.Entity;

namespace Main.Repositories
{
    public class InstanceRepository : Repository<Instance>
    {
        public DestinyRepository DestinyRepository { get; set; }
        
        public InstanceRepository(DbContext dataContext) : base(dataContext)
        {
            DestinyRepository = new DestinyRepository(dataContext);
        }

        public Instance GetBy(string author, string set, string name)
        {
            var intances = GetAll().Where(i => i.Author == author && i.Set == set && i.Name == name);
            if(!intances.Any())
                throw new Exception(string.Format("No instance found. Author: {0}; Set: {1}; Name: {2}", author, set, name));
            if(intances.Count() > 1)
                throw new Exception(string.Format("Multiple instances found. Author: {0}; Set: {1}; Name: {2}", author, set, name));

            return intances.First();
        }

        //public Instance GetById(int id)
        //{
        //    var intance = GetAll().FirstOrDefault(i => i.Id == id);

        //    if (intance == null)
        //        throw new Exception(string.Format("No instance found. Id: {0}", id));

        //    return intance;
        //}

        public void SaveInstance(ProblemResourceProvider source, string fileName, string author = null, string set = null)
        {
            var instance = new Instance
            {
                DestiniesCount = source.GetAmountOfDestinations(),
                Name = fileName,
                Vehicles = source.GetAmountOfVehicles(),
                TMax = source.GetVehiclesMaxDistance(),
                Author = author,
                Set = set
            };
            Insert(instance);
            SaveChanges();

            foreach (var destination in source.GetDestinations())
                DestinyRepository.SaveDestiny(destination, instance);
        }
    }
}
