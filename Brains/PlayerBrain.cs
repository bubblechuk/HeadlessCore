using HeadlessCore.Actions;
using HeadlessCore.Characters;
using HeadlessCore.Combat;

namespace HeadlessCore.Brains;

public class PlayerBrain : BaseBrain
{
    private TaskCompletionSource<(string, IReadOnlyList<ITargetable>)>? _tcs;
    public override Task<(string actionId, IReadOnlyList<ITargetable> target)> DecideTurnAsync(
        BattleSession session, 
        CancellationToken cancellationToken = default)
    {
        _tcs = new TaskCompletionSource<(string, IReadOnlyList<ITargetable>)>();
        
        return _tcs.Task;
    }
    public void SelectAction(string actionId, IReadOnlyList<ITargetable> targets)
    {
        _tcs?.TrySetResult((actionId, targets));
    }

    public PlayerBrain(CBaseEntity owner) : base(owner)
    {
    }
}