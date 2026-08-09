using HeadlessCore.Characters;
using HeadlessCore.Effects;

namespace HeadlessCore.Actions
{
    public interface IAction
    {
        int Id { get; }
        string Name { get; }
        int Cost { get; }
        IReadOnlyList<IEffect> Effects { get; }
        void Cast(ITarget target);
        void Cast(IEnumerable<ITarget> targets);
    }
}
