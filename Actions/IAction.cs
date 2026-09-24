using HeadlessCore.Effects;

namespace HeadlessCore.Actions
{
    public interface IAction
    {
        string Id { get; }
        //string Name { get; }
        //string Description { get; }
        int Cost { get; }
        TargetScope Scope { get; }
        DamageType ActionType { get; }
        IReadOnlyList<CStatusEffect> Effects { get; }

        bool CanCast(ITargetable caster, ITargetable target, bool ignoreCost = false)
            => CanCast(caster, new[] { target }, ignoreCost);

        void Cast(ITargetable caster, ITargetable target, bool ignoreCost = false)
            => Cast(caster, new[] { target }, ignoreCost);

        bool CanCast(ITargetable caster, IReadOnlyList<ITargetable> targets, bool ignoreCost = false);
        void Cast(ITargetable caster, IReadOnlyList<ITargetable> targets, bool ignoreCost = false);
    }
}
