using Main.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Repositories
{
    public class ExternalSolutionsBdmRepository : Repository<ExternalSolutionsBDM>
    {
        public ExternalSolutionsBdmRepository(DbContext dataContext) : base(dataContext)
        {
        }
    }
}
