using Main.Entities;
using System.Data.Entity;
using System.Linq;

namespace Main.Repositories
{
    public class SolutionRepository : Repository<Solution>
    {
        public VehicleRepository VehicleRepository { get; set; }

        public SolutionRepository(DbContext dataContext) : base(dataContext)
        {
            VehicleRepository = new VehicleRepository(dataContext);
        }

        public void SaveSolution(Model.Solution modelSolution)
        {
            if (!ValidateModelSolution(modelSolution))
                return;

            var solution = TryGetPreviousSolution(modelSolution);
            
            SaveChanges();

            modelSolution.Id = solution.Id;

            if (modelSolution.Id == 0)
                modelSolution.Id = solution.Id;
            else
            {
                var vehicles = VehicleRepository.GetAll().Where(v => v.SolutionId == modelSolution.Id).ToList();
                foreach (var t in vehicles)
                    VehicleRepository.Delete(t);

                foreach (var vehicle in modelSolution.VehicleFleet.Vehicles)
                    vehicle.Id = 0;
                
                SaveChanges();
            }

            foreach (var vehicle in modelSolution.VehicleFleet.Vehicles)
                VehicleRepository.SaveVehicle(vehicle, modelSolution);
        }

        private Solution TryGetPreviousSolution(Model.Solution modelSolution)
        {
            if (modelSolution.Id != 0 || GetAll().Any(s => s.InstanceId == modelSolution.InstanceId && s.Name == modelSolution.Name))
                return UpdateSolution(modelSolution);

            return CreateNewSolution(modelSolution);
        }

        private Solution UpdateSolution(Model.Solution modelSolution)
        {
            var solution = GetById(modelSolution.Id) ?? GetAll().First(s => s.InstanceId == modelSolution.InstanceId && s.Name == modelSolution.Name);
            solution.InstanceId = modelSolution.InstanceId;
            solution.Name = modelSolution.Name;
            solution.Generation = modelSolution.Generation;
            solution.FatherId = modelSolution.FatherId;
            solution.MotherId = modelSolution.MotherId;
            solution.Mutant = modelSolution.Mutant;
            solution.BestInGeneration = modelSolution.BestInGeneration;
            solution.BestOfAll = modelSolution.BestOfAll;
            solution.LastGeneration = modelSolution.IsFinal;
            solution.TimeElapsedInMilliseconds = modelSolution.TimeElapsedInMilliseconds;
            return solution;
        }

        private Solution CreateNewSolution(Model.Solution modelSolution)
        {
            var solution = new Solution();
            solution.InstanceId = modelSolution.InstanceId;
            solution.Name = modelSolution.Name;
            solution.Generation = modelSolution.Generation;
            solution.FatherId = modelSolution.FatherId;
            solution.MotherId = modelSolution.MotherId;
            solution.Mutant = modelSolution.Mutant;
            solution.BestInGeneration = modelSolution.BestInGeneration;
            solution.BestOfAll = modelSolution.BestOfAll;
            solution.TimeElapsedInMilliseconds = modelSolution.TimeElapsedInMilliseconds;
            Insert(solution);
            return solution;
        }

        private bool ValidateModelSolution(Model.Solution modelSolution)
        {
            return modelSolution.InstanceId != 0;
        }
    }
}
