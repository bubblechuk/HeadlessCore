using HeadlessCore.Characters;
using HeadlessCore.Effects;

namespace HeadlessCore.Actions
{
    public interface IAction
    {
        int Id { get; }
        string Name { get; }
        string Description { get; }
        int Cost { get; }
        IReadOnlyList<CStatusEffect> Effects { get; }
        bool CanCast(ITargetable caster, ITargetable target);
        void Cast(ITargetable caster, ITargetable target);
        void Cast(IEnumerable<ITargetable> targets);
    }
}
