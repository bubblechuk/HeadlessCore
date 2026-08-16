using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadlessCore.Configurations
{
    public class EntityConfig
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Nickname { get; set; } = string.Empty;
        public int StartingLevel { get; set; } = 1;
        public StatsConfig BaseStats { get; set; } = new();
        public List<string> ActionIds { get; set; } = new();
    }
}
