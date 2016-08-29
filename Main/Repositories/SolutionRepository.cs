using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Main.Entities;

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

            var solution = modelSolution.Id != 0 ? UpdateSolution(modelSolution) : CreateNewSolution(modelSolution);
            
            SaveChanges();

            if (modelSolution.Id == 0)
                modelSolution.Id = solution.Id;
            else
            {
                foreach(var vehicle in VehicleRepository.GetAll().Where(v => v.SolutionId == modelSolution.Id && v.SolutionStep == modelSolution.Step))
                    VehicleRepository.Delete(vehicle);
            }

            SaveChanges();

            foreach (var vehicle in modelSolution.VehicleFleet.Vehicles)
                VehicleRepository.SaveVehicle(vehicle, modelSolution);
        }

        private Solution UpdateSolution(Model.Solution modelSolution)
        {
            var solution = GetById(modelSolution.Id);
            solution.InstanceId = modelSolution.InstanceId;
            solution.Name = modelSolution.Name;
            return solution;
        }

        private Solution CreateNewSolution(Model.Solution modelSolution)
        {
            var solution = new Solution()
            {
                InstanceId = modelSolution.InstanceId,
                Name = modelSolution.Name,
            };
            Insert(solution);
            return solution;
        }

        private bool ValidateModelSolution(Model.Solution modelSolution)
        {
            return modelSolution.InstanceId != 0 && !string.IsNullOrEmpty(modelSolution.Name);
        }
    }
}
