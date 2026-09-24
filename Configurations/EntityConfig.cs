namespace HeadlessCore.Configurations
{
    public class EntityConfig
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Nickname { get; set; } = string.Empty;
        public string BrainTypeId { get; set; } = string.Empty;
        public int StartingLevel { get; set; } = 1;
        public StatsConfig BaseStats { get; set; } = new();
        public List<ItemConfig> Inventory { get; init; } = new();
        public List<string> Equipment { get; init; } = new();
        public List<string> ActionIds { get; set; } = new();
    }
}
