using HeadlessCore.Actions;
using HeadlessCore.Characters;

namespace HeadlessCore.Effects
{
    public abstract class CStatusEffect
    {
        public string Id { get; protected set; } = string.Empty;
        public bool IsPermanent => RemainingTurns < 0;
        public int RemainingTurns { get; protected set; }
        public virtual Stats GetStatModifier() => Stats.Zero;
        public bool IsExpired = false;
        public virtual void OnApply(ITargetable target) { }
        public virtual void OnRemove(ITargetable target) { }
        public virtual void OnTurnTick(ITargetable target) { }
        public virtual bool ShouldSuppressAction(CBaseAction action) => false;
        public virtual bool ShouldSuppressItem(object item) => false;
    }
}
