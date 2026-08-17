using HeadlessCore.Characters;
using HeadlessCore.Effects;

namespace HeadlessCore.Configurations
{
    public class EffectConfig
    {
        public string Id { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int Turns { get; set; } = -1;
        public StatsConfig? StatModifier { get; set; }
        public int HpDrain { get; set; }
        public int SpDrain { get; set; }
        public int XpDrain { get; set; }
        public float BlockChance { get; set; }
        public List<string> BlockedTags { get; set; } = new();
        public SuppressionType TargetType { get; set; } = SuppressionType.Actions;
        public CStatusEffect ToDomain()
        {
            return Type.ToLowerInvariant() switch
            {
                "statmodifier" or "stat_modifier" => new StatModifierEffect(
                    Id,
                    StatModifier?.ToDomainStats() ?? Stats.Zero,
                    Turns
                ),

                "poison" => new PoisonEffect(
                    Id,
                    Turns,
                    HpDrain,
                    SpDrain,
                    XpDrain
                ),

                "illness" => new IllnessEffect(
                    Id,
                    Turns,
                    BlockChance,
                    BlockedTags,
                    TargetType
                ),

                _ => throw new ArgumentOutOfRangeException(nameof(Type), $"Неизвестный тип эффекта: '{Type}'")
            };
        }
    }
}
