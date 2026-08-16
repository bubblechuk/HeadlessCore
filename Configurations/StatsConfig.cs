using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadlessCore.Configurations
{
    public class StatsConfig
    {
        public int Vitality { get; set; }
        public int Willpower { get; set; }
        public int Caliber { get; set; }
        public int Technicality { get; set; }
        public int Speed { get; set; }
        public Stats ToDomainStats()
        {
            return new Stats(Vitality, Willpower, Caliber, Technicality, Speed);
        }
    }
}
