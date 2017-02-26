using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Model
{
    public class BrkgaConfiguration
    {
        public string Description { get; set; }

        public int MinIerations { get; set; }

        public int MinHistoricalChanges { get; set; }

        public int PopulationSize { get; set; }

        public decimal ElitePercentage { get; set; }

        public decimal MutantPercentage { get; set; }

        public int EliteGenChance { get; set; }
    }
}
