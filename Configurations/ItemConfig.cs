using HeadlessCore.Characters;
using HeadlessCore.Items;

namespace HeadlessCore.Configurations
{
    public class ItemConfig
    {
        public string Id { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string Type { get; init; } = "Default";
        public int Count { get; init; } = 1;
        public int MaxStackSize { get; init; } = 1;
        public string? ActionId { get; init; }
        public EquipmentSlot Slot { get; init; }
        public StatsConfig BonusStats { get; init; } = new();
    }
}