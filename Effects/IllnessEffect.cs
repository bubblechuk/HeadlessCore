using HeadlessCore.Actions;

namespace HeadlessCore.Effects
{
    [Flags]
    public enum SuppressionType
    {
        None = 0,
        Actions = 1 << 0,
        Items = 1 << 1,
        All = Actions | Items
    }
    internal class IllnessEffect : CStatusEffect
    {
        public float BlockChance { get; }
        public HashSet<string> BlockedTags { get; }
        public SuppressionType TargetType { get; }

        private static readonly Random _rng = new();
        public IllnessEffect(
        string id,
        int durationTurns,
        float blockChance,
        IEnumerable<string> blockedTags,
        SuppressionType targetType = SuppressionType.Actions)
        {
            Id = id;
            RemainingTurns = durationTurns;
            BlockChance = Math.Clamp(blockChance, 0f, 1f);
            BlockedTags = new HashSet<string>(blockedTags, StringComparer.OrdinalIgnoreCase);
            TargetType = targetType;
        }

        public override bool ShouldSuppressAction(CBaseAction action)
        {
            if (!TargetType.HasFlag(SuppressionType.Actions)) return false;

            bool tagMatches = BlockedTags.Count == 0 || BlockedTags.Contains(action.Id);
            return tagMatches && _rng.NextDouble() < BlockChance;
        }

        public override bool ShouldSuppressItem(object item)
        {
            if (!TargetType.HasFlag(SuppressionType.Items)) return false;

            return _rng.NextDouble() < BlockChance;
        }
        public override void OnTurnTick(ITargetable target)
        {
            if (RemainingTurns < 0) return;
            RemainingTurns--;
            if (RemainingTurns == 0)
            {
                IsExpired = true;
                return;
            }
        }
    }
}
