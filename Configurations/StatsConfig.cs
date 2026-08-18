using HeadlessCore.Characters;

namespace HeadlessCore.Configurations
{
    public class StatsConfig
    {
        public int Caliber { get; set; }
        public int Spirituality { get; set; }
        public int Tinkering { get; set; }
        public int Resonance { get; set; }
        public int Willpower { get; set; }
        public Stats ToDomainStats()
        {
            return new Stats(Caliber, Spirituality, Tinkering, Resonance, Willpower);
        }
    }
}
