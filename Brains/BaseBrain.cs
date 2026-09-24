using HeadlessCore.Actions;
using HeadlessCore.Characters;
using HeadlessCore.Combat;

namespace HeadlessCore.Brains;

public abstract class BaseBrain
{
    protected CBaseEntity Owner { get; }
    public abstract Task<(string actionId, IReadOnlyList<ITargetable> target)> DecideTurnAsync(
        BattleSession session, 
        CancellationToken cancellationToken = default);
    public BaseBrain(CBaseEntity owner)
    {
        Owner = owner;
    }
}