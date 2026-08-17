using HeadlessCore.Characters;

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
