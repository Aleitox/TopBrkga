using System;
using System.Linq;
using Main.Entities;
using Main.Model;
using System.Data.Entity;

namespace Main.Repositories
{
    public class ExternalSolutionsRepository : Repository<ExternalSolution>
    {
        public ExternalSolutionsRepository(DbContext dataContext) : base(dataContext)
        {
        }
    }
}
