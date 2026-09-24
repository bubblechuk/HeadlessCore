using HeadlessCore.Actions;

namespace HeadlessCore.Configurations
{
    public class ActionConfig
    {
        public string Id { get; init; } = string.Empty;
        //public string Name { get; init; } = string.Empty;
        //public string Description { get; init; } = string.Empty;
        public int Cost { get; init; }
        public TargetScope Scope { get; init; }
        public DamageType ActionType { get; init; }
        public int BaseValue { get; init; }
        public float StatScaling { get; init; }
        public List<string> EffectIds { get; init; } = new();
    }
}