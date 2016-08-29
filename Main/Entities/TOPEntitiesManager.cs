using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Entities
{
    public static class TopEntitiesManager
    {
        private static TOPEntities Context { get; set; }

        public static TOPEntities GetContext()
        {
            return Context ?? (Context = new TOPEntities());
        }
    }
}
